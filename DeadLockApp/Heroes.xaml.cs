using DeadLockApp.Models;
using DeadLockApp.ViewModels;
using Microsoft.Maui.Controls;
using System;

namespace DeadLockApp
{
    public partial class Heroes : ContentPage
    {
        private HeroesViewModel ViewModel => BindingContext as HeroesViewModel;

        public Heroes()
        {
            InitializeComponent();
            BindingContext = new HeroesViewModel();
        }

        private async void OnCharacterSelected(object sender, SelectionChangedEventArgs e)
        {
            var selectedCharacter = e.CurrentSelection.FirstOrDefault() as Character;
            if (selectedCharacter != null)
            {
                // Переходим на страницу с билдами и передаем ID выбранного персонажа
                await Navigation.PushAsync(new BuildsPage(selectedCharacter.Id));
            }
        }
    }
}
