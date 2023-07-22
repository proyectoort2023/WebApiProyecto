using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTOs
{
    public class UsuarioLogueado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Mail { get; set; }
        public string Rol { get; set; }
        public string Tel { get; set; }
        public string Token { get; set; }
        public bool HabilitadoCobroMercadopago { get; set; }
    }
}
