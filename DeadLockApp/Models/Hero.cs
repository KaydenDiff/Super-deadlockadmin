using System.ComponentModel;

namespace DeadLockApp.Models
{
    public class Hero : INotifyPropertyChanged
    {
        private bool _isFavorite;

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                if (_isFavorite != value)
                {
                    _isFavorite = value;
                    OnPropertyChanged(nameof(IsFavorite));  // Уведомляем интерфейс об изменении
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}