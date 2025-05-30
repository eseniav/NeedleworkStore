using Microsoft.Win32;
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
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        private string imgPath = null;
        string imgFullName = null;
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
            foreach (var item in FilterVM.AllProd.Items)
            {
                item.IsChecked = prod.ProductNeedleworkTypes.Select(n => n.NeedleworkTypeID).ToList().Contains(item.Item.NeedleworkTypeID);
            }
            foreach (var item in FilterVM.AllStitch.Items)
            {
                item.IsChecked = prod.ProductStitchTypes.Select(s => s.StitchTypeID).ToList().Contains(item.Item.StitchTypeID);
            }
            foreach (var item in FilterVM.AllProdTypes.Items)
            {
                item.IsChecked = prod.ProductNeedleworkTypes.Select(s => s.NeedleworkTypeID).ToList().Contains(item.Item.ProductTypeID);
            }
            foreach (var item in FilterVM.AllAccessoryTypes.Items)
            {
                item.IsChecked = prod.ProductAccessoryTypes.Select(a => a.AccessoryTypeID).ToList().Contains(item.Item.AccessoryTypeID);
            }
            FilterVM.AllDesigners.Items.FirstOrDefault(d => d.Item.DesignerID == prod.DesignerID).IsChecked = true;
            foreach (var item in FilterVM.AllThemes.Items)
            {
                item.IsChecked = prod.ProductThemes.Select(t => t.ThemeID).ToList().Contains(item.Item.ThemeID);
            }
        }
        public AddProdPage(MyProducts selectedProduct = null)
        {
            InitializeComponent();
            mainWindow.btnProd.IsEnabled = true;
            DataContext = this;
            prod = selectedProduct;
            if (prod != null)
            {
                SetProduct();
                return;
            }
        }
        public bool CheckValid()
        {
            return !string.IsNullOrEmpty(txtAddNameProd.Text) &&
                !string.IsNullOrEmpty(txtbAdddescriptionProd.Text) &&
                !string.IsNullOrEmpty(txtbAddPriceProd.Text) &&
                txtbAddPriceProd.Text.All(char.IsDigit) &&
                FilterVM.AllDesigners.Items.Any(x => x.IsChecked) &&
                FilterVM.AllProdTypes.Items.Any(x => x.IsChecked) &&
                cmbAvail.SelectedIndex != -1;
        }
        public void Clear()
        {
            txtAddNameProd.Clear();
            txtbAddPriceProd.Clear();
            txtbAdddescriptionProd.Clear();
            txtbQR.Clear();
            cmbAvail.SelectedIndex = -1;
            FilterVM.Reset();
            imgFullName = null;
            imgAdd.Source = new BitmapImage(new Uri("/ResImages/NoPicture.png", UriKind.RelativeOrAbsolute));
        }
        public void SaveProd()
        {
            Products product = new Products
            {
                ProductName = txtAddNameProd.Text,
                ProductPrice = Int32.Parse(txtbAddPriceProd.Text),
                Description = txtbAdddescriptionProd.Text,
                QRLink = txtbQR.Text,
                AvailabilityStatusID = cmbAvail.SelectedIndex == 0 ? 1 : 2,
                ProductImage = imgFullName,
                DesignerID = FilterVM.AllDesigners.GetIDs(n => n.DesignerID).FirstOrDefault(),
                ProductTypeID = FilterVM.AllProdTypes.GetIDs(n => n.ProductTypeID).FirstOrDefault(),
            };
            List<int> selectedNWID = FilterVM.AllProd.GetIDs(n => n.NeedleworkTypeID);
            foreach(int id in selectedNWID)
            {
                product.ProductNeedleworkTypes.Add(new ProductNeedleworkTypes
                {
                    NeedleworkTypeID = id,
                    Products = product,
                });
            }
            List<int> selectedStitchID = FilterVM.AllStitch.GetIDs(n => n.StitchTypeID);
            foreach (int id in selectedStitchID)
            {
                product.ProductStitchTypes.Add(new ProductStitchTypes
                {
                    StitchTypeID = id,
                    Products = product,
                });
            }
            List<int> selectedAccessoryTypesID = FilterVM.AllAccessoryTypes.GetIDs(n => n.AccessoryTypeID);
            foreach (int id in selectedAccessoryTypesID)
            {
                product.ProductAccessoryTypes.Add(new ProductAccessoryTypes
                {
                    AccessoryTypeID = id,
                    Products = product,
                });
            }
            List<int> selectedThemesID = FilterVM.AllThemes.GetIDs(n => n.ThemeID);
            foreach (int id in selectedThemesID)
            {
                product.ProductThemes.Add(new ProductThemes
                {
                    ThemeID = id,
                    Products = product,
                });
            }
            try
            {
                App.ctx.Products.Add(product);
                App.ctx.SaveChanges();
                MessageBox.Show(
                            "✅ Товар успешно добавлен!\n\n",
                            "Добавление товара",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                        );
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка базы данных");
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckValid())
            {
                MessageBox.Show(
                            "Проверьте данные в обязательных полях!\n\n",
                            "Предупреждение",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                return;
            }
            SaveProd();
            if(imgFullName != null)
                SaveImg();
        }
        public void SetPreviewImage()
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(imgPath);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            imgAdd.Source = image;
        }
        public void SaveImg()
        {
            string basePath = "ProdImages";
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = System.IO.Path.Combine(appDir, "..", "..");
            string fullPath = System.IO.Path.GetFullPath(projectRoot);
            string destFolder = System.IO.Path.Combine(fullPath, basePath);
            string targetPath = System.IO.Path.Combine(destFolder, imgFullName);
            System.IO.File.Copy(imgPath, targetPath);
        }
        private void btnAddPicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выберите изображение";
            ofd.Filter = "Изображения (*.jpeg;*.png;*.gif;*jpg)|*.jpeg;*.png;*.gif;*jpg";
            if (ofd.ShowDialog() != true)
                return;
            imgPath = ofd.FileName;
            imgFullName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(imgPath).ToLower();
            SetPreviewImage();
        }
    }
}
