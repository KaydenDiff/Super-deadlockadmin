using Microsoft.Maui.Controls;
using DeadLockApp.ViewModels;

namespace DeadLockApp;

[QueryProperty(nameof(ItemId), "itemId")]
public partial class ItemDetailsPage : ContentPage
{
    private readonly ItemDetailsViewModel _viewModel;

    private int _itemId;
    public int ItemId
    {
        get => _itemId;
        set
        {
            _itemId = value;
            LoadItem();
        }
    }

    public ItemDetailsPage()
    {
        InitializeComponent();
        _viewModel = new ItemDetailsViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel != null && _viewModel.SelectedItem != null)
        {
            await _viewModel.LoadItemDetails(_viewModel.SelectedItem.Id);
        }
    }

    private async void LoadItem()
    {
        if (_itemId > 0)
        {
            await _viewModel.InitializeAsync(_itemId);
        }
    }
}
