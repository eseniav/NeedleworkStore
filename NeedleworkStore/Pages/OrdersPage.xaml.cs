﻿using NeedleworkStore.AppData;
using NeedleworkStore.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using PdfSharp.Xps;
using System.Windows.Xps.Packaging;
using Microsoft.Win32;
using System.IO.Packaging;
using System.IO;
using System.Windows.Media.Animation;
using static NeedleworkStore.Pages.OrdersPage;

namespace NeedleworkStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public List<Orders> OrdersList { get; set; } = App.ctx.Orders.ToList();
        private DispatcherTimer _checkTimer;
        private string _lastValidText = "";
        public List<OrderViewModel> orders = new List<OrderViewModel>();
        private OrderViewModel currentOrder;
        public OrderViewModel CurrentOrder
        {
            get => currentOrder;
            set
            {
                currentOrder = value;
                OnPropertyChanged(nameof(CurrentOrder));
            }
        }
        public class OrderViewModel : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            private readonly MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;
            private Visibility _AdminCmb;
            private Visibility _StatusLabel;
            public ProcessingStatuses _processingStatus;
            public PaymentStatuses _paymentStatus;
            public ReceivingStatuses _receivingStatus;
            public bool IsModified => _processingStatus != processingStatus || _paymentStatus != paymentStatus || _receivingStatus != receivingStatus;
            public List<ProcessingStatuses> AvailableProcessingStatuses { get; set; } = App.ctx.ProcessingStatuses.ToList();
            public List<PaymentStatuses> AvailablePaymentStatuses { get; set; } = App.ctx.PaymentStatuses.ToList();
            public List<ReceivingStatuses> AvailableReceivingStatuses { get; set; } = App.ctx.ReceivingStatuses.ToList();
            public int OrderID { get; set; }
            public DateTime? FormationDate { get; set; }
            public string PickUpPointAddress { get; set; }
            public List<OrderItemViewModel> Items { get; set; }
            public decimal TotalAmount => Items.Sum(i => i.Price * i.Quantity);
            public string CardNumber { get; set; }
            private PaymentStatuses paymentStatus;
            public PaymentStatuses PaymentStatus
            {
                get => paymentStatus;
                set
                {
                    paymentStatus = value;
                    OnPropertyChanged(nameof(PaymentStatus));
                    OnPropertyChanged(nameof(IsModified));
                }
            }
            public bool IsPayed { get; set; }
            private ProcessingStatuses processingStatus;
            public ProcessingStatuses ProcessingStatus { get => processingStatus;
                set
                {
                    processingStatus = value;
                    OnPropertyChanged(nameof(ProcessingStatus));
                    OnPropertyChanged(nameof(IsModified));
                }
            }
            private ReceivingStatuses receivingStatus;
            public ReceivingStatuses ReceivingStatus
            {
                get => receivingStatus;
                set
                {
                    receivingStatus = value;
                    OnPropertyChanged(nameof(ReceivingStatus));
                    OnPropertyChanged(nameof(IsModified));
                }
            }
            public Visibility AdminCmb
            {
                get => _mainWindow.RoleID == 1 ? Visibility.Visible : Visibility.Collapsed;
                set => _AdminCmb = value;
            }
            public Visibility StatusLabel
            {
                get => _mainWindow.RoleID == 1 ? Visibility.Collapsed : Visibility.Visible;
                set => _StatusLabel = value;
            }
            public void UpdateVisibility()
            {
                OnPropertyChanged(nameof(AdminCmb));
                OnPropertyChanged(nameof(StatusLabel));
                OnPropertyChanged(nameof(cmbOrders));
            }
        }
        public class OrderItemViewModel
        {
            public string ProductName { get; set; }
            public string DesignName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal Amount => Quantity * Price;
        }
        public void SetAdminMenu()
        {
            if (mainWindow.RoleID != 1)
            {
                txtOrders.Visibility = Visibility.Collapsed;
                cmbOrders.Visibility = Visibility.Collapsed;
                btnSavechanges.Visibility = Visibility.Collapsed;
                btnBackProfile.Visibility = Visibility.Visible;
                return;
            }
            txtOrders.Visibility = Visibility.Visible;
            cmbOrders.Visibility = Visibility.Visible;
            btnSavechanges.Visibility = cmbOrders.SelectedIndex != -1 ? Visibility.Visible : Visibility.Collapsed;
            btnBackProfile.Visibility = Visibility.Collapsed;
        }
        public OrdersPage()
        {
            InitializeComponent();
            mainWindow.UpdateCartState();
            LoadOrders();
            SetAdminMenu();
            _lastValidText = "";
            mainWindow.btnProd.IsEnabled = true;
            mainWindow.btnAddProd.IsEnabled = true;
            OrderViewModel om = new OrderViewModel();
            om.UpdateVisibility();
            DataContext = this;
        }
        private void LoadOrdersByAdmin(int orderID)
        {
            try
            {
                var allStatuses = App.ctx.ProcessingStatuses
                              .Select(s => s.ProcessingStatus)
                              .ToList();
                orders = App.ctx.Orders
                    .Where(o => o.OrderID == orderID)
                    .ToList()
                    .Select(o => new OrderViewModel
                    {
                        OrderID = o.OrderID,
                        FormationDate = o.FormationDate,
                        CardNumber = o.CardNumber,
                        PickUpPointAddress = o.PickUpPoints != null ? (o.PickUpPoints.Cities != null ?
                                            o.PickUpPoints.Cities.CityName + ", " + o.PickUpPoints.Adress
                                            : o.PickUpPoints.Adress) : "Адрес не указан",
                        Items = o.OrderCompositions?.Select(oc => new OrderItemViewModel
                        {
                            ProductName = oc.Products?.ProductName ?? "Товар не найден",
                            DesignName = oc.Products?.Designers?.DesignerName ?? "Производитель не указан",
                            Quantity = oc.Quantity,
                            Price = oc.OrderPrice
                        }).ToList() ?? new List<OrderItemViewModel>(),
                        PaymentStatus = o.AssigningStatuses?.LastOrDefault()?.PaymentStatuses,
                        _paymentStatus = o.AssigningStatuses?.LastOrDefault()?.PaymentStatuses,
                        IsPayed = o.AssigningStatuses?.LastOrDefault()?.PaymentStatuses.PaymentID == 1 ? true : false,
                        ProcessingStatus = o.AssigningStatuses?.LastOrDefault()?.ProcessingStatuses,
                        _processingStatus = o.AssigningStatuses?.LastOrDefault()?.ProcessingStatuses,
                        ReceivingStatus = o.AssigningStatuses?.LastOrDefault()?.ReceivingStatuses,
                        _receivingStatus = o.AssigningStatuses?.LastOrDefault()?.ReceivingStatuses
                    }).ToList();

                ICorders.ItemsSource = orders;
                ICorders.DataContext = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}");
            }
        }
        private void CheckOrderTimer_Tick(object sender, EventArgs e)
        {
            ((DispatcherTimer)sender).Stop();
            SetOrderLayout(CheckExistingOrder(cmbOrders.Text));
        }
        private void cmbOrders_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text[0]))
            {
                e.Handled = true;
                return;
            }
            if (_checkTimer != null)
            {
                _checkTimer.Stop();
            }
            _checkTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };

            _checkTimer.Tick += CheckOrderTimer_Tick;
            _checkTimer.Start();
        }
        public void SetOrderLayout(bool state)
        {
            ICorders.Visibility = state ? Visibility.Visible : Visibility.Collapsed;
            btnSavechanges.Visibility = state ? Visibility.Visible : Visibility.Collapsed;
            if (!state)
            {
                cmbOrders.Text = "";
                cmbOrders.SelectedIndex = -1;
            }
        }
        public bool CheckExistingOrder(string idOrder)
        {
            if (int.TryParse(idOrder, out int orderId))
            {
                if (OrdersList.Any(o => o.OrderID == orderId))
                {
                    _lastValidText = idOrder;
                    return true;
                }
                MessageBox.Show($"Заказ с номером {orderId} не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return false;
        }
        private void cmbOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainWindow.RoleID == 1 && cmbOrders.SelectedItem != null)
            {
                Orders selectedOrder = cmbOrders.SelectedItem as Orders;
                if (selectedOrder != null)
                {
                    SetOrderLayout(CheckExistingOrder(selectedOrder.OrderID.ToString()));
                    LoadOrdersByAdmin(selectedOrder.OrderID);
                    SetAdminMenu();
                    CurrentOrder = orders.FirstOrDefault(c => c.OrderID == selectedOrder.OrderID);
                }
            }
        }
        private void LoadOrders()
        {
            try
            {
               orders = App.ctx.Orders
                        .OrderByDescending(o => o.FormationDate)
                        .Where(o => o.UserID == mainWindow.UserID)
                        .ToList()
                        .Select(o => new OrderViewModel
                        {
                            OrderID = o.OrderID,
                            FormationDate = o.FormationDate,
                            CardNumber = o.CardNumber,
                            PickUpPointAddress = o.PickUpPoints != null ? (o.PickUpPoints.Cities != null ?
                                            o.PickUpPoints.Cities.CityName + ", " + o.PickUpPoints.Adress
                                            : o.PickUpPoints.Adress) : "Адрес не указан",
                            Items = o.OrderCompositions?.Select(oc => new OrderItemViewModel
                            {
                                ProductName = oc.Products?.ProductName ?? "Товар не найден",
                                DesignName = oc.Products?.Designers?.DesignerName ?? "Производитель не указан",
                                Quantity = oc.Quantity,
                                Price = oc.OrderPrice
                            }).ToList() ?? new List<OrderItemViewModel>(),
                            PaymentStatus = o.AssigningStatuses?.LastOrDefault()?.PaymentStatuses,
                            IsPayed = o.AssigningStatuses?.LastOrDefault()?.PaymentStatuses.PaymentID == 1 ? true : false,
                            ProcessingStatus = o.AssigningStatuses?.LastOrDefault()?.ProcessingStatuses,
                            ReceivingStatus = o.AssigningStatuses?.LastOrDefault()?.ReceivingStatuses
                        }).ToList();

                ICorders.ItemsSource = orders;
                ICorders.DataContext = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}");
            }
        }

    public FlowDocument GenerateCheckDocument(OrderViewModel orderViewModel)
    {
        FlowDocument document = new FlowDocument();
        document.PagePadding = new Thickness(20);
        document.FontFamily = new FontFamily("Arial");
        document.FontSize = 12;
        document.Blocks.Add(CreateParagraph("Кассовый чек. Приход", FontWeights.Bold, 14));
        foreach(var item in orderViewModel.Items.Select((val, index) => new { val, index }))
            {
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run((item.index + 1).ToString())
                {
                    FontWeight = FontWeights.Bold,
                });
                paragraph.Inlines.Add(".");
                paragraph.Inlines.Add($" {item.val.DesignName} - {item.val.ProductName}");

                document.Blocks.Add(paragraph);
                document.Blocks.Add(CreateParagraph(item.val.Quantity + " × " + String.Format("{0:0.00}", item.val.Price) + " = " + String.Format("{0:0.00}", item.val.Amount), FontWeights.Light, 10));
                if (item.index < orderViewModel.Items.Count)
                {
                    document.Blocks.Add(new Paragraph(new Run(new string('_', 30)))
                    {
                        FontSize = 10,
                        Foreground = Brushes.LightGray,
                        Margin = new Thickness(0, 5, 0, 5)
                    });
                }
            }
        document.Blocks.Add(new Section()
        {
            Blocks = {
            CreateParagraph("ИТОГО: " + String.Format("{0:0.00}", orderViewModel.TotalAmount), FontWeights.Bold, 12),
            CreateParagraph("НДС 20%: " + orderViewModel.TotalAmount / 100 * 20, FontWeights.Normal, 10)
        }
        });
            if (!string.IsNullOrEmpty(orderViewModel.CardNumber))
            {
                document.Blocks.Add(
                    CreateParagraph("Номер карты: **** **** **** " + orderViewModel.CardNumber.Substring(12), FontWeights.Normal, 10)
                );
            }
            document.Blocks.Add(new Section()
        {
            Blocks = {
            CreateParagraph("Магазин \"Мастерская вдохновения\"", FontWeights.Normal, 10),
            CreateParagraph("Пользователь АО \"Планета увлечений\"", FontWeights.Normal, 10),
            CreateParagraph("ИНН 7705814643", FontWeights.Normal, 10),
            CreateParagraph("Адрес пункта выдачи: ", FontWeights.Normal, 10),
            CreateParagraph(orderViewModel.PickUpPointAddress, FontWeights.Normal, 10)
        }
        });
        document.Blocks.Add(new Section()
        {
            Blocks = {
            CreateParagraph("Дата выдачи: " + orderViewModel.FormationDate, FontWeights.Normal, 10),
            CreateParagraph("Номер заказа: " + orderViewModel.OrderID, FontWeights.Normal, 10)
        }
        });
        return document;
    }
    private Paragraph CreateParagraph(string text, FontWeight weight, double size)
    {
        return new Paragraph(
            new Run(text)
            {
                FontWeight = weight,
                FontSize = size
            }
        );
    }
    private void DownloadChequeInPdf(FlowDocument doc, OrderViewModel order)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                FileName = "Check_" + order.OrderID + "_" + order.FormationDate?.ToString("dd_MM_yyyy"),
                DefaultExt = ".pdf",
                Filter = "Документ (*.pdf)|*.pdf"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    string pdfPath = sfd.FileName;
                    string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid() + ".xps");
                    using (Package package = Package.Open(tempPath, System.IO.FileMode.Create))
                    {
                        XpsDocument xpsDoc = new XpsDocument(package);
                        var xpsWriter = XpsDocument.CreateXpsDocumentWriter(xpsDoc);
                        var paginator = ((IDocumentPaginatorSource)doc).DocumentPaginator;
                        xpsWriter.Write(paginator);
                        xpsDoc.Close();
                    }
                    XpsConverter.Convert(tempPath, pdfPath, 0);
                    File.Delete(tempPath);
                    MessageBox.Show("Файл успешно сохранен", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}");
                }
            }
        }
        private void btnChequePdf_Click(object sender, RoutedEventArgs e)
        {
            OrderViewModel order = (OrderViewModel)((Button)sender).DataContext;
            DownloadChequeInPdf(GenerateCheckDocument(order), order);
        }
        public void SaveStatus()
        {
            try
            {
                AssigningStatuses assigningStatuses = new AssigningStatuses
                {
                    PaymentStatusID = CurrentOrder.PaymentStatus.PaymentID,
                    ReceivingStatusID = CurrentOrder.ReceivingStatus.ReceivingStatusID,
                    ModifiedDate = DateTime.Now,
                    OrderID = CurrentOrder.OrderID,
                    ProcessingStatusID = CurrentOrder.ProcessingStatus.ProcessingStatusID,
                };
                App.ctx.AssigningStatuses.Add(assigningStatuses);
                App.ctx.SaveChanges();
                MessageBox.Show("Данные обновлены", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }
        private void btnSavechanges_Click(object sender, RoutedEventArgs e)
        {
            SaveStatus();
        }
        private void btnBackProfile_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ProfilePage());
        }
    }
}
