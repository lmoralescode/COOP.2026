using COOPERATIVA.V2.ViewsModel;
using System.Collections.ObjectModel;

namespace COOPERATIVA.V2.Servicios;

public static class DatosCooperativa
{
    public static ObservableCollection<_5_SociosViewsModel> Socios { get; } = new();
    public static ObservableCollection<_6_AhorrosViewsModel> Movimientos { get; } = new();
    public static ObservableCollection<_7_PrestamosViewsModel> Prestamos { get; } = new();
    public static ObservableCollection<_8_PagosViewsModel> Pagos { get; } = new();

    public static int NuevoCodigoSocio =>
        Socios.Count == 0 ? 1 : Socios.Max(s => s.Codigo) + 1;
}