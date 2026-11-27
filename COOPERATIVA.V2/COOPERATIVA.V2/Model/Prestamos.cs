
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace COOPERATIVA.V2.Model;

public class Prestamos : INotifyPropertyChanged
{
    public int Id { get; set; }

    public string NumeroPrestamo { get; set; } = string.Empty;

    public string SocioNombre { get; set; } = string.Empty;

    public string SocioCedula { get; set; } = string.Empty;

    public decimal Monto { get; set; }

    public decimal TasaInteres { get; set; }

    public int PlazoMeses { get; set; }

    public decimal CuotaMensual { get; set; }

    private decimal totalPagar;

    public decimal TotalPagar
    {
        get => totalPagar;
        set
        {
            if (totalPagar == value)
                return;

            totalPagar = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(SaldoPendiente));
            OnPropertyChanged(nameof(Estado));
        }
    }

    private decimal totalPagado;

    public decimal TotalPagado
    {
        get => totalPagado;
        set
        {
            if (totalPagado == value)
                return;

            totalPagado = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(SaldoPendiente));
            OnPropertyChanged(nameof(Estado));
        }
    }

    public decimal SaldoPendiente =>
        Math.Max(0m, TotalPagar - TotalPagado);

    public DateTime Fecha { get; set; } = DateTime.Today;

    public string Estado =>
        SaldoPendiente <= 0 ? "Pagado" : "Pendiente";

    // Número de préstamo y nombre del socio.
    public string DescripcionPrestamo =>
        $"{NumeroPrestamo} - {SocioNombre}";

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
