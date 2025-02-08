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

    // �������� ��� ��������
    public string Username { get; set; }
    public string Role { get; set; }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // ������ ��������� �������� ��� ��������
        BindingContext = this;  // ����� ������������ ������� ��������� ��������
    }

    public void SetUserInfo(string userName, string role)
    {
        Username = userName;
        Role = role;

        // ���������� ��������
        OnPropertyChanged(nameof(Username));
        OnPropertyChanged(nameof(Role));
    }
}
