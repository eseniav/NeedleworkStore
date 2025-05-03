using NeedleworkStore.AppData;
using NeedleworkStore.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    public partial class CartPage : Page, INotifyPropertyChanged
    {
        // Событие интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        // Метод для вызова события PropertyChanged
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // Определение отслеживаемой коллекции
        ObservableCollection<Carts> cart;
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public const int maxItemCopacity = 100;
        // Определение таймера для сохранения
        DispatcherTimer saveTimer;
        public int TotalQuantity { get; set; }
        public int TotalQty { get; set; }
        public int TotalSum { get; set; }
        public int totalPrice => cart.Sum(item => item.TotalSum);
        public int totalQuantity => cart.Sum(item => item.QuantityCart);
        public int selectedQty => cart.Count(item => item.IsChecked);
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
            // Установка контекста данных для итоговых значений
            totals.DataContext = this;
            // Установка глобального контекста данных страницы на корневой контейнер
            this.DataContext = this;
            ChangeQuantityProducts();
            // Установка таймера на сохранение
            saveTimer = new DispatcherTimer();
            saveTimer.Interval = TimeSpan.FromMilliseconds(500); // задержка 500 мс после последнего нажатия
            saveTimer.Tick += SaveCart; // добавляем обработчик для сохранения в БД
            // Включение логирования запросов в БД
            App.ctx.Database.Log =
                (s => System.Diagnostics.Debug.WriteLine(s));
        }
        // Получение данных из БД
        private ObservableCollection<Carts> GetCarts() =>
            new ObservableCollection<Carts>(App.ctx.Carts.Where(c => c.UserID == mainWindow.UserID).ToList());
        private void UpdateSelectedQty()
        {
            OnPropertyChanged(nameof(selectedQty));
        }
        /// <summary>
        /// Обновляет общие значения
        /// </summary>
        private void UpdateTotals()
        {
            UpdateTotalSum();
            UpdateTotalQty();
            OnPropertyChanged(nameof(totalPrice));
            OnPropertyChanged(nameof(totalQuantity));
        }
        /// <summary>
        /// Обновляет общую сумму
        /// </summary>
        private void UpdateTotalSum()
        {
            TotalSum = cart.Sum(item => item.TotalSum);
            totalSum.Text = TotalSum.ToString();
        }
        /// <summary>
        /// Обновляет общее количество
        /// </summary>
        private void UpdateTotalQty()
        {
            TotalQty = cart.Sum(item => item.QuantityCart);
            totalQty.Text = TotalQty.ToString();
        }
        /// <summary>
        /// Устанавливает или сбрасывает коллекцию
        /// </summary>
        private void ResetCart()
        {
            cart = GetCarts();
            UpdateTotals();
        }
        // Обработчик изменения состава коллекции
        private void Cart_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (Carts item in e.NewItems)
                    item.PropertyChanged += CartItem_PropertyChanged;

            if (e.OldItems != null)
                foreach (Carts item in e.OldItems)
                    item.PropertyChanged -= CartItem_PropertyChanged;

            UpdateTotals();
        }
        // Обработчик изменения элемента коллекции
        private void CartItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Carts.QuantityCart)) // если изменилось количество
                UpdateTotals();

            if (e.PropertyName == nameof(Carts.IsChecked)) // если изменилось выделение
                UpdateSelectedQty();
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
            List<OrderCompositions> crt = new List<OrderCompositions>() {
                new OrderCompositions {
                    ProductID = cart.ProductID,
                    OrderPrice = cart.Products.ProductPrice ?? 0,
                    Quantity = cart.QuantityCart
                }
            };
            this.NavigationService.Navigate(new OrderRegistrationPage(crt));
        }
        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            Carts selectedCartProd = (Carts)((Button)sender).DataContext;
            MakeOrder(new List<Carts> { selectedCartProd });
        }
        // Сохраняет данные в БД по таймеру
        private void SaveCart(object sender, EventArgs e)
        {
            saveTimer.Stop(); // Останавливаем таймер, чтобы не повторять сохранение
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
        // Реализует условную логику изменения количества
        private void ChangeQuantity(Carts cr, RepeatButton rb, bool IsAscending = true)
        {
            cr.Quantity = IsAscending ? ++cr.Quantity : --cr.Quantity;
            // Перезапускаем таймер при каждом нажатии
            saveTimer.Stop();
            saveTimer.Start();
        }
        // Обработчики нажатий
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
        private void RemoveMultiple(List<Carts> items)
        {
            try
            {
                App.ctx.Carts.RemoveRange(items);
                App.ctx.SaveChanges();
                foreach (var item in items)
                    cart.Remove(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                ResetCart();
            }
        }
        private void btnDelAll_Click(object sender, RoutedEventArgs e)
        {
            List<Carts> toDel = cart.Where(item => item.IsChecked).ToList();
            if (toDel.Count == 0)
            {
                MessageBox.Show("Для удаления сначала выберите элементы");
                return;
            }
            MessageBoxResult msgInf = MessageBox.Show
                    ("Удалить все выбранные товары из корзины?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgInf == MessageBoxResult.Yes)
                RemoveMultiple(cart.Where(item => item.IsChecked).ToList());
        }
        private void MakeOrder(List<Carts> cart)
        {
            List<OrderCompositions> order = cart.Select(item => new OrderCompositions
                {
                    ProductID = item.ProductID,
                    OrderPrice = item.Products.ProductPrice ?? 0,
                    Quantity = item.QuantityCart
                }
            ).ToList();
            this.NavigationService.Navigate(new OrderRegistrationPage(order));
        }
        private void GroupSelect(bool IsChecked = true)
        {
            foreach (var item in cart) item.IsChecked = IsChecked;
        }
        private void btnPlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            // Если не выбрано ничего - автоматически выбирает все товары
            if (!cart.Any(item => item.IsChecked)) GroupSelect();
            // Если выбраны определенные товары - формирует заказ из них
            MakeOrder(cart.Where(item => item.IsChecked).ToList());
        }
        private void btnEmptyBuy_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ProductsPage());
        }

        private void GroupSelect_Checked(object sender, RoutedEventArgs e)
        {
            GroupSelect(((CheckBox)sender).IsChecked == true);
        }
    }
}
