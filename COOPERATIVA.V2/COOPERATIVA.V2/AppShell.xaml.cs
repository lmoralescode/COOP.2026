using COOPERATIVA.V2.Views;

namespace COOPERATIVA.V2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registrando rutas para navegación
            Routing.RegisterRoute(nameof(InicioViewsPage), typeof(InicioViewsPage));
            Routing.RegisterRoute(nameof(LoginViewsPage), typeof(LoginViewsPage));
            Routing.RegisterRoute(nameof(RegistroViewsPage), typeof(RegistroViewsPage));
            Routing.RegisterRoute(nameof(MenuViewsPage), typeof(MenuViewsPage));
            Routing.RegisterRoute(nameof(SociosViewsPage), typeof(SociosViewsPage));
            Routing.RegisterRoute(nameof(AhorrosViewsPage), typeof(AhorrosViewsPage));
            Routing.RegisterRoute(nameof(PrestamosViewsPage), typeof(PrestamosViewsPage));
            Routing.RegisterRoute(nameof(PagosViewsPage), typeof(PagosViewsPage));

        }
    }
}
