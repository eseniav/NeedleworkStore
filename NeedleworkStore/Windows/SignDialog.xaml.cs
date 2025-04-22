using NeedleworkStore.Pages;
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
using System.Windows.Shapes;

namespace NeedleworkStore.Windows
{
    /// <summary>
    /// Interaction logic for SignDialog.xaml
    /// </summary>
    public partial class SignDialog : Window
    {
        public SignDialog()
        {
            InitializeComponent();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.frame.Navigate(new AuthPage());
            this.Close();
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.frame.Navigate(new RegistrationPage());
            this.Close();
        }
    }
}
