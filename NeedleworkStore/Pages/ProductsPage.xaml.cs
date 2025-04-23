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
        List<AppData.Products> products;
        List<MyProducts> myProducts;
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
            products = App.ctx.Products.ToList();
            myProducts = products.Select(p => new MyProducts(p)).ToList();
            ProdList.ItemsSource = myProducts;
            ProdList.DataContext = myProducts;
            cmbIAvail.IsSelected = true;
        }
        
        private void btnCartIn_Click(object sender, RoutedEventArgs e)
        {
            // @TODO: Add to cart
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
        private void SortProd(string cmbName)
        {
            List<MyProducts> sortedProducts;
            switch (cmbName)
            {
                case "cmbIAZ":
                    sortedProducts = myProducts.OrderBy(p => p.ProductName).ToList();
                    break;
                case "cmbIZA":
                    sortedProducts = myProducts.OrderByDescending(p => p.ProductName).ToList();
                    break;
                case "cmbILowPrice":
                    sortedProducts = myProducts.OrderBy(p => p.ProductPrice).ToList();
                    break;
                case "cmbIHighPrice":
                    sortedProducts = myProducts.OrderByDescending(p => p.ProductPrice).ToList();
                    break;
                case "cmbIAvail":
                    sortedProducts = myProducts.OrderBy(p => p.AvailabilityStatusID).ToList();
                    break;
                case "cmbINotAvail":
                    sortedProducts = myProducts.OrderByDescending(p => p.AvailabilityStatusID).ToList();
                    break;
                default:
                    sortedProducts = myProducts;
                    break;
            }
            ProdList.ItemsSource = sortedProducts;
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = ((ComboBox)sender).SelectedItem as ComboBoxItem;
            SortProd(selectedItem.Name);
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
