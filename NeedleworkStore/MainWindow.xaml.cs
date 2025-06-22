using NeedleworkStore.AppData;
using NeedleworkStore.Classes;
using NeedleworkStore.Pages;
using NeedleworkStore.UCElements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static Frame frame;
        private int? _userID;
        public int RoleID;
        private string searchTxt;
        public string SearchTxt
        {
            get => searchTxt;
            set
            {
                if (value == searchTxt)
                    return;
                searchTxt = value;
                OnPropertyChanged(nameof(SearchTxt));
                OnPropertyChanged(nameof(IsSearchText));
            }
        }
        public bool IsSearchText => !string.IsNullOrEmpty(SearchTxt);
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
            DataContext = this;
            txtSearch.KeyDown += txtSearch_KeyDown;
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
            var us = App.ctx.Users.FirstOrDefault(u => u.UserID == UserID);
            if (us == null)
                return;
            txtblUserName.Text = !string.IsNullOrEmpty(us.UserName) ? $"{us.UserName}!" : $"{us.Login}!";
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
            Mainfrm.Navigate(new AboutShopPage());
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
            Mainfrm.Navigate(new ProductsPage(SearchTxt));
        }
        private void btnProd_Click(object sender, RoutedEventArgs e)
        {
            Mainfrm.Navigate(new ProductsPage());
        }
        private void txtSearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
        private void btnAuthReg_Click(object sender, RoutedEventArgs e)
        {
            Mainfrm.Navigate(new AuthPage());
        }
       
        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            Mainfrm.Navigate(new RegistrationPage());
        }
        private void Mainfrm_Navigated(object sender, NavigationEventArgs e)
        {
            string page = e.Content.GetType().Name;
            Dictionary<string, Button> topMenu = new Dictionary<string, Button>
            {
                { "RegistrationPage", btnReg },
                { "CartPage", btnCart },
                { "AuthPage", btnAuthReg },
                { "ProfilePage", btnProfile },
                { "OrdersPage", btnOrders },
                { "AboutShopPage", btnShop },
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

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            SearchTxt = string.Empty;
            txtSearch.Focus();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSearch.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                e.Handled = true;
            }
        }
        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
