using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using COOPERATIVA.V2.Model;
using COOPERATIVA.V2.Views;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Mail;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace COOPERATIVA.V2.ViewModels;

public partial class RegistroSocioViewModel : ObservableObject
{
    [ObservableProperty]
    private string nombre = "";

    [ObservableProperty]
    private string apellidos = "";

    [ObservableProperty]
    private string cedula = "";

    [ObservableProperty]
    private string telefono = "";

    [ObservableProperty]
    private string direccion = "";

    [ObservableProperty]
    private string correo = "";

    [ObservableProperty]
    private DateTime fechaNacimiento = DateTime.Today;

    [ObservableProperty]
    private string nombreError = "";

    [ObservableProperty]
    private string apellidosError = "";

    [ObservableProperty]
    private string cedulaError = "";

    [ObservableProperty]
    private string telefonoError = "";

    [ObservableProperty]
    private string correoError = "";

    [ObservableProperty]
    private bool esEdicion;

    public string TextoBoton => EsEdicion
        ? "Guardar cambios"
        : "Registrar socio";

    private Socios? socioEditar;

    public RegistroSocioViewModel()
    {
        if (SociosViewsPage.SocioEnEdicion is Socios socio)
        {
            socioEditar = socio;
            EsEdicion = true;

            Nombre = socio.Nombre;
            Apellidos = socio.Apellidos;
            Cedula = socio.Cedula;
            Telefono = socio.Telefono;
            Direccion = socio.Direccion;
            Correo = socio.Correo;
            FechaNacimiento = socio.FechaNacimiento;
        }
    }

    partial void OnEsEdicionChanged(bool value)
    {
        OnPropertyChanged(nameof(TextoBoton));
    }

    [RelayCommand]
    private async Task RegistrarSocio()
    {
        NombreError = "";
        ApellidosError = "";
        CedulaError = "";
        TelefonoError = "";
        CorreoError = "";

        Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
            Nombre.Trim().ToLower());

        Apellidos = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
            Apellidos.Trim().ToLower());

        Cedula = Cedula.Trim();
        Telefono = Telefono.Trim();
        Correo = Correo.Trim().ToLower();

        // Validar nombre.
        if (string.IsNullOrWhiteSpace(Nombre))
            NombreError = "El nombre del socio es obligatorio.";
        else if (Nombre.Length < 2)
            NombreError = "El nombre debe tener al menos 2 caracteres.";

        // Validar apellidos.
        if (string.IsNullOrWhiteSpace(Apellidos))
            ApellidosError = "Los apellidos son obligatorios.";

        // Validar cédula.
        if (string.IsNullOrWhiteSpace(Cedula))
            CedulaError = "La cédula es obligatoria.";
        else if (Cedula.Length != 11 || !Cedula.All(char.IsDigit))
            CedulaError = "La cédula debe contener exactamente 11 dígitos.";
        else if (SociosViewsPage.Socios.Any(s =>
            s.Cedula == Cedula && s != socioEditar))
            CedulaError = "Ya existe un socio registrado con esta cédula.";

        // Validar teléfono.
        if (string.IsNullOrWhiteSpace(Telefono))
            TelefonoError = "El teléfono es obligatorio.";
        else if (Telefono.Length != 10 || !Telefono.All(char.IsDigit))
            TelefonoError = "El teléfono debe contener 10 dígitos.";

        // Validar correo.
        if (!string.IsNullOrWhiteSpace(Correo) &&
            !MailAddress.TryCreate(Correo, out _))
            CorreoError = "Introduce un correo electrónico válido.";

        if (!string.IsNullOrEmpty(NombreError) ||
            !string.IsNullOrEmpty(ApellidosError) ||
            !string.IsNullOrEmpty(CedulaError) ||
            !string.IsNullOrEmpty(TelefonoError) ||
            !string.IsNullOrEmpty(CorreoError))
            return;

        if (EsEdicion && socioEditar != null)
        {
            socioEditar.Nombre = Nombre;
            socioEditar.Apellidos = Apellidos;
            socioEditar.Cedula = Cedula;
            socioEditar.Telefono = Telefono;
            socioEditar.Direccion = Direccion.Trim();
            socioEditar.Correo = Correo;
            socioEditar.FechaNacimiento = FechaNacimiento;

            SociosViewsPage.SocioEnEdicion = null;

            await Shell.Current.DisplayAlertAsync(
                "Actualización exitosa",
                "Los datos del socio se actualizaron correctamente.",
                "Aceptar");
        }
        else
        {
            var socio = new Socios
            {
                Nombre = Nombre,
                Apellidos = Apellidos,
                Cedula = Cedula,
                Telefono = Telefono,
                Direccion = Direccion.Trim(),
                Correo = Correo,
                FechaNacimiento = FechaNacimiento,
                FechaIngreso = DateTime.Today,
                TipoSocio = "Regular",
                Estado = "Activo"
            };

            SociosViewsPage.AgregarSocio(socio);

            await Shell.Current.DisplayAlertAsync(
                "Registro exitoso",
                "El socio se ha registrado correctamente.",
                "Aceptar");
        }

        LimpiarFormulario();

        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private void LimpiarFormulario()
    {
        Nombre = "";
        Apellidos = "";
        Cedula = "";
        Telefono = "";
        Direccion = "";
        Correo = "";
        FechaNacimiento = DateTime.Today;

        NombreError = "";
        ApellidosError = "";
        CedulaError = "";
        TelefonoError = "";
        CorreoError = "";
    }
}