using NeedleworkStore.AppData;
using NeedleworkStore.Classes;
using NeedleworkStore.Pages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class ProductFilterViewModel : INotifyPropertyChanged
    {
        public void Reset()
        {
            AllProd.Reset();
            AllStitch.Reset();
            AllProdTypes.Reset();
            AllAccessoryTypes.Reset();
            AllDesigners.Reset();
            AllThemes.Reset();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ProductFilterViewModel()
        {
            AllProd = new AllTypeWrapper<NeedleworkTypes>(App.ctx.NeedleworkTypes.ToList());
            AllStitch = new AllTypeWrapper<StitchTypes>(App.ctx.StitchTypes.ToList());
            AllProd.Items.FirstOrDefault(c => c.Item.NeedleworkTypeID == 1).PropertyChanged += NeedleworkItem_PropertyChange;
            AllProdTypes = new AllTypeWrapper<ProductTypes>(App.ctx.ProductTypes.ToList());
            AllAccessoryTypes = new AllTypeWrapper<AccessoryTypes>(App.ctx.AccessoryTypes.ToList());
            AllProdTypes.Items.FirstOrDefault(c => c.Item.ProductTypeID == 2).PropertyChanged += ProductTypeItem_PropertyChange;
            AllDesigners = new AllTypeWrapper<Designers>(App.ctx.Designers.ToList());
            AllThemes = new AllTypeWrapper<Themes>(App.ctx.Themes.ToList());
        }
        public AllTypeWrapper<NeedleworkTypes> AllProd { get; set; }
        public AllTypeWrapper<StitchTypes> AllStitch { get; set; }
        public AllTypeWrapper<ProductTypes> AllProdTypes { get; set; }
        public AllTypeWrapper<AccessoryTypes> AllAccessoryTypes { get; set; }
        public AllTypeWrapper<Designers> AllDesigners { get; set; }
        public AllTypeWrapper<Themes> AllThemes { get; set; }
        public int? MinPrice { get; set; } = null;
        public int? MaxPrice { get; set; } = null;
        public bool IsValid { get; set; } = true;
        private void NeedleworkItem_PropertyChange(object sender, PropertyChangedEventArgs e)
        {
            ItemWrapper<NeedleworkTypes> itemWrapper = sender as ItemWrapper<NeedleworkTypes>;
            AllStitch.AllChecked = itemWrapper.IsChecked;
            OnPropertyChanged(nameof(IsStitchTabEnabled));
        }
        public bool IsStitchTabEnabled => AllProd.Items.FirstOrDefault(c => c.Item.NeedleworkTypeID == 1).IsChecked;
        private void ProductTypeItem_PropertyChange(object sender, PropertyChangedEventArgs e)
        {
            ItemWrapper<ProductTypes> itemWrapper = sender as ItemWrapper<ProductTypes>;
            AllAccessoryTypes.AllChecked = itemWrapper.IsChecked;
            OnPropertyChanged(nameof(IsAccessoryTabEnabled));
        }
        public bool IsAccessoryTabEnabled => AllProdTypes.Items.FirstOrDefault(c => c.Item.ProductTypeID == 2).IsChecked;
    }
    public class ItemWrapper<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public T Item { get; set; }
        public ItemWrapper(T item)
        {
            Item = item;
        }
        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }
    }
    public class AllTypeWrapper<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private List<ItemWrapper<T>> _items;
        public List<ItemWrapper<T>> Items
        {
            get => _items;
            set
            {
                _items = value;
                foreach (var item in _items)
                {
                    item.PropertyChanged += Item_PropertyChanged;
                }
            }
        }
        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ItemWrapper<T>.IsChecked))
                OnPropertyChanged(nameof(AllChecked));
        }
        public bool AllChecked
        {
            get => Items.All(x => x.IsChecked);
            set
            {
                Items.ForEach(f => f.IsChecked = value);
                OnPropertyChanged(nameof(AllChecked));
            }
        }
        public void Reset() => AllChecked = false;
        public List<int> GetIDs(Func<T, int> idSelector) => Items.Where(n => n.IsChecked).Select(k => idSelector(k.Item)).ToList();
        public AllTypeWrapper(List<T> items)
        {
            Items = items.Select(t => new ItemWrapper<T>(t)).ToList();
        }
    }
    /// <summary>
    /// Логика взаимодействия для SearchSidebarUC.xaml
    /// </summary>
    public partial class SearchSidebarUC : UserControl
    {
        public void SetTab()
        {
            TINeedle.IsSelected = true;
            TIProdTypes.IsSelected = true;
        }
        public SearchSidebarUC()
        {
            InitializeComponent();
        }
        public bool ContainsOnlyDigits(string input) =>  input.All(char.IsDigit);
        private void txtTo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
        private void txtFrom_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
        private void txtFrom_LostFocus(object sender, RoutedEventArgs e)
        {
            ProductFilterViewModel viewModel = DataContext as ProductFilterViewModel;
            if (!ContainsOnlyDigits(txtFrom.Text))
            {
                viewModel.IsValid = false;
                return;
            }
            if (string.IsNullOrEmpty(txtFrom.Text))
                viewModel.MinPrice = null;
            else
                viewModel.MinPrice = Int32.Parse(txtFrom.Text);
            viewModel.IsValid = true;
        }

        private void txtTo_LostFocus(object sender, RoutedEventArgs e)
        {
            ProductFilterViewModel viewModel = DataContext as ProductFilterViewModel;
            if (!ContainsOnlyDigits(txtTo.Text))
            {
                viewModel.IsValid = false;
                return;
            }
            if (string.IsNullOrEmpty(txtTo.Text))
                viewModel.MaxPrice = null;
            else
                viewModel.MaxPrice = Int32.Parse(txtTo.Text);
            viewModel.IsValid = true;
        }
    }
}
