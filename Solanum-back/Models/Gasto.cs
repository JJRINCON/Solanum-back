using System;
using System.Collections.Generic;

namespace Solanum_back.Models
{
    public partial class Gasto
    {
        public int IdGasto { get; set; }
        public string NombreGasto { get; set; } = null!;
        public string TipoGasto { get; set; } = null!;
        public int ValorGasto { get; set; }
        public string DescripcionGasto { get; set; } = null!;
        public string Etapa { get; set; } = null!;
        public int IdCultivo { get; set; }
        public int? IdTrabajador { get; set; }
        public string Estado { get; set; } = null!;
        public string EstadoPago { get; set; } = null!;
        public DateTime FechaGasto { get; set; }
        public int? NumeroJorbul { get; set; }
        public int? CalorUnidad { get; set; }

        public virtual Cultivo IdCultivoNavigation { get; set; } = null!;
        public virtual Trabajadore? IdTrabajadorNavigation { get; set; }
    }
}
