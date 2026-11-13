using CommunityToolkit.Mvvm.ComponentModel;

namespace COOPERATIVA.V2.ViewsModel;

public partial class BaseViewsModel : ObservableObject
{
    [ObservableProperty]
    private string mensajeExito = string.Empty;
}