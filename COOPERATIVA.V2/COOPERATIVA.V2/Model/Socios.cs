
namespace COOPERATIVA.V2.Model;

public class Socios
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string NombreCompleto =>
        $"{Nombre} {Apellidos}".Trim();

    public string Cedula { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    public string Correo { get; set; } = string.Empty;

    public DateTime FechaNacimiento { get; set; }

    public DateTime FechaIngreso { get; set; } = DateTime.Today;

    public string TipoSocio { get; set; } = "Regular";

    public string Estado { get; set; } = "Activo";
}
