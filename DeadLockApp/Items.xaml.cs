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

        Debug.WriteLine("Страница Items появилась — обновляем данные...");
        await _viewModel.LoadItemsAsync(); // Загружаем предметы при каждом появлении страницы
    }
}
