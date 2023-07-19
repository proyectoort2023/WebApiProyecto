using DTOs_Compartidos.Models;
using TorneusClienteWeb.Servicios_de_Datos;

namespace TorneusClienteWeb.Servicios
{
    public class MedioPagoServicio
    {
		private readonly MedioPagoServicioDatos _servicioMedioPagoServicioDatos;

    	public MedioPagoServicio(MedioPagoServicioDatos servicioMedioPagoServicioDatos)
		{
			_servicioMedioPagoServicioDatos = servicioMedioPagoServicioDatos;
		}

        public async Task<AccessTokenMercadoPago> ObtennerAccessTokenVendedor(string codigo)
        {
			try
			{
				return await _servicioMedioPagoServicioDatos.ObtenerTokenMercadopago(codigo);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
        }
    }
}
