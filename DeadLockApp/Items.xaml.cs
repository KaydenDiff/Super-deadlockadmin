using DeadLockApp.ViewModels;
using System.Diagnostics;

namespace DeadLockApp;

public partial class Items : ContentPage
{
    private readonly ItemsViewModel _viewModel;

    public Items()
    {
        InitializeComponent();
        _viewModel = new ItemsViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Debug.WriteLine("�������� Items ��������� � ��������� ������...");
        await _viewModel.LoadItemsAsync(); // ��������� �������� ��� ������ ��������� ��������
    }
    private async void OnCreateCharacterButtonClicked(object sender, EventArgs e)
    {
        // ������� �� �������� �������� ���������
        await Shell.Current.GoToAsync("createItemPage");
    }
}
