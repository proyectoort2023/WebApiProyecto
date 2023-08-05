using Negocio.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_Compartidos.Models
{
    public class SelectEquipo
    {
        public string Grupo { get; set; }
        public EquipoDTO Equipo { get; set; }
    }
}
