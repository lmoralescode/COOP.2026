
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
}
