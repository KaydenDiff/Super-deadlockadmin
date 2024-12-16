using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using DeadLockApp.Models;

namespace DeadLockApp.ViewModels
{
    public class ItemsViewModel : INotifyPropertyChanged
    {
        private const string ItemsApiUrl = "http://course-project-4/api/items";

        public ObservableCollection<Item> TierOneItems { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<Item> TierTwoItems { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<Item> TierThreeItems { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<Item> TierFourItems { get; set; } = new ObservableCollection<Item>();

        private List<Item> _items = new List<Item>();


        public List<Item> Items
        {
            get => _items;
            set
            {
                if (_items != value)
                {
                    _items = value;
                    OnPropertyChanged(nameof(Items));
                }
            }
        }
        public ICommand ChangeCategoryCommand { get; }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                }
            }
        }

        public ItemsViewModel()
        {
            ChangeCategoryCommand = new Command<string>(ChangeCategory);
            LoadItemsAsync(); // Загрузка предметов при инициализации
        }

        private async Task<List<Item>> FetchItemsAsync()
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetStringAsync(ItemsApiUrl);
                var data = JsonSerializer.Deserialize<ApiResponse2>(response);
                return data?.Предметы ?? new List<Item>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading items: {ex.Message}");
                return new List<Item>();
            }
        }

        private void ChangeCategory(string category)
        {
            SelectedCategory = category;
            UpdateFilteredItems(category);
        }

        private void UpdateFilteredItems(string category)
        {
            TierOneItems.Clear();
            TierTwoItems.Clear();
            TierThreeItems.Clear();
            TierFourItems.Clear();

            foreach (var item in Items.Where(x => x.Type_id == GetTypeIdFromCategory(category)))
            {
                switch (item.Tier_id)
                {
                    case 1: TierOneItems.Add(item); break;
                    case 2: TierTwoItems.Add(item); break;
                    case 3: TierThreeItems.Add(item); break;
                    case 4: TierFourItems.Add(item); break;
                }
            }

            OnPropertyChanged(nameof(TierOneItems));
            OnPropertyChanged(nameof(TierTwoItems));
            OnPropertyChanged(nameof(TierThreeItems));
            OnPropertyChanged(nameof(TierFourItems));
        }
        private int GetTypeIdFromCategory(string category)
        {
            return category switch
            {
                "Weapon" => 1,
                "Vitality" => 2,
                "Spirit" => 3,
                _ => 0,
            };
        }
        private async void LoadItemsAsync()
        {
            var items = await FetchItemsAsync();

            foreach (var item in items)
            {
                item.Image = $"http://course-project-4/public/storage/{item.Image}";
            }

            Items = items; // Заполнить свойство Items
            UpdateFilteredItems("Weapon"); // Обновить отображение
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
