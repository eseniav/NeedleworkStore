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
using NeedleworkStore.AppData;

namespace NeedleworkStore.Pages
{  
    public partial class AuthPage : Page
    {
        MainWindow mainWindow = (MainWindow) Application.Current.MainWindow;
        public AuthPage()
        {
            InitializeComponent();
            txtLog.Focus();
            mainWindow.btnProd.IsEnabled = true;
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
                Users user = App.ctx.Users.FirstOrDefault(u => u.Login == txtLog.Text && u.Password == txtPass.Password);
                if (user != null)
                {
                    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.UserID = user.UserID;
                    this.NavigationService.Navigate(new ProductsPage());
                    mainWindow.SetMenuForRoles();
                    return;
                }
                if (App.ctx.Users.FirstOrDefault(u => u.Login == txtLog.Text) == null)
                {
                    MessageBox.Show("Пользователь не был найден", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (user == null)
                {
                    MessageBox.Show("Неверный пароль", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
