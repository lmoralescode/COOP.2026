namespace COOPERATIVA.V2.Views;

public class Socios
{
    public int Codigo { get; set; }
    public string NombreCompleto { get; set; } = "";
    public string Apellidos { get; set; } = "";
    public string Cedula { get; set; } = "";
    public DateTime FechaNacimiento { get; set; }
    public string Telefono { get; set; } = "";
    public string Direccion { get; set; } = "";
    public string Correo { get; set; } = "";
    public DateTime FechaIngreso { get; set; } = DateTime.Today;
    public string Tipo { get; set; } = "Regular";
    public string Estado { get; set; } = "Activo";
}

public class Ahorros
{
    public int CodigoSocio { get; set; }
    public string Tipo { get; set; } = "";
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
}

public class Prestamos
{
    public int Id { get; set; }
    public int CodigoSocio { get; set; }
    public decimal Monto { get; set; }
    public decimal TasaAnual { get; set; }
    public int PlazoMeses { get; set; }
    public decimal Cuota { get; set; }
    public decimal SaldoPendiente { get; set; }
    public string Estado { get; set; } = "Pendiente";
}

public class Pagos
{
    public int IdPrestamo { get; set; }
    public int NumeroCuota { get; set; }
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
}