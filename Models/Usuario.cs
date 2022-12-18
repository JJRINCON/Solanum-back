using System;
using System.Collections.Generic;

namespace Solanum_back.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Contratos = new HashSet<Contrato>();
            Cultivos = new HashSet<Cultivo>();
        }

        public int IdUsuario { get; set; }
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string NombreUsuario { get; set; } = null!;
        public string Contrasenia { get; set; } = null!;
        public string Estado { get; set; } = null!;

        public virtual ICollection<Contrato> Contratos { get; set; }
        public virtual ICollection<Cultivo> Cultivos { get; set; }
    }
}
