using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using COOPERATIVA.V2.Views;

namespace COOPERATIVA.V2.ViewsModel;

public partial class InicioViewModel : ObservableObject
{
    [RelayCommand]
    public async Task Comenzar()
    {
        await Shell.Current.GoToAsync("//LoginViewsPage");
    }

    [RelayCommand]
    public async Task Registro()
    {
        await Shell.Current.GoToAsync(nameof(RegistroViewsPage));
    }
}