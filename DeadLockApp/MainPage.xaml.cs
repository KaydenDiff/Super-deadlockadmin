using DeadLockApp.ViewModels;
namespace DeadLockApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new ItemsViewModel();

        }
    }

}
