using System;
using System.Collections.Generic;
using System.Text;

namespace COOPERATIVA.V2.Model
{
    internal class _7_Prestamos
    {
        public class Prestamo
        {
            public int Codigo { get; set; }
            public int CodigoSocio { get; set; }
            public decimal Monto { get; set; }
            public int PlazoMeses { get; set; }

            public override string ToString() =>
                $"{Codigo} - Socio: {CodigoSocio} | RD$ {Monto:N2} | {PlazoMeses} meses";
        }
    }
}
