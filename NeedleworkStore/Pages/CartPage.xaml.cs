using NeedleworkStore.AppData;
using NeedleworkStore.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        // Определение отслеживаемой коллекции
        ObservableCollection<Carts> cart;
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public const int maxItemCopacity = 100;
        public int TotalQuantity { get; set; }
        public int TotalQty { get; set; }
        public int TotalSum { get; set; }
        public CartPage()
        {
            InitializeComponent();
            // Установка коллекции
            ResetCart();
            // Добавление обработчика на изменение коллекции
            cart.CollectionChanged += Cart_CollectionChanged;
            // Добавление обработчиков на изменения элементов
            foreach (var item in cart)
                item.PropertyChanged += CartItem_PropertyChanged;
            // Установка источника данных (однократно)
            ICCart.ItemsSource = cart;
            // Установка контекста данных (однократно)
            ICCart.DataContext = cart;
            ChangeQuantityProducts();
        }
        // Получение данных из БД
        private ObservableCollection<Carts> GetCarts() =>
            new ObservableCollection<Carts>(App.ctx.Carts.Where(c => c.UserID == mainWindow.UserID).ToList());
        private void UpdateTotals()
        {
            UpdateTotalSum();
            UpdateTotalQty();
        }
        private void UpdateTotalSum()
        {
            TotalSum = cart.Sum(item => item.TotalSum);
        }
        private void UpdateTotalQty()
        {
            TotalQty = cart.Sum(item => item.QuantityCart);
        }
        // Установка или сброс коллекции
        private void ResetCart()
        {
            cart = GetCarts();
            UpdateTotals();
        }
        // Обработчик изменения состава коллекции
        private void Cart_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateTotals();
        }
        // Обработчик изменения элемента коллекции
        private void CartItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Carts.QuantityCart)) // если изменилось количество
                UpdateTotals();
        }
        private void ChangeQuantityProducts()
        {
            if (App.ctx.Carts.FirstOrDefault(c => c.UserID == mainWindow.UserID) != null)
            {
                stPEmpty.Visibility = Visibility.Collapsed;
                GrTopPan.Visibility = Visibility.Visible;
                ICCart.Visibility = Visibility.Visible;
                GrBottomPan.Visibility = Visibility.Visible;
                TotalQuantity = App.ctx.Carts
                                .Where(c => c.UserID == mainWindow.UserID)
                                .Sum(c => c.QuantityCart);
                txtBlQuantity.Text = TotalQuantity.ToString();
            }
            else
            {
                stPEmpty.Visibility = Visibility.Visible;
                GrTopPan.Visibility = Visibility.Hidden;
                ICCart.Visibility = Visibility.Hidden;
                GrBottomPan.Visibility = Visibility.Hidden;
                txtBlQuantity.Text = "0";
            }
            mainWindow.UpdateCartState();
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
            MessageBox.Show("Соответствующая сортировка");
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                ResetCart();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                ResetCart();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                ResetCart();
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
