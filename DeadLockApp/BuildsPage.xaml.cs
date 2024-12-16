using DeadLockApp.Models;
using DeadLockApp.ViewModels;
using Microsoft.Maui.Controls;
using System;

namespace DeadLockApp
{
    public partial class BuildsPage : ContentPage
    {
        private BuildsViewModel _viewModel;
        private int _characterId;

        public BuildsPage(int characterId)
        {
            InitializeComponent();
            _viewModel = new BuildsViewModel();
            BindingContext = _viewModel;

            _characterId = characterId;

            // Загружаем билды для выбранного персонажа
            LoadBuilds();
        }

        private async void LoadBuilds()
        {
            try
            {
                await _viewModel.LoadBuilds(_characterId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading builds: {ex.Message}");
            }
        }
    }
}
