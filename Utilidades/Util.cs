using System;

namespace Utilidades
{
    public static class Util
    {

        public static string TOKEN_LOCAL = "LoginTorneoStorageToken102023";
        public static string URL_BASE_IMAGENES = "https://imagenescontenedor.blob.core.windows.net/torneusimagenes";
        public static string URL_BASE_CONFIG_IMAGENES = "UrlBaseParaImagenes";
        public static string REGISTRARSE_GOOGLE = "REGISTRARSE_GOOGLE";
        public static string ACCESSKEY = "ACCESSKEY_MERCADOPAGO";

        public static string MERCADOPAGO_CONST = "MERCADOPAGO_CONST";

        public static string IconoEstrella = "fas fa-star";
        public static string IconoEquipo = "fas fa-users";
        public static string IconoInscripcion = "fas fa-pencil-alt";
        public static string IconoMedalla = "fas fa-medal";
        public static string IconoJugados = "fas fa-sitemap";

        public static string TIPO_PRECIO_POR_EQUIPO = "EQUIPO";

        public static string CLIENTE_ID_MERCADOPAGO_INTERMEDIARIO = "ClientIdMercadoPago";
        public static string REDIRECT_URL_OAUTH_VENDEDOR = "Redirect_Codigo_Vendedor";

        public static string HABILITACION_MARKETPLACE = "habMarketPlace";

        public static string LOCAL = "LOCAL";
        public static string VISITANTE = "VISITANTE";

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
        public static int TiempoEnMinutos(DateTime inicio, DateTime fin)
        {
            TimeSpan diferenciaTiempo = fin.Subtract(inicio);
            double minutosTRanscurridos = diferenciaTiempo.TotalMinutes;

            return (int)minutosTRanscurridos;
        }
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
            {"MIXTO_5_3","Equipos mixtos minimo 3 mujeres" },
            {"MUJERES","Equipo solo de mujeres" },
        };

        public static Dictionary<string, string> ConfigEquiposImagenes = new Dictionary<string, string>()
        {
            {"HOMBRES","ce_hombres.png" },
            {"MIXTO_5_1","ce_mix51.png" },
            {"MIXTO_5_2","ce_mix52.png" },
            {"MIXTO_5_3","ce_mix53.png" },
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
            {"NO_SELECCIONADO","grey" },
            {"PENDIENTE","darkorange" },
            {"RECHAZADO","red" },
            {"PAGADO","forestgreen" }
        };


        public static Dictionary<int, string> gruposLetra = new Dictionary<int, string>()
        {
            {0,"A" },
            {1,"B" },
            {2,"C" },
            {3,"D" },
            {4,"E" },
            {5,"F" },
            {6,"G" },
            {7,"H" },
            {8,"I" },
            {9,"J" },
        };

        public static Dictionary<string, int> gruposNumeros = new Dictionary<string, int>()
        {
            {"A",0 },
            {"B",1 },
            {"C",2 },
            {"D",3 },
            {"E",4 },
            {"F",5 },
            {"G",6 },
            {"H",7 },
            {"I",8 },
            {"J",9 },
        };


        public enum EstadoPartido
        {
            PENDIENTE,
            POR_COMENZAR,
            EN_PROCESO,
            FINALIZADO
        }





    }
}