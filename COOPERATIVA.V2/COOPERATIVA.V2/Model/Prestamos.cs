namespace COOPERATIVA.V2.Model;

public class Prestamos
{
    public int Id { get; set; }

    public string NumeroPrestamo { get; set; } = string.Empty;

    public string SocioNombre { get; set; } = string.Empty;

    public string SocioCedula { get; set; } = string.Empty;

    public decimal Monto { get; set; }

    public decimal TasaInteres { get; set; }

    public int PlazoMeses { get; set; }

    public decimal CuotaMensual { get; set; }

    public decimal TotalPagar { get; set; }

    public decimal TotalPagado { get; set; }

    public decimal SaldoPendiente =>
        Math.Max(0m, TotalPagar - TotalPagado);

    public DateTime Fecha { get; set; } = DateTime.Today;

    public string Estado =>
        SaldoPendiente <= 0 ? "Pagado" : "Pendiente";
}