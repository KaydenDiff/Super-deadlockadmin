using DeadLockApp.ViewModels;
namespace DeadLockApp;

public partial class Items : ContentPage
{
	public Items()
	{
		InitializeComponent();
        BindingContext = new MainViewModel();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Подробнее", "Супер мега подробное описание", "OK");
    }
}