using Microsoft.Win32;
using NeedleworkStore.AppData;
using NeedleworkStore.Classes;
using NeedleworkStore.UCElements;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
        public ProductFilterViewModel FilterVM { get; set; } = new ProductFilterViewModel(false);
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
            FilterVM.AllProdTypes.Items.FirstOrDefault(d => d.Item.ProductTypeID == prod.ProductTypeID).IsChecked = true;
            FilterVM.AllDesigners.Items.FirstOrDefault(d => d.Item.DesignerID == prod.DesignerID).IsChecked = true;
            foreach (var item in FilterVM.AllProd.Items)
            {
                item.IsChecked = prod.ProductNeedleworkTypes.Select(n => n.NeedleworkTypeID).ToList().Contains(item.Item.NeedleworkTypeID);
            }
            foreach (var item in FilterVM.AllStitch.Items)
            {
                item.IsChecked = prod.ProductStitchTypes.Select(s => s.StitchTypeID).ToList().Contains(item.Item.StitchTypeID);
            }
            foreach (var item in FilterVM.AllAccessoryTypes.Items)
            {
                item.IsChecked = prod.ProductAccessoryTypes.Select(a => a.AccessoryTypeID).ToList().Contains(item.Item.AccessoryTypeID);
            }
            foreach (var item in FilterVM.AllThemes.Items)
            {
                item.IsChecked = prod.ProductThemes.Select(t => t.ThemeID).ToList().Contains(item.Item.ThemeID);
            }
            if (string.IsNullOrEmpty(prod.ProductImage))
            {
                imgPath = null;
                SetDefaultImg();
            } else
            {
                string basePath = "ProdImages";
                string appDir = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = System.IO.Path.Combine(appDir, "..", "..", basePath, prod.ProductImage);
                imgPath = System.IO.Path.GetFullPath(filePath);
                SetPreviewImage();
            }
            imgFullName = prod.ProductImage;
        }
        public string PageTitle => prod != null ? "Редактирование товара" : "Новый товар";
        public string SetTextToAddButton => prod.ProductImage == null ? "Добавить" : "Изменить";
        public bool SetEnableTopMenuButon() => mainWindow.btnAddProd.IsEnabled = prod != null;
        public AddProdPage(MyProducts selectedProduct = null)
        {
            InitializeComponent();
            mainWindow.btnProd.IsEnabled = true;
            DataContext = this;
            prod = selectedProduct;
            SetEnableTopMenuButon();
            if (prod != null)
            {
                SetProduct();
                return;
            }
        }
        public void SetDefaultImg()
        {
            imgAdd.Source = new BitmapImage(new Uri("/ResImages/NoPicture.png", UriKind.RelativeOrAbsolute));
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
            SetDefaultImg();
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
            if (prod != null)
            {
                Products p = App.ctx.Products.FirstOrDefault(c => c.ProductID == prod.ProductID);
                string[] props = new string[] {
                    "ProductName",
                    "ProductPrice",
                    "Description",
                    "QRLink",
                    "AvailabilityStatusID",
                    "ProductImage",
                    "DesignerID",
                    "ProductTypeID",
                };
                foreach (var propName in props)
                {
                    PropertyInfo prop = typeof(Products).GetProperty(propName);
                    var value = prop.GetValue(product);
                    prop.SetValue(p, value);
                }
                foreach (var oldItem in p.ProductNeedleworkTypes.ToList())
                {
                    App.ctx.ProductNeedleworkTypes.Remove(oldItem);
                }
                foreach (var oldItem in p.ProductStitchTypes.ToList())
                {
                    App.ctx.ProductStitchTypes.Remove(oldItem);
                }
                foreach (var oldItem in p.ProductThemes.ToList())
                {
                    App.ctx.ProductThemes.Remove(oldItem);
                }
                foreach (var oldItem in p.ProductAccessoryTypes.ToList())
                {
                    App.ctx.ProductAccessoryTypes.Remove(oldItem);
                }
                foreach (int id in selectedNWID)
                {
                    p.ProductNeedleworkTypes.Add(new ProductNeedleworkTypes
                    {
                        NeedleworkTypeID = id,
                        Products = p,
                    });
                }
                foreach (int id in selectedStitchID)
                {
                    p.ProductStitchTypes.Add(new ProductStitchTypes
                    {
                        StitchTypeID = id,
                        Products = p,
                    });
                }
                foreach (int id in selectedThemesID)
                {
                    p.ProductThemes.Add(new ProductThemes
                    {
                        ThemeID = id,
                        Products = p,
                    });
                }
                foreach (int id in selectedAccessoryTypesID)
                {
                    p.ProductAccessoryTypes.Add(new ProductAccessoryTypes
                    {
                        AccessoryTypeID = id,
                        Products = p,
                    });
                }
            }
            try
            {
                if(prod == null)
                    App.ctx.Products.Add(product);
                App.ctx.SaveChanges();
                MessageBox.Show(
                            "✅ Товар успешно сохранен!\n\n",
                            "Сохранение товара",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                        );
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
            if(imgFullName != null && imgFullName != prod?.ProductImage)
                SaveImg();
            if (prod == null)
                Clear();
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
        private void btnDelPicture_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult msgInf = MessageBox.Show
                        ("Удалить изображение?",
                        "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgInf != MessageBoxResult.Yes)
                return;
            imgFullName = null;
            imgAdd.Source = new BitmapImage(new Uri("/ResImages/NoPicture.png", UriKind.RelativeOrAbsolute));
        }
    }
}
