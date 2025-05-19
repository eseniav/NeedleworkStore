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
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ProductFilterViewModel()
        {
            AllProd = new NeedleworkAllTypeWrapper(App.ctx.NeedleworkTypes.ToList());
            AllStitch = new StitchAllTypeWrapper(App.ctx.StitchTypes.ToList());
            AllProd.Items.FirstOrDefault(c => c.Item.NeedleworkTypeID == 1).PropertyChanged += NeedleworkItem_PropertyChange;
        }
        public NeedleworkAllTypeWrapper AllProd { get; set; }
        public StitchAllTypeWrapper AllStitch { get; set; }
        private void NeedleworkItem_PropertyChange(object sender, PropertyChangedEventArgs e)
        {
            NeedleworkTypeWrapper itemWrapper = sender as NeedleworkTypeWrapper;
            AllStitch.AllChecked = itemWrapper.IsChecked;
            OnPropertyChanged(nameof(IsStitchTabEnabled));
        }
        public bool IsStitchTabEnabled => AllProd.Items.FirstOrDefault(c => c.Item.NeedleworkTypeID == 1).IsChecked;
    }
    public class NeedleworkTypeWrapper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public NeedleworkTypes Item { get; set; }
        public NeedleworkTypeWrapper(NeedleworkTypes item)
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
    public class NeedleworkAllTypeWrapper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private List<NeedleworkTypeWrapper> _items;
        public List<NeedleworkTypeWrapper> Items
        {
            get => _items;
            set
            {
                _items = value;
                foreach (NeedleworkTypeWrapper item in _items)
                {
                    item.PropertyChanged += Item_PropertyChanged;
                }
            }
        }
        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NeedleworkTypeWrapper.IsChecked))
            {
                OnPropertyChanged(nameof(AllChecked));
                OnPropertyChanged(nameof(CheckedIDs));
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
        public List<int> CheckedIDs => Items.Where(n => n.IsChecked).Select(k => k.Item.NeedleworkTypeID).ToList();
        public NeedleworkAllTypeWrapper(List<NeedleworkTypes> items)
        {
            Items = items.Select(t => new NeedleworkTypeWrapper(t)).ToList();
        }
    }
    public class StitchTypeWrapper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public StitchTypes Item { get; set; }
        public StitchTypeWrapper(StitchTypes item)
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
    public class StitchAllTypeWrapper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private List<StitchTypeWrapper> _items;
        public List<StitchTypeWrapper> Items
        {
            get => _items;
            set
            {
                _items = value;
                foreach (StitchTypeWrapper item in _items)
                {
                    item.PropertyChanged += Item_PropertyChanged;
                }
            }
        }
        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(StitchTypeWrapper.IsChecked))
            {
                OnPropertyChanged(nameof(AllChecked));
                OnPropertyChanged(nameof(CheckedIDs));
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
        public List<int> CheckedIDs => Items.Where(n => n.IsChecked).Select(k => k.Item.StitchTypeID).ToList();
        public StitchAllTypeWrapper(List<StitchTypes> items)
        {
            Items = items.Select(t => new StitchTypeWrapper(t)).ToList();
        }
    }
    /// <summary>
    /// Логика взаимодействия для SearchSidebarUC.xaml
    /// </summary>
    public partial class SearchSidebarUC : UserControl
    {
        List<ProductTypes> productTypes;
        List<AccessoryTypes> accessoryTypes;
        List<Designers> designers;
        List<Themes> themes;
        public void SetTab()
        {
            TINeedle.IsSelected = true;
        }
        private void GetDataToList()
        {
            productTypes = App.ctx.ProductTypes.ToList();
            accessoryTypes = App.ctx.AccessoryTypes.ToList();
            designers = App.ctx.Designers.ToList();
            themes = App.ctx.Themes.ToList();
        }
        private void SetItemSource()
        {
            Dictionary<ListBox, IEnumerable> lbxDbData = new Dictionary<ListBox, IEnumerable>
            {
                { lbxProdTypes, productTypes },
                { lbxAccess, accessoryTypes },
                { lbxDesigners, designers },
                { lbxThemes, themes },
            };
            foreach (var item in lbxDbData)
            {
                item.Key.ItemsSource = item.Value;
                item.Key.DataContext = item.Value;
            }
        }
        public SearchSidebarUC()
        {
            InitializeComponent();
            GetDataToList();
            SetItemSource();
        }

        private void txtTo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void txtFrom_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
    }
}
