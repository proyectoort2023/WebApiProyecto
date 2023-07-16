using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_Compartidos.DTOs
{
    public class PreferenciaMercadopagoDTO
    {
        public int InscripcionId { get; set; }
        public string PreferenciaId { get; set; }
        public string OrdenPagoId { get; set; }

        public PreferenciaMercadopagoDTO(int inscripcionId, string preferenciaId, string ordenPagoId)
        {
            InscripcionId = inscripcionId;
            PreferenciaId = preferenciaId;
            OrdenPagoId = ordenPagoId;

        }
    }
}
