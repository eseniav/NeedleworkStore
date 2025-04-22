using NeedleworkStore.Classes;
using NeedleworkStore.Pages;
using NeedleworkStore.UCElements;
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

namespace NeedleworkStore
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    
    public partial class MainWindow : Window
    {
        public static Frame frame;
        private int? _userID;
        // Определение элементов управления
        Dictionary<FrameworkElement, bool> menuElements { get; set; }
        public int? UserID {
            get => _userID;
            set {
                _userID = value;

                // Логика переключения элементов управления
                ToggleTopMenuControls(value);
                UpdateCartState(value);
            }
        }
        public MainWindow()
        {
            InitializeComponent();

            // Инициализация элементов управления
            menuElements = new Dictionary<FrameworkElement, bool>
            {
                {  btnProfile, false },
                {  btnExit, false },
                {  txtBlQuanProd, false },
                {  txtBlQuan, false },
                {  txtBlGreeting, false },
                {  txtblUserName, false },
                {  btnReg, true },
                {  btnAuthReg, true },
            };
            frame = Mainfrm;
            UserID = null;
        }

        /// <summary>
        /// Sets visibility to top menu controls
        /// </summary>
        /// <param name="uid">a user identifier</param>
        /// <example>Some example</example>
        private void ToggleTopMenuControls(int? uid)
        {
            if (uid == null)
                foreach (var item in menuElements) item.Key.Visibility = !item.Value ? Visibility.Collapsed : Visibility.Visible;
            else
                foreach (var item in menuElements) item.Key.Visibility = item.Value ? Visibility.Collapsed : Visibility.Visible;
        }
        /// <summary>
        /// Triggers shopping cart products quantity update
        /// </summary>
        /// <param name="uid">User identifier</param>
        private void UpdateCartState(int? uid)
        {
            txtBlQuan.Text = uid == null ? string.Empty : App.ctx.Carts.Where(c => c.UserID == uid).Count().ToString();
        }
        private void btnCart_Click(object sender, RoutedEventArgs e)
        {
            Mainfrm.Navigate(new CartPage());
        }
        private void btnShop_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу О магазине");
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            Mainfrm.Navigate(new ProfilePage());
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            UserID = null;
            Mainfrm.Navigate(new AuthPage());
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation.CheckEmptyNull(txtSearch.Text))
            {
                MessageBox.Show("Переход на страницу с найденной информацией\n" +
                "или подсвеченная информация на этой странице");
            }
            else
            {
                MessageBox.Show("Заполните поле!");
            }
        }
        private void btnProd_Click(object sender, RoutedEventArgs e)
        {
            Mainfrm.Navigate(new ProductsPage());
        }
        private void txtSearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }        
        private void btnCartGuest_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Предложение зарегистрироваться или авторизоваться");
        }
        private void btnAuthReg_Click(object sender, RoutedEventArgs e)
        {
            Mainfrm.Navigate(new AuthPage());
        }
       
        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            Mainfrm.Navigate(new RegistrationPage());
        }
        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            if (Mainfrm.NavigationService.CanGoForward)
            {
                Mainfrm.NavigationService.GoForward();
            }
            else
            {
                MessageBox.Show("No page to go forward");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (Mainfrm.NavigationService.CanGoBack)
            {
                Mainfrm.NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("No page to go back");
            }
        }

        private void Mainfrm_Navigated(object sender, NavigationEventArgs e)
        {
            string page = e.Content.GetType().Name;
            Dictionary<string, Button> topMenu = new Dictionary<string, Button>
            {
                { "RegistrationPage", btnReg },
                { "ProductsPage", btnProd },
                { "CartPage", btnCart },
                { "AuthPage", btnAuthReg },
                { "ProfilePage", btnProfile }
            };
            foreach (var item in topMenu) item.Value.IsEnabled = true;
            if (topMenu.ContainsKey(page)) topMenu[page].IsEnabled = false;
        }
    }
}
