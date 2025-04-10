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

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {           
            if (!CheckValidation.CheckEmptyNull(txtLog.Text) || !CheckValidation.CheckEmptyNull(txtPass.Password))
            {
                MessageBox.Show("Все поля должны быть заполнены!");
                return;
            }
            this.NavigationService.Navigate(new ProductsPage());
        }
    }
}
