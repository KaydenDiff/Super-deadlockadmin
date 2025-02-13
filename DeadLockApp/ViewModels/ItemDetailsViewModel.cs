using Microsoft.Maui.Media;  // Исправленный using для MediaPicker
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DeadLockApp.Models;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Collections.ObjectModel;

namespace DeadLockApp.ViewModels
{
    public class ItemDetailsViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;
        private string _fullImagePath;
        private string _imagePath;
        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();

        public ItemDetailsViewModel()
        {
            _httpClient = new HttpClient();
            EditItemCommand = new Command(async () => await EditItemAsync());
            DeleteItemCommand = new Command(async () => await DeleteItemAsync());
            SelectImageCommand = new Command(async () => await SelectImageAsync());
        }

        // Команды
        public ICommand EditItemCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand SelectImageCommand { get; }

        private Item _selectedItem;
        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(Cost));
                OnPropertyChanged(nameof(FullImagePath));
            }
        }

        // Свойство для пути изображения
        public string FullImagePath
        {
            get => !string.IsNullOrEmpty(_fullImagePath) ? _fullImagePath : $"http://192.168.0.105/storage/{SelectedItem?.Image}";
            set
            {
                if (_fullImagePath != value)
                {
                    _fullImagePath = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name => SelectedItem?.Name;
        public string Description => SelectedItem?.Description;
        public int Cost => SelectedItem?.Cost ?? 0;
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public async Task InitializeAsync(int itemId)
        {
            await LoadItemDetails(itemId);
        }

        public async Task LoadItemDetails(int itemId)
        {
            var allItems = await new ItemsViewModel().FetchItemsAsync();
            SelectedItem = allItems.FirstOrDefault(i => i.Id == itemId);
        }

        private async Task EditItemAsync()
        {
            if (string.IsNullOrWhiteSpace(Name) || Cost <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Все поля должны быть заполнены.", "ОК");
                return;
            }

            try
            {
                var token = await SecureStorage.GetAsync("auth_token");
                if (string.IsNullOrEmpty(token))
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось получить токен. Выполните вход заново.", "ОК");
                    return;
                }

                var formData = new MultipartFormDataContent
                {
                    { new StringContent(Name, Encoding.UTF8, "text/plain"), "name" },
                    { new StringContent(Description ?? "", Encoding.UTF8, "text/plain"), "description" },
                    { new StringContent(Cost.ToString(), Encoding.UTF8, "text/plain"), "cost" }
                };

                if (!string.IsNullOrEmpty(ImagePath) && File.Exists(ImagePath))
                {
                    var fileStream = new FileStream(ImagePath, FileMode.Open, FileAccess.Read);
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                    formData.Add(fileContent, "image", Path.GetFileName(ImagePath));
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsync($"http://192.168.0.105/api/items/{SelectedItem.Id}", formData);

                if (response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Успех", "Предмет успешно обновлен", "ОК");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", $"Ошибка обновления: {response.StatusCode}", "ОК");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Ошибка: {ex.Message}", "ОК");
            }
        }

        private async Task DeleteItemAsync()
        {
            if (SelectedItem == null)
                return;

            bool isConfirmed = await Application.Current.MainPage.DisplayAlert(
                "Удаление",
                $"Удалить {SelectedItem.Name}?",
                "Да", "Нет");

            if (!isConfirmed)
                return;

            try
            {
                var token = await SecureStorage.GetAsync("auth_token"); // Получаем токен
                if (!string.IsNullOrEmpty(token))
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"http://192.168.0.105/api/items/{SelectedItem.Id}");

                if (response.IsSuccessStatusCode)
                {
                    Items.Remove(SelectedItem); // Удаляем предмет из коллекции
                    SelectedItem = null; // Сбрасываем выделенный предмет

                    // Переход на страницу со всеми предметами
                    await Shell.Current.GoToAsync("Items");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось удалить предмет.", "ОК");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Ошибка: {ex.Message}", "ОК");
            }
        }

        private async Task SelectImageAsync()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Выберите изображение"
                });

                if (result != null)
                {
                    ImagePath = result.FullPath;
                    FullImagePath = result.FullPath;
                    SelectedItem.Image = result.FullPath;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Ошибка выбора изображения: " + ex.Message, "ОК");
            }
        }
    }
}
