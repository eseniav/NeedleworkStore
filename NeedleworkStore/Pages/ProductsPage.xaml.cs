using System;
using System.Collections.Generic;
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
using NeedleworkStore.Classes;

namespace NeedleworkStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public class MyProducts : Products
        {
            public MyProducts(Products p)
            {
                foreach (var property in typeof(Products).GetProperties()) property.SetValue(this, property.GetValue(p));
            }
            public string ImagePath => "/ProdImages/" + ProductImage;
        }

        public ProductsPage()
        {
            InitializeComponent();
            List<AppData.Products> products = App.ctx.Products.ToList();
            List<MyProducts> myProducts = products.Select(p => new MyProducts(p)).ToList();
            ProdList.ItemsSource = myProducts;
            ProdList.DataContext = myProducts;
        }
        
        private void btnCartIn_Click(object sender, RoutedEventArgs e)
        {
            // @TODO: Add to cart
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            if (mainWindow.UserID == null)
            {
                MessageBoxResult msgInf = MessageBox.Show
                    ("Добавить товар в корзину могут только зарегистрированные пользователи. Хотите зарегистрироваться?",
                    "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (msgInf == MessageBoxResult.Yes)
                    this.NavigationService.Navigate(new RegistrationPage());
                return;
            } else
            {
                txtBlPopup.Text = "Товар добавлен в корзину";
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
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("Соответствующая сортировка");
        }

        private void hlAbout_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new OneProductPage());
        }

        private void btnFavor_Click(object sender, RoutedEventArgs e)
        {
            // @TODO: Add to favorites
            txtBlPopup.Text = "Товар добавлен в избранное";
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
    }
}
