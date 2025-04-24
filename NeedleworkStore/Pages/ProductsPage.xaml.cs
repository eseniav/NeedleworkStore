using System;
using System.Collections.Generic;
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

namespace NeedleworkStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
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
            List<AppData.Products> products = App.ctx.Products.ToList();
            myProducts = products.Select(p => new MyProducts(p)).ToList();
            ProdList.ItemsSource = myProducts;
            ProdList.DataContext = myProducts;
            SortByAvailability.IsSelected = true;
        }
        private void AddToCart(int prodId)
        {
            MainWindow mainWindow = ((MainWindow)Application.Current.MainWindow);
            try
            {
                // Check if the same product already exists
                Carts existingCartItem = App.ctx.Carts
                    .FirstOrDefault(c => c.UserID == mainWindow.UserID && c.ProductID == prodId);
                if (existingCartItem?.QuantityCart >= CartPage.maxCapacity)
                {
                    // Show warning
                    MessageBox.Show("Превышен лимит добавления одного товара в корзину!",
                        "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                // if (existingCartItem?.QuantityCart > 0) // Alternatively
                if (existingCartItem != null)
                {
                    // Increment quantity
                    existingCartItem.QuantityCart++;
                }
                else
                {
                    // Create new cart item
                    Carts cart = new Carts
                    {
                        UserID = (int)mainWindow.UserID,
                        ProductID = prodId,
                        QuantityCart = 1,
                        FormationDate = DateTime.Now,
                    };
                    // Add a new cart item to Db
                    App.ctx.Carts.Add(cart);
                }
                App.ctx.SaveChanges();
                // Show success confirmation popup
                txtBlPopup.Text = "Товар добавлен в корзину";
                mainWindow.UpdateCartState();
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ShowModalDialog()
        {
            // Создание экземпляра модального диалога: Вариант 1
            /*
            NeedleworkStore.Windows.ThreeChoiceModal modal = new Windows.ThreeChoiceModal(
                "Добавить товар в корзину могут только зарегистрированные пользователи.",
                "Предупреждение",
                "Регистрация",
                "Вход",
                "Отмена"
                );
            */
            // Создание экземпляра модального диалога: Вариант 2
            NeedleworkStore.Windows.ThreeChoiceModal modal = new Windows.ThreeChoiceModal
            {
                Message = "Добавить товар в корзину могут только зарегистрированные пользователи.",
                Opt1Text = "Регистрация",
                Opt2Text = "Вход",
                WindowTitle = "Уведомление",
            };
            // Отображение окна как модального диалога
            bool? result = modal.ShowDialog();
            // Обработка результатов пользовательского выбора
            if (result != true) return;
            if (modal.Choice == 1) NavigationService.Navigate(new RegistrationPage());
            else NavigationService.Navigate(new AuthPage());
        }
        private void SortProducts(string sortParam)
        {
            List<MyProducts> products;
            switch (sortParam)
            {
                case "SortByPrice":
                    products = myProducts.OrderBy(p => p.ProductPrice).ToList();
                    break;
                case "SortByName":
                    products = myProducts.OrderBy(p => p.ProductName).ToList();
                    break;
                case "SortByAvailability":
                    products = myProducts.OrderBy(p => p.AvailabilityStatusID).ToList();
                    break;
                case "SortByRating":
                default:
                    products = myProducts;
                    break;
            }
            ProdList.ItemsSource = products;
        }
        
        private void btnCartIn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            MainWindow mainWindow = ((MainWindow)Application.Current.MainWindow);
            if (!mainWindow.IsAuthenticated)
            {
                ShowModalDialog();
                return;
            }
            var prodId = ((Products)btn.DataContext).ProductID;
            AddToCart(prodId);
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox ctl = (ComboBox)sender;
            ComboBoxItem selectedItem = ctl.SelectedItem as ComboBoxItem;
            string selectedValue = selectedItem.Content as string;
            string selectedName = selectedItem.Name.ToString();
            SortProducts(selectedName);
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
