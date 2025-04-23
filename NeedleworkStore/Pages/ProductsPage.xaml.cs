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

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show("Соответствующая сортировка");
            //ProdList.ItemsSource = myProducts.OrderBy(p => p.ProductName).ToList();
            if (cmbSort.SelectedItem is ComboBoxItem selectedItem)
            {
                IOrderedEnumerable<MyProducts> sortedProducts = null;

                switch (selectedItem.Name)
                {
                    case "cmbIAZ":
                        sortedProducts = myProducts.OrderBy(p => p.ProductName);
                        break;
                    case "cmbIZA":
                        sortedProducts = myProducts.OrderByDescending(p => p.ProductName);
                        break;
                    case "cmbILowPrice":
                        sortedProducts = myProducts.OrderBy(p => p.ProductPrice);
                        break;
                    case "cmbIHighPrice":
                        sortedProducts = myProducts.OrderByDescending(p => p.ProductPrice);
                        break;
                    //case "cmbIAvail":
                    //    sortedProducts = myProducts.OrderBy(p => p.AvailabilityStatuses.AvailabilityStatus == "есть в наличии")
                    //        .ThenBy(p => p.AvailabilityStatuses.AvailabilityStatus == "нет в наличии");
                    //    break;
                    //case "cmbINotAvail":
                    //    sortedProducts = myProducts.OrderBy(p => p.AvailabilityStatuses.AvailabilityStatus == "нет в наличии")
                    //        .ThenBy(p => p.AvailabilityStatuses.AvailabilityStatus == "есть в наличии");
                    //    break;
                }

                if (sortedProducts != null)
                {
                    ProdList.ItemsSource = sortedProducts.ToList();
                }
            }
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
