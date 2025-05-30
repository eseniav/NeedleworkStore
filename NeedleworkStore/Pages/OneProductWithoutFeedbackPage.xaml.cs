using NeedleworkStore.AppData;
using NeedleworkStore.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using static NeedleworkStore.Pages.ProductsPage;

namespace NeedleworkStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для OneProductWithoutFeedbackPage.xaml
    /// </summary>
    public partial class OneProductWithoutFeedbackPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Products _product;
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public string ProductImage { get; set; }
        private Visibility _favoriteIconVisibility;
        private Visibility _notFavoriteIconVisibility;
        public Visibility FavoriteIconVisibility
        {
            get
            {
                bool isFavorite = mainWindow.IsAuthenticated &&
                                App.ctx.Favourities.Any(f =>
                                    f.UserID == mainWindow.UserID &&
                                    f.ProductID == _product.ProductID);

                return isFavorite ? Visibility.Visible : Visibility.Collapsed;
            }
            set => _favoriteIconVisibility = value;
        }

        public Visibility NotFavoriteIconVisibility
        {
            get
            {
                bool isNotFavorite = !mainWindow.IsAuthenticated ||
                                   !App.ctx.Favourities.Any(f =>
                                       f.UserID == mainWindow.UserID &&
                                       f.ProductID == _product.ProductID);

                return isNotFavorite ? Visibility.Visible : Visibility.Collapsed;
            }
            set => _notFavoriteIconVisibility = value;
        }
        public void RefreshFavorites()
        {
            OnPropertyChanged(nameof(FavoriteIconVisibility));
            OnPropertyChanged(nameof(NotFavoriteIconVisibility));
        }
        public OneProductWithoutFeedbackPage(Products prod)
        {
            InitializeComponent();
            _product = prod;
            SetProdValues();
            txtDescription.Text = _product.Description;
            List<string> themes = App.ctx.ProductThemes.Where(pt => pt.ProductID == _product.ProductID).Select(pt => pt.Themes.ThemeName).Distinct().ToList();
            lblThemes.Content = string.Join(" ", themes);
            mainWindow.btnProd.IsEnabled = true;
            ProductImage = prod.ProductImage;
            DataContext = this;
        }
        private void SetProdValues()
        {
            Dictionary<string, string> prodData = new Dictionary<string, string>
            {
                { "lblDiz", _product.Designers.DesignerName },
                { "lblNameProd", _product.ProductName },
                { "lblCountry", _product.Designers.Countries.CountryName },
                { "lblAvail", _product.AvailabilityStatuses.AvailabilityStatus },
            };
            string defaultText = "Данные не указаны";
            SolidColorBrush defaultColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#852614"));
            foreach (var item in prodData)
            {
                Label label = (Label)FindName(item.Key);
                label.Content = string.IsNullOrEmpty(item.Value) ? defaultText : item.Value;
                label.Foreground = string.IsNullOrEmpty(item.Value) ? defaultColor : label.Foreground;
            }
        }
        private void btnCartIn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Добавляет товар в корзину");
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (s, args) =>
            {
                ppCartIn.IsOpen = false;
                timer.Stop();
            };

            ppCartIn.IsOpen = true;
            timer.Start();
        }
        
        private void btnFavor_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Добавляет товар в избранное");
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (s, args) =>
            {
                ppFavorIn.IsOpen = false;
                timer.Stop();
            };

            ppFavorIn.IsOpen = true;
            timer.Start();
        }
        private void imQR_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Переход на страницу сайта с готовыми работами");
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mainWindow.RoleID == 1)
                return;
            if (!mainWindow.IsAuthenticated)
            {
                MessageBoxResult msgInf = MessageBox.Show
                    ("Добавить товар в избранное могут только зарегистрированные пользователи. Хотите зарегистрироваться?",
                    "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (msgInf == MessageBoxResult.Yes)
                    this.NavigationService.Navigate(new RegistrationPage());
                return;
            }
            bool alreadyInFavorites = App.ctx.Favourities
                                         .Any(f => f.UserID == mainWindow.UserID && f.ProductID == _product.ProductID);
            if (alreadyInFavorites)
            {
                Favourities delFav = App.ctx.Favourities.Where(f => f.ProductID == _product.ProductID).FirstOrDefault();
                App.ctx.Favourities.Remove(delFav);
                try
                {
                    App.ctx.SaveChanges();
                    RefreshFavorites();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка базы данных");
                    return;
                }
                return;
            }
            Favourities newprodInFav = new Favourities
            {
                UserID = (int)mainWindow.UserID,
                ProductID = _product.ProductID,
            };
            App.ctx.Favourities.Add(newprodInFav);
            App.ctx.SaveChanges();
            RefreshFavorites();
        }
    }
}
