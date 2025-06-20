﻿using NeedleworkStore.AppData;
using NeedleworkStore.Classes;
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
using static NeedleworkStore.Pages.ProductsPage;
using QRCoder;
using System.IO;

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
        private BitmapImage GetQRImage(byte[] imageData)
        {
            using (var ms = new MemoryStream(imageData))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
        private byte[] GetQRData(string url)
        {
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qRCode = new PngByteQRCode(qRCodeData);
            return qRCode.GetGraphic(20);
        }
        private void SetQR()
        {
            string baseUrl = "https://www.stitch.su/";
            string url = baseUrl + _product.QRLink;
            imQR.Source = GetQRImage(GetQRData(url));
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
            RecomendUC.ListException.Add(_product);
            if (!string.IsNullOrEmpty(_product.QRLink))
            {
                SetQR();
                stPQR.Visibility = Visibility.Visible;
            }
        }
        private void SetProdValues()
        {
            Dictionary<string, string> prodData = new Dictionary<string, string>
            {
                { "lblDiz", _product.Designers.DesignerName },
                { "lblNameProd", _product.ProductName },
                { "lblCountry", _product.Designers.Countries.CountryName },
                { "lblAvail", _product.AvailabilityStatuses.AvailabilityStatus },
                { "lblPrice", _product.ProductPrice.ToString() },
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
        private void AddInCart()
        {
            try
            {
                Carts existingCartItem = App.ctx.Carts
                    .FirstOrDefault(c => c.UserID == mainWindow.UserID && c.ProductID == _product.ProductID);
                if (existingCartItem?.QuantityCart == Carts.maxItemCopacity)
                {
                    MessageBox.Show("Превышен лимит добавления одного товара в корзину!",
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
                        ProductID = _product.ProductID,
                        QuantityCart = 1,
                    };
                    App.ctx.Carts.Add(newprodInCart);
                }
                App.ctx.SaveChanges();
                mainWindow.UpdateCartState();
                ShowAddedPopup(1);
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
                    this.NavigationService.Navigate(new RegistrationPage());
                return;
            }
            AddInCart();
        }
        public void AddInFav()
        {
            try
            {
                bool alreadyInFavorites = App.ctx.Favourities
                                         .Any(f => f.UserID == mainWindow.UserID && f.ProductID == _product.ProductID);
                if (alreadyInFavorites)
                {
                    MessageBox.Show("Товар уже есть в избранном",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    this.NavigationService.Navigate(new RegistrationPage());
                return;
            }
            AddInFav();
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
