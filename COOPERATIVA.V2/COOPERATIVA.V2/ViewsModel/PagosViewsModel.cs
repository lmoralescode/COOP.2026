
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using COOPERATIVA.V2.Model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;

namespace COOPERATIVA.V2.ViewsModel;

public partial class PagosViewsModel : BaseViewsModel
{
    // Colección compartida de pagos.
    public static ObservableCollection<Pagos> PagosRegistrados { get; } = new();

    // Utiliza la misma colección donde se registran los préstamos.
    public ObservableCollection<Prestamos> ListaPrestamos =>
        PrestamosViewsModel.Prestamos;

    // Historial que se muestra en el XAML.
    public ObservableCollection<Pagos> HistorialPagos { get; } = new();

    [ObservableProperty]
    private Prestamos? prestamoSeleccionado;

    [ObservableProperty]
    private string numeroCuotaTexto = "";

    [ObservableProperty]
    private string montoTexto = "";

    [ObservableProperty]
    private DateTime fechaPago = DateTime.Today;

    [ObservableProperty]
    private string errorPrestamo = "";

    [ObservableProperty]
    private string errorNumeroCuota = "";

    [ObservableProperty]
    private string errorMonto = "";

    [ObservableProperty]
    private string numeroPrestamoResumen = "Préstamo: --";

    [ObservableProperty]
    private string saldoPendienteResumen = "Saldo pendiente: RD$ 0.00";

    [ObservableProperty]
    private string estadoResumen = "Estado: Sin seleccionar";

    public PagosViewsModel()
    {
        PagosRegistrados.CollectionChanged +=
            PagosRegistrados_CollectionChanged;

        CargarHistorial();
    }

    // NUEVO: actualizar resumen e historial desde la página.
    public void ActualizarPantalla()
    {
        ActualizarResumen();
        CargarHistorial();
    }

    // Detectar cuando se registra un nuevo pago.
    private void PagosRegistrados_CollectionChanged(
        object? sender,
        NotifyCollectionChangedEventArgs e)
    {
        CargarHistorial();
        ActualizarResumen();
    }

    // Detectar cuando se selecciona otro préstamo.
    partial void OnPrestamoSeleccionadoChanged(Prestamos? value)
    {
        ErrorPrestamo = "";

        ActualizarResumen();
        CargarHistorial();
    }

    // Actualizar los datos del préstamo seleccionado.
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

        EstadoResumen =
            $"Estado: {PrestamoSeleccionado.Estado}";
    }

    // Cargar únicamente los pagos del préstamo seleccionado.
    private void CargarHistorial()
    {
        HistorialPagos.Clear();

        if (PrestamoSeleccionado == null)
            return;

        var pagos = PagosRegistrados
            .Where(p => p.PrestamoId == PrestamoSeleccionado.Id)
            .OrderBy(p => p.NumeroCuota)
            .ToList();

        foreach (var pago in pagos)
        {
            HistorialPagos.Add(pago);
        }
    }

    // Registrar un nuevo pago.
    [RelayCommand]
    private void RegistrarPago()
    {
        ErrorPrestamo = "";
        ErrorNumeroCuota = "";
        ErrorMonto = "";
        MensajeExito = "";

        bool valido = true;

        if (PrestamoSeleccionado == null)
        {
            ErrorPrestamo = "Selecciona el préstamo que deseas pagar.";
            valido = false;
        }

        if (!int.TryParse(
            NumeroCuotaTexto,
            out int numeroCuota) || numeroCuota <= 0)
        {
            ErrorNumeroCuota =
                "El número de cuota debe ser mayor que cero.";

            valido = false;
        }

        if (!decimal.TryParse(
            MontoTexto,
            NumberStyles.Number,
            CultureInfo.CurrentCulture,
            out decimal monto) || monto <= 0)
        {
            ErrorMonto =
                "El monto del pago debe ser mayor que cero.";

            valido = false;
        }

        if (!valido)
            return;

        Prestamos prestamo = PrestamoSeleccionado!;

        // Comprobar si la cuota ya fue pagada.
        if (PagosRegistrados.Any(p =>
            p.PrestamoId == prestamo.Id &&
            p.NumeroCuota == numeroCuota))
        {
            ErrorNumeroCuota =
                "Esta cuota ya fue registrada.";

            return;
        }

        // Comprobar si el préstamo ya está pagado.
        if (prestamo.SaldoPendiente <= 0)
        {
            ErrorPrestamo =
                "Este préstamo ya está completamente pagado.";

            return;
        }

        // Evitar pagos superiores al saldo pendiente.
        if (monto > prestamo.SaldoPendiente)
        {
            ErrorMonto =
                $"El pago no puede superar el saldo de RD$ {prestamo.SaldoPendiente:N2}.";

            return;
        }

        // Crear el registro.
        var nuevoPago = new Pagos
        {
            Id = PagosRegistrados.Count == 0
                ? 1
                : PagosRegistrados.Max(p => p.Id) + 1,

            PrestamoId = prestamo.Id,
            NumeroCuota = numeroCuota,
            SocioNombre = prestamo.SocioNombre,
            Monto = monto,
            Fecha = FechaPago,
            Estado = "Pagado"
        };

        // Actualizar el préstamo.
        prestamo.TotalPagado += monto;

        // Registrar el pago en la colección compartida.
        PagosRegistrados.Add(nuevoPago);

        // Actualizar los datos de pantalla.
        ActualizarResumen();
        CargarHistorial();

        MensajeExito =
            $"El pago de la cuota {numeroCuota} se registró correctamente.";

        LimpiarFormulario();
    }

    // Limpiar los campos después del registro.
    [RelayCommand]
    private void LimpiarFormulario()
    {
        NumeroCuotaTexto = "";
        MontoTexto = "";
        FechaPago = DateTime.Today;

        ErrorPrestamo = "";
        ErrorNumeroCuota = "";
        ErrorMonto = "";
    }
}
