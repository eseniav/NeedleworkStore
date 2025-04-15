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
using NeedleworkStore.Classes;

namespace NeedleworkStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
            List<AppData.Products> products = App.ctx.Products.ToList();
            products.ForEach(p => p.ProductImage = "/ProdImages/" + p.ProductImage);
            ProdList.ItemsSource = products;
            ProdList.DataContext = products;
        }
        
        private void btnCartIn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Добавляет товар в корзину");
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (s, args) =>
            {
                //ppCartIn.IsOpen = false;
                timer.Stop();
            };

            //ppCartIn.IsOpen = true;
            timer.Start();
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
            MessageBox.Show("Добавляет товар в избранное");
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (s, args) =>
            {
                //ppFavorIn.IsOpen = false;
                timer.Stop();
            };

            //ppFavorIn.IsOpen = true;
            timer.Start();
        }

       
    }
}
