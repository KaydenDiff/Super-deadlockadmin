using Microsoft.Maui.Controls;

namespace DeadLockApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            Shell.SetBackgroundColor(this, Color.FromArgb("#23221e")); 
            InitializeComponent();
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("builds", typeof(BuildsPage));
            Routing.RegisterRoute("Heroes", typeof(Heroes));
            Routing.RegisterRoute("Items", typeof(Items));
            Routing.RegisterRoute("UserPage", typeof(UserPage));
            Routing.RegisterRoute(nameof(ItemDetailsPage), typeof(ItemDetailsPage));
            Routing.RegisterRoute(nameof(BuildDetailsPage), typeof(BuildDetailsPage));
            Routing.RegisterRoute(nameof(BuildsPage), typeof(BuildsPage));
            Routing.RegisterRoute("createCharacterPage", typeof(CreateCharacterPage));
            Routing.RegisterRoute("createItemPage", typeof(CreateItemPage));
            Routing.RegisterRoute("EditCharacterPage", typeof(EditCharacterPage));
        }
    }
}
