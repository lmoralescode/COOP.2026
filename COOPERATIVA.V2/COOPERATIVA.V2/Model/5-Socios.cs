using System;
using System.Collections.Generic;
using System.Text;

namespace COOPERATIVA.V2.Model
{
    internal class _5_Socios
    {
    }

    public class Socio
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; } = "";
        public string Cedula { get; set; } = "";
        public string Telefono { get; set; } = "";

        public override string ToString() =>
            $"{Codigo} - {Nombre} | Cédula: {Cedula} | Teléfono: {Telefono}";
    }
}
