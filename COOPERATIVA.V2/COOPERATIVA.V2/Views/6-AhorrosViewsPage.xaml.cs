using COOPERATIVA.V2.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace COOPERATIVA.V2.Views;

public partial class AhorrosViewsPage : ContentPage
{
    public static ObservableCollection<Ahorros> Movimientos { get; } = new();

    public AhorrosViewsPage()
    {
        InitializeComponent();

        // Cargar los socios registrados.
        SocioPicker.ItemsSource = SociosViewsPage.Socios;
        SocioPicker.ItemDisplayBinding =
            new Binding(nameof(Socios.NombreCompleto));

        // Mostrar los movimientos del socio seleccionado.
        ListaMovimientos.ItemsSource = new ObservableCollection<Ahorros>();

        SocioPicker.SelectedIndexChanged += SocioPicker_SelectedIndexChanged;
    }

    private void SocioPicker_SelectedIndexChanged(
        object? sender, EventArgs e)
    {
        ActualizarBalance();
        CargarMovimientos();
    }

    private Socios? ObtenerSocioSeleccionado()
    {
        return SocioPicker.SelectedItem as Socios;
    }

    private decimal CalcularBalance(string cedula)
    {
        return Movimientos
            .Where(m => m.SocioCedula == cedula)
            .Sum(m => m.Tipo == "Depósito" ? m.Monto : -m.Monto);
    }

    private void ActualizarBalance()
    {
        Socios? socio = ObtenerSocioSeleccionado();

        if (socio == null)
        {
            BalanceLabel.Text = "RD$ 0.00";
            return;
        }

        decimal balance = CalcularBalance(socio.Cedula);

        BalanceLabel.Text = $"RD$ {balance:N2}";
    }

    private void CargarMovimientos()
    {
        Socios? socio = ObtenerSocioSeleccionado();

        var movimientosSocio = new ObservableCollection<Ahorros>();

        if (socio != null)
        {
            var lista = Movimientos
                .Where(m => m.SocioCedula == socio.Cedula)
                .OrderByDescending(m => m.Fecha)
                .ThenByDescending(m => m.Id);

            foreach (Ahorros movimiento in lista)
            {
                movimientosSocio.Add(movimiento);
            }
        }

        ListaMovimientos.ItemsSource = movimientosSocio;
    }

    private async void OnRegistrarMovimientoClicked(
        object sender, EventArgs e)
    {
        Socios? socio = ObtenerSocioSeleccionado();

        if (socio == null)
        {
            await DisplayAlertAsync(
                "Validación",
                "Debes seleccionar un socio.",
                "Aceptar");
            return;
        }

        if (TipoMovimientoPicker.SelectedItem == null)
        {
            await DisplayAlertAsync(
                "Validación",
                "Selecciona el tipo de movimiento.",
                "Aceptar");
            return;
        }

        if (!decimal.TryParse(
                MontoEntry.Text,
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.CurrentCulture,
                out decimal monto)
            || monto <= 0)
        {
            await DisplayAlertAsync(
                "Validación",
                "Introduce un monto válido mayor que cero.",
                "Aceptar");
            return;
        }

        string tipo = TipoMovimientoPicker.SelectedItem.ToString()!;

        decimal balanceActual = CalcularBalance(socio.Cedula);

        if (tipo == "Retiro" && monto > balanceActual)
        {
            await DisplayAlertAsync(
                "Fondos insuficientes",
                $"El balance disponible es RD$ {balanceActual:N2}.",
                "Aceptar");
            return;
        }

        var movimiento = new Ahorros
        {
            Id = Movimientos.Count == 0
                ? 1
                : Movimientos.Max(m => m.Id) + 1,

            SocioNombre = $"{socio.Nombre} {socio.Apellidos}",
            SocioCedula = socio.Cedula,
            Tipo = tipo,
            Monto = monto,
            Fecha = DateTime.Today
        };

        Movimientos.Add(movimiento);

        ActualizarBalance();
        CargarMovimientos();

        MontoEntry.Text = string.Empty;
        TipoMovimientoPicker.SelectedItem = null;
        FechaMovimientoPicker.Date = DateTime.UtcNow;

        await DisplayAlertAsync(
            "Operación exitosa",
            $"{tipo} registrado correctamente por RD$ {monto:N2}.",
            "Aceptar");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        ActualizarBalance();
        CargarMovimientos();
    }
}