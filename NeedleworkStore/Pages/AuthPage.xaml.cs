using NeedleworkStore.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NeedleworkStore.Pages
{  
    public partial class AuthPage : Page
    {       
        public AuthPage()
        {
            InitializeComponent();
            txtLog.Focus();            
        } 

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {            
            //определение запрашиваемого ресурса - страницы
            Uri res = new System.Uri("Pages/RegistrationPage.xaml", UriKind.Relative);
            //получение экземпляра навигационного сервиса
            NavigationService nav = NavigationService.GetNavigationService(this);
            //вызов метода навигации
            nav.Navigate(res);      
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            if(this.NavigationService.CanGoForward)
            {
                this.NavigationService.GoForward();
            } else
            {
                MessageBox.Show("No page to go forward");
            }            
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("No page to go back");
            }

        }
        private void btnShop_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу О магазине");
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {           
            if (!CheckValidation.CheckEmptyNull(txtLog.Text) || !CheckValidation.CheckEmptyNull(txtPass.Password))
            {
                MessageBox.Show("Все поля должны быть заполнены!");
                return;
            }
            this.NavigationService.Navigate(new ProductsPage());
        }

        private void btnGuest_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу с товарами для гостя");
        }
    }
}
