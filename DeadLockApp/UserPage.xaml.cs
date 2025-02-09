using Microsoft.Maui.Controls;
using DeadLockApp.ViewModels;
using DeadLockApp.Models;
namespace DeadLockApp;

public partial class UserPage : ContentPage
{
    public string UserName { get; set; }
    public string RoleCode { get; set; }

    public UserPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Загружаем данные из SecureStorage
        UserName = await SecureStorage.GetAsync("username");
        RoleCode = await SecureStorage.GetAsync("role_code");

        // Обновляем UI с полученными данными
        userNameLabel.Text = UserName; // Например, метка для имени пользователя
        roleCodeLabel.Text = RoleCode; // Например, метка для кода роли
    }
}