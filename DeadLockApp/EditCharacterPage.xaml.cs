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
            InitializeComponent();  // ��� ���������� ��� ���������� XAML � C#
            BindingContext = this;
            Shell.SetNavBarIsVisible(this, false);
        }

        private async void OnSelectImageClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "�������� ����������� ���������",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                _imagePath = result.FullPath;
                CharacterImage.Source = ImageSource.FromFile(_imagePath);
            }
        }

       
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadCharacterData(CharacterId, Name, Image);
        }

        // ����� ����� ��� ���������� ������ ��� ����� �� ��������
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
      
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            _ = SaveCharacterAsync(); // �������� ����������� ����� ��� �������� ����������
        }
        private async Task SaveCharacterAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await DisplayAlert("������", "��� ��������� �� ����� ���� ������.", "��");
                return;
            }

            if (string.IsNullOrEmpty(_imagePath) && string.IsNullOrEmpty(Image))
            {
                await DisplayAlert("������", "����������� �� �������.", "��");
                return;
            }

            try
            {
                var token = await SecureStorage.GetAsync("auth_token");
                if (string.IsNullOrEmpty(token))
                {
                    await DisplayAlert("������", "�� ������� �������� �����. ��������� ���� ������.", "��");
                    return;
                }

                // �������� ����� ��������� �������
                Console.WriteLine($"����� ��������� ������:\n���: {Name}\n���� � �����������: {_imagePath}");

                var updatedCharacter = new Character
                {
                    Id = CharacterId,
                    Name = Name
                };

                var content = new MultipartFormDataContent();
                // �������, ��� ������������ ���������� ��������
                content.Add(new StringContent(updatedCharacter.Name), "name");

                if (!string.IsNullOrEmpty(_imagePath))
                {
                    var fileContent = new StreamContent(File.OpenRead(_imagePath));
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                    content.Add(fileContent, "image", Path.GetFileName(_imagePath));
                }

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PostAsync($"http://192.168.0.105/api/characters/{CharacterId}", content);

                if (response.IsSuccessStatusCode)
                {
                   

                    await DisplayAlert("�����", "�������� ������� �������.", "��");
                    await Shell.Current.GoToAsync("Heroes");

                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"����� �� �������: {responseContent}");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("������", $"�� ������� �������� ���������: {errorMessage}", "��");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("������", $"��������� ������: {ex.Message}", "��");
            }
        }
        private void LoadCharacterData(int id, string name, string image)
        {
            CharacterId = id;
            Name = name;

            if (string.IsNullOrEmpty(name))
            {
                Name = "��� �����"; // ��� �������� �����-�� ��������� �����
            }

            Image = image;

            // �������� �������� ��� �����������
            if (!string.IsNullOrEmpty(image))
            {
                try
                {
                    CharacterImage.Source = ImageSource.FromUri(new Uri(image));
                }
                catch (UriFormatException)
                {
                    // �������� ��� ���������� ������ ��� ������������� URL
                    CharacterImage.Source = null;
                }
            }
            else
            {
                // ���� ����������� ���, ����� ������������ �������� ��� ������ �����������
                CharacterImage.Source = null;
            }
        }
    }
}
