using System;
using System.Collections.Generic;

namespace Solanum_back.Models
{
    public partial class Trabajadore
    {
        public Trabajadore()
        {
            Contratos = new HashSet<Contrato>();
            Gastos = new HashSet<Gasto>();
        }

        public int IdTrabajador { get; set; }
        public string NombreTrabajador { get; set; } = null!;
        public string NumeroContacto { get; set; } = null!;
        public string Estado { get; set; } = null!;

        public virtual ICollection<Contrato> Contratos { get; set; }
        public virtual ICollection<Gasto> Gastos { get; set; }
    }
}
