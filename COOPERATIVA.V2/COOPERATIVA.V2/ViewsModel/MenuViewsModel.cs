
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using COOPERATIVA.V2.Views;

namespace COOPERATIVA.V2.ViewsModel;

public partial class MenuViewsModel : BaseViewsModel
{
    public MenuViewsModel()
    {
    }

    [RelayCommand]
    private async Task Socios()
    {
        await Shell.Current.GoToAsync(nameof(SociosViewsPage));
    }

    [RelayCommand]
    private async Task Ahorros()
    {
        await Shell.Current.GoToAsync(nameof(AhorrosViewsPage));
    }

    [RelayCommand]
    private async Task Prestamos()
    {
        await Shell.Current.GoToAsync(nameof(PrestamosViewsPage));
    }

    [RelayCommand]
    private async Task Pagos()
    {
        await Shell.Current.GoToAsync(nameof(PagosViewsPage));
    }

    [RelayCommand]
    private async Task CerrarSesion()
    {
        bool confirmar = await Shell.Current.DisplayAlertAsync(
            "Cerrar sesión",
            "¿Estás seguro de que deseas cerrar sesión?",
            "Sí",
            "No");

        if (!confirmar)
            return;

        await Shell.Current.GoToAsync("//LoginViewsPage");
    }
}
