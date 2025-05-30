using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для AboutShopPage.xaml
    /// </summary>
    public partial class AboutShopPage : Page
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public AboutShopPage()
        {
            InitializeComponent();
            mainWindow.btnProd.IsEnabled = true;
        }
    }
}
