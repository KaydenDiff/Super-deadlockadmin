using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;
using System.Windows.Input;
using DeadLockApp.ViewModels;
using DeadLockApp.Models;

public class CreateCharacterViewModel : BaseViewModel
{
    private const string ApiUrl = "http://192.168.0.105/api/characters/create";
    private readonly HttpClient _httpClient;

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
            OnPropertyChanged(nameof(ImagePath)); // Уведомление об изменении свойства
        }
    }
    public ICommand CreateCharacterCommand { get; }

    public CreateCharacterViewModel()
    {
        _httpClient = new HttpClient(); // Инициализация HTTP-клиента
        CreateCharacterCommand = new Command(async () => await CreateCharacterAsync());
    }
    private async Task CreateCharacterAsync()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(ImagePath))
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", $"Все поля должны быть заполнены.\nName: {Name}\nImagePath: {ImagePath}", "ОК");
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

            var newCharacter = new Character
            {
                Name = Name,
                Image = ImagePath // Просто передаем путь к изображению на сервер
            };

            var formData = new MultipartFormDataContent();

            // Если изображение - это файл, то добавим его как файл в форму
            if (File.Exists(ImagePath))
            {
                var fileStream = new FileStream(ImagePath, FileMode.Open);
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                formData.Add(fileContent, "image", Path.GetFileName(ImagePath));
            }

            // Добавим остальные данные персонажа в форму
            formData.Add(new StringContent(Name), "name");

            // Добавляем заголовок Authorization с токеном
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync(ApiUrl, formData);
            if (response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Успех", "Персонаж успешно создан!", "ОК");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                if (errorMessage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось создать персонажа. {errorMessage}", "ОК");
                }
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", $"Произошла ошибка: {ex.Message}", "ОК");
        }
    }
    
}