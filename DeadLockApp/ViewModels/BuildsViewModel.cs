using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DeadLockApp.Models;
using System;

namespace DeadLockApp.ViewModels
{
    public class BuildsViewModel
    {
        private const string ApiUrlTemplate = "http://course-project-4/api/character/{0}/builds";
        private HttpClient _httpClient;

        public ObservableCollection<Build> Builds { get; set; }
        public Build SelectedBuild { get; set; }

        public BuildsViewModel()
        {
            _httpClient = new HttpClient();
            Builds = new ObservableCollection<Build>();
        }

        // Асинхронный метод для получения билдов и обновления коллекции
        public async Task LoadBuilds(int characterId)
        {
            string url = string.Format(ApiUrlTemplate, characterId);

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var data = JsonSerializer.Deserialize<ApiResponse>(response);

                if (data?.Builds != null)
                {
                    // Очистить коллекцию перед добавлением новых данных
                    Builds.Clear();

                    // Добавить билды в коллекцию
                    foreach (var build in data.Builds)
                    {
                        Builds.Add(build);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching builds: {ex.Message}");
            }
        }

        public class ApiResponse
        {
            public Character Character { get; set; }
            public Build[] Builds { get; set; }
        }
    }
}
