using Microsoft.Maui.Controls;
using DeadLockApp.ViewModels;
using DeadLockApp.Models;
namespace DeadLockApp;

public partial class UserPage : ContentPage
{
    public UserPage()
    {
        InitializeComponent();
    }

    // Свойства для привязки
    public string Username { get; set; }
    public string Role { get; set; }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Пример установки значений для привязки
        BindingContext = this;  // Здесь используется текущий экземпляр страницы
    }

    public void SetUserInfo(string userName, string role)
    {
        Username = userName;
        Role = role;

        // Обновление привязки
        OnPropertyChanged(nameof(Username));
        OnPropertyChanged(nameof(Role));
    }
}
