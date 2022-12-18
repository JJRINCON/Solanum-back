using System;
using System.Collections.Generic;

namespace Solanum_back.Models
{
    public partial class Contrato
    {
        public int IdUsuario { get; set; }
        public int IdTrabajador { get; set; }
        public string? Dato { get; set; }
        public string Estado { get; set; } = null!;

        public virtual Trabajadore IdTrabajadorNavigation { get; set; } = null!;
        public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
    }
}
