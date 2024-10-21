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

    
    private void OnFavoriteButtonClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var hero = (Hero)button.BindingContext;  // Получаем объект героя из контекста привязки

        // Изменяем состояние избранного
        hero.IsFavorite = !hero.IsFavorite;

        // Можно добавить отладочный вывод, чтобы убедиться, что значение изменилось
        Console.WriteLine($"{hero.Name} - Избранное: {hero.IsFavorite}");
    }
}