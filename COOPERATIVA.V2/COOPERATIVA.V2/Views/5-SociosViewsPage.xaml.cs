
using COOPERATIVA.V2.Model;
using System.Collections.ObjectModel;

namespace COOPERATIVA.V2.Views;

public partial class SociosViewsPage : ContentPage
{
    // Colección principal de socios.
    public static ObservableCollection<Socios> Socios { get; } = new();

    // Socio que se está editando.
    public static Socios? SocioEnEdicion { get; set; }

    // Colección mostrada en pantalla.
    private readonly ObservableCollection<Socios> _sociosFiltrados = new();

    public SociosViewsPage()
    {
        InitializeComponent();

        ListaSocios.ItemsSource = _sociosFiltrados;
        BuscarSocio.TextChanged += BuscarSocio_TextChanged;

        CargarSocios();
    }

    public static void AgregarSocio(Socios socio)
    {
        Socios.Add(socio);
    }

    private void CargarSocios()
    {
        _sociosFiltrados.Clear();

        string busqueda = BuscarSocio.Text?.Trim() ?? "";

        var resultados = Socios.Where(s =>
            ($"{s.Nombre} {s.Apellidos}")
                .Contains(busqueda, StringComparison.OrdinalIgnoreCase)
            || s.Cedula.Contains(
                busqueda,
                StringComparison.OrdinalIgnoreCase));

        foreach (var socio in resultados)
        {
            _sociosFiltrados.Add(socio);
        }
    }

    private void BuscarSocio_TextChanged(object? sender, TextChangedEventArgs e)
    {
        CargarSocios();
    }

    // Botón Nuevo socio.
    private async void OnNuevoSocioClicked(object sender, EventArgs e)
    {
        SocioEnEdicion = null;

        await Navigation.PushAsync(new RegistroViewsPage());
    }

    // Botón Editar / consultar.
    private async void OnEditarSocioClicked(object sender, EventArgs e)
    {
        if (Socios.Count == 0)
        {
            await DisplayAlertAsync(
                "Consultar socios",
                "Todavía no hay socios registrados.",
                "Aceptar");

            return;
        }

        string[] opciones = Socios
            .Select(s => $"{s.Nombre} {s.Apellidos} - {s.Cedula}")
            .ToArray();

        string seleccion = await DisplayActionSheet(
            "Selecciona el socio que deseas editar",
            "Cancelar",
            null,
            opciones);

        if (string.IsNullOrWhiteSpace(seleccion) ||
            seleccion == "Cancelar")
        {
            return;
        }

        int indice = Array.IndexOf(opciones, seleccion);

        if (indice < 0)
            return;

        SocioEnEdicion = Socios[indice];

        await Navigation.PushAsync(new RegistroViewsPage());
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Actualiza la lista después de registrar o editar.
        CargarSocios();
    }
}
