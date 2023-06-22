namespace Utilidades
{
    public static class Util
    {

        public static string TOKEN_LOCAL = "LoginTorneoStorageToken102023";
        public static string URL_BASE_IMAGENES = "https://imagenescontenedor.blob.core.windows.net/torneusimagenes";

        public enum  Roles
        {
                EQUIPO,
                ORGANIZADOR,
                PLANILLERO,
                ESPECTADOR
        }

        public static Dictionary<string, string> RolesDiccionario = new Dictionary<string, string>()
        {
            {"EQUIPO","/Equipo/Principal" },
            {"ORGANIZADOR","/Organizador/Principal" },
            {"PLANILLERO","" },
            {"ESPECTADOR","/Espectador/Principal" },
        };

    }
}