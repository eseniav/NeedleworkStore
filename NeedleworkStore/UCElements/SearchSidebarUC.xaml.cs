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
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ProductFilterViewModel()
        {
            nwTypes = App.ctx.NeedleworkTypes.ToList().Select(t => new NeedleworkTypeWrapper(t)).ToList();
        }
        private List<NeedleworkTypeWrapper> _nw;
        public List<NeedleworkTypeWrapper> nwTypes
        {
            get => _nw;
            set
            {
                _nw = value;
                foreach (NeedleworkTypeWrapper item in _nw)
                {
                    item.PropertyChanged += NeedleworkItem_PropertyChanged;
                }
            }
        }
        private void NeedleworkItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
           if(e.PropertyName == nameof(NeedleworkTypeWrapper.IsChecked))
                OnPropertyChanged(nameof(AllNeedleworkTypeChecked));
        }
        public bool AllNeedleworkTypeChecked {
            get => nwTypes.All(x => x.IsChecked);
            set
            {
                nwTypes.ForEach(f => f.IsChecked = value);
                OnPropertyChanged(nameof(AllNeedleworkTypeChecked));
            }
        }
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
    /// <summary>
    /// Логика взаимодействия для SearchSidebarUC.xaml
    /// </summary>
    public partial class SearchSidebarUC : UserControl
    {
        List<StitchTypes> stitchTypes;
        List<ProductTypes> productTypes;
        List<AccessoryTypes> accessoryTypes;
        List<Designers> designers;
        List<Themes> themes;
        List<Countries> countries;
        private void GetDataToList()
        {
            stitchTypes = App.ctx.StitchTypes.ToList();
            productTypes = App.ctx.ProductTypes.ToList();
            accessoryTypes = App.ctx.AccessoryTypes.ToList();
            designers = App.ctx.Designers.ToList();
            themes = App.ctx.Themes.ToList();
            countries = App.ctx.Countries.ToList();
        }
        private void SetItemSource()
        {
            Dictionary<ListBox, IEnumerable> lbxDbData = new Dictionary<ListBox, IEnumerable>
            {
                //{ lbxNedleTypes, nwTypes },
                { lbxStitchTypes, stitchTypes },
                { lbxProdTypes, productTypes },
                { lbxAccess, accessoryTypes },
                { lbxDesigners, designers },
                { lbxThemes, themes },
                { lbxCountries, countries },
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
