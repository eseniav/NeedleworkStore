using NeedleworkStore.AppData;
using NeedleworkStore.Classes;
using System;
using System.Collections.Generic;
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

namespace NeedleworkStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public class OrderViewModel
        {
            public int OrderID { get; set; }
            public DateTime? FormationDate { get; set; }
            public string PickUpPointAddress { get; set; }
            public List<OrderItemViewModel> Items { get; set; }
            public decimal TotalAmount => Items.Sum(i => i.Price * i.Quantity);
            public string PaymentStatus { get; set; }
            public string ProcessingStatus { get; set; }
            public string ReceivingStatus { get; set; }
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
                wrPSort.Visibility = Visibility.Collapsed;
                return;
            }
            wrPSort.Visibility = Visibility.Visible;
        }
        public OrdersPage()
        {
            InitializeComponent();
            mainWindow.UpdateCartState();
            LoadOrders();
            SetAdminMenu();
            mainWindow.btnProd.IsEnabled = true;
        }
        private void LoadOrders()
        {
            try
            {
                List<OrderViewModel> orders = App.ctx.Orders
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
                            PaymentStatus = o.AssigningStatuses?.FirstOrDefault()?.PaymentStatuses?.PaymentStatus ?? "Не определен",
                            ProcessingStatus = o.AssigningStatuses?.FirstOrDefault()?.ProcessingStatuses?.ProcessingStatus ?? "Не определен",
                            ReceivingStatus = o.AssigningStatuses?.FirstOrDefault()?.ReceivingStatuses?.ReceivingStatus ?? "Не определен"
                        }).ToList();

                ICorders.ItemsSource = orders;
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
    }
}
