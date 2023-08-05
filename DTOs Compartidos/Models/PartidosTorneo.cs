using Negocio.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_Compartidos.Models
{
    public class PartidosTorneo
    {
        public int TorneoId { get; set; }
        public List<PartidoDTO> Fixture { get; set; }
    }
}
