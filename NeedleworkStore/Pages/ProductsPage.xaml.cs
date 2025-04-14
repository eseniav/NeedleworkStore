using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
    public class ImagePathConverter : IValueConverter
    {
        private string basePath = "ProdImages"; // Базовый путь к изображениям

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string ProductImage && !String.IsNullOrWhiteSpace((string)value))
            {
                return $"/{basePath}/{ProductImage}";
            }
            return "pack://application:,,,/ProdImages/KlartLavandadlyanee.jpg";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public class RelayCommand<T> : ICommand
        {
            private readonly Action<T> _execute;
            private readonly Func<T, bool> _canExecute;

            public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute == null || _canExecute((T)parameter);
            }

            public void Execute(object parameter)
            {
                _execute((T)parameter);
            }

            public event EventHandler CanExecuteChanged;
        }
        public class ProductViewModel : NeedleworkStore.AppData.Products
        {
            public ProductViewModel(NeedleworkStore.AppData.Products product)
            {
                foreach (var property in typeof(NeedleworkStore.AppData.Products).GetProperties().Where(p => p.CanWrite)) // Only include writable properties)
                {
                    property.SetValue(this, property.GetValue(product));
                }

                AddToCartCommand = new RelayCommand<int>(AddToCart);
            }
            public string ImgPath => string.IsNullOrWhiteSpace(ProductImage)
                ? "/ResImages/KlartLavandadlyanee.jpg"
                : $"/ProdImages/{ProductImage}";
            public ICommand AddToCartCommand { get; }
            private void AddToCart(int productId)
            {
                MessageBox.Show($"Product with ID {productId} added to cart.");
                // Your logic for adding product to cart
            }
        }
        public partial class ProductsViewModel : Products
        {
            public string ImgPath =>
            string.IsNullOrWhiteSpace(ProductImage)
                ? "/ResImages/KlartLavandadlyanee.jpg"
                : $"/ProdImages/{ProductImage}";
        }
        public ProductsPage()
        {
            InitializeComponent();

            NeedleworkStoreEntities ctx = new NeedleworkStoreEntities();
            var products = ctx.Products.ToList();

            var viewModelProducts = products.Select(p => new ProductsViewModel
            {
                ProductImage = p.ProductImage,
                ProductName = p.ProductName,
                ProductPrice = p.ProductPrice,
                Description = p.Description,
                Designers = p.Designers,
                AvailabilityStatuses = p.AvailabilityStatuses,
            }).ToList();

            productList.DataContext = viewModelProducts;
            productList.ItemsSource = viewModelProducts;

            var vmProducts = products.Select(p => new ProductViewModel(p)).ToList();
            productList.ItemsSource = vmProducts;
            productList.DataContext = vmProducts;
        }

        private void btnCartIn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (s, args) =>
            {
                ppCartIn.IsOpen = false;
                timer.Stop();
            };

            var product = button?.DataContext as ProductViewModel;
            ppCartInTxt.Text = $"Товар {button?.Tag} {product.ProductName} добавлен в корзину";
            ppCartIn.IsOpen = true;
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
            Button btn = (Button)sender;  // Get the button that was clicked
            Popup ppFavorIn = (Popup)btn.FindName("ppFavorIn"); // Find the popup within the button

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
    }
}
