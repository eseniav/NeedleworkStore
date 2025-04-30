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
            }
        }
        public bool IsNotMin => QuantityCart > minItemCopacity;
        public bool IsNotMax => QuantityCart < maxItemCopacity;
    }
}
