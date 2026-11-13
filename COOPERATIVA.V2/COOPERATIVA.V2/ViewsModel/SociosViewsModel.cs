
using COOPERATIVA.V2.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace COOPERATIVA.V2.ViewsModel;

public partial class SociosViewsModel : BaseViewsModel
{

    // Colección principal compartida en memoria.
    public static ObservableCollection<Socios> Socios { get; } = new();

    // Campo de búsqueda.
    [ObservableProperty]
    public partial string Busqueda { get; set; } = string.Empty;

    // Socio seleccionado.
    [ObservableProperty]
    public partial Socios? SocioSeleccionado { get; set; }

    // Colección que se muestra en la pantalla.
    public ObservableCollection<Socios> SociosFiltrados { get; } = new();

    public SociosViewsModel()
    {
        // Detectar cuando se agrega o elimina un socio.
        Socios.CollectionChanged += OnSociosCollectionChanged;

        CargarSocios();
    }

    // Actualizar automáticamente al cambiar la búsqueda.
    partial void OnBusquedaChanged(string value)
    {
        CargarSocios();
    }

    // Detectar cambios en la colección principal.
    private void OnSociosCollectionChanged(
        object? sender,
        NotifyCollectionChangedEventArgs e)
    {
        CargarSocios();
    }

    // Cargar los socios y aplicar el filtro.
    public void CargarSocios()
    {
        SociosFiltrados.Clear();

        string filtro = Busqueda?.Trim() ?? string.Empty;

        var resultados = Socios.Where(s =>
            (s.NombreCompleto ?? string.Empty).Contains(
                filtro,
                StringComparison.OrdinalIgnoreCase)
            ||
            (s.Cedula ?? string.Empty).Contains(
                filtro,
                StringComparison.OrdinalIgnoreCase));

        foreach (var socio in resultados)
        {
            SociosFiltrados.Add(socio);
        }
    }

    // Agregar un socio a la colección principal.
    public static void AgregarSocio(Socios socio)
    {
        if (socio == null)
            return;

        Socios.Add(socio);
    }

    // Eliminar un socio.
    public static void EliminarSocio(Socios socio)
    {
        if (socio == null)
            return;

        Socios.Remove(socio);
    }

    // Limpiar el buscador.
    [RelayCommand]
    private void LimpiarBusqueda()
    {
        Busqueda = string.Empty;
    }
}
