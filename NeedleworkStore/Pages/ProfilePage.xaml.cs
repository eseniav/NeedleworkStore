using NeedleworkStore.AppData;
using NeedleworkStore.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Application = System.Windows.Application;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace NeedleworkStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        Users user;
        private void SetProfileValues()
        {
            Dictionary<string, string> profileData = new Dictionary<string, string>
            {
                { "txtLog", user.Login },
                { "txtEmail", user.UserEmail },
                { "txtLastname", user.UserLastname },
                { "txtFirstname", user.UserName },
                { "txtPatr", user.UserPatronymic },
                { "txtBirthDate", user.Birthday?.ToString("dd.MM.yyyy") },
                { "txtPhone", user.UserPhone },
            };
            string defaultText = "Данные не указаны";
            SolidColorBrush defaultColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#852614"));
            foreach (var item in profileData)
            {
                TextBlock textBlock = (TextBlock)FindName(item.Key);
                textBlock.Text = string.IsNullOrEmpty(item.Value) ? defaultText : item.Value;
                textBlock.Foreground = string.IsNullOrEmpty(item.Value) ? defaultColor : textBlock.Foreground;
            }
        }
        public ProfilePage()
        {
            InitializeComponent();
            user = App.ctx.Users.FirstOrDefault(u => u.UserID == mainWindow.UserID);
            SetProfileValues();
        }
        private void btnFav_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new FavorPage());
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new OrdersPage());
        }

        private void btnRedact_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу Редактирование профиля");
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new OneProductPage());
        }
        private void chbShowPass_Click(object sender, RoutedEventArgs e)
        {
            txtPass.Text = (bool)chbShowPass.IsChecked ? user.Password : "********";
        }
    }
}
