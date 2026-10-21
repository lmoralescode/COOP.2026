using COOPERATIVA.V2.Views;

namespace COOPERATIVA.V2.Model
{
    public class _5_SociosViewsModel
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Estado { get; set; } = "Activo";
    }
}