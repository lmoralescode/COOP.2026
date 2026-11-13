
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using COOPERATIVA.V2.Model;
using System.Globalization;
using System.Text.RegularExpressions;

namespace COOPERATIVA.V2.ViewsModel;

public partial class RegistroViewsModel : BaseViewsModel
{
    [ObservableProperty] private string nombre = "";
    [ObservableProperty] private string apellidos = "";
    [ObservableProperty] private string cedula = "";
    [ObservableProperty] private string telefono = "";
    [ObservableProperty] private string direccion = "";
    [ObservableProperty] private string correo = "";

    [ObservableProperty]
    private DateTime fechaNacimiento = DateTime.Today;

    [ObservableProperty] private string errorNombre = "";
    [ObservableProperty] private string errorApellidos = "";
    [ObservableProperty] private string errorCedula = "";
    [ObservableProperty] private string errorTelefono = "";
    [ObservableProperty] private string errorCorreo = "";

    public Socios? SocioEnEdicion { get; set; }

    public bool EsEdicion => SocioEnEdicion != null;

    public RegistroViewsModel(Socios? socio = null)
    {
        SocioEnEdicion = socio;

        if (socio != null)
        {
            Nombre = socio.Nombre;
            Apellidos = socio.Apellidos;
            Cedula = socio.Cedula;
            Telefono = socio.Telefono;
            Direccion = socio.Direccion;
            Correo = socio.Correo;
            FechaNacimiento = socio.FechaNacimiento;
        }
    }

    private string Capitalizar(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return "";

        return CultureInfo.CurrentCulture.TextInfo
            .ToTitleCase(texto.Trim().ToLower());
    }

    private bool Validar()
    {
        ErrorNombre = "";
        ErrorApellidos = "";
        ErrorCedula = "";
        ErrorTelefono = "";
        ErrorCorreo = "";

        bool valido = true;

        if (string.IsNullOrWhiteSpace(Nombre))
        {
            ErrorNombre = "El nombre del socio es obligatorio.";
            valido = false;
        }

        if (string.IsNullOrWhiteSpace(Apellidos))
        {
            ErrorApellidos = "Los apellidos del socio son obligatorios.";
            valido = false;
        }

        string cedulaLimpia = Cedula.Trim();

        if (cedulaLimpia.Length != 11 ||
            !cedulaLimpia.All(char.IsDigit))
        {
            ErrorCedula = "La cédula debe contener exactamente 11 dígitos.";
            valido = false;
        }
        else if (SociosViewsModel.Socios.Any(s =>
            s.Cedula == cedulaLimpia && s != SocioEnEdicion))
        {
            ErrorCedula = "Ya existe un socio registrado con esta cédula.";
            valido = false;
        }

        if (string.IsNullOrWhiteSpace(Telefono))
        {
            ErrorTelefono = "El teléfono del socio es obligatorio.";
            valido = false;
        }

        if (!string.IsNullOrWhiteSpace(Correo) &&
            !Regex.IsMatch(Correo.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            ErrorCorreo = "Introduce un correo electrónico válido.";
            valido = false;
        }

        return valido;
    }

    private object CorreoRegex()
    {
        throw new NotImplementedException();
    }

    [RelayCommand]
    private void Guardar()
    {
        if (!Validar())
            return;

        string nombreFinal = Capitalizar(Nombre);
        string apellidosFinal = Capitalizar(Apellidos);

        if (SocioEnEdicion != null)
        {
            SocioEnEdicion.Nombre = nombreFinal;
            SocioEnEdicion.Apellidos = apellidosFinal;
            SocioEnEdicion.Cedula = Cedula.Trim();
            SocioEnEdicion.Telefono = Telefono.Trim();
            SocioEnEdicion.Direccion = Direccion.Trim();
            SocioEnEdicion.Correo = Correo.Trim().ToLower();
            SocioEnEdicion.FechaNacimiento = FechaNacimiento;

            MensajeExito = "Los datos del socio se actualizaron correctamente.";
        }
        else
        {
            var socio = new Socios
            {
                Id = SociosViewsModel.Socios.Count == 0
                    ? 1
                    : SociosViewsModel.Socios.Max(s => s.Id) + 1,

                Nombre = nombreFinal,
                Apellidos = apellidosFinal,
                Cedula = Cedula.Trim(),
                Telefono = Telefono.Trim(),
                Direccion = Direccion.Trim(),
                Correo = Correo.Trim().ToLower(),
                FechaNacimiento = FechaNacimiento,
                FechaIngreso = DateTime.Today,
                TipoSocio = "Regular",
                Estado = "Activo"
            };

            SociosViewsModel.AgregarSocio(socio);

            MensajeExito = "El socio se ha registrado correctamente.";
        }

        LimpiarFormulario();
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

        ErrorNombre = "";
        ErrorApellidos = "";
        ErrorCedula = "";
        ErrorTelefono = "";
        ErrorCorreo = "";
    }
}
