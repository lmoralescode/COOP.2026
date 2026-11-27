
using COOPERATIVA.V2.Model;
using COOPERATIVA.V2.ViewsModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace COOPERATIVA.V2.Views;

public partial class PagosViewsPage : ContentPage
{
    public static ObservableCollection<Pagos> PagosRegistrados =>
        PagosViewsModel.PagosRegistrados;

    private Prestamos? _prestamoSeleccionado;

    // Se conserva la colección, pero ahora comparte
    // la misma instancia del ViewModel.
    private ObservableCollection<Pagos> _historialPagos;

    private readonly PagosViewsModel _vm;

    public PagosViewsPage()
    {
        InitializeComponent();

        // Obtener el ViewModel definido en el XAML.
        _vm = (PagosViewsModel)BindingContext;

        // Utilizar la colección original del ViewModel.
        _historialPagos = _vm.HistorialPagos;

        PrestamoPicker.ItemsSource = PrestamosViewsModel.Prestamos;

        PrestamoPicker.ItemDisplayBinding =
            new Binding("DescripcionPrestamo");

        // Ambas referencias utilizan la misma colección observable.
        ListaPagos.ItemsSource = _historialPagos;

        PrestamoPicker.SelectedIndexChanged +=
            PrestamoPicker_SelectedIndexChanged;
    }

    private void PrestamoPicker_SelectedIndexChanged(
        object? sender,
        EventArgs e)
    {
        _prestamoSeleccionado =
            PrestamoPicker.SelectedItem as Prestamos;

        // Sincronizar el préstamo seleccionado con el ViewModel.
        _vm.PrestamoSeleccionado = _prestamoSeleccionado;

        if (_prestamoSeleccionado == null)
        {
            LimpiarResumen();
            return;
        }

        ActualizarResumen();
        CargarHistorial();
    }

    private void ActualizarResumen()
    {
        // El ViewModel controla los valores enlazados al XAML.
        _vm.ActualizarPantalla();

        if (_prestamoSeleccionado == null)
            return;

        NumeroPrestamoLabel.Text =
            $"Préstamo: {_prestamoSeleccionado.NumeroPrestamo}";

        SaldoPendienteLabel.Text =
            $"Saldo pendiente: RD$ {_prestamoSeleccionado.SaldoPendiente:N2}";

        EstadoPrestamoLabel.Text =
            $"Estado: {_prestamoSeleccionado.Estado}";
    }

    private async void OnRegistrarPagoClicked(
        object sender,
        EventArgs e)
    {
        if (_prestamoSeleccionado == null)
        {
            await DisplayAlertAsync(
                "Validación",
                "Seleccione un préstamo.",
                "Aceptar");
            return;
        }

        if (!int.TryParse(
            NumeroCuotaEntry.Text,
            out int numeroCuota) ||
            numeroCuota <= 0)
        {
            await DisplayAlertAsync(
                "Validación",
                "Ingrese un número de cuota válido.",
                "Aceptar");
            return;
        }

        if (!decimal.TryParse(
            MontoPagoEntry.Text,
            out decimal monto) ||
            monto <= 0)
        {
            await DisplayAlertAsync(
                "Validación",
                "Ingrese un monto de pago válido.",
                "Aceptar");
            return;
        }

        if (_prestamoSeleccionado.SaldoPendiente <= 0)
        {
            await DisplayAlertAsync(
                "Préstamo pagado",
                "Este préstamo ya está completamente pagado.",
                "Aceptar");
            return;
        }

        if (monto > _prestamoSeleccionado.SaldoPendiente)
        {
            await DisplayAlertAsync(
                "Validación",
                "El pago no puede ser mayor que el saldo pendiente.",
                "Aceptar");
            return;
        }

        if (PagosRegistrados.Any(p =>
            p.PrestamoId == _prestamoSeleccionado.Id &&
            p.NumeroCuota == numeroCuota))
        {
            await DisplayAlertAsync(
                "Validación",
                "Esta cuota ya fue registrada.",
                "Aceptar");
            return;
        }

        Pagos nuevoPago = new()
        {
            Id = PagosRegistrados.Count == 0
                ? 1
                : PagosRegistrados.Max(p => p.Id) + 1,

            PrestamoId = _prestamoSeleccionado.Id,
            NumeroCuota = numeroCuota,
            SocioNombre = _prestamoSeleccionado.SocioNombre,
            Monto = monto,
            Fecha = (DateTime)FechaPagoPicker.Date,
            Estado = "Pagado"
        };

        // Actualizar el préstamo.
        _prestamoSeleccionado.TotalPagado += monto;

        // Registrar el pago en la colección compartida.
        PagosRegistrados.Add(nuevoPago);

        // Actualizar el resumen del ViewModel y la pantalla.
        _vm.ActualizarPantalla();

        // Actualizar también el historial.
        ActualizarResumen();
        CargarHistorial();

        await DisplayAlertAsync(
            "Pago registrado",
            "El pago se registró correctamente.",
            "Aceptar");

        LimpiarFormulario();
    }

    // Actualizar el historial sin crear otra colección.
    private void CargarHistorial()
    {
        _historialPagos.Clear();

        if (_prestamoSeleccionado == null)
            return;

        var historial = PagosRegistrados
            .Where(p => p.PrestamoId == _prestamoSeleccionado.Id)
            .OrderBy(p => p.NumeroCuota)
            .ToList();

        foreach (var pago in historial)
        {
            _historialPagos.Add(pago);
        }
    }

    private void LimpiarResumen()
    {
        _vm.PrestamoSeleccionado = null;
        _vm.ActualizarPantalla();
        _historialPagos.Clear();

        NumeroPrestamoLabel.Text = "Préstamo: --";

        SaldoPendienteLabel.Text =
            "Saldo pendiente: RD$ 0.00";

        EstadoPrestamoLabel.Text =
            "Estado: Sin seleccionar";

        _historialPagos.Clear();
    }

    private void LimpiarFormulario()
    {
        NumeroCuotaEntry.Text = string.Empty;
        MontoPagoEntry.Text = string.Empty;
        FechaPagoPicker.Date = DateTime.Today;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        PrestamoPicker.ItemsSource =
            PrestamosViewsModel.Prestamos;

        if (_prestamoSeleccionado != null)
        {
            ActualizarResumen();
            CargarHistorial();
        }

        // Sincronizar el resumen y el historial del ViewModel.
        _vm.ActualizarPantalla();
    }
}
