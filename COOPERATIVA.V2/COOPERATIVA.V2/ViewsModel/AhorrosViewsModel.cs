using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using COOPERATIVA.V2.Model;
using COOPERATIVA.V2.ViewsModel;
using System.Collections.ObjectModel;
using System.Globalization;

namespace COOPERATIVA.V2.ViewModels;

public partial class AhorrosViewModel : BaseViewsModel
{
    public static ObservableCollection<Ahorros> Movimientos { get; } = new();

    public ObservableCollection<Socios> ListaSocios =>
        SociosViewsModel.Socios;

    public ObservableCollection<Ahorros> MovimientosSocio { get; } = new();

    [ObservableProperty] private Socios? socioSeleccionado;
    [ObservableProperty] private string tipoMovimiento = "";
    [ObservableProperty] private string montoTexto = "";
    [ObservableProperty] private DateTime fechaMovimiento = DateTime.Today;
    [ObservableProperty] private decimal balance;
    [ObservableProperty] private string errorSocio = "";
    [ObservableProperty] private string errorTipo = "";
    [ObservableProperty] private string errorMonto = "";

    partial void OnSocioSeleccionadoChanged(Socios? value)
    {
        ErrorSocio = "";
        ActualizarBalance();
        CargarMovimientos();
    }

    private decimal CalcularBalance(string cedula)
    {
        return Movimientos
            .Where(m => m.SocioCedula == cedula)
            .Sum(m => m.Tipo == "Depósito" ? m.Monto : -m.Monto);
    }

    private void ActualizarBalance()
    {
        Balance = SocioSeleccionado == null
            ? 0
            : CalcularBalance(SocioSeleccionado.Cedula);
    }

    private void CargarMovimientos()
    {
        MovimientosSocio.Clear();

        if (SocioSeleccionado == null)
            return;

        var lista = Movimientos
            .Where(m => m.SocioCedula == SocioSeleccionado.Cedula)
            .OrderByDescending(m => m.Fecha)
            .ThenByDescending(m => m.Id);

        foreach (var movimiento in lista)
            MovimientosSocio.Add(movimiento);
    }

    [RelayCommand]
    private void RegistrarMovimiento()
    {
        ErrorSocio = "";
        ErrorTipo = "";
        ErrorMonto = "";

        bool valido = true;

        if (SocioSeleccionado == null)
        {
            ErrorSocio = "Selecciona el socio que realizará la operación.";
            valido = false;
        }

        if (TipoMovimiento != "Depósito" &&
            TipoMovimiento != "Retiro")
        {
            ErrorTipo = "Selecciona Depósito o Retiro.";
            valido = false;
        }

        if (!decimal.TryParse(
            MontoTexto,
            NumberStyles.Number,
            CultureInfo.CurrentCulture,
            out decimal monto) || monto <= 0)
        {
            ErrorMonto = "El monto debe ser un número mayor que cero.";
            valido = false;
        }

        if (!valido)
            return;

        decimal balanceActual = CalcularBalance(SocioSeleccionado!.Cedula);

        if (TipoMovimiento == "Retiro" && monto > balanceActual)
        {
            ErrorMonto = $"Fondos insuficientes. Disponible: RD$ {balanceActual:N2}.";
            return;
        }

        var movimiento = new Ahorros
        {
            Id = Movimientos.Count == 0 ? 1 : Movimientos.Max(m => m.Id) + 1,
            SocioNombre = SocioSeleccionado.NombreCompleto,
            SocioCedula = SocioSeleccionado.Cedula,
            Tipo = TipoMovimiento,
            Monto = monto,
            Fecha = FechaMovimiento
        };

        Movimientos.Add(movimiento);

        MensajeExito = $"{TipoMovimiento} registrado correctamente por RD$ {monto:N2}.";

        MontoTexto = "";
        TipoMovimiento = "";
        FechaMovimiento = DateTime.Today;

        ActualizarBalance();
        CargarMovimientos();
    }
}