using COOPERATIVA.V2.ViewsModel;

namespace COOPERATIVA.V2.Views;

public partial class RegistroViewsPage : ContentPage
{
    private List<Socios> _Socio = new();

	public RegistroViewsPage()
	{
        InitializeComponent();
    }

    private async void OnRegistrarSocioClicked(object sender, EventArgs e)
    {
        var _Socio = new Socios
        {
            NombreCompleto = NombreEntry.Text,
            Apellidos = ApellidosEntry.Text,
            Cedula = CedulaEntry.Text,
            Telefono = TelefonoEntry.Text,
            Direccion = DireccionEntry.Text,
            FechaNacimiento = DateTime.Today,
            Correo = CorreoEntry.Text,
            Clave = ContrasenaEntry.Text
        };

    }

    private void OnLimpiarSocioClicked(object sender, EventArgs e)
    {
        CorreoEntry.Text = "";
        ContrasenaEntry.Text = "";
    }

    private async void OnCancelarSocioClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}