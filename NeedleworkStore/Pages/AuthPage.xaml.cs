using NeedleworkStore.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.ComTypes;
using System.Linq;

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
            try
            {
                if (!CheckValidation.CheckEmptyNull(txtLog.Text) || !CheckValidation.CheckEmptyNull(txtPass.Password))
                {
                    MessageBox.Show("Все поля должны быть заполнены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (App.ctx.Users.FirstOrDefault(u => u.Login == txtLog.Text) == null)
                {
                    MessageBox.Show("Пользователь не был найден", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (App.ctx.Users.FirstOrDefault(u => u.Password == txtPass.Password) == null)
                {
                    MessageBox.Show("Неверный пароль", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                this.NavigationService.Navigate(new ProductsPage());
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
