using NeedleworkStore.AppData;
using NeedleworkStore.Classes;
using System;
using System.Collections.Generic;
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

namespace NeedleworkStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для OneProductWithoutFeedbackPage.xaml
    /// </summary>
    public partial class OneProductWithoutFeedbackPage : Page
    {
        public Products _product;
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public string ProductImage { get; set; }
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
        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoForward)
            {
                this.NavigationService.GoForward();
            }
            else
            {
                MessageBox.Show("No page to go forward");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("No page to go back");
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
    }
}
