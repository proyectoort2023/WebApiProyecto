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
    public class UsuarioDTO
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

        public List<EquipoDTO> Equipos { get; set; } = new List<EquipoDTO>();
        public List<TorneoDTO> Torneos { get; set; } = new List<TorneoDTO>();
        public List<InscripcionDTO> Inscripciones { get; set; } = new List<InscripcionDTO>();
    }
}
