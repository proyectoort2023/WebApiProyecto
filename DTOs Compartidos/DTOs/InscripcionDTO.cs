using BDTorneus;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTOs
{
    public class InscripcionDTO
    {
        public int Id { get; set; }
        public int TorneoId { get; set; }
        public TorneoDTO Torneo { get; set; }
        public int EquipoId { get; set; }
        public EquipoDTO Equipo { get; set; }
        public double Monto { get; set; }
        public string MedioPago { get; set; }
        public string Estado { get; set; }
        public string PreferenciaMP { get; set; }
        public string OrdenPagoMP { get; set; }
        public int UsuarioId { get; set; }
        public UsuarioDTO Usuario { get; set; }

    }
}
