using System;
using System.Collections.Generic;

namespace Solanum_back.Models
{
    public partial class Cultivo
    {
        public Cultivo()
        {
            Gastos = new HashSet<Gasto>();
            Ingresos = new HashSet<Ingreso>();
        }

        public int IdCultivo { get; set; }
        public string NombreCultivo { get; set; } = null!;
        public string DescripcionCultivo { get; set; } = null!;
        public int IdUsuario { get; set; }
        public string Estado { get; set; } = null!;

        public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
        public virtual ICollection<Gasto> Gastos { get; set; }
        public virtual ICollection<Ingreso> Ingresos { get; set; }
    }
}
