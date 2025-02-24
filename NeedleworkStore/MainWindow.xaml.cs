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

namespace NeedleworkStore
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Button btn = new Button();
            btn.Width = 100;
            btn.Height = 30;
            btn.Content = "3";
            LayOut.Children.Add(btn);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ok");
            MessageBox.Show("Hello");
        }

    }
}
