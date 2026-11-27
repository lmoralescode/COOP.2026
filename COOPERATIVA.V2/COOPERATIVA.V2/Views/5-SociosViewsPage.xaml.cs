
using COOPERATIVA.V2.Model;
using COOPERATIVA.V2.ViewsModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;

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

    // REGISTRAR UN NUEVO SOCIO.
    private async void OnNuevoSocioClicked(
        object sender,
        EventArgs e)
    {
        SocioEnEdicion = null;

        await Navigation.PushAsync(
            new RegistroViewsPage());
    }

    // CONSULTAR O EDITAR UN SOCIO.
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

        string seleccion = await DisplayActionSheetAsync(
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

    // ELIMINAR SOCIO DIRECTAMENTE DESDE LA LISTA.
    private async void OnEliminarSocioClicked(
        object sender,
        EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not Socios socio)
        {
            return;
        }

        // SOLICITAR CONFIRMACIÓN.
        bool confirmar = await DisplayAlertAsync(
            "Confirmar eliminación",
            $"¿Estás seguro de eliminar al socio {socio.Nombre} {socio.Apellidos}?",
            "Eliminar",
            "Cancelar");

        // SI CANCELA, NO SE ELIMINA.
        if (!confirmar)
            return;

        // ELIMINAR DE LA COLECCIÓN PRINCIPAL.
        SociosViewsModel.EliminarSocio(socio);

        // MOSTRAR RESULTADO.
        await DisplayAlertAsync(
            "Operación exitosa",
            "El socio se eliminó correctamente.",
            "Aceptar");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Actualizar la lista al regresar del formulario.
        CargarSocios();
    }
}
