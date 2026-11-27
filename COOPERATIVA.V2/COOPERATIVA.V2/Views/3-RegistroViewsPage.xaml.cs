
using COOPERATIVA.V2.Model;
using COOPERATIVA.V2.ViewsModel;

namespace COOPERATIVA.V2.Views;

public partial class RegistroViewsPage : ContentPage
{
    private readonly RegistroViewsModel viewModel;

    public RegistroViewsPage(Socios? socio = null)
    {
        InitializeComponent();

        viewModel = new RegistroViewsModel(socio);

        BindingContext = viewModel;

        // CONFIRMACIÓN ANTES DE ELIMINAR
        viewModel.SolicitarConfirmacionEliminar +=
            OnSolicitarConfirmacionEliminar;

        // MENSAJE DE RESULTADO Y REGRESO
        viewModel.SolicitarAlertaYRegreso +=
            OnSolicitarAlertaYRegreso;
    }

    // CONFIRMAR ELIMINACIÓN
    private Task<bool> OnSolicitarConfirmacionEliminar()
    {
        return DisplayAlertAsync(
            "Confirmar eliminación",
            $"¿Estás seguro de eliminar al socio {viewModel.Nombre} {viewModel.Apellidos}?",
            "Eliminar",
            "Cancelar");
    }

    // MOSTRAR RESULTADO Y REGRESAR
    private async void OnSolicitarAlertaYRegreso(string mensaje)
    {
        await DisplayAlertAsync(
            "Operación exitosa",
            mensaje,
            "Aceptar");

        await Navigation.PopAsync();
    }

    // LIBERAR EVENTOS
    protected override void OnDisappearing()
    {
        viewModel.SolicitarAlertaYRegreso -=
            OnSolicitarAlertaYRegreso;

        viewModel.SolicitarConfirmacionEliminar -=
            OnSolicitarConfirmacionEliminar;

        base.OnDisappearing();
    }
}
