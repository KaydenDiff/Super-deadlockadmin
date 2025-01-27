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
            if (CharacterId <= 0 || string.IsNullOrWhiteSpace(Name))
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

                var formData = new MultipartFormDataContent();

                // Если изображение - это файл, то добавляем его в форму
                if (!string.IsNullOrEmpty(ImagePath) && File.Exists(ImagePath))
                {
                    var fileStream = new FileStream(ImagePath, FileMode.Open);
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                    formData.Add(fileContent, "image", Path.GetFileName(ImagePath));
                }

                // Добавляем остальные данные персонажа в форму
                formData.Add(new StringContent(Name), "name");

                // Добавляем заголовок Authorization с токеном
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsync($"{ApiUrl}/{CharacterId}", formData);
                if (response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Успех", "Персонаж успешно обновлен!", "ОК");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    if (errorMessage != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось обновить персонажа. {errorMessage}", "ОК");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Произошла ошибка: {ex.Message}", "ОК");
            }
        }
    }
}