using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using DeadLockApp.Models;

namespace DeadLockApp.ViewModels
{
    public class HeroesViewModel : BaseViewModel
    {
        private const string CharactersApiUrl = "http://course-project-4/api/characters";

        private readonly HttpClient _httpClient = new HttpClient(); // Инициализация HTTP клиента

        public ObservableCollection<Character> Characters { get; set; } = new ObservableCollection<Character>(); // Коллекция персонажей для привязки
        public Character SelectedCharacter { get; set; } // Выбранный персонаж
        public ICommand DeleteCharacterCommand { get; }
        public HeroesViewModel()
        {
            _ = LoadCharactersAsync(); // Загружаем персонажей асинхронно при инициализации ViewModel
            EditCharacterCommand = new Command<Character>(OnEditCharacter);
            DeleteCharacterCommand = new Command<Character>(DeleteCharacter);
        }

        // Асинхронный метод для получения списка персонажей из API
        private async Task<List<Character>> FetchCharactersAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetStringAsync(CharactersApiUrl, cancellationToken); // Получаем строку ответа от API
                var data = JsonSerializer.Deserialize<ApiResponse>(response); // Десериализуем ответ
                return data?.Персонажи ?? new List<Character>(); // Возвращаем список персонажей, если он есть, или пустой список
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching characters: {ex.Message}"); // Логирование ошибки при получении данных
                return new List<Character>(); // Возвращаем пустой список в случае ошибки
            }
        }

        // Асинхронный метод для загрузки и обработки персонажей
        private async Task LoadCharactersAsync()
        {
            try
            {
                var characters = await FetchCharactersAsync(); // Загружаем персонажей

                if (characters != null && characters.Any()) // Проверка на успешную загрузку персонажей
                {
                    Characters.Clear(); // Очищаем текущий список персонажей
                    foreach (var character in characters)
                    {
                        character.Image = $"http://course-project-4/storage/{character.Image}"; // Формируем полный путь для изображения
                        Characters.Add(character); // Добавляем персонажа в коллекцию
                        Debug.WriteLine($"Loaded: {character.Image}"); // Логирование загруженного персонажа
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading characters: {ex.Message}"); // Логирование ошибки при загрузке персонажей
            }
        }

        // Метод для сброса выбранного персонажа
        public void ResetSelectedCharacter()
        {
            SelectedCharacter = null; // Сбрасываем выбор персонажа
        }

        // Событие изменения свойства для привязки данных
        public event PropertyChangedEventHandler PropertyChanged;

        // Метод для уведомления об изменении свойства
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Вызываем событие изменения свойства
        }
        public async Task CreateCharacterAsync(string name, string imagePath = null)
        {
            try
            {
                var characterData = new MultipartFormDataContent();

                // Добавляем имя персонажа в тело запроса
                characterData.Add(new StringContent(name), "name");

                // Если изображение передается, добавляем его в запрос
                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    var imageContent = new StreamContent(File.OpenRead(imagePath));
                    imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg"); // Замените на правильный MIME-тип изображения

                    characterData.Add(imageContent, "image", Path.GetFileName(imagePath)); // "image" — это имя поля на сервере
                }

                // Отправка POST-запроса на создание персонажа
                var response = await _httpClient.PostAsync(CharactersApiUrl, characterData);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var createdCharacter = JsonSerializer.Deserialize<Character>(responseData);

                    Characters.Add(createdCharacter); // Добавление нового персонажа в коллекцию
                    Debug.WriteLine("Character created successfully");
                }
                else
                {
                    // Если ошибка не успешна, получаем ответ от сервера
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Error creating character: {errorResponse}");

                    // Можно добавить обработку ошибки по статусу, например:
                    if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        // Сообщение о недостаточных правах
                        Debug.WriteLine("Недостаточно прав для создания персонажа.");
                    }
                    else
                    {
                        // Обработка других ошибок
                        Debug.WriteLine($"Ошибка: {errorResponse}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating character: {ex.Message}"); // Логирование ошибки
            }

        }
        public ICommand EditCharacterCommand { get; }



        private async void OnEditCharacter(Character character)
        {
            if (character == null)
                return;

            // Используем абсолютный маршрут с /// и кодируем параметры
            await Shell.Current.GoToAsync($"EditCharacterPage?characterId={character.Id}&name={Uri.EscapeDataString(character.Name)}&image={Uri.EscapeDataString(character.Image)}");
        }
        private async void DeleteCharacter(Character character)
        {
            if (character == null)
                return;

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Подтверждение",
                $"Вы уверены, что хотите удалить {character.Name}?",
                "Да", "Нет");

            if (!confirm)
                return;

            // Удаление персонажа из коллекции
            Characters.Remove(character);

            // Если у вас API, добавьте удаление с сервера:
            try
            {
                var httpClient = new HttpClient();
                var token = await SecureStorage.GetAsync("auth_token");

                if (!string.IsNullOrEmpty(token))
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.DeleteAsync($"http://course-project-4/api/characters/{character.Id}");
              
                if (!response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось удалить персонажа.", "ОК");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Ошибка: {ex.Message}", "ОК");
            }
        }
    }
}
