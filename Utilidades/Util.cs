namespace Utilidades
{
    public static class Util
    {

        public static string TOKEN_LOCAL = "LoginTorneoStorageToken102023";
        public static string URL_BASE_IMAGENES = "https://imagenescontenedor.blob.core.windows.net/torneusimagenes";
        public static string REGISTRARSE_GOOGLE = "REGISTRARSE_GOOGLE";

        public enum  Roles
        {
                EQUIPO,
                ORGANIZADOR,
                PLANILLERO,
                ESPECTADOR
        }

        public static Dictionary<string, string> TipoPrecioDiccionario = new Dictionary<string, string>()
        {
            {"Precio por equipo","EQUIPO" },
            {"Precio por jugador","JUGADOR" },
        };

        public static Dictionary<string, string> RolesDiccionario = new Dictionary<string, string>()
        {
            {"EQUIPO","/Equipo/Principal" },
            {"ORGANIZADOR","/Organizador/Principal" },
            {"PLANILLERO","" },
            {"ESPECTADOR","/Espectador/Principal" },
        };

        public enum TipoImagen
        {
            BANNER,
            LOGO
        }

        public static Dictionary<string, string> ConfigEquiposDiccionario = new Dictionary<string, string>()
        {
            {"HOMBRES","Equipo sólo de hombres" },
            {"MIXTO_5_1","Equipos mixtos mínimo 1 mujer" },
            {"MIXTO_5_2","Equipos mixtos mínimo 2 mujeres" },
            {"MUJERES","Equipo solo de mujeres" },
        };





    }
}