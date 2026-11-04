
namespace COOPERATIVA.V2.Model;

public class Prestamos
{
    public int Id { get; set; }
    public int SocioId { get; set; }
    public decimal MontoSolicitado { get; set; }
    public decimal TasaInteres { get; set; }
    public int PlazoMeses { get; set; }
    public DateTime FechaSolicitud { get; set; } = DateTime.Today;
    public string Estado { get; set; } = "Pendiente";
    public string NumeroPrestamo { get; internal set; }
    public string SocioNombre { get; internal set; }
    public string SocioCedula { get; internal set; }
    public decimal Monto { get; internal set; }
    public decimal CuotaMensual { get; internal set; }
    public decimal TotalPagar { get; internal set; }
    public int TotalPagado { get; internal set; }
    public DateTime Fecha { get; internal set; }
    public decimal SaldoPendiente => Math.Max(0m, TotalPagar - TotalPagado);
}
