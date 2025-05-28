using NeedleworkStore.AppData;
using NeedleworkStore.UCElements;
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
using static NeedleworkStore.Pages.ProductsPage;

namespace NeedleworkStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddProdPage.xaml
    /// </summary>
    public partial class AddProdPage : Page
    {
        Products prod;
        public ProductFilterViewModel FilterVM { get; set; } = new ProductFilterViewModel();
        public void SetProduct()
        {
            txtAddNameProd.Text = prod.ProductName;
            txtbAddPriceProd.Text = prod.ProductPrice.ToString();
            txtbAdddescriptionProd.Text = prod.Description;
            txtbQR.Text = prod.QRLink;
            if(prod.AvailabilityStatusID == 1)
                cmbAvailYes.IsSelected = true;
            else
                cmbAvailNo.IsSelected = true;
        }
        public AddProdPage(MyProducts selectedProduct = null)
        {
            InitializeComponent();
            DataContext = this;
            prod = selectedProduct;
            if (prod != null)
            {
                SetProduct();
                return;
            }
        }
    }
}
