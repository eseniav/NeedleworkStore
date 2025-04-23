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
            MainWindow mainWindow = ((MainWindow)Application.Current.MainWindow);
            if (!mainWindow.IsAuthenticated)
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
                return;
            }
            // @TODO: Add to cart
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
