
using BlazorTorneusClient.Modelos;
using Microsoft.AspNetCore.Components;
using TorneusClienteWeb.Modelos;
using TorneusClienteWeb.Servicios;

namespace BlazorTorneusClient.Servicios
{

    public class ServicioMenu
    {
        private List<MenuItem> items = new List<MenuItem>();
        public string Titulo = "";
        public NavegacionPOP navegacionPop = new();

        public event Action OnActualizarMenu;
        public bool nuevaNotificacion = false;

        public void ActualizarItems(List<MenuItem> nuevoMenuItems)
        {
            items = nuevoMenuItems;
            ActualizarMenu();
        }

        private void ActualizarMenu()
        {
            OnActualizarMenu?.Invoke();
        }

        public List<MenuItem> ObtenerItemsMenu()
        {
            return items;
        }

        public void ActualizarTitulo(string titulo)
        {
           Titulo = titulo;
           ActualizarMenu();
        }

        public string ClassFabColor()
        {
            return "background-color:#6A1B9A;color:white;position:fixed;bottom:90px;right:20px;z-index:160;";
        }
        public void ActualizarNavegacionPOP(bool habilitadoBotonAtras, bool esPaginaPrincipal, string url)
        {
            navegacionPop.BotonAtrasHabilitado = habilitadoBotonAtras;
            navegacionPop.EsPaginaPrincipal = esPaginaPrincipal;
            navegacionPop.AccionIrAtrasUrl = url;
            ActualizarMenu();
        }

        public void NuevaNotificacionIcono()
        {
            nuevaNotificacion = true;
            ActualizarMenu();
        }




    }
}
