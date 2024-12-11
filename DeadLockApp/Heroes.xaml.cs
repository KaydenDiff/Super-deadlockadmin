using DeadLockApp.ViewModels;
using DeadLockApp.Models;
using System;
using Microsoft.Maui.Controls;
namespace DeadLockApp;

public partial class Heroes : ContentPage
{
	public Heroes()
	{
		InitializeComponent();
        BindingContext = new MainViewModel();
    }
    private async void OnCharacterSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Character selectedCharacter)
        {
            await Shell.Current.GoToAsync($"builds?characterId={selectedCharacter.Id}");
        }
    }


}