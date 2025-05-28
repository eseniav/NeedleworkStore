using NeedleworkStore.AppData;
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
        public int RoleID;
        public int? UserID {
            get => _userID;
            set {
                _userID = value;
                UpdateCartState();
                SetUserGreeting();
            }
        }
        public void SetMenuForRoles()
        {
            RoleID = App.ctx.Users.FirstOrDefault(u => u.UserID == _userID)?.RoleID ?? 3;
            if (RoleID == 2)
            {
                btnAddProd.Visibility = Visibility.Collapsed;
                btnShop.Visibility = Visibility.Visible;
                btnOrders.Visibility = Visibility.Collapsed;
                btnCart.Visibility = Visibility.Visible;
                btnProfile.Visibility = Visibility.Visible;
                btnReg.Visibility = Visibility.Collapsed;
                btnExit.Visibility = Visibility.Visible;
                btnAuthReg.Visibility = Visibility.Collapsed;
                txtBlGreeting.Visibility = Visibility.Visible;
                txtBlQuanProd.Visibility = Visibility.Visible;
                txtBlQuan.Visibility = Visibility.Visible;
            }
            if (RoleID == 1)
            {
                btnAddProd.Visibility = Visibility.Visible;
                btnShop.Visibility = Visibility.Collapsed;
                btnOrders.Visibility = Visibility.Visible;
                btnCart.Visibility = Visibility.Collapsed;
                btnProfile.Visibility = Visibility.Visible;
                btnReg.Visibility = Visibility.Collapsed;
                btnExit.Visibility = Visibility.Visible;
                btnAuthReg.Visibility = Visibility.Collapsed;
                txtBlQuanProd.Visibility = Visibility.Collapsed;
                txtBlQuan.Visibility = Visibility.Collapsed;
                txtBlGreeting.Visibility = Visibility.Visible;
            }
            if (RoleID == 3)
            {
                btnAddProd.Visibility = Visibility.Collapsed;
                btnShop.Visibility = Visibility.Visible;
                btnOrders.Visibility = Visibility.Collapsed;
                btnCart.Visibility = Visibility.Visible;
                btnProfile.Visibility = Visibility.Collapsed;
                btnReg.Visibility = Visibility.Visible;
                btnExit.Visibility = Visibility.Collapsed;
                btnAuthReg.Visibility = Visibility.Visible;
                txtBlQuanProd.Visibility = Visibility.Collapsed;
                txtBlQuan.Visibility = Visibility.Collapsed;
                txtBlGreeting.Visibility = Visibility.Collapsed;
            }
        }
        public bool IsAuthenticated => UserID != null;
        public MainWindow()
        {
            InitializeComponent();
            frame = Mainfrm;
            UserID = null;
            SetMenuForRoles();
        }
        public void UpdateCartState()
        {
            if (!IsAuthenticated)
                return;
            txtBlQuan.Text = App.ctx.Carts.FirstOrDefault(c => c.UserID == UserID) != null
                ? App.ctx.Carts.Where(c => c.UserID == UserID).Sum(c => c.QuantityCart).ToString()
                : "0";
        }
        public void SetUserGreeting()
        {
            if (!IsAuthenticated)
                return;

            if (App.ctx.Users.FirstOrDefault(u => u.UserID == UserID).UserName != null)
                txtblUserName.Text = App.ctx.Users.FirstOrDefault(u => u.UserID == UserID).UserName + "!";
            else
                txtblUserName.Text = App.ctx.Users.FirstOrDefault(u => u.UserID == UserID).Login + "!";
        }
        private void btnCart_Click(object sender, RoutedEventArgs e)
        {
            if (UserID == null)
            {
                MessageBoxResult msgInf = MessageBox.Show
                    ("Доступно только для зарегистрированных пользователей. Зарегистрироваться сейчас?",
                    "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (msgInf == MessageBoxResult.Yes)
                    Mainfrm.Navigate(new RegistrationPage());
                return;
            }
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
            SetMenuForRoles();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if(!CheckValidation.CheckEmptyNull(txtSearch.Text))
            {
                MessageBox.Show("Заполните поле!");
                return;
            }
            Mainfrm.Navigate(new ProductsPage(txtSearch.Text));
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
                UpdateCartState();
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
                UpdateCartState();
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

        private void btnAddProd_Click(object sender, RoutedEventArgs e)
        {
            Mainfrm.Navigate(new AddProdPage());
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            Mainfrm.Navigate(new OrdersPage());
        }
    }
}
