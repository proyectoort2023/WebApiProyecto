using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_Compartidos.Models
{
    public class AccessTokenMercadoPago
    {
        public string access_token { get; set; }
        public string public_key { get; set; }
        public string refresh_token { get; set;}
        public string expires_in { get; set; }
    }
}
