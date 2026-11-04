using COOPERATIVA.V2.Model;
using System.Collections.ObjectModel;
using System.Globalization;

namespace COOPERATIVA.V2.Views;

public partial class PrestamosViewsPage : ContentPage
{
    public static ObservableCollection<Prestamos> Prestamos { get; } = new();

    public PrestamosViewsPage()
    {
        InitializeComponent();

        SocioPrestamoPicker.ItemsSource = SociosViewsPage.Socios;
        SocioPrestamoPicker.ItemDisplayBinding =
            new Binding("NombreCompleto");

        ListaPrestamos.ItemsSource = Prestamos;
    }

    private bool ValidarDatos(out decimal monto, out decimal tasa, out int plazo)
    {
        monto = 0;
        tasa = 0;
        plazo = 0;

        if (SocioPrestamoPicker.SelectedItem == null)
        {
            DisplayAlertAsync("Validación", "Seleccione un socio.", "Aceptar");
            return false;
        }

        if (!decimal.TryParse(MontoPrestamoEntry.Text, out monto) || monto <= 0)
        {
            DisplayAlertAsync("Validación", "Ingrese un monto válido.", "Aceptar");
            return false;
        }

        if (!decimal.TryParse(TasaInteresEntry.Text, out tasa) ||
            tasa < 0 || tasa > 100)
        {
            DisplayAlertAsync("Validación", "Ingrese una tasa de interés válida.", "Aceptar");
            return false;
        }

        if (PlazoPicker.SelectedItem == null)
        {
            DisplayAlertAsync("Validación", "Seleccione el plazo del préstamo.", "Aceptar");
            return false;
        }

        string textoPlazo = PlazoPicker.SelectedItem.ToString()!;
        plazo = int.Parse(textoPlazo.Split(' ')[0]);

        return true;
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

    private void OnCalcularCuotaClicked(object sender, EventArgs e)
    {
        if (!ValidarDatos(out decimal monto, out decimal tasa, out int plazo))
            return;

        decimal cuota = CalcularCuota(monto, tasa, plazo);
        decimal total = cuota * plazo;

        CuotaLabel.Text = $"Cuota mensual estimada: RD$ {cuota:N2}";

        ResumenPrestamoLabel.Text =
            $"Monto: RD$ {monto:N2}\n" +
            $"Interés anual: {tasa:N2}%\n" +
            $"Plazo: {plazo} meses\n" +
            $"Total a pagar: RD$ {total:N2}";
    }

    private async void OnRegistrarPrestamoClicked(object sender, EventArgs e)
    {
        if (!ValidarDatos(out decimal monto, out decimal tasa, out int plazo))
            return;

        Socios socio = (Socios)SocioPrestamoPicker.SelectedItem;

        decimal cuota = CalcularCuota(monto, tasa, plazo);
        decimal total = cuota * plazo;

        int nuevoId = Prestamos.Count + 1;

        Prestamos nuevoPrestamo = new()
        {
            Id = nuevoId,
            NumeroPrestamo = $"PRE-{nuevoId:0000}",
            SocioNombre = socio.NombreCompleto,
            SocioCedula = socio.Cedula,
            Monto = monto,
            TasaInteres = tasa,
            PlazoMeses = plazo,
            CuotaMensual = cuota,
            TotalPagar = total,
            TotalPagado = 0,
            Fecha = DateTime.Today
        };

        Prestamos.Add(nuevoPrestamo);

        ListaPrestamos.ItemsSource = null;
        ListaPrestamos.ItemsSource = Prestamos;

        await DisplayAlertAsync(
            "Préstamo registrado",
            $"Se registró el préstamo {nuevoPrestamo.NumeroPrestamo} correctamente.",
            "Aceptar");

        LimpiarFormulario();
    }

    private void LimpiarFormulario()
    {
        SocioPrestamoPicker.SelectedItem = null;
        MontoPrestamoEntry.Text = string.Empty;
        TasaInteresEntry.Text = string.Empty;
        PlazoPicker.SelectedItem = null;

        CuotaLabel.Text = "Cuota mensual estimada: RD$ 0.00";
        ResumenPrestamoLabel.Text = string.Empty;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        SocioPrestamoPicker.ItemsSource = SociosViewsPage.Socios;
        ListaPrestamos.ItemsSource = Prestamos;
    }
}