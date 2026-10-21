
namespace COOPERATIVA.V2.Views;

public partial class RegistroViewsPage : ContentPage
{
    public string Nombre { get; private set; }
    public string Apellidos { get; private set; }
    public string Cedula { get; private set; }
    public string Telefono { get; private set; }
    public string Direccion { get; private set; }
    public DateTime FechaNacimiento { get; private set; }
    public string Correo { get; private set; }

    public RegistroViewsPage()
    {
        InitializeComponent();
    }


    private async void OnRegistrarSocioClicked(object sender, EventArgs e)
    {

        Nombre = NombreEntry.Text;
        Apellidos = ApellidosEntry.Text;
        Cedula = CedulaEntry.Text;
        Telefono = TelefonoEntry.Text;
        Direccion = DireccionEntry.Text;
        FechaNacimiento = DateTime.Today;
        Correo = CorreoEntry.Text;
        
    }

    private void OnLimpiarClicked(object sebder, EventArgs e)
    {
        NombreEntry.Text = "";
        ApellidosEntry.Text = "";
        CedulaEntry.Text = "";
        TelefonoEntry.Text = "";
        DireccionEntry.Text = "";
        CorreoEntry.Text = "";
        ContrasenaEntry.Text = "";
        FechaNacimientoPicker.Date = DateTime.Today;
    }
}