using NeedleworkStore.AppData;
using NeedleworkStore.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        DispatcherTimer saveTimer;
        public CartPage()
        {
            InitializeComponent();
            ResetCart();
            ICCart.ItemsSource = cart;
            ICCart.DataContext = cart;
            ChangeFullEmptyCart();
            SetTotalSum();
            ChangeSelectedQuantityBottomMenu();
            cmbIAvail.IsSelected = true;
            mainWindow.UpdateCartState();
            saveTimer = new DispatcherTimer();
            saveTimer.Interval = TimeSpan.FromMilliseconds(500);
            saveTimer.Tick += OnSaveCart;
        }
        private void OnSaveCart(object sender, EventArgs e) => SaveCart();
        private ObservableCollection<Carts> GetCarts() =>
         new ObservableCollection<Carts>(App.ctx.Carts.Where(c => c.UserID == mainWindow.UserID).ToList());
        private void SetGroupCheck() => chbAll.IsChecked = !cart.Any(c => c.IsChecked != true);
        private void CartItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Carts.IsChecked))
                SetGroupCheck();
        }
        private void SetTotalSum() => lblTotalSum.Content = cart.Sum(c => c.SumProducts).ToString();
        private void ResetCart()
        {
            cart = GetCarts();
            foreach (Carts crt in cart)
                crt.PropertyChanged += CartItem_PropertyChanged;
        }
        private void SaveCart()
        {
            try
            {
                App.ctx.SaveChanges();
                mainWindow.UpdateCartState();
                SetTotalSum();
                ChangeSelectedQuantityBottomMenu();
            }
            catch (Exception ex)
            {
                ResetCart();
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка базы данных");
            }
        }
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
        private List<Carts> GetSortedProd(string cmbName)
        {
            switch (cmbName)
            {
                case "cmbIAZ":
                    return cart.OrderBy(p => p.Products.ProductName).ToList();
                case "cmbIZA":
                    return cart.OrderByDescending(p => p.Products.ProductName).ToList();
                case "cmbILowPrice":
                    return cart.OrderBy(p => p.SumProducts).ToList();
                case "cmbIHighPrice":
                    return cart.OrderByDescending(p => p.SumProducts).ToList();
                case "cmbIAvail":
                    return cart.OrderBy(p => p.Products.AvailabilityStatusID).ToList();
                case "cmbINotAvail":
                    return cart.OrderByDescending(p => p.Products.AvailabilityStatusID).ToList();
                default:
                    return cart.ToList();
            }
        }
        private void SortProd(string cmbName)
        {
            ICCart.ItemsSource = GetSortedProd(cmbName);
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
        private void ChangeQuantity(Carts cr, RepeatButton rb, bool isIncrement = true)
        {
            cr.Quantity = isIncrement? ++cr.Quantity : --cr.Quantity;
            saveTimer.Stop();
            saveTimer.Start();
        }
        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            Carts selectedCartProd = (Carts)((RepeatButton)sender).DataContext;
            ChangeQuantity(selectedCartProd, (RepeatButton)sender, false);
        }
        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            Carts selectedCartProd = (Carts)((RepeatButton)sender).DataContext;
            ChangeQuantity(selectedCartProd, (RepeatButton)sender);
        }
        private void DelOneProduct(Carts cr)
        {
            MessageBoxResult msgInf = MessageBox.Show
                    ("Удалить выбранный товар из корзины?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgInf != MessageBoxResult.Yes)
                return;
            App.ctx.Carts.Remove(cr);
            cart.Remove(cr);
            ICCart.ItemsSource = cart;
            SaveCart();
        }
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            Carts selectedCartProd = (Carts)((Button)sender).DataContext;
            DelOneProduct(selectedCartProd);
        }
        private void DelSelectedProduct(List<Carts> cr)
        {
            MessageBoxResult msgInf = MessageBox.Show
                    ("Удалить выбранные товары из корзины?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgInf != MessageBoxResult.Yes)
                return;
            App.ctx.Carts.RemoveRange(cr);
            foreach(Carts crt in cr)
                cart.Remove(crt);
            ICCart.ItemsSource = cart;
            SaveCart();
        }
        private void btnDelAll_Click(object sender, RoutedEventArgs e)
        {
            DelSelectedProduct(cart.Where(c => c.IsChecked).ToList());
        }
        private void btnPlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            List<Carts> crt = cart.Where(c => c.IsChecked).ToList();
            if(crt.Count == 0)
            {
                MessageBox.Show("Выберите товары для формирования заказа");
                return;
            }
            this.NavigationService.Navigate(new OrderRegistrationPage(crt));
        }
        private void btnEmptyBuy_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ProductsPage());
        }
        private void GroupSelect(bool IsChecked = true)
        {
            foreach (Carts crt in cart)
                crt.IsChecked = IsChecked;
        }
        private void chbAll_Click(object sender, RoutedEventArgs e)
        {
            GroupSelect(((CheckBox)sender).IsChecked == true);
        }
    }
}
