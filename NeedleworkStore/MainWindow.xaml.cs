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
        List<UIElement> topMenu;
        List<UIElement> topMenuUser;
        List<UIElement> topMenuAdmin;
        List<UIElement> topMenuAdminUnv;
        public int? UserID {
            get => _userID;
            set {
                _userID = value;
                RoleID = App.ctx.Users.FirstOrDefault(u => u.UserID == _userID)?.RoleID ?? 3;
                if (value == null)
                {
                    topMenu.ForEach(el => el.Visibility = Visibility.Collapsed);
                    topMenuUser.ForEach(el => el.Visibility = Visibility.Visible);
                    btnAddProd.Visibility = Visibility.Collapsed;
                    btnOrders.Visibility = Visibility.Collapsed;
                } else
                {
                    if(RoleID == 2)
                    {
                        topMenu.ForEach(el => el.Visibility = Visibility.Visible);
                        topMenuUser.ForEach(el => el.Visibility = Visibility.Collapsed);
                    }
                    if (RoleID == 1)
                    {
                        topMenuAdmin.ForEach(el => el.Visibility = Visibility.Visible);
                        topMenuAdminUnv.ForEach(el => el.Visibility = Visibility.Collapsed);
                    }
                }
                UpdateCartState();
                SetUserGreeting();
            }
        }
        public bool IsAuthenticated => UserID != null;
        public MainWindow()
        {
            InitializeComponent();
            topMenu = new List<UIElement> {
                    btnProfile,
                    btnExit,
                    txtBlQuanProd,
                    txtBlQuan,
                    txtBlGreeting,
                    txtblUserName,
                };
            topMenuUser = new List<UIElement>
                {
                    btnReg,
                    btnAuthReg,
                };
            topMenuAdmin = new List<UIElement>
            {
                btnAddProd,
                btnProd,
                btnOrders,
                btnProfile,
                btnExit
            };
            topMenuAdminUnv = new List<UIElement>
            {
                btnShop,
                btnCart,
                btnReg,
                btnAuthReg,
            };
            frame = Mainfrm;
            UserID = null;
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
            //MessageBox.Show("Переход на страницу О магазине");
            Mainfrm.Navigate(new AddProdPage());
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
    }
}
