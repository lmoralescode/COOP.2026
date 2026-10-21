using System.Collections.ObjectModel;

namespace COOPERATIVA.V2.Views;

public partial class SociosViewsPage : ContentPage
{
    private ObservableCollection<Socio> _socios;

    public SociosViewsPage()
    {
        InitializeComponent();

        _socios = new ObservableCollection<Socio>
        {
            new Socio
            {
                NombreCompleto = "Juan Pérez",
                Cedula = "001-1234567-8",
                Telefono = "809-555-1234",
                Estado = "Activo"
            },
            new Socio
            {
                NombreCompleto = "María Rodríguez",
                Cedula = "001-9876543-2",
                Telefono = "829-555-7890",
                Estado = "Activo"
            }
        };

        ListaSocios.ItemsSource = _socios;
    }

    public class Socio
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }

    private async void OnNuevoSocioClicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync(
            "Nuevo Socio",
            "Aquí abrirás la página de registro de socios.",
            "Aceptar");
        await Navigation.PushAsync(new RegistroViewsPage());
    }

    private async void OnEditarSocioClicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync(
            "Editar Socio",
            "Aquí abrirás la página de consulta o edición.",
            "Aceptar");
        await Navigation.PushAsync(new RegistroViewsPage());
    }
}
