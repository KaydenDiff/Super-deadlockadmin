using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DeadLockApp.Models;

namespace DeadLockApp.ViewModels
{
    public class HeroesViewModel : INotifyPropertyChanged
    {
        private const string CharactersApiUrl = "http://course-project-4/api/characters";

        private readonly HttpClient _httpClient = new HttpClient();

        public ObservableCollection<Character> Characters { get; set; } = new ObservableCollection<Character>();
        public Character SelectedCharacter { get; set; }
        public HeroesViewModel()
        {
            _ = LoadCharactersAsync();
        }

        private async Task<List<Character>> FetchCharactersAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetStringAsync(CharactersApiUrl, cancellationToken);
                var data = JsonSerializer.Deserialize<ApiResponse>(response);
                return data?.Персонажи ?? new List<Character>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching characters: {ex.Message}");
                return new List<Character>();
            }
        }

        private async Task LoadCharactersAsync()
        {
            try
            {
                var characters = await FetchCharactersAsync();

                if (characters != null && characters.Any())
                {
                    Characters.Clear();
                    foreach (var character in characters)
                    {
                        character.Image = $"http://course-project-4/storage/{character.Image}";
                        Characters.Add(character);
                        Debug.WriteLine($"Loaded: {character.Image}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading characters: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
