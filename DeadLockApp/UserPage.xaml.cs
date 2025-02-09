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

        // ��������� ������ �� SecureStorage
        UserName = await SecureStorage.GetAsync("username");
        RoleCode = await SecureStorage.GetAsync("role_code");

        // ��������� UI � ����������� �������
        userNameLabel.Text = UserName; // ��������, ����� ��� ����� ������������
        roleCodeLabel.Text = RoleCode; // ��������, ����� ��� ���� ����
    }
}