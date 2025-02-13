using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Input;

using DeadLockApp.Models;

namespace DeadLockApp.ViewModels
{
    class CreateItemViewModel : BaseViewModel
    {
        private const string ApiUrl = "http://192.168.0.105/api/items/create";
        private static readonly HttpClient _httpClient = new HttpClient();

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        private int _cost;
        public int Cost
        {
            get => _cost;
            set
            {
                _cost = value;
                OnPropertyChanged(nameof(Cost));
            }
        }

        private int _tierId;
        public int TierId
        {
            get => _tierId;
            set
            {
                _tierId = value;
                OnPropertyChanged(nameof(TierId));
            }
        }

        private int _typeId;
        public int TypeId
        {
            get => _typeId;
            set
            {
                _typeId = value;
                OnPropertyChanged(nameof(TypeId));
            }
        }

        public ICommand CreateItemCommand { get; }

        public CreateItemViewModel()
        {
            CreateItemCommand = new Command(async () => await CreateItemAsync());
        }

        private async Task CreateItemAsync()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description) ||
                string.IsNullOrWhiteSpace(ImagePath) || Cost <= 0 || TierId == null || TypeId == null)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Все поля должны быть заполнены.", "ОК");
                return;
            }

            try
            {
                // Получаем токен из SecureStorage
                var token = await SecureStorage.GetAsync("auth_token");

                if (string.IsNullOrEmpty(token))
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось получить токен. Выполните вход заново.", "ОК");
                    return;
                }

                // Создаем объект для отправки данных на сервер
                var formData = new MultipartFormDataContent();

                // Добавляем текстовые данные
                formData.Add(new StringContent(Name), "name");
                formData.Add(new StringContent(Description), "description");
                formData.Add(new StringContent(Cost.ToString()), "cost");
                formData.Add(new StringContent(TierId.ToString()), "tier_id");
                formData.Add(new StringContent(TypeId.ToString()), "type_id");

                // Проверка наличия изображения
                if (File.Exists(ImagePath))
                {
                    var fileStream = new FileStream(ImagePath, FileMode.Open);
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                    formData.Add(fileContent, "image", Path.GetFileName(ImagePath));
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Файл изображения не найден.", "ОК");
                    return;
                }

                // Устанавливаем заголовок с токеном
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Отправляем запрос
                var response = await _httpClient.PostAsync(ApiUrl, formData);

                if (response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Успех", "Предмет успешно создан!", "ОК");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    if (errorMessage != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось создать предмет. {errorMessage}", "ОК");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Произошла ошибка: {ex.Message}", "ОК");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
