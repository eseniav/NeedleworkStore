using NeedleworkStore.AppData;
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

namespace NeedleworkStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }
        
        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            Users newUser = new Users
            {
                UserLastname = txtBLastName.Text,
                UserName = txtBFirstName.Text,
                UserPatronymic = txtBPatr.Text,
                Login = txtBLogin.Text,
                Password = psxB.Password,
                UserPhone = txtBPhone.Text,
                UserEmail = txtBEmail.Text,
                Birthday = dtPBirth.SelectedDate,
                RoleID = 2
            };
            App.ctx.Users.Add(newUser);
            App.ctx.SaveChanges();
            this.NavigationService.Navigate(new ProductsPage());
        }
    }
}
