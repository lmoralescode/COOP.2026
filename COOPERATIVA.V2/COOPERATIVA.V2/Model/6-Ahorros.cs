using System;
using System.Collections.Generic;
using System.Text;

namespace COOPERATIVA.V2.Model
{
    internal class _6_Ahorros
    {

        public class Ahorro
        {
            public int Codigo { get; set; }
            public int CodigoSocio { get; set; }
            public string TipoMovimiento { get; set; } = "";
            public decimal Monto { get; set; }

            public override string ToString() =>
                $"{Codigo} - Socio: {CodigoSocio} | {TipoMovimiento}: RD$ {Monto:N2}";
        }

    }
}
