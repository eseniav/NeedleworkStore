using NeedleworkStore.Classes;
using NeedleworkStore.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private static string uid;
        public static string UserId
        {
            get { return uid; }
            set {
                uid = value;
                SetBtnVisibility(uid);
            }
        }
        public static SetBtnDelegate SetBtnVisibility;
        public delegate void SetBtnDelegate(string uid);
        public void SetButtonVisibility(string uid)
        {
            MessageBox.Show($"uid: {uid}");
            if (uid == null)
            {
                pageButtons["RegistrationPage"].Visibility = Visibility.Visible;
                pageButtons["AuthPage"].Visibility = Visibility.Visible;
                pageButtons["ProfilePage"].Visibility = Visibility.Collapsed;
                pageButtons["Exit"].Visibility = Visibility.Collapsed;
            }
            else
            {
                pageButtons["RegistrationPage"].Visibility = Visibility.Collapsed;
                pageButtons["AuthPage"].Visibility = Visibility.Collapsed;
                pageButtons["ProfilePage"].Visibility = Visibility.Visible;
                pageButtons["Exit"].Visibility = Visibility.Visible;
            }
        }
        public Dictionary<string, Button> pageButtons;
        public MainWindow()
        {
            InitializeComponent();
            pageButtons = new Dictionary<string, Button>{
                {"AuthPage", btnAuthReg},
                {"CartPage", btnCart},
                {"Exit", btnExit},
                {"ProductsPage", btnProd},
                {"ProfilePage", btnProfile},
                {"RegistrationPage", btnReg}
            };
            MainWindow.SetBtnVisibility = SetButtonVisibility;
            UserId = null;
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
        private void SetBtnState(object page)
        {
            string pg = page.GetType().Name;
            Debug.WriteLine(pg, "Navigated");
            Button[] menuButtons = new Button[]
            {
                    btnProfile,
                    btnReg,
                    btnExit,
                    btnCart,
                    btnAuthReg,
                    btnSearch
            };
            foreach (Button button in menuButtons) button.IsEnabled = true;
            foreach (var item in pageButtons) item.Value.IsEnabled = true;

            switch (pg)
            {
                case "AuthPage":
                default:
                    btnAuthReg.IsEnabled = false;
                    break;
                case "CartPage":
                    btnCart.IsEnabled = false;
                    break;
                case "ProductsPage":
                    btnProd.IsEnabled = false;
                    break;
                case "RegistrationPage":
                    btnReg.IsEnabled = false;
                    break;
            }
            pageButtons[pg].IsEnabled = false;
        }
        private void Mainfrm_Navigated(object sender, NavigationEventArgs e)
        {
            // Получение текущей страницы
            object currentPage = Mainfrm.Content;

            // Если страница загружена успешно
            if (currentPage != null) SetBtnState(currentPage);
        }
    }
}
