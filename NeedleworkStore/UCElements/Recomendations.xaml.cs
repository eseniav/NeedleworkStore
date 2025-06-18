using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using NeedleworkStore.AppData;
using NeedleworkStore.Pages;
using static NeedleworkStore.Pages.ProductsPage;

namespace NeedleworkStore.UCElements
{
    /// <summary>
    /// Логика взаимодействия для Recomendations.xaml
    /// </summary>
    public partial class Recomendations : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event Action<Products> ProductAddedToCart;
        public List<Products> ListException
        {
            get => (List<Products>)GetValue(ListExceptionProperty);
            set => SetValue(ListExceptionProperty, value);
        }
        public static readonly DependencyProperty ListExceptionProperty =
            DependencyProperty.Register("ListException", typeof(List<Products>), typeof(Recomendations), new PropertyMetadata(new List<Products>()));
        public int ItemsCount
        {
            get => (int)GetValue(ItemsCountProperty);
            set => SetValue(ItemsCountProperty, value);
        }
        public static readonly DependencyProperty ItemsCountProperty =
            DependencyProperty.Register("ItemsCount", typeof(int), typeof(Recomendations), new PropertyMetadata(3));

        private List<Products> products;
        private List<Products> productsList;
        public List<Products> ProductsList {
            get => productsList;
            set {
                productsList = value;
                OnPropertyChanged(nameof(ProductsList));
            }
        }
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        private DispatcherTimer _randomSubsetTimer;
        public void RandomSubset()
        {
            Random random = new Random();
            ProductsList = products.OrderBy(p => random.Next()).Take(ItemsCount).ToList();
        }
        public Recomendations()
        {
            InitializeComponent();
            InitializeTimer();
        }
        private void InitializeTimer()
        {
            _randomSubsetTimer = new DispatcherTimer();
            _randomSubsetTimer.Interval = TimeSpan.FromSeconds(5);
            _randomSubsetTimer.Tick += RandomSubsetTimer_Tick;
            _randomSubsetTimer.Start();
        }

        private void RandomSubsetTimer_Tick(object sender, EventArgs e)
        {
            RandomSubset();
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.frame.Navigate(new OneProductWithoutFeedbackPage((Products)((Image)sender).DataContext));
        }
        private void ShowAddedPopup(int type)
        {
            txtBlPopup.Text = type == 1 ? "Товар добавлен в корзину" : "Товар добавлен в избранное";
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (s, args) =>
            {
                popup.IsOpen = false;
                timer.Stop();
            };

            popup.IsOpen = true;
            timer.Start();
        }
        private void AddInCart(Products selectedProduct)
        {
            try
            {
                Carts existingCartItem = App.ctx.Carts
                    .FirstOrDefault(c => c.UserID == mainWindow.UserID && c.ProductID == selectedProduct.ProductID);
                if (existingCartItem?.QuantityCart == Carts.maxItemCopacity)
                {
                    MessageBox.Show("Превышен лимит добавления одного товара в корзину!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (existingCartItem?.QuantityCart == Carts.maxItemCart)
                {
                    MessageBox.Show("Максимальное количество товара в корзине не может превышать 2000 позиций",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (existingCartItem != null)
                    existingCartItem.QuantityCart++;
                else
                {
                    Carts newprodInCart = new Carts
                    {
                        UserID = (int)mainWindow.UserID,
                        ProductID = selectedProduct.ProductID,
                        QuantityCart = 1,
                    };
                    App.ctx.Carts.Add(newprodInCart);
                }
                App.ctx.SaveChanges();
                mainWindow.UpdateCartState();
                ShowAddedPopup(1);
                ProductAddedToCart?.Invoke(selectedProduct);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnCartIn_Click(object sender, RoutedEventArgs e)
        {
            if (!mainWindow.IsAuthenticated)
            {
                MessageBoxResult msgInf = MessageBox.Show
                    ("Добавить товар в корзину могут только зарегистрированные пользователи. Хотите зарегистрироваться?",
                    "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (msgInf == MessageBoxResult.Yes)
                    MainWindow.frame.Navigate(new RegistrationPage());
                return;
            }
            AddInCart((Products)((Button)sender).DataContext);
        }
        public void AddInFav(Products selectedProduct)
        {
            try
            {
                bool alreadyInFavorites = App.ctx.Favourities
                                         .Any(f => f.UserID == mainWindow.UserID && f.ProductID == selectedProduct.ProductID);
                if (alreadyInFavorites)
                {
                    MessageBox.Show("Товар уже есть в избранном",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                Favourities newprodInFav = new Favourities
                {
                    UserID = (int)mainWindow.UserID,
                    ProductID = selectedProduct.ProductID,
                };
                App.ctx.Favourities.Add(newprodInFav);
                App.ctx.SaveChanges();
                ShowAddedPopup(2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnFavor_Click(object sender, RoutedEventArgs e)
        {
            if (!mainWindow.IsAuthenticated)
            {
                MessageBoxResult msgInf = MessageBox.Show
                    ("Добавить товар в избранное могут только зарегистрированные пользователи. Хотите зарегистрироваться?",
                    "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (msgInf == MessageBoxResult.Yes)
                    MainWindow.frame.Navigate(new RegistrationPage());
                return;
            }
            AddInFav((Products)((Button)sender).DataContext);
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var excludedIds = ListException.Select(ex => ex.ProductID).ToList();
            products = App.ctx.Products.Where(p => !excludedIds.Contains(p.ProductID)).Where(pr => pr.AvailabilityStatusID == 1).ToList();
            DataContext = this;
            RandomSubset();
            if (_randomSubsetTimer == null)
                InitializeTimer();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _randomSubsetTimer?.Stop();
            _randomSubsetTimer = null;
        }
    }
}
