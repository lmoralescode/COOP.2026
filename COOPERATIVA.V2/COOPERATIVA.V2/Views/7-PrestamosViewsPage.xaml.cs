
using COOPERATIVA.V2.Model;
using COOPERATIVA.V2.ViewsModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace COOPERATIVA.V2.Views;

public partial class PrestamosViewsPage : ContentPage
{
    // Utiliza la misma colección compartida con la pantalla de Pagos.
    public static ObservableCollection<Prestamos> Prestamos =>
        PrestamosViewsModel.Prestamos;

    public PrestamosViewsPage()
    {
        InitializeComponent();

        SocioPrestamoPicker.ItemsSource = SociosViewsModel.Socios;

        SocioPrestamoPicker.ItemDisplayBinding =
            new Binding("NombreCompleto");

        ListaPrestamos.ItemsSource = Prestamos;
    }

    // Validación síncrona: no utiliza async ni parámetros incompatibles.
    private bool ValidarDatos(
        out decimal monto,
        out decimal tasa,
        out int plazo,
        out string mensaje)
    {
        monto = 0;
        tasa = 0;
        plazo = 0;
        mensaje = "";

        if (SocioPrestamoPicker.SelectedItem == null)
        {
            mensaje = "Seleccione un socio.";
            return false;
        }

        if (!decimal.TryParse(MontoPrestamoEntry.Text, out monto) ||
            monto <= 0)
        {
            mensaje = "Ingrese un monto válido.";
            return false;
        }

        if (!decimal.TryParse(TasaInteresEntry.Text, out tasa) ||
            tasa < 0 || tasa > 100)
        {
            mensaje = "Ingrese una tasa de interés válida entre 0 y 100%.";
            return false;
        }

        if (PlazoPicker.SelectedItem == null)
        {
            mensaje = "Seleccione el plazo del préstamo.";
            return false;
        }

        string textoPlazo = PlazoPicker.SelectedItem.ToString()!;

        if (!int.TryParse(textoPlazo.Split(' ')[0], out plazo) ||
            plazo <= 0)
        {
            mensaje = "Seleccione un plazo válido.";
            return false;
        }

        return true;
    }

    private decimal CalcularCuota(
        decimal monto,
        decimal tasa,
        int plazo)
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

    // Calcular cuota mensual.
    private async void OnCalcularCuotaClicked(
        object sender,
        EventArgs e)
    {
        if (!ValidarDatos(
            out decimal monto,
            out decimal tasa,
            out int plazo,
            out string mensaje))
        {
            await DisplayAlertAsync(
                "Validación",
                mensaje,
                "Aceptar");

            return;
        }

        decimal cuota = CalcularCuota(monto, tasa, plazo);
        decimal total = cuota * plazo;

        CuotaLabel.Text =
            $"Cuota mensual estimada: RD$ {cuota:N2}";

        ResumenPrestamoLabel.Text =
            $"Monto: RD$ {monto:N2}\n" +
            $"Interés anual: {tasa:N2}%\n" +
            $"Plazo: {plazo} meses\n" +
            $"Total a pagar: RD$ {total:N2}";
    }

    // Registrar préstamo.
    private async void OnRegistrarPrestamoClicked(
        object sender,
        EventArgs e)
    {
        if (!ValidarDatos(
            out decimal monto,
            out decimal tasa,
            out int plazo,
            out string mensaje))
        {
            await DisplayAlertAsync(
                "Validación",
                mensaje,
                "Aceptar");

            return;
        }

        Socios socio = (Socios)SocioPrestamoPicker.SelectedItem;

        decimal cuota = CalcularCuota(monto, tasa, plazo);
        decimal total = cuota * plazo;

        // Generar identificador único.
        int nuevoId = PrestamosViewsModel.Prestamos.Count == 0
            ? 1
            : PrestamosViewsModel.Prestamos.Max(p => p.Id) + 1;

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

        // Registrar en la colección única compartida.
        PrestamosViewsModel.Prestamos.Add(nuevoPrestamo);

        // Actualizar el historial.
        ListaPrestamos.ItemsSource = null;
        ListaPrestamos.ItemsSource = PrestamosViewsModel.Prestamos;

        await DisplayAlertAsync(
            "Préstamo registrado",
            $"Se registró el préstamo {nuevoPrestamo.NumeroPrestamo} correctamente.",
            "Aceptar");

        LimpiarFormulario();
    }

    // Limpiar formulario.
    private void LimpiarFormulario()
    {
        SocioPrestamoPicker.SelectedItem = null;
        MontoPrestamoEntry.Text = string.Empty;
        TasaInteresEntry.Text = string.Empty;
        PlazoPicker.SelectedItem = null;

        CuotaLabel.Text = "RD$ 0.00";

        ResumenPrestamoLabel.Text =
            "Complete los datos para calcular.";
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        SocioPrestamoPicker.ItemsSource = SociosViewsModel.Socios;

        ListaPrestamos.ItemsSource =
            PrestamosViewsModel.Prestamos;
    }
}
