using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using COOPERATIVA.V2.Model;
using COOPERATIVA.V2.ViewsModel;
using System.Collections.ObjectModel;
using System.Globalization;

namespace COOPERATIVA.V2.ViewsModel;

public partial class PagosViewsModel : BaseViewsModel
{
    public static ObservableCollection<Pagos> PagosRegistrados { get; } = new();

    public ObservableCollection<Prestamos> ListaPrestamos =>
        PrestamosViewsModel.Prestamos;

    public ObservableCollection<Pagos> HistorialPagos { get; } = new();

    [ObservableProperty] private Prestamos? prestamoSeleccionado;
    [ObservableProperty] private string numeroCuotaTexto = "";
    [ObservableProperty] private string montoTexto = "";
    [ObservableProperty] private DateTime fechaPago = DateTime.Today;

    [ObservableProperty] private string errorPrestamo = "";
    [ObservableProperty] private string errorNumeroCuota = "";
    [ObservableProperty] private string errorMonto = "";

    [ObservableProperty] private string numeroPrestamoResumen = "Préstamo: --";
    [ObservableProperty] private string saldoPendienteResumen = "Saldo pendiente: RD$ 0.00";
    [ObservableProperty] private string estadoResumen = "Estado: Sin seleccionar";

    partial void OnPrestamoSeleccionadoChanged(Prestamos? value)
    {
        ActualizarResumen();
        CargarHistorial();
        ErrorPrestamo = "";
    }

    private void ActualizarResumen()
    {
        if (PrestamoSeleccionado == null)
        {
            NumeroPrestamoResumen = "Préstamo: --";
            SaldoPendienteResumen = "Saldo pendiente: RD$ 0.00";
            EstadoResumen = "Estado: Sin seleccionar";
            return;
        }

        NumeroPrestamoResumen =
            $"Préstamo: {PrestamoSeleccionado.NumeroPrestamo}";

        SaldoPendienteResumen =
            $"Saldo pendiente: RD$ {PrestamoSeleccionado.SaldoPendiente:N2}";

        EstadoResumen = $"Estado: {PrestamoSeleccionado.Estado}";
    }

    private void CargarHistorial()
    {
        HistorialPagos.Clear();

        if (PrestamoSeleccionado == null)
            return;

        var pagos = PagosRegistrados
            .Where(p => p.PrestamoId == PrestamoSeleccionado.Id)
            .OrderBy(p => p.NumeroCuota);

        foreach (var pago in pagos)
            HistorialPagos.Add(pago);
    }

    [RelayCommand]
    private void RegistrarPago()
    {
        ErrorPrestamo = "";
        ErrorNumeroCuota = "";
        ErrorMonto = "";

        bool valido = true;

        if (PrestamoSeleccionado == null)
        {
            ErrorPrestamo = "Selecciona el préstamo que deseas pagar.";
            valido = false;
        }

        if (!int.TryParse(NumeroCuotaTexto, out int numeroCuota) ||
            numeroCuota <= 0)
        {
            ErrorNumeroCuota = "El número de cuota debe ser mayor que cero.";
            valido = false;
        }

        if (!decimal.TryParse(MontoTexto, NumberStyles.Number,
            CultureInfo.CurrentCulture, out decimal monto) || monto <= 0)
        {
            ErrorMonto = "El monto del pago debe ser mayor que cero.";
            valido = false;
        }

        if (!valido)
            return;

        if (PrestamoSeleccionado!.SaldoPendiente <= 0)
        {
            ErrorPrestamo = "Este préstamo ya está completamente pagado.";
            return;
        }

        if (monto > PrestamoSeleccionado.SaldoPendiente)
        {
            ErrorMonto =
                $"El pago no puede superar el saldo de RD$ {PrestamoSeleccionado.SaldoPendiente:N2}.";
            return;
        }

        var pago = new Pagos
        {
            Id = PagosRegistrados.Count == 0
                ? 1
                : PagosRegistrados.Max(p => p.Id) + 1,

            PrestamoId = PrestamoSeleccionado.Id,
            NumeroCuota = numeroCuota,
            SocioNombre = PrestamoSeleccionado.SocioNombre,
            Monto = monto,
            Fecha = FechaPago,
            Estado = "Pagado"
        };

        PrestamoSeleccionado.TotalPagado += monto;

        PagosRegistrados.Add(pago);

        MensajeExito = "El pago se registró correctamente.";

        LimpiarFormulario();
        ActualizarResumen();
        CargarHistorial();
    }

    [RelayCommand]
    private void LimpiarFormulario()
    {
        NumeroCuotaTexto = "";
        MontoTexto = "";
        FechaPago = DateTime.Today;

        ErrorNumeroCuota = "";
        ErrorMonto = "";
    }
}