using NeedleworkStore.AppData;
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

namespace NeedleworkStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public List<Orders> OrdersList { get; set; } = App.ctx.Orders.ToList();
        private DispatcherTimer _checkTimer;
        private string _currentText = "";
        private string _lastValidText = "";
        List<OrderViewModel> orders = new List<OrderViewModel>();
        OrderViewModel CurrentOrder;
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
            public List<ProcessingStatuses> AvailableProcessingStatuses { get; set; } = App.ctx.ProcessingStatuses.ToList();
            public List<PaymentStatuses> AvailablePaymentStatuses { get; set; } = App.ctx.PaymentStatuses.ToList();
            public List<ReceivingStatuses> AvailableReceivingStatuses { get; set; } = App.ctx.ReceivingStatuses.ToList();
            public int OrderID { get; set; }
            public DateTime? FormationDate { get; set; }
            public string PickUpPointAddress { get; set; }
            public List<OrderItemViewModel> Items { get; set; }
            public decimal TotalAmount => Items.Sum(i => i.Price * i.Quantity);
            public PaymentStatuses PaymentStatus { get; set; }
            private ProcessingStatuses _processingStatuses;
            public ProcessingStatuses ProcessingStatus { get => _processingStatuses;
                set
                {
                    _processingStatuses = value;
                    OnPropertyChanged(nameof(ProcessingStatus));
                }
            }
            public ReceivingStatuses ReceivingStatus { get; set; }
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
            cmbOrders.Visibility = Visibility.Visible; ;
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
                        PickUpPointAddress = o.PickUpPoints?.Adress ?? "Адрес не указан",
                        Items = o.OrderCompositions?.Select(oc => new OrderItemViewModel
                        {
                            ProductName = oc.Products?.ProductName ?? "Товар не найден",
                            DesignName = oc.Products?.Designers?.DesignerName ?? "Производитель не указан",
                            Quantity = oc.Quantity,
                            Price = oc.OrderPrice
                        }).ToList() ?? new List<OrderItemViewModel>(),
                        PaymentStatus = o.AssigningStatuses?.LastOrDefault()?.PaymentStatuses,
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
        private void CheckOrderTimer_Tick(object sender, EventArgs e)
        {
            ((DispatcherTimer)sender).Stop();

            if (int.TryParse(_currentText, out int orderId))
            {
                if (!OrdersList.Any(o => o.OrderID == orderId))
                {
                    ICorders.Visibility = Visibility.Collapsed;
                    btnSavechanges.Visibility = Visibility.Collapsed;
                    MessageBox.Show("Заказ с таким номером не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    cmbOrders.Text = "";
                    cmbOrders.SelectedIndex = -1;
                }
                else
                {
                    _lastValidText = _currentText;
                    ICorders.Visibility = Visibility.Visible;
                }
            }
            else
            {
                cmbOrders.Text = "";
                cmbOrders.SelectedIndex = -1;
            }
        }
        private void cmbOrders_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text[0]))
            {
                e.Handled = true;
                return;
            }
            _currentText = cmbOrders.Text + e.Text;
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
        public void CheckExistingOrder(string idOrder)
        {
            if (int.TryParse(idOrder, out int orderId))
            {
                if (!OrdersList.Any(o => o.OrderID == orderId))
                {
                    MessageBox.Show("Заказ с таким номером не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    cmbOrders.Text = "";
                    cmbOrders.SelectedIndex = -1;
                }
                else
                {
                    _lastValidText = idOrder;
                }
            }
            else
            {
                cmbOrders.Text = "";
                cmbOrders.SelectedIndex = -1;
            }

        }
        private void cmbOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainWindow.RoleID == 1 && cmbOrders.SelectedItem != null)
            {
                Orders selectedOrder = cmbOrders.SelectedItem as Orders;
                if (selectedOrder != null)
                {
                    CheckExistingOrder(selectedOrder.OrderID.ToString());
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
                            PickUpPointAddress = o.PickUpPoints?.Adress ?? "Адрес не указан",
                            Items = o.OrderCompositions?.Select(oc => new OrderItemViewModel
                            {
                                ProductName = oc.Products?.ProductName ?? "Товар не найден",
                                DesignName = oc.Products?.Designers?.DesignerName ?? "Производитель не указан",
                                Quantity = oc.Quantity,
                                Price = oc.OrderPrice
                            }).ToList() ?? new List<OrderItemViewModel>(),
                            PaymentStatus = o.AssigningStatuses?.LastOrDefault()?.PaymentStatuses,
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
        private void DownloadChequeInPdf()
        {
            MessageBox.Show("Скачать чек в пдф");
        }
        private void btnChequePdf_Click(object sender, RoutedEventArgs e)
        {
            DownloadChequeInPdf();
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
