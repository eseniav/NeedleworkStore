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
using NeedleworkStore.UCElements;

namespace NeedleworkStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        List<AppData.Products> products;
        List<MyProducts> myProducts;
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public ProductFilterViewModel FilterVM { get; set; } = new ProductFilterViewModel();
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
            DataContext = this;
        }
        private void ShowAddedPopup()
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
        private void AddInCart(MyProducts selectedProduct)
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
                ShowAddedPopup();
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
            AddInCart((MyProducts)((Button)sender).DataContext);
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
        private void SetFilter()
        {
            List<MyProducts> filterProd;
            List<int> selectedNWID = FilterVM.AllProd.Items.Where(n => n.IsChecked).Select(k => k.Item.NeedleworkTypeID).ToList();
            List<int> selectedStitchID = FilterVM.AllStitch.Items.Where(n => n.IsChecked).Select(k => k.Item.StitchTypeID).ToList();
            filterProd = myProducts.Where(p => p.ProductNeedleworkTypes.Any(c => selectedNWID.Contains(c.NeedleworkTypeID))).ToList();
            filterProd = myProducts.Where(p =>
            {
                bool nwMatch = p.ProductNeedleworkTypes.Any(c => selectedNWID.Contains(c.NeedleworkTypeID));
                bool stitchMatch = selectedStitchID.Count == 0 || FilterVM.AllStitch.AllChecked
                || p.ProductStitchTypes.Any(c => selectedStitchID.Contains(c.StitchTypeID));
                return nwMatch && stitchMatch;
            }).ToList();
            ProdList.ItemsSource = filterProd;
            ProdList.DataContext = filterProd;
        }
        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            SetFilter();
        }
        private void ResetFilter()
        {
            MessageBox.Show("Сбросить все фильтры");
        }
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetFilter();
        }
    }
}
