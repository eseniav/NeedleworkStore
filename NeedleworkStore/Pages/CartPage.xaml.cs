using NeedleworkStore.AppData;
using NeedleworkStore.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static NeedleworkStore.Pages.ProductsPage;

namespace NeedleworkStore.Pages{
    public class ImagePathConverter : IValueConverter
    {
        private string basePath = "/ProdImages/";
        public object Convert(object value, Type targetType, object parametr, CultureInfo culture) {
            if (value is string imgName && !String.IsNullOrWhiteSpace(imgName))
                return basePath + imgName;

            return "/ResImages/NoPicture.png";
        }
        public object ConvertBack(object value, Type targetType, object parametr, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        ObservableCollection<Carts> cart;
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public int? TotalSum;
        public CartPage()
        {
            InitializeComponent();
            cart = GetCarts();
            ICCart.ItemsSource = cart;
            ICCart.DataContext = cart;
            ChangeFullEmptyCart();
            SetTotalSum();
            ChangeSelectedQuantityBottomMenu();
            cmbIAvail.IsSelected = true;
        }
        private ObservableCollection<Carts> GetCarts() =>
         new ObservableCollection<Carts>(App.ctx.Carts.Where(c => c.UserID == mainWindow.UserID).ToList());
        private void SetTotalSum() => lblTotalSum.Content = cart.Sum(c => c.SumProducts).ToString();
        private void ChangeFullEmptyCart()
        {
            if (App.ctx.Carts.FirstOrDefault(c => c.UserID == mainWindow.UserID) != null)
            {
                stPEmpty.Visibility = Visibility.Collapsed;
                GrTopPan.Visibility = Visibility.Visible;
                ICCart.Visibility = Visibility.Visible;
                GrBottomPan.Visibility = Visibility.Visible;
            }
            else
            {
                stPEmpty.Visibility = Visibility.Visible;
                GrTopPan.Visibility = Visibility.Hidden;
                ICCart.Visibility = Visibility.Hidden;
                GrBottomPan.Visibility = Visibility.Hidden;
            }
        }
        private void ChangeSelectedQuantityBottomMenu()
        {
            lblTotalQuantity.Content = cart.Sum(p => p.QuantityCart).ToString();
        }
        private void SortProd(string cmbName)
        {
            List<Carts> sortedProducts;
            switch (cmbName)
            {
                case "cmbIAZ":
                    sortedProducts = cart.OrderBy(p => p.Products.ProductName).ToList();
                    break;
                case "cmbIZA":
                    sortedProducts = cart.OrderByDescending(p => p.Products.ProductName).ToList();
                    break;
                case "cmbILowPrice":
                    sortedProducts = cart.OrderBy(p => p.SumProducts).ToList();
                    break;
                case "cmbIHighPrice":
                    sortedProducts = cart.OrderByDescending(p => p.SumProducts).ToList();
                    break;
                case "cmbIAvail":
                    sortedProducts = cart.OrderBy(p => p.Products.AvailabilityStatusID).ToList();
                    break;
                case "cmbINotAvail":
                    sortedProducts = cart.OrderByDescending(p => p.Products.AvailabilityStatusID).ToList();
                    break;
                default:
                    sortedProducts = cart.ToList();
                    break;
            }
            ICCart.ItemsSource = sortedProducts;
        }
        private void GoAboutProduct(Carts cart)
        {
            this.NavigationService.Navigate(new OneProductPage());
        }
        private void hlAbout_Click(object sender, RoutedEventArgs e)
        {
            Carts selectedCartProd = (Carts)((Hyperlink)sender).DataContext;
            GoAboutProduct(selectedCartProd);
        }
        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortProd((((ComboBox)sender).SelectedItem as ComboBoxItem).Name);
        }
        private void MakingOrderOneProduct(Carts cart)
        {
            List<Carts> crt = new List<Carts>() { cart };
            this.NavigationService.Navigate(new OrderRegistrationPage(crt));
        }
        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            Carts selectedCartProd = (Carts)((Button)sender).DataContext;
            MakingOrderOneProduct(selectedCartProd);
        }
        private void MinusQuantity(Carts cr, RepeatButton rb)
        {
            cr.Quantity--;
            try
            {
                App.ctx.SaveChanges();
                mainWindow.UpdateCartState();
                SetTotalSum();
                ChangeSelectedQuantityBottomMenu();
            }
            catch (Exception ex)
            {
                cart = GetCarts();
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка базы данных");
            }
        }
        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            Carts selectedCartProd = (Carts)((RepeatButton)sender).DataContext;
            MinusQuantity(selectedCartProd, (RepeatButton)sender);
        }
        private void PlusQuantity(Carts cr, RepeatButton rb)
        {
            cr.Quantity++;
            try
            {
                App.ctx.SaveChanges();
                mainWindow.UpdateCartState();
                SetTotalSum();
                ChangeSelectedQuantityBottomMenu();
            }
            catch (Exception ex)
            {
                cart = GetCarts();
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            Carts selectedCartProd = (Carts)((RepeatButton)sender).DataContext;
            PlusQuantity(selectedCartProd, (RepeatButton)sender);
        }        
        private void DelOneProduct(Carts cr)
        {
            MessageBoxResult msgInf = MessageBox.Show
                    ("Удалить выбранный товар из корзины?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgInf != MessageBoxResult.Yes)
                return;
            try
            {
                App.ctx.Carts.Remove(cr);
                App.ctx.SaveChanges();
                cart.Remove(cr);
                SetTotalSum();
                ChangeSelectedQuantityBottomMenu();
                ICCart.ItemsSource = cart;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            Carts selectedCartProd = (Carts)((Button)sender).DataContext;
            DelOneProduct(selectedCartProd);
        }

        private void btnDelAll_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Удаляет выбранные товары.\nПеред удалением появляется специальное окошко с выбором");
        }

        private void btnPlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Если не выбрано ничего - автоматически выбирает все товары\n" +
                "Если выбраны определенные товары - формирует заказ из них");
        }

        private void btnEmptyBuy_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ProductsPage());
        }
    }
}
