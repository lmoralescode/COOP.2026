using COOPERATIVA.V2.Model;
using COOPERATIVA.V2.Data;

namespace COOPERATIVA.V2.Views;

public partial class RegistroViewsPage : ContentPage
{
    public RegistroViewsPage()
    {
        InitializeComponent();
    }


    private async void OnRegistrarSocioClicked(object sender, EventArgs e)
    {
        RegistroModel socio = new RegistroModel
        {
            Nombre = NombreEntry.Text,
            Apellidos = ApellidosEntry.Text,
            Cedula = CedulaEntry.Text,
            Telefono = TelefonoEntry.Text,
            Direccion = DireccionEntry.Text,
            FechaNacimiento = DateTime.Today,
            Correo = CorreoEntry.Text
        };

        SociosMemoria.ListaSocios.Add(socio);

        await DisplayAlertAsync("Éxito",
            "Socio registrado correctamente",
            "Aceptar");

        LimpiarCampos();
    }

    private void LimpiarCampos()
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