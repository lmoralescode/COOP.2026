using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using COOPERATIVA.V2.Model;
using COOPERATIVA.V2.ViewsModel;
using System.Collections.ObjectModel;
using System.Globalization;

namespace COOPERATIVA.V2.ViewsModel;

public partial class PrestamosViewsModel : BaseViewsModel
{
    public static ObservableCollection<Prestamos> Prestamos { get; } = new();

    public ObservableCollection<Socios> ListaSocios =>
        SociosViewsModel.Socios;

    public ObservableCollection<string> Plazos { get; } =
        new() { "6 meses", "12 meses", "18 meses", "24 meses", "36 meses" };

    [ObservableProperty] private Socios? socioSeleccionado;
    [ObservableProperty] private string montoTexto = "";
    [ObservableProperty] private string tasaTexto = "";
    [ObservableProperty] private string plazoSeleccionado = "";

    [ObservableProperty] private string errorSocio = "";
    [ObservableProperty] private string errorMonto = "";
    [ObservableProperty] private string errorTasa = "";
    [ObservableProperty] private string errorPlazo = "";

    [ObservableProperty] private decimal cuotaMensual;
    [ObservableProperty] private decimal totalPagar;
    [ObservableProperty] private string resumenPrestamo = "";

    private bool Validar(out decimal monto, out decimal tasa, out int plazo)
    {
        monto = 0;
        tasa = 0;
        plazo = 0;

        ErrorSocio = "";
        ErrorMonto = "";
        ErrorTasa = "";
        ErrorPlazo = "";

        bool valido = true;

        if (SocioSeleccionado == null)
        {
            ErrorSocio = "Selecciona el socio solicitante.";
            valido = false;
        }

        if (!decimal.TryParse(MontoTexto, NumberStyles.Number,
            CultureInfo.CurrentCulture, out monto) || monto <= 0)
        {
            ErrorMonto = "El monto debe ser mayor que RD$ 0.00.";
            valido = false;
        }

        if (!decimal.TryParse(TasaTexto, NumberStyles.Number,
            CultureInfo.CurrentCulture, out tasa) || tasa < 0 || tasa > 100)
        {
            ErrorTasa = "La tasa anual debe estar entre 0 y 100%.";
            valido = false;
        }

        if (string.IsNullOrWhiteSpace(PlazoSeleccionado))
        {
            ErrorPlazo = "Selecciona el plazo del préstamo.";
            valido = false;
        }
        else
        {
            int.TryParse(PlazoSeleccionado.Split(' ')[0], out plazo);
        }

        return valido;
    }

    private decimal CalcularCuota(decimal monto, decimal tasa, int plazo)
    {
        decimal tasaMensual = tasa / 100 / 12;

        if (tasaMensual == 0)
            return Math.Round(monto / plazo, 2);

        double r = (double)tasaMensual;
        double n = plazo;

        double cuota = (double)monto * r /
                       (1 - Math.Pow(1 + r, -n));

        return Math.Round((decimal)cuota, 2);
    }

    [RelayCommand]
    private void CalcularCuota()
    {
        if (!Validar(out decimal monto, out decimal tasa, out int plazo))
            return;

        CuotaMensual = CalcularCuota(monto, tasa, plazo);
        TotalPagar = CuotaMensual * plazo;

        ResumenPrestamo =
            $"Monto: RD$ {monto:N2}\n" +
            $"Interés anual: {tasa:N2}%\n" +
            $"Plazo: {plazo} meses\n" +
            $"Total a pagar: RD$ {TotalPagar:N2}";
    }

    [RelayCommand]
    private void RegistrarPrestamo()
    {
        if (!Validar(out decimal monto, out decimal tasa, out int plazo))
            return;

        decimal cuota = CalcularCuota(monto, tasa, plazo);
        decimal total = cuota * plazo;

        int nuevoId = Prestamos.Count == 0
            ? 1
            : Prestamos.Max(p => p.Id) + 1;

        var prestamo = new Prestamos
        {
            Id = nuevoId,
            NumeroPrestamo = $"PRE-{nuevoId:0000}",
            SocioNombre = SocioSeleccionado!.NombreCompleto,
            SocioCedula = SocioSeleccionado.Cedula,
            Monto = monto,
            TasaInteres = tasa,
            PlazoMeses = plazo,
            CuotaMensual = cuota,
            TotalPagar = total,
            TotalPagado = 0,
            Fecha = DateTime.Today
        };

        Prestamos.Add(prestamo);

        MensajeExito = $"El préstamo {prestamo.NumeroPrestamo} se registró correctamente.";

        LimpiarFormulario();
    }

    [RelayCommand]
    private void LimpiarFormulario()
    {
        SocioSeleccionado = null;
        MontoTexto = "";
        TasaTexto = "";
        PlazoSeleccionado = "";

        CuotaMensual = 0;
        TotalPagar = 0;
        ResumenPrestamo = "";

        ErrorSocio = "";
        ErrorMonto = "";
        ErrorTasa = "";
        ErrorPlazo = "";
    }
}