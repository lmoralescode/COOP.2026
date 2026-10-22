using System.Globalization;
using COOPERATIVA.V2.Model;

namespace COOPERATIVA.V2.Views;

public partial class RegistroViewsPage : ContentPage
{
    public RegistroViewsPage()
    {
        InitializeComponent();

        if (SociosViewsPage.SocioEnEdicion is Socios socio)
        {
            Title = "Editar socio";

            NombreEntry.Text = socio.Nombre;
            ApellidosEntry.Text = socio.Apellidos;
            CedulaEntry.Text = socio.Cedula;
            TelefonoEntry.Text = socio.Telefono;
            DireccionEntry.Text = socio.Direccion;
            CorreoEntry.Text = socio.Correo;
            FechaNacimientoPicker.Date = socio.FechaNacimiento;

            if (Content is ScrollView scrollView &&
                scrollView.Content is VerticalStackLayout layout &&
                layout.Children.OfType<Button>().FirstOrDefault() is Button boton)
            {
                boton.Text = "Guardar cambios";
            }
        }
        else
        {
            Title = "Registrar socio";
            LimpiarFormulario();
        }
    }

    private string CapitalizarTexto(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return string.Empty;

        return CultureInfo.CurrentCulture.TextInfo
            .ToTitleCase(texto.Trim().ToLower());
    }

    private async void OnRegistrarSocioClicked(object sender, EventArgs e)
    {
        string nombre = CapitalizarTexto(NombreEntry.Text);
        string apellidos = CapitalizarTexto(ApellidosEntry.Text);
        string cedula = CedulaEntry.Text?.Trim() ?? "";
        string telefono = TelefonoEntry.Text?.Trim() ?? "";

        // Validar campos obligatorios.
        if (string.IsNullOrWhiteSpace(nombre) ||
            string.IsNullOrWhiteSpace(apellidos) ||
            string.IsNullOrWhiteSpace(cedula) ||
            string.IsNullOrWhiteSpace(telefono))
        {
            await DisplayAlertAsync(
                "Validación",
                "Nombre, apellidos, cédula y teléfono son obligatorios.",
                "Aceptar");

            return;
        }

        // Validar cédula.
        if (cedula.Length != 11 || !cedula.All(char.IsDigit))
        {
            await DisplayAlertAsync(
                "Validación",
                "La cédula debe contener 11 dígitos.",
                "Aceptar");

            return;
        }

        // Evitar cédulas duplicadas.
        bool cedulaExiste = SociosViewsPage.Socios.Any(s =>
            s.Cedula == cedula &&
            s != SociosViewsPage.SocioEnEdicion);

        if (cedulaExiste)
        {
            await DisplayAlertAsync(
                "Validación",
                "Ya existe un socio registrado con esa cédula.",
                "Aceptar");

            return;
        }

        // MODO EDICIÓN.
        if (SociosViewsPage.SocioEnEdicion is Socios socioExistente)
        {
            socioExistente.Nombre = nombre;
            socioExistente.Apellidos = apellidos;
            socioExistente.Cedula = cedula;
            socioExistente.Telefono = telefono;
            socioExistente.Direccion = DireccionEntry.Text?.Trim() ?? "";
            socioExistente.Correo = CorreoEntry.Text?.Trim().ToLower() ?? "";
            socioExistente.FechaNacimiento = DateTime.Today;

            SociosViewsPage.SocioEnEdicion = null;

            await DisplayAlertAsync(
                "Actualización exitosa",
                "Los datos del socio se actualizaron correctamente.",
                "Aceptar");
        }
        else
        {
            // MODO REGISTRO.
            var socio = new Socios
            {
                Nombre = nombre,
                Apellidos = apellidos,
                Cedula = cedula,
                Telefono = telefono,
                Direccion = DireccionEntry.Text?.Trim() ?? "",
                Correo = CorreoEntry.Text?.Trim().ToLower() ?? "",
                FechaNacimiento = DateTime.Today,
                FechaIngreso = DateTime.Today,
                TipoSocio = "Regular",
                Estado = "Activo"
            };

            SociosViewsPage.AgregarSocio(socio);

            await DisplayAlertAsync(
                "Registro exitoso",
                "El socio se ha registrado correctamente.",
                "Aceptar");
        }

        LimpiarFormulario();

        // Regresar a la lista de socios.
        await Navigation.PopAsync();
    }

    private void OnLimpiarClicked(object sender, EventArgs e)
    {
        LimpiarFormulario();
    }

    private void LimpiarFormulario()
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