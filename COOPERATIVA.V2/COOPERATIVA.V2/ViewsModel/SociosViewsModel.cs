using COOPERATIVA.V2.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace COOPERATIVA.V2.ViewsModel;

public partial class SociosViewsModel : BaseViewsModel
{
    public static ObservableCollection<Socios> Socios { get; } = new();

    [ObservableProperty]
    public partial string Busqueda { get; set; } = string.Empty;

    [ObservableProperty]
    public partial Socios? SocioSeleccionado { get; set; }
    public ObservableCollection<Socios> SociosFiltrados { get; } = new();

    public SociosViewsModel()
    {
        CargarSocios();
    }

    public void OnBusquedaChanged(string value)
    {
        CargarSocios();
    }

    public void CargarSocios()
    {
        SociosFiltrados.Clear();

        var resultados = Socios.Where(s =>
            s.NombreCompleto.Contains(
                Busqueda.Trim(),
                StringComparison.OrdinalIgnoreCase)
            ||
            s.Cedula.Contains(
                Busqueda.Trim(),
                StringComparison.OrdinalIgnoreCase));

        foreach (var socio in resultados)
            SociosFiltrados.Add(socio);
    }

    public static void AgregarSocio(Socios socio)
    {
        Socios.Add(socio);
    }

    [RelayCommand]
    private void LimpiarBusqueda()
    {
        Busqueda = string.Empty;
    }
}