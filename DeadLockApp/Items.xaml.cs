using DeadLockApp.ViewModels;
namespace DeadLockApp;

public partial class Items : ContentPage
{
	public Items()
	{
		InitializeComponent();
        BindingContext = new MainViewModel();
    }
}