using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using DeadLockApp.Models;

namespace DeadLockApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private const string ItemsApiUrl = "http://course-project-4/public/api/items";
        private const string CharactersApiUrl = "http://course-project-4/public/api/characters";

        private readonly HttpClient _httpClient;

        public ObservableCollection<Character> Characteres { get; set; } = new ObservableCollection<Character>();
        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<Item> TierOneItems { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<Item> TierTwoItems { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<Item> TierThreeItems { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<Item> TierFourItems { get; set; } = new ObservableCollection<Item>();
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

        public MainViewModel()
        {
            _httpClient = new HttpClient();
            ChangeCategoryCommand = new Command<string>(ChangeCategory);

            // Асинхронная загрузка данных
            LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            await LoadCharactersAsync();
            await LoadItemsAsync();
            ChangeCategory("Weapon"); // Установка категории по умолчанию
        }

        private async Task LoadCharactersAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(CharactersApiUrl);
                var characters = JsonSerializer.Deserialize<List<Character>>(response);

                if (characters != null)
                {
                    Characteres.Clear();
                    foreach (var character in characters)
                    {
                        Characteres.Add(character);
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                Console.WriteLine($"Error loading characters: {ex.Message}");
            }
        }

        private async Task LoadItemsAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(ItemsApiUrl);
                var items = JsonSerializer.Deserialize<List<Item>>(response);

                if (items != null)
                {
                    Items.Clear();
                    foreach (var item in items)
                    {
                        Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                Console.WriteLine($"Error loading items: {ex.Message}");
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

            foreach (var item in Items.Where(x => x.Category == category))
            {
                switch (item.Tier)
                {
                    case "I": TierOneItems.Add(item); break;
                    case "II": TierTwoItems.Add(item); break;
                    case "III": TierThreeItems.Add(item); break;
                    case "IV": TierFourItems.Add(item); break;
                }
            }

            OnPropertyChanged(nameof(TierOneItems));
            OnPropertyChanged(nameof(TierTwoItems));
            OnPropertyChanged(nameof(TierThreeItems));
            OnPropertyChanged(nameof(TierFourItems));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}