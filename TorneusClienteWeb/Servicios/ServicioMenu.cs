
using BlazorTorneusClient.Modelos;

namespace BlazorTorneusClient.Servicios
{
    public class ServicioMenu
    {
        private List<MenuItem> items = new List<MenuItem>();

        public event Action OnMenuUpdated;

        public void ActualizarItems(List<MenuItem> nuevoMenuItems)
        {
            items = nuevoMenuItems;
            ActualizarListado();
        }

        private void ActualizarListado()
        {
            OnMenuUpdated?.Invoke();
        }

        public List<MenuItem> ObtenerItemsMenu()
        {
            return items;
        }
    }
}
