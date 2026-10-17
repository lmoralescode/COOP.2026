using COOPERATIVA.V2.Views;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;

namespace COOPERATIVA.V2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

        }

        public object FlexLatyuouBehavior { get; private set; }

        public void ActivarMenu()
        {
            Menu.IsEnabled = true;
            Socio.IsEnabled = true;
            Ahorros.IsEnabled = true;
            Prestamos.IsEnabled = true;
            Pagos.IsEnabled = true;
            CerrarSesion.IsEnabled = true;

            FlyoutBehavior = FlyoutBehavior.Flyout;
        }
    

        public void DesactivarMenu()
        {
            Menu.IsEnabled = false;
            Socio.IsEnabled = false;
            Ahorros.IsEnabled = false;
            Prestamos.IsEnabled = false;
            Pagos.IsEnabled = false;
            CerrarSesion.IsEnabled = false;

            FlyoutBehavior = FlyoutBehavior.Disabled;
        }
    }
}
