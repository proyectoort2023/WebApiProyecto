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

        public async Task<bool> ImplementarMercadoPagoVendedor(string codigo, int usuarioId, string token)
        {
			try
			{
				MpAuthVendedor mpAuthVendedor = new()
				{
					Codigo = codigo,
					UsuarioId = usuarioId
				};

				bool tokenVendedorCreado = await _servicioMedioPagoServicioDatos.ImplementarMercadoPagoVendedorData(mpAuthVendedor, token);

				return tokenVendedorCreado;

			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
        }


        public async Task<string> ObtenerAccessTokenMPVendedor(int usuarioId, string token)
        {
            try
            {
                string tokenVendedor = await _servicioMedioPagoServicioDatos.ObtenerAccessTokenVendedor(usuarioId, token);

                return tokenVendedor;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




    }
}
