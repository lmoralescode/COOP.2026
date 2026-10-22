
namespace COOPERATIVA.V2.Model;

public class Pagos
{
    public int Id { get; set; }
    public int SocioId { get; set; }
    public int PrestamoId { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaPago { get; set; } = DateTime.Today;
    public string MetodoPago { get; set; } = "Efectivo";
    public string Observacion { get; set; } = string.Empty;
}
