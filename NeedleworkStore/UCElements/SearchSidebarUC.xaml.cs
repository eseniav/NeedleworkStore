using NeedleworkStore.Classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NeedleworkStore.UCElements
{
    /// <summary>
    /// Логика взаимодействия для SearchSidebarUC.xaml
    /// </summary>
    public partial class SearchSidebarUC : UserControl
    {
        public SearchSidebarUC()
        {
            InitializeComponent();
        }

        private void txtTo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void txtFrom_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
        private void SetFilter()
        {
            MessageBox.Show("Установить фильтр");
        }
        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            SetFilter();
        }
        private void ResetFilter()
        {
            MessageBox.Show("Сбросить все фильтры");
        }
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetFilter();
        }
    }
}
