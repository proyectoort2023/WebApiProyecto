using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BDTorneus
{
    [DataContract(IsReference = true)]
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Mail { get; set; }
        public string Pass { get; set; }
        public string Rol { get; set; }
        public string Tel { get; set; }
        public string Token { get; set; }
        public string AccessTokenMercadopago { get; set; }
        public string AccessTokenRefreshMercadopago { get; set; }
        public DateTime FechaCaducidadTokenMercadopago { get; set; }

        public List<Equipo> Equipos { get; set; } =new List<Equipo>();
        public List<Torneo> Torneos { get; set; } = new List<Torneo>();
        public List<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();

    }
}
