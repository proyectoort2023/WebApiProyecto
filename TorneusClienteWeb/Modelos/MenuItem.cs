namespace BlazorTorneusClient.Modelos
{
    public class MenuItem
    {
        public string Titulo { get; set; }
        public string Icono { get; set; }
        public string UrlLink { get; set; }

        public MenuItem(string titulo, string icono, string urlLink)
        {
            Titulo = titulo;
            Icono = icono;
            UrlLink = urlLink;
        }
    }
}
