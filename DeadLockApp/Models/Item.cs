using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadLockApp.Models
{
    public class Item : INotifyPropertyChanged
    {
        private string _name;
        private int _price;
        private string _imageUrl;
        private string _categoty;
        private string _tier;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        public string Tier
        {
            get => _tier;
            set
            {
                if (_tier != value)
                {
                    _tier = value;
                    OnPropertyChanged(nameof(Tier));
                }
            }
        }
        public string Category
        {
            get => _categoty;
            set
            {
                if (_categoty != value)
                {
                    _categoty = value;
                    OnPropertyChanged(nameof(Category));
                }
            }
        }
        public int Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                if (_imageUrl != value)
                {
                    _imageUrl = value;
                    OnPropertyChanged(nameof(ImageUrl));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
