namespace COOPERATIVA.V2.Model;

public class Pagos
{
    public int Id { get; set; }

    public int PrestamoId { get; set; }

    public int NumeroCuota { get; set; }

    public string SocioNombre { get; set; } = string.Empty;

    public decimal Monto { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Today;

    public string Estado { get; set; } = "Pagado";
}