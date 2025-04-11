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
        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {           
            if (!CheckValidation.CheckEmptyNull(txtLog.Text) || !CheckValidation.CheckEmptyNull(txtPass.Password))
            {
                MessageBox.Show("Все поля должны быть заполнены!");
                return;
            }
            MainWindow.UserId = "123";
            this.NavigationService.Navigate(new ProductsPage());
        }
    }
}
