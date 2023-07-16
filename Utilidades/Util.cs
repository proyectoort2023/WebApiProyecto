namespace Utilidades
{
    public static class Util
    {

        public static string TOKEN_LOCAL = "LoginTorneoStorageToken102023";
        public static string URL_BASE_IMAGENES = "https://imagenescontenedor.blob.core.windows.net/torneusimagenes";
        public static string REGISTRARSE_GOOGLE = "REGISTRARSE_GOOGLE";
        public static string ACCESSKEY = "ACCESSKEY_MERCADOPAGO";

        public static string MERCADOPAGO_CONST = "MERCADOPAGO_CONST";

        public static string IconoEstrella = "fas fa-star";
        public static string IconoEquipo = "fas fa-users";
        public static string IconoInscripcion = "fas fa-pencil-alt";
        public static string IconoMedalla = "fas fa-medal";
        public static string IconoJugados = "fas fa-sitemap";

        public static string TIPO_PRECIO_POR_EQUIPO = "EQUIPO";

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
            {"EQUIPO","/EQUIPO/Principal" },
            {"ORGANIZADOR","/ORGANIZADOR/Principal" },
            {"PLANILLERO","" },
            {"ESPECTADOR","/ESPECTADOR/Principal" },
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

        public static Dictionary<string, string> ConfigEquiposImagenes = new Dictionary<string, string>()
        {
            {"HOMBRES","ce_hombres.png" },
            {"MIXTO_5_1","ce_mix51.png" },
            {"MIXTO_5_2","ce_mix52.png" },
            {"MUJERES","ce_mujeres.png" },
        };

        public enum EstadoPago
        {
          NO_SELECCIONADO,
          PENDIENTE,
          RECHAZADO,
          PAGADO
        }

        public enum MedioPago
        {
           EFECTIVO,
           MERCADOPAGO
        }

        public static Dictionary<string, string> ColorEstadoPAgo = new Dictionary<string, string>()
        {
            {"NO_SELECCIONAD","grey" },
            {"PENDIENTE","darkorange" },
            {"RECHAZADO","red" },
            {"PAGADO","forestgreen" }
        };



    }
}