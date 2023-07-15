using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BDTorneus
{
    [DataContract(IsReference = true)]
    public class Inscripcion
    {

        public int Id { get; set; }
        public int TorneoId { get; set; }
        public Torneo Torneo { get; set; }
        public int EquipoId { get; set; }
        public Equipo Equipo { get; set; }
        public double Monto { get; set; }
        public string MedioPago { get; set; }
        public string Estado { get; set; }
        public string PreferenciaMP { get; set; }
        public string OrdenPagoMP { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario{ get; set; }
    }
}
