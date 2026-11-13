
using COOPERATIVA.V2.Model;
using COOPERATIVA.V2.ViewsModel;
using System.Collections.ObjectModel;

namespace COOPERATIVA.V2.Views;

public partial class SociosViewsPage : ContentPage
{
    private readonly SociosViewsModel viewModel;

    // Utilizar la misma colección del ViewModel.
    public static ObservableCollection<Socios> Socios =>
        SociosViewsModel.Socios;

    public static Socios? SocioEnEdicion { get; set; }

    public SociosViewsPage()
    {
        InitializeComponent();

        viewModel = new SociosViewsModel();

        // Mostrar los socios del ViewModel.
        ListaSocios.ItemsSource = viewModel.SociosFiltrados;

        // Conectar el buscador.
        BuscarSocio.TextChanged += BuscarSocio_TextChanged;

        CargarSocios();
    }

    private void CargarSocios()
    {
        viewModel.Busqueda = BuscarSocio.Text ?? string.Empty;

        viewModel.CargarSocios();
    }

    private void BuscarSocio_TextChanged(
        object? sender,
        TextChangedEventArgs e)
    {
        viewModel.Busqueda = e.NewTextValue ?? string.Empty;
    }

    // Registrar un nuevo socio.
    private async void OnNuevoSocioClicked(
        object sender,
        EventArgs e)
    {
        SocioEnEdicion = null;

        await Navigation.PushAsync(
            new RegistroViewsPage());
    }

    // Consultar o editar un socio.
    private async void OnEditarSocioClicked(
        object sender,
        EventArgs e)
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

        await Navigation.PushAsync(
            new RegistroViewsPage(SocioEnEdicion));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Actualizar la lista al regresar del formulario.
        CargarSocios();
    }
}
