using System.Text;
using System.Text.Json;
using DeadLockApp.ViewModels;
using DeadLockApp.Models;
using System.Xml.Linq;
using System.ComponentModel;

namespace DeadLockApp
{
    [QueryProperty(nameof(CharacterId), "characterId")]
    [QueryProperty(nameof(Name), "name")]
    [QueryProperty(nameof(Image), "image")]
    public partial class EditCharacterPage : ContentPage, INotifyPropertyChanged
    {
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        private string _imagePath;

        public EditCharacterPage()
        {
            InitializeComponent();  // Это необходимо для связывания XAML и C#
            BindingContext = this;
            Shell.SetNavBarIsVisible(this, false);
        }

        private async void OnSelectImageClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Выберите изображение персонажа",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                _imagePath = result.FullPath;
                CharacterImage.Source = ImageSource.FromFile(_imagePath);
            }
        }

        private async Task OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await DisplayAlert("Ошибка", "Имя персонажа не может быть пустым.", "ОК");
                return;
            }

            if (string.IsNullOrEmpty(_imagePath) && string.IsNullOrEmpty(Image))
            {
                await DisplayAlert("Ошибка", "Изображение не выбрано.", "ОК");
                return;
            }

            try
            {
                var token = await SecureStorage.GetAsync("auth_token");
                if (string.IsNullOrEmpty(token))
                {
                    await DisplayAlert("Ошибка", "Не удалось получить токен. Выполните вход заново.", "ОК");
                    return;
                }

                var updatedCharacter = new Character
                {
                    Id = CharacterId,
                    Name = Name
                };

                var content = new MultipartFormDataContent();
                content.Add(new StringContent(updatedCharacter.Name), "name");

                if (!string.IsNullOrEmpty(_imagePath))
                {
                    var fileContent = new StreamContent(File.OpenRead(_imagePath));
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                    content.Add(fileContent, "image", Path.GetFileName(_imagePath));
                }

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PostAsync($"http://course-project-4/api/characters/{CharacterId}", content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Успех", "Персонаж успешно обновлён.", "ОК");
                    await Shell.Current.GoToAsync("Heroes");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Ошибка", $"Не удалось обновить персонажа: {errorMessage}", "ОК");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Произошла ошибка: {ex.Message}", "ОК");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadCharacterData(CharacterId, Name, Image);
        }

        // Новый метод для сохранения данных при уходе со страницы
        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            // Проводим сохранение при попытке вернуться
            if (!string.IsNullOrWhiteSpace(Name) || !string.IsNullOrEmpty(_imagePath))
            {
                await OnSaveButtonClicked(null, null);
            }
        }
        private void LoadCharacterData(int id, string name, string image)
        {
            CharacterId = id;
            Name = name;

            if (string.IsNullOrEmpty(name))
            {
                Name = "Без имени"; // Или выберите какой-то дефолтный текст
            }

            Image = image;

            // Добавьте проверку для изображения
            if (!string.IsNullOrEmpty(image))
            {
                try
                {
                    CharacterImage.Source = ImageSource.FromUri(new Uri(image));
                }
                catch (UriFormatException)
                {
                    // Логируем или показываем ошибку для некорректного URL
                    CharacterImage.Source = null;
                }
            }
            else
            {
                // Если изображения нет, можно использовать заглушку или пустое изображение
                CharacterImage.Source = null;
            }
        }
    }
}
