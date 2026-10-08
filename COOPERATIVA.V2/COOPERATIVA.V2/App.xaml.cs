using COOPERATIVA.V2.Views;
using Microsoft.Extensions.DependencyInjection;

namespace COOPERATIVA.V2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(new InicioViewsPage()));
        }
    }
}