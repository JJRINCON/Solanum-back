using System;
using System.Collections.Generic;

namespace Solanum_back.Models
{
    public partial class Ingreso
    {
        public int IdIngreso { get; set; }
        public string NombreIngreso { get; set; } = null!;
        public DateTime FechaIngreso { get; set; }
        public int ValorIngreso { get; set; }
        public int Cantidad { get; set; }
        public string DescripcionIngreso { get; set; } = null!;
        public int IdCultivo { get; set; }
        public string Estado { get; set; } = null!;

        public virtual Cultivo IdCultivoNavigation { get; set; } = null!;
    }
}
