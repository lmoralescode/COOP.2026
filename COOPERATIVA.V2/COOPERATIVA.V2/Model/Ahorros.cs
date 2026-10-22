namespace COOPERATIVA.V2.Model;

public class Ahorros
{
    public int Id { get; set; }

    public string SocioNombre { get; set; } = string.Empty;

    public string SocioCedula { get; set; } = string.Empty;

    public string Tipo { get; set; } = string.Empty;

    public decimal Monto { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Today;
}