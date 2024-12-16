using DeadLockApp.Models;
using DeadLockApp.ViewModels;
using System.Diagnostics;

namespace DeadLockApp
{
    [QueryProperty(nameof(CharacterId), "characterId")]
    [QueryProperty(nameof(CharacterName), "name")]
    public partial class BuildsPage : ContentPage
    {
        public int CharacterId { get; set; }
        public string CharacterName { get; set; }
        private BuildsViewModel _viewModel;

        public BuildsPage()
        {
            InitializeComponent();
            _viewModel = new BuildsViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.Builds.Clear();
            PageTitle.Text = $"Сборки {CharacterName}";
            // Принудительная загрузка данных каждый раз при появлении
            Task.Run(async () => await _viewModel.LoadBuildsAsync(CharacterId));
        }
    }


}
