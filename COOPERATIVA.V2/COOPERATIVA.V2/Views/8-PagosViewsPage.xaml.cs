using COOPERATIVA.V2.Model;
using System.Collections.ObjectModel;

namespace COOPERATIVA.V2.Views;

public partial class PagosViewsPage : ContentPage
{
    public static ObservableCollection<Pagos> PagosRegistrados { get; } = new();

    private Prestamos? _prestamoSeleccionado;

    public PagosViewsPage()
    {
        InitializeComponent();

        PrestamoPicker.ItemsSource = PrestamosViewsPage.Prestamos;
        PrestamoPicker.ItemDisplayBinding = new Binding("NumeroPrestamo");

        ListaPagos.ItemsSource = PagosRegistrados;

        PrestamoPicker.SelectedIndexChanged += PrestamoPicker_SelectedIndexChanged;
    }

    // Detectar el préstamo seleccionado
    private void PrestamoPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        _prestamoSeleccionado = PrestamoPicker.SelectedItem as Prestamos;

        if (_prestamoSeleccionado == null)
        {
            LimpiarResumen();
            return;
        }

        ActualizarResumen();
        CargarHistorial();
    }

    // Mostrar información del préstamo
    private void ActualizarResumen()
    {
        if (_prestamoSeleccionado == null)
            return;

        NumeroPrestamoLabel.Text =
            $"Préstamo: {_prestamoSeleccionado.NumeroPrestamo}";

        SaldoPendienteLabel.Text =
            $"Saldo pendiente: RD$ {_prestamoSeleccionado.SaldoPendiente:N2}";

        EstadoPrestamoLabel.Text =
            $"Estado: {_prestamoSeleccionado.Estado}";
    }

    // Registrar un pago
    private async void OnRegistrarPagoClicked(object sender, EventArgs e)
    {
        if (_prestamoSeleccionado == null)
        {
            await DisplayAlertAsync("Validación", "Seleccione un préstamo.", "Aceptar");
            return;
        }

        if (!int.TryParse(NumeroCuotaEntry.Text, out int numeroCuota) ||
            numeroCuota <= 0)
        {
            await DisplayAlertAsync("Validación", "Ingrese un número de cuota válido.", "Aceptar");
            return;
        }

        if (!decimal.TryParse(MontoPagoEntry.Text, out decimal monto) || monto <= 0)
        {
            await DisplayAlertAsync("Validación", "Ingrese un monto de pago válido.", "Aceptar");
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

        if (_prestamoSeleccionado.SaldoPendiente <= 0)
        {
            await DisplayAlertAsync(
                "Préstamo pagado",
                "Este préstamo ya está completamente pagado.",
                "Aceptar");

            return;
        }

        // Crear el registro del pago
        Pagos nuevoPago = new()
        {
            Id = PagosRegistrados.Count + 1,
            PrestamoId = _prestamoSeleccionado.Id,
            NumeroCuota = numeroCuota,
            SocioNombre = _prestamoSeleccionado.SocioNombre,
            Monto = monto,
            Fecha = DateTime.Today,
            Estado = "Pagado"
        };

        // Actualizar el préstamo
        _prestamoSeleccionado.TotalPagado += (int)monto;

        // Guardar el pago
        PagosRegistrados.Add(nuevoPago);

        // Actualizar pantalla
        ActualizarResumen();
        CargarHistorial();

        await DisplayAlertAsync(
            "Pago registrado",
            "El pago se registró correctamente.",
            "Aceptar");

        LimpiarFormulario();
    }

    // Mostrar historial del préstamo seleccionado
    private void CargarHistorial()
    {
        if (_prestamoSeleccionado == null)
        {
            ListaPagos.ItemsSource = null;
            return;
        }

        var historial = PagosRegistrados
            .Where(p => p.PrestamoId == _prestamoSeleccionado.Id)
            .OrderBy(p => p.NumeroCuota)
            .ToList();

        ListaPagos.ItemsSource = historial;
    }

    private void LimpiarResumen()
    {
        NumeroPrestamoLabel.Text = "Préstamo: --";
        SaldoPendienteLabel.Text = "Saldo pendiente: RD$ 0.00";
        EstadoPrestamoLabel.Text = "Estado: Sin seleccionar";

        ListaPagos.ItemsSource = null;
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

        PrestamoPicker.ItemsSource = PrestamosViewsPage.Prestamos;

        if (_prestamoSeleccionado != null)
        {
            ActualizarResumen();
            CargarHistorial();
        }
    }
}