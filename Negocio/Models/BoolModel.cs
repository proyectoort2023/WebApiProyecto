using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Models
{
    public class BoolModel
    {
        public bool Bandera { get; set; }

        public BoolModel(bool bandera)
        {
           Bandera = bandera;
        }
    }
}
