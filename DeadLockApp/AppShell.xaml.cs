using Microsoft.Maui.Controls;

namespace DeadLockApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("builds", typeof(BuildsPage));
            Routing.RegisterRoute("Heroes", typeof(Heroes));
            Routing.RegisterRoute("UserPage", typeof(UserPage));
            Routing.RegisterRoute(nameof(BuildDetailsPage), typeof(BuildDetailsPage));
            Routing.RegisterRoute(nameof(BuildsPage), typeof(BuildsPage));
            Routing.RegisterRoute("createCharacterPage", typeof(CreateCharacterPage));
            Routing.RegisterRoute("EditCharacterPage", typeof(EditCharacterPage));
        }
    }
}
