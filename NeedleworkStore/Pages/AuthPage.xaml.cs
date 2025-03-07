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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //определение запрашиваемого ресурса - страницы
                Uri res = new System.Uri("Pages/RegistrationPage.xaml", UriKind.Relative);
                //получение экземпляра навигационного сервиса
                NavigationService nav = NavigationService.GetNavigationService(this);
                //вызов метода навигации
                nav.Navigate(res);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
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
    }
}
