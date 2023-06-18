using Negocio.DTOs;

namespace TorneusClienteWeb.Servicios
{
    public class UsuarioServicio
    {
        private UsuarioLogueado _usuarioLogueado;

        public void ActualizarUsuarioLogueado(UsuarioLogueado usuario)
        {
            _usuarioLogueado = usuario;
        }

        public UsuarioLogueado ObtenerUsuarioLogueado()
        {
            return _usuarioLogueado;
        }
        
    }
}
