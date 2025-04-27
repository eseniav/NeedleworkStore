using NeedleworkStore.Classes;
using NeedleworkStore.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private List<ToggleButton> prodTypesCtl;

        public SearchSidebarUC()
        {
            InitializeComponent();

            prodTypesCtl = ProductTypes.Children.OfType<ToggleButton>().ToList();
        }

        private void toggleType(ToggleButton ctl)
        {
            foreach (var control in prodTypesCtl) control.IsChecked = false;
            ctl.IsChecked = true;
        }

        private void btnAdvSearch_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.frame.Navigate(new AdvancedSearchPage());
        }

        private void txtTo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void txtFrom_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void lbxDesigners_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("Переход на страницу с отфильтрованными товарами по каждому производителю");
        }

        private void lbxThemes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("Переход на страницу с отфильтрованными товарами по каждой теме");
        }

        private void lbxTechnic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("Переход на страницу с отфильтрованными товарами по каждой технике");
        }
        private void toggleTypeClick(object sender, RoutedEventArgs e)
        {
            toggleType((ToggleButton)e.OriginalSource);
        }
        private void btnStitch_Click(object sender, RoutedEventArgs e)
        {
            //toggleType((ToggleButton)sender);
        }

        private void btnSew_Click(object sender, RoutedEventArgs e)
        {
            //toggleType((ToggleButton)sender);
        }

        private void btnAccess_Click(object sender, RoutedEventArgs e)
        {
            //toggleType((ToggleButton)sender);
        }

        private void btnKits_Click(object sender, RoutedEventArgs e)
        {
            //toggleType((ToggleButton)sender);
        }

        private void btnPrice_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckValidation.CheckEmptyNull(txtFrom.Text) && !CheckValidation.CheckEmptyNull(txtTo.Text))
            {
                MessageBox.Show("Хотя бы одно поле должно быть заполнено!");
                return;
            }
            if ((CheckValidation.CheckInt(txtFrom.Text) || !CheckValidation.CheckEmptyNull(txtFrom.Text)) &&
                (CheckValidation.CheckInt(txtTo.Text) || !CheckValidation.CheckEmptyNull(txtTo.Text)))
            {
                MessageBox.Show("Переход на страницу с отфильтрованными товарами по диапазону цены");
                return;
            }
            else
            {
                MessageBox.Show("Введены неверные данные!");
            }
        }

    }
}
