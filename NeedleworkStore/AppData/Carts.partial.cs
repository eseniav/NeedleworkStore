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
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private const int MaxCapacityPerItem = 100;
        private const int MinCapacityPerItem = 1;

        public bool IsNotMax => QuantityCart < MaxCapacityPerItem;
        public bool IsNotMin => QuantityCart > MinCapacityPerItem;
        public int? TotalPrice => QuantityCart * Products.ProductPrice;
        // Better alternative:
        public int TotalSum => QuantityCart * (Products?.ProductPrice ?? 0);
        public int Quantity 
        {
            get => QuantityCart;
            set 
            {
                if (QuantityCart == value) return;

                QuantityCart = value;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(QuantityCart));
                OnPropertyChanged(nameof(IsNotMax));
                OnPropertyChanged(nameof(IsNotMin));
                OnPropertyChanged(nameof(TotalPrice));
                OnPropertyChanged(nameof(TotalSum));
            }
        }
    }
}
