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
        List<UIElement> topMenu;
        List<UIElement> topMenuUser;
        public int? UserID {
            get => _userID;
            set {
                _userID = value;

                if(value == null)
                {
                    topMenu.ForEach(el => el.Visibility = Visibility.Collapsed);
                    topMenuUser.ForEach(el => el.Visibility = Visibility.Visible);
                } else
                {
                    topMenu.ForEach(el => el.Visibility = Visibility.Visible);
                    topMenuUser.ForEach(el => el.Visibility = Visibility.Collapsed);
                }
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
            frame = Mainfrm;
            UserID = null;
            if (UserID != null)
            {
                //List<Carts> carts = new List<Carts>();
                //carts.ForEach(c => c.CartID).Where(c => c.UserID == UserID);
                //int quantityInCart = App.ctx.Carts.Count(c => c.UserID == UserID) * carts.QuantityCart;
            }
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
