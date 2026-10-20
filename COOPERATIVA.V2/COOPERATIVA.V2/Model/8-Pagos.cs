using System;
using System.Collections.Generic;
using System.Text;

namespace COOPERATIVA.V2.Model
{
    internal class _8_Pagos
    {
        public class Pago
        {
            public int Codigo { get; set; }
            public int IdPrestamo { get; set; }
            public decimal Monto { get; set; }
            public DateTime Fecha { get; set; }

            public override string ToString() =>
                $"{Codigo} - Préstamo: {IdPrestamo} | RD$ {Monto:N2} | {Fecha:dd/MM/yyyy}";
        }
    }
}
