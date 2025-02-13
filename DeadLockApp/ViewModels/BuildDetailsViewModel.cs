﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using DeadLockApp.Models;

namespace DeadLockApp.ViewModels
{
    public class BuildDetailsViewModel : BaseViewModel
    {

        private const string BuildDetailsApiUrl = "http://192.168.0.105/api/character/"; // URL для API

        public ObservableCollection<Item> StartItems { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<Item> MiddleItems { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<Item> EndItems { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<Item> SituationsItems { get; set; } = new ObservableCollection<Item>();
        private async Task<List<Item>> GetItemsAsync()
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync("http://192.168.0.105/api/items"); // URL для API
            Debug.WriteLine("Items API Response:");
            Debug.WriteLine(response); // Отладочный вывод ответа

            try
            {
                // Десериализуем ApiResponse2, чтобы учесть корневое поле "Предметы"
                var apiResponse = JsonSerializer.Deserialize<ApiResponse2>(response);

                if (apiResponse != null && apiResponse.Предметы != null)
                {
                    Debug.WriteLine($"Total items fetched: {apiResponse.Предметы.Count}");
                    return apiResponse.Предметы;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось найти токен авторизации. Пожалуйста, войдите в систему.", "OK");
                    return new List<Item>();
                }
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"JSON Deserialization Error: {ex.Message}");
                return new List<Item>();
            }
        }


    

private string _buildName;
        private string _buildAuthor;

        // Добавляем свойство для хранения ID текущего билда
        private int _selectedBuildId;
        public int SelectedBuildId
        {
            get => _selectedBuildId;
            set
            {
                _selectedBuildId = value;
                OnPropertyChanged(nameof(SelectedBuildId));
            }
        }

        public string BuildName
        {
            get => _buildName;
            set
            {
                _buildName = value;
                OnPropertyChanged(nameof(BuildName));
            }
        }

        public string BuildAuthor
        {
            get => _buildAuthor;
            set
            {
                _buildAuthor = value;
                OnPropertyChanged(nameof(BuildAuthor));
            }
        }

        public ICommand DeleteBuildCommand { get; }

        public BuildDetailsViewModel()
        {
            // Инициализация команды удаления
            DeleteBuildCommand = new Command(async () => await DeleteBuildAsync());
        }

        public async Task LoadBuildDetailsAsync(int buildId)
        {
            try
            {
                // Присваиваем ID текущего билда
                SelectedBuildId = buildId;

                var client = new HttpClient();
                var response = await client.GetStringAsync($"{BuildDetailsApiUrl}{buildId}/builds");

                Debug.WriteLine("API Response:");
                Debug.WriteLine(response);  // Отладочный вывод полученного ответа

                var data = JsonSerializer.Deserialize<BuildResponse>(response);

                if (data != null && data.Builds != null && data.Builds.Any())
                {
                    var build = data.Builds.FirstOrDefault(b => b.Id == buildId);
                    if (build != null)
                    {
                        BuildName = build.Name;
                        BuildAuthor = build.Author;

                        StartItems.Clear();
                        MiddleItems.Clear();
                        EndItems.Clear();
                        SituationsItems.Clear();

                        var items = await GetItemsAsync();

                        foreach (var item in build.Items)
                        {
                            var itemDetail = items.FirstOrDefault(i => i.Id == item.ItemId);
                            if (itemDetail != null)
                            {
                                itemDetail.Image = $"http://192.168.0.105/public/storage/{itemDetail.Image}";
                                switch (item.PartId)
                                {
                                    case 1: StartItems.Add(itemDetail); break;
                                    case 2: MiddleItems.Add(itemDetail); break;
                                    case 3: EndItems.Add(itemDetail); break;
                                    case 4: SituationsItems.Add(itemDetail); break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading build details: {ex.Message}");
            }
        }

        private async Task DeleteBuildAsync()
        {
            try
            {
                var client = new HttpClient();

                // Получаем токен авторизации из SecureStorage
                string token = await SecureStorage.GetAsync("auth_token");

                // Проверка, что токен существует
                if (string.IsNullOrEmpty(token))
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось найти токен авторизации. Пожалуйста, войдите в систему.", "OK");
                    return;
                }

                // Добавляем токен в заголовки запроса
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Формируем правильный URL для удаления билда
                var deleteUrl = $"http://192.168.0.105/api/builds/{SelectedBuildId}";

                // Отправляем запрос DELETE
                var response = await client.DeleteAsync(deleteUrl);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Build {SelectedBuildId} deleted successfully.");

                    // Показать успешное сообщение
                    await Application.Current.MainPage.DisplayAlert("Успех", "Сборка успешно удалена", "OK");

                    // Возвращаемся на предыдущую страницу
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    // Получаем ответ от сервера в случае ошибки
                    var errorResponse = await response.Content.ReadAsStringAsync();

                    // Логирование ошибки
                    Debug.WriteLine($"Error response: {errorResponse}");

                    // Показать ошибку, если не удалось удалить
                    await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось удалить сборку. Ответ сервера: {errorResponse}", "OK");
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибок и отображение сообщения
                Debug.WriteLine($"Error deleting build: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Произошла ошибка при удалении сборки", "OK");
            }
        }

    }
}
