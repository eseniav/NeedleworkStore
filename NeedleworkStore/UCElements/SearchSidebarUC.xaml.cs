using NeedleworkStore.AppData;
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
        List<NeedleworkTypes> nwTypes;
        List<StitchTypes> stitchTypes;
        List<ProductTypes> productTypes;
        List<AccessoryTypes> accessoryTypes;
        List<Designers> designers;
        List<Themes> themes;
        List<Countries> countries;
        public SearchSidebarUC()
        {
            InitializeComponent();
            nwTypes = new List<NeedleworkTypes>(App.ctx.NeedleworkTypes.ToList());
            lbxNedleTypes.ItemsSource = nwTypes;
            lbxNedleTypes.DataContext = nwTypes;
            stitchTypes = new List<StitchTypes>(App.ctx.StitchTypes.ToList());
            lbxStitchTypes.ItemsSource = stitchTypes;
            lbxStitchTypes.DataContext = stitchTypes;
            productTypes = new List<ProductTypes>(App.ctx.ProductTypes.ToList());
            lbxProdTypes.ItemsSource = productTypes;
            lbxProdTypes.DataContext = productTypes;
            accessoryTypes = new List<AccessoryTypes>(App.ctx.AccessoryTypes.ToList());
            lbxAccess.ItemsSource = accessoryTypes;
            lbxAccess.DataContext = accessoryTypes;
            designers = new List<Designers>(App.ctx.Designers.ToList());
            lbxDesigners.ItemsSource = designers;
            lbxDesigners.DataContext = designers;
            themes = new List<Themes>(App.ctx.Themes.ToList());
            lbxThemes.ItemsSource = themes;
            lbxThemes.DataContext = themes;
            countries = new List<Countries>(App.ctx.Countries.ToList());
            lbxCountries.ItemsSource = countries;
            lbxCountries.DataContext = countries;
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
