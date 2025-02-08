using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using DeadLockApp.ViewModels;
using DeadLockApp.Models;
using Newtonsoft.Json;


namespace DeadLockApp.ViewModels
{
    public class EditCharacterViewModel : BaseViewModel
    {
        private const string ApiUrl = "http://course-project-4/api/characters"; // URL для редактирования персонажа
        private readonly HttpClient _httpClient;

        private int _characterId;
        public int CharacterId
        {
            get => _characterId;
            set
            {
                _characterId = value;
                OnPropertyChanged(nameof(CharacterId));
            }
        }

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

        public ICommand EditCharacterCommand { get; }

        public EditCharacterViewModel()
        {
            _httpClient = new HttpClient();
            EditCharacterCommand = new Command(async () => await EditCharacterAsync());
        }

        public async Task LoadCharacterData(int characterId)
        {
            try
            {
                // Получаем токен из SecureStorage
                var token = await SecureStorage.GetAsync("auth_token");
                if (string.IsNullOrEmpty(token))
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось получить токен. Выполните вход заново.", "ОК");
                    return;
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"{ApiUrl}/{characterId}");
                if (response.IsSuccessStatusCode)
                {
                    var character = JsonConvert.DeserializeObject<Character>(await response.Content.ReadAsStringAsync());
                    if (character != null)
                    {
                        CharacterId = character.Id;
                        Name = character.Name;
                        ImagePath = character.Image; // Предполагается, что сервер возвращает URL изображения
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось загрузить данные персонажа.", "ОК");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Произошла ошибка: {ex.Message}", "ОК");
            }
        }

        private async Task EditCharacterAsync()
        {
            // Логируем вызов метода
            await Application.Current.MainPage.DisplayAlert("DEBUG", "Метод EditCharacterAsync вызван", "ОК");

            // Проверяем, что все необходимые поля заполнены
            if (CharacterId <= 0 || string.IsNullOrWhiteSpace(Name))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Все поля должны быть заполнены.", "ОК");
                return;
            }

            try
            {
                // Получаем токен из безопасного хранилища
                var token = await SecureStorage.GetAsync("auth_token");
                if (string.IsNullOrEmpty(token))
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось получить токен. Выполните вход заново.", "ОК");
                    return;
                }

                // Создаем MultipartFormDataContent для отправки данных
                var formData = new MultipartFormDataContent();

                // Логируем имя перед отправкой
                await Application.Current.MainPage.DisplayAlert("DEBUG", $"Отправляем имя: {Name}", "ОК");

                // Добавляем имя в запрос
                var nameContent = new StringContent(Name, Encoding.UTF8);
                nameContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "name"
                };
                formData.Add(nameContent);

                // Если есть изображение, добавляем его
                if (!string.IsNullOrEmpty(ImagePath) && File.Exists(ImagePath))
                {
                    var fileStream = new FileStream(ImagePath, FileMode.Open, FileAccess.Read);
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                    formData.Add(fileContent, "image", Path.GetFileName(ImagePath));

                    // Логируем путь к изображению перед отправкой
                    await Application.Current.MainPage.DisplayAlert("DEBUG", $"Отправляем изображение: {ImagePath}", "ОК");
                }

                // Добавляем заголовок авторизации
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Отправляем запрос на сервер
                var response = await _httpClient.PostAsync($"{ApiUrl}/{CharacterId}", formData);
                var responseContent = await response.Content.ReadAsStringAsync();

                // Логируем код ответа и содержимое ответа
                await Application.Current.MainPage.DisplayAlert("DEBUG", $"Код ответа: {response.StatusCode}", "ОК");
                await Application.Current.MainPage.DisplayAlert("Ответ сервера", string.IsNullOrWhiteSpace(responseContent) ? "Пустой ответ" : responseContent, "ОК");

                // Проверяем успешность ответа
                if (response.IsSuccessStatusCode)
                {
                    // Логируем, что отправлялось в запросе
                    await Application.Current.MainPage.DisplayAlert("DEBUG",
                        $"Отправленные данные:\nИмя: {Name}\nИзображение: {ImagePath}",
                        "ОК");

                    // Если обновление прошло успешно, показываем сообщение об успехе
                    await Application.Current.MainPage.DisplayAlert("Успех", $"Отправленные данные:\nИмя: {Name}\nИзображение: {ImagePath}",
                        "ОК");

                    // Переходим назад на предыдущую страницу
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    // Если ошибка, показываем сообщение с ошибкой
                    await Application.Current.MainPage.DisplayAlert("Ошибка", $"Ошибка обновления персонажа: {response.StatusCode}", "ОК");
                }
            }
            catch (Exception ex)
            {
                // Ловим исключения и выводим сообщение об ошибке
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Исключение: {ex.Message}", "ОК");
            }
        }


    }
}