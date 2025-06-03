using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        public bool IsProdPage { get; set; }
        public ProductFilterViewModel FilterVM { get; set; } = new ProductFilterViewModel();
        public class MyProducts : Products, INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            private readonly MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;
            private readonly bool _isProdPage;
            private Visibility _favoriteIconVisibility;
            private Visibility _notFavoriteIconVisibility;
            public Visibility FavoriteIconVisibility
            {
                get
                {
                    bool isFavorite = _mainWindow.IsAuthenticated &&
                                    App.ctx.Favourities.Any(f =>
                                        f.UserID == _mainWindow.UserID &&
                                        f.ProductID == ProductID);

                    return isFavorite ? Visibility.Visible : Visibility.Collapsed;
                }
                set => _favoriteIconVisibility = value;
            }

            public Visibility NotFavoriteIconVisibility
            {
                get
                {
                    bool isNotFavorite = !_mainWindow.IsAuthenticated ||
                                       !App.ctx.Favourities.Any(f =>
                                           f.UserID == _mainWindow.UserID &&
                                           f.ProductID == ProductID);

                    return isNotFavorite ? Visibility.Visible : Visibility.Collapsed;
                }
                set => _notFavoriteIconVisibility = value;
            }
            public Visibility OneProdPageVisibility
            {
                get
                {
                    return _mainWindow.RoleID == 1 ? Visibility.Collapsed : Visibility.Visible;
                }
            }
            public void RefreshFavorites()
            {
                OnPropertyChanged(nameof(FavoriteIconVisibility));
                OnPropertyChanged(nameof(NotFavoriteIconVisibility));
            }
            public MyProducts(Products p, bool prPage)
            {
                foreach (var property in typeof(Products).GetProperties()) property.SetValue(this, property.GetValue(p));

                FavoriteIconVisibility = Visibility.Collapsed;
                NotFavoriteIconVisibility = Visibility.Visible;
                UpdateButtonTexts();
                _isProdPage = prPage;
            }
            public string ImagePath => "/ProdImages/" + ProductImage;
            public string ButtonTextCart { get; set; }
            public string ButtonTextFav { get; set; }
            public void UpdateButtonTexts()
            {
                ButtonTextCart = _mainWindow.RoleID != 1 ? "В корзину" : "Редактировать";
                ButtonTextFav = (_mainWindow.RoleID == 1 || !_isProdPage) ? "Удалить" : "В избранное";
            }
            public bool CanAddToCart
            {
                get
                {
                    if (_mainWindow.RoleID == 1)
                        return true;
                    return AvailabilityStatusID == 1;
                }
            }
        }
        public void UpdateButtonsForRole()
        {
            foreach (var product in myProducts)
            {
                product.UpdateButtonTexts();
            }
            ProdList.Items.Refresh();
        }
        public void SetFavHead() => stPFav.Visibility = !IsProdPage ? Visibility.Visible : Visibility.Collapsed;
        public ProductsPage(string searchText = null, bool prodPage = true)
        {
            InitializeComponent();
            IsProdPage = prodPage;
            SetFavHead();
            if (!IsProdPage)
            {
                var favoriteProductIds = App.ctx.Favourities.Where(f => f.UserID == mainWindow.UserID)
                                        .Select(f => f.ProductID).ToList();
                products = App.ctx.Products.Where(p => favoriteProductIds.Contains(p.ProductID)).ToList();
                mainWindow.btnProd.IsEnabled = true;
            }
            else
            {
                products = App.ctx.Products.ToList();
                mainWindow.btnProd.IsEnabled = false;
            }
            myProducts = products.Select(p => new MyProducts(p, IsProdPage)).ToList();
            filterProducts = searchText == null
                ? myProducts.ToList()
                : myProducts.Where(p => p.ProductName.ToLower().Contains(searchText.ToLower())
                || p.Designers.DesignerName.ToLower().Contains(searchText.ToLower())
                || p.Description.ToLower().Contains(searchText.ToLower())).ToList();
            ProdList.ItemsSource = filterProducts;
            ProdList.DataContext = filterProducts;
            cmbIAvail.IsSelected = true;
            DataContext = this;
            mainWindow.SetMenuForRoles();
            UpdateButtonsForRole();
            SetInfoForEmptyList();
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
        private void AddInCart(MyProducts selectedProduct)
        {
            try
            {
                if(mainWindow.RoleID == 1)
                {
                    this.NavigationService.Navigate(new AddProdPage(selectedProduct));
                    return;
                }
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
            Products myProd = (Products)((Hyperlink)sender).DataContext;
            this.NavigationService.Navigate(new OneProductWithoutFeedbackPage(myProd));
        }
        public void AddInFav(MyProducts selectedProduct)
        {
            try
            {
                if (mainWindow.RoleID == 1)
                {
                    MessageBoxResult msgInf = MessageBox.Show
                        ("Удалить выбранный товар?",
                        "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (msgInf != MessageBoxResult.Yes)
                        return;
                    App.ctx.Products.Remove(selectedProduct);
                    try
                    {
                        App.ctx.SaveChanges();
                        ProdList.Items.Refresh();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка базы данных");
                        return;
                    }
                    ProdList.Items.Refresh();
                    return;
                }
                if(!IsProdPage)
                {
                    MessageBoxResult msgInf = MessageBox.Show
                        ("Удалить выбранный товар из избранного?",
                        "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (msgInf != MessageBoxResult.Yes)
                        return;
                    Favourities delFav = App.ctx.Favourities.Where(f => f.ProductID == selectedProduct.ProductID).FirstOrDefault();
                    App.ctx.Favourities.Remove(delFav);
                    try
                    {
                        App.ctx.SaveChanges();
                        MessageBox.Show("Товар удален из избранного", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        selectedProduct.RefreshFavorites();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка базы данных");
                        return;
                    }
                    ProdList.Items.Refresh();
                    return;
                }
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
                selectedProduct.RefreshFavorites();
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
            AddInFav((MyProducts)((Button)sender).DataContext);
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
            filterProd = filterProd.Where(p =>
            {
                if (FilterVM.MinPrice == null && FilterVM.MaxPrice == null)
                    return true;
                if (FilterVM.MinPrice == null)
                    return p.ProductPrice <= FilterVM.MaxPrice;
                if (FilterVM.MaxPrice == null)
                    return p.ProductPrice >= FilterVM.MinPrice;
                return p.ProductPrice >= FilterVM.MinPrice && p.ProductPrice <= FilterVM.MaxPrice;
            }).ToList();
            return filterProd;
        }
        private void ShowWarning()
        {
            MessageBox.Show("Проверьте правильность вводимых данных",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void SetFilter()
        {
            if(!FilterVM.IsValid || FilterVM.MaxPrice < FilterVM.MinPrice)
            {
                ShowWarning();
                return;
            }
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
            SearchBar.Reset();
        }
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            stPEmpty.Visibility = Visibility.Collapsed;
            ResetFilter();
        }
        private void SetInfoForEmptyList()
        {
            if (!IsProdPage)
            {
                stPEmpty.Visibility = Visibility.Collapsed;
                wrPSort.Visibility = filterProducts.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
                stPEmptyFav.Visibility = filterProducts.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                return;
            }
            stPEmpty.Visibility = filterProducts.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            wrPSort.Visibility = filterProducts.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
            stPEmptyFav.Visibility = Visibility.Collapsed;
        }
        private void btnEmptyBuy_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ProductsPage(null, IsProdPage));
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
            MyProducts selectedProduct = (MyProducts)((Image)sender).DataContext;
            bool alreadyInFavorites = App.ctx.Favourities
                                         .Any(f => f.UserID == mainWindow.UserID && f.ProductID == selectedProduct.ProductID);
            if (alreadyInFavorites)
            {
                Favourities delFav = App.ctx.Favourities.Where(f => f.ProductID == selectedProduct.ProductID).FirstOrDefault();
                App.ctx.Favourities.Remove(delFav);
                try
                {
                    App.ctx.SaveChanges();
                    selectedProduct.RefreshFavorites();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка базы данных");
                    return;
                }
                ProdList.Items.Refresh();
                return;
            }
            Favourities newprodInFav = new Favourities
            {
                UserID = (int)mainWindow.UserID,
                ProductID = selectedProduct.ProductID,
            };
            App.ctx.Favourities.Add(newprodInFav);
            App.ctx.SaveChanges();
            selectedProduct.RefreshFavorites();
        }
        private void btnFavToProd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductsPage(null, IsProdPage));
        }
    }
}
