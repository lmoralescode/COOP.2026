
using COOPERATIVA.V2.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace COOPERATIVA.V2.ViewsModel;

public partial class SociosViewsModel : BaseViewsModel
{
    // COLECCIÓN PRINCIPAL COMPARTIDA EN MEMORIA.
    public static ObservableCollection<Socios> Socios { get; } = new();

    // CAMPO DE BÚSQUEDA.
    [ObservableProperty]
    public partial string Busqueda { get; set; } = string.Empty;

    // SOCIO SELECCIONADO.
    [ObservableProperty]
    public partial Socios? SocioSeleccionado { get; set; }

    // COLECCIÓN QUE SE MUESTRA EN LA PANTALLA.
    public ObservableCollection<Socios> SociosFiltrados { get; } = new();

    public SociosViewsModel()
    {
        // DETECTAR CUANDO SE AGREGA O ELIMINA UN SOCIO.
        Socios.CollectionChanged += OnSociosCollectionChanged;

        CargarSocios();
    }

    // ACTUALIZAR AUTOMÁTICAMENTE AL CAMBIAR LA BÚSQUEDA.
    partial void OnBusquedaChanged(string value)
    {
        CargarSocios();
    }

    // DETECTAR CAMBIOS EN LA COLECCIÓN PRINCIPAL.
    private void OnSociosCollectionChanged(
        object? sender,
        NotifyCollectionChangedEventArgs e)
    {
        CargarSocios();
    }

    // CARGAR LOS SOCIOS Y APLICAR EL FILTRO.
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

    // AGREGAR UN SOCIO A LA COLECCIÓN PRINCIPAL.
    public static void AgregarSocio(Socios socio)
    {
        if (socio == null)
            return;

        Socios.Add(socio);
    }

    // ELIMINAR UN SOCIO DE LA COLECCIÓN PRINCIPAL.
    public static void EliminarSocio(Socios socio)
    {
        if (socio == null)
            return;

        Socios.Remove(socio);
    }

    // LIMPIAR EL BUSCADOR.
    [RelayCommand]
    private void LimpiarBusqueda()
    {
        Busqueda = string.Empty;
    }
}
