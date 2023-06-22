
using BlazorTorneusClient.Modelos;

namespace BlazorTorneusClient.Servicios
{
    public class ServicioMenu
    {
        private List<MenuItem> items = new List<MenuItem>();
        private string Titulo = "";

        public event Action OnActualizarMenu;
        public event Action OnActualizarTitulo;

        public void ActualizarItems(List<MenuItem> nuevoMenuItems)
        {
            items = nuevoMenuItems;
            ActualizarListado();
        }

        private void ActualizarListado()
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
           OnActualizarTitulo?.Invoke();
        }

        public string ObtenerTitulo()
        {
            return Titulo;
        }
    }
}
