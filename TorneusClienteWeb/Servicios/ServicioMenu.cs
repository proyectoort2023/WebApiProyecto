
using BlazorTorneusClient.Modelos;
using TorneusClienteWeb.Modelos;

namespace BlazorTorneusClient.Servicios
{
    public class ServicioMenu
    {
        private List<MenuItem> items = new List<MenuItem>();
        public string Titulo = "";
        public NavegacionPOP navegacionPop = new();

        public event Action OnActualizarMenu;

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
            return "background-color:#6A1B9A;color:white;position:fixed;bottom:90px;right:20px;";
        }
        public void ActualizarNavegacionPOP(bool habilitadoBotonAtras, bool esPaginaPrincipal, string url)
        {
            navegacionPop.BotonAtrasHabilitado = habilitadoBotonAtras;
            navegacionPop.EsPaginaPrincipal = esPaginaPrincipal;
            navegacionPop.AccionIrAtrasUrl = url;
            ActualizarMenu();
        }
    }
}
