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
        private List<MyProducts> filterProducts;
        string sortCrit = "cmbIAvail";
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
            filterProducts = myProducts.ToList();
            ProdList.ItemsSource = filterProducts;
            ProdList.DataContext = filterProducts;
            cmbIAvail.IsSelected = true;
            DataContext = this;
            SetInfoForEmptyList();
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
        private List<MyProducts> GetSortedProd(string sortCrit, List<MyProducts> myProd)
        {
            switch (sortCrit)
            {
                case "cmbIAZ":
                    return myProd.OrderBy(p => p.ProductName).ToList();
                case "cmbIZA":
                    return myProd.OrderByDescending(p => p.ProductName).ToList();
                case "cmbILowPrice":
                    return myProd.OrderBy(p => p.ProductPrice).ToList();
                case "cmbIHighPrice":
                    return myProd.OrderByDescending(p => p.ProductPrice).ToList();
                case "cmbIAvail":
                    return myProd.OrderBy(p => p.AvailabilityStatusID).ToList();
                case "cmbINotAvail":
                    return myProd.OrderByDescending(p => p.AvailabilityStatusID).ToList();
                default:
                    return myProd;
            }
        }
        private void SortProd()
        {
            filterProducts = GetSortedProd(sortCrit, filterProducts);
            ProdList.ItemsSource = filterProducts;
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = ((ComboBox)sender).SelectedItem as ComboBoxItem;
            sortCrit = selectedItem.Name;
            SortProd();
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
        private List<MyProducts> GetFilteredProd(List<MyProducts> myPr, ProductFilterViewModel filter)
        {
            List<MyProducts> filterProd;
            List<int> selectedNWID = filter.AllProd.GetIDs(n => n.NeedleworkTypeID);
            List<int> selectedStitchID = filter.AllStitch.GetIDs(n => n.StitchTypeID);
            List<int> selectedProdTypesID = filter.AllProdTypes.GetIDs(n => n.ProductTypeID);
            List<int> selectedAccessoryTypesID = filter.AllAccessoryTypes.GetIDs(n => n.AccessoryTypeID);
            List<int> selectedDesignersID = filter.AllDesigners.GetIDs(n => n.DesignerID);
            List<int> selectedThemesID = filter.AllThemes.GetIDs(n => n.ThemeID);
            filterProd = myPr.Where(p =>
            {
                if (selectedNWID.Count == 0)
                    return true;
                bool nwMatch = p.ProductNeedleworkTypes.Any(c => selectedNWID.Contains(c.NeedleworkTypeID));
                bool stitchMatch = selectedStitchID.Count == 0 ||
                                   filter.AllStitch.AllChecked ||
                                   p.ProductNeedleworkTypes.Any(c => c.NeedleworkTypeID == 2) ||
                                   p.ProductStitchTypes.Any(c => selectedStitchID.Contains(c.StitchTypeID));
                return nwMatch && stitchMatch;
            }).ToList();
            filterProd = filterProd.Where(p =>
            {
                if (selectedProdTypesID.Count == 0)
                    return true;
                bool prodTypeMatch = p.ProductTypes.Products.Any(c => selectedProdTypesID.Contains(c.ProductTypeID));
                bool accessoryMatch = selectedAccessoryTypesID.Count == 0 ||
                                    filter.AllAccessoryTypes.AllChecked ||
                                    p.ProductTypes.ProductTypeID == 1 || p.ProductTypes.ProductTypeID == 3 ||
                                    p.ProductAccessoryTypes.Any(c => selectedAccessoryTypesID.Contains(c.AccessoryTypeID));
                return prodTypeMatch && accessoryMatch;
            }).ToList();
            filterProd = filterProd.Where(p =>
            {
                if (selectedDesignersID.Count == 0)
                    return true;
                return p.Designers.Products.Any(c => selectedDesignersID.Contains(c.DesignerID));
            }).ToList();
            filterProd = filterProd.Where(p =>
            {
                if (selectedThemesID.Count == 0)
                    return true;
                return p.ProductThemes.Any(c => selectedThemesID.Contains(c.ThemeID));
            }).ToList();
            return filterProd;
        }
        private void SetFilter()
        {
            filterProducts = GetSortedProd(sortCrit, GetFilteredProd(myProducts, FilterVM));
            ProdList.ItemsSource = filterProducts;
            SetInfoForEmptyList();
        }
        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            SetFilter();
        }
        private void ResetFilter()
        {
            filterProducts = myProducts.ToList();
            SortProd();
            FilterVM.Reset();
            SearchBar.SetTab();
        }
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetFilter();
        }
        private void SetInfoForEmptyList()
        {
            stPEmpty.Visibility = filterProducts.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            wrPSort.Visibility = filterProducts.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
        }
        private void btnEmptyBuy_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ProductsPage());
        }
    }
}
