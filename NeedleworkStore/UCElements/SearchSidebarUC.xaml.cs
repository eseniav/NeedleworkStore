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
            MinPrice = null;
            MaxPrice = null;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool IsAllSelect { get; set; }
        public ProductFilterViewModel(bool isMultiselect = true)
        {
            IsAllSelect = isMultiselect;
            AllProd = new AllTypeWrapper<NeedleworkTypes>(App.ctx.NeedleworkTypes.ToList());
            AllStitch = new AllTypeWrapper<StitchTypes>(App.ctx.StitchTypes.ToList());
            AllProd.Items.FirstOrDefault(c => c.Item.NeedleworkTypeID == 1).PropertyChanged += NeedleworkItem_PropertyChange;
            AllProdTypes = new AllTypeWrapper<ProductTypes>(App.ctx.ProductTypes.ToList(), isMultiselect);
            AllAccessoryTypes = new AllTypeWrapper<AccessoryTypes>(App.ctx.AccessoryTypes.ToList());
            AllProdTypes.Items.FirstOrDefault(c => c.Item.ProductTypeID == 2).PropertyChanged += ProductTypeItem_PropertyChange;
            AllDesigners = new AllTypeWrapper<Designers>(App.ctx.Designers.ToList(), isMultiselect);
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
            OnPropertyChanged(nameof(IsStitchTabEnabled));
            if (!IsAllSelect)
                return;
            ItemWrapper<NeedleworkTypes> itemWrapper = sender as ItemWrapper<NeedleworkTypes>;
            AllStitch.AllChecked = itemWrapper.IsChecked;
        }
        public bool IsStitchTabEnabled => AllProd.Items.FirstOrDefault(c => c.Item.NeedleworkTypeID == 1).IsChecked;
        private void ProductTypeItem_PropertyChange(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(IsAccessoryTabEnabled));
            if (!IsAllSelect)
                return;
            ItemWrapper<ProductTypes> itemWrapper = sender as ItemWrapper<ProductTypes>;
            AllAccessoryTypes.AllChecked = itemWrapper.IsChecked;
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
            {
                var changedItem = sender as ItemWrapper<T>;
                if(!IsMultiselect && changedItem.IsChecked)
                {
                    foreach (var item in Items)
                    {
                        if (!ReferenceEquals(item, changedItem))
                            item.IsChecked = false;
                    }
                }
                OnPropertyChanged(nameof(AllChecked));
            }
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
        public AllTypeWrapper(List<T> items, bool isMultiselect = true)
        {
            IsMultiselect = isMultiselect;
            Items = items.Select(t => new ItemWrapper<T>(t)).ToList();
        }
        public bool IsMultiselect { get; set; }
    }
    /// <summary>
    /// Логика взаимодействия для SearchSidebarUC.xaml
    /// </summary>
    public partial class SearchSidebarUC : UserControl
    {
        public bool IsPriceVisible
        {
            get => (bool)GetValue(IsPriceVisibleProperty);
            set => SetValue(IsPriceVisibleProperty, value);
        }
        public static readonly DependencyProperty IsPriceVisibleProperty =
            DependencyProperty.Register("IsPriceVisible", typeof(bool), typeof(SearchSidebarUC), new PropertyMetadata(false));
        public bool IsMultiselect
        {
            get => (bool)GetValue(IsMultiselectProperty);
            set => SetValue(IsMultiselectProperty, value);
        }
        public static readonly DependencyProperty IsMultiselectProperty =
            DependencyProperty.Register("IsMultiselect", typeof(bool), typeof(SearchSidebarUC), new PropertyMetadata(true));
        public void ClearInputs()
        {
            txtTo.Clear();
            txtFrom.Clear();
        }
        public void SetTab()
        {
            TINeedle.IsSelected = true;
            TIProdTypes.IsSelected = true;
        }
        public void Reset()
        {
            SetTab();
            ClearInputs();
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
