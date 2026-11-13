
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

        viewModel.SolicitarAlertaYRegreso += OnSolicitarAlertaYRegreso;
    }

    private async void OnSolicitarAlertaYRegreso(string mensaje)
    {
        await DisplayAlertAsync(
            "Operación exitosa",
            mensaje,
            "Aceptar");

        await Navigation.PopAsync();
    }

    protected override void OnDisappearing()
    {
        viewModel.SolicitarAlertaYRegreso -= OnSolicitarAlertaYRegreso;

        base.OnDisappearing();
    }
}
