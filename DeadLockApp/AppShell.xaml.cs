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

        }
    }
}
