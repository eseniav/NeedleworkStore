using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeedleworkStore.AppData
{
    public partial class Carts : INotifyPropertyChanged
    {
        public const int maxItemCopacity = 100;
        public const int minItemCopacity = 1;
        public const int maxItemCart = 2000;
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public int Quantity 
        {
            get => QuantityCart;
            set 
            {
                if (QuantityCart == value)
                    return;
                QuantityCart = value;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(QuantityCart));
                OnPropertyChanged(nameof(IsNotMin));
                OnPropertyChanged(nameof(IsNotMax));
                OnPropertyChanged(nameof(SumProducts));
            }
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
        public bool IsNotMin => QuantityCart > minItemCopacity;
        public bool IsNotMax => QuantityCart < maxItemCopacity;
        public int? SumProducts => QuantityCart * Products.ProductPrice;
    }
}
