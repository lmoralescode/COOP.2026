
namespace COOPERATIVA.V2.Model;

public class Ahorros
{
    public int Id { get; set; }
    public int SocioId { get; set; }
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Today;
    public string TipoAhorro { get; set; } = "Ordinario";
    public string Observacion { get; set; } = string.Empty;
}
