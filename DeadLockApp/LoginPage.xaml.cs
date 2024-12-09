namespace DeadLockApp;
public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
        BindingContext = new DeadLockApp.ViewModels.LoginViewModel();

    }
}