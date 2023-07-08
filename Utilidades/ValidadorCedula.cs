namespace Utilidades
{
    public static class ValidadorCedula
    {
        public static bool Validar(string cedula)
        {
            try { 
            int cedulaNumerica = Int32.Parse(cedula);

            int cedulaSinDigitoVerificador = cedulaNumerica / 10;
            int digitoVerificador = cedulaNumerica % 10;

            int VerificadorRecursivo = DigitoVerificadorRecursivo(cedulaSinDigitoVerificador);

            return VerificadorRecursivo == digitoVerificador;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        private static int DigitoVerificadorRecursivo(int cedulaSinDigitoVerificador, int semilla = 2987634, int suma = 0)
        {
            if (cedulaSinDigitoVerificador < 10)
            {
                suma += semilla * cedulaSinDigitoVerificador;
                return 10 - (suma % 10);
            }

            int restoCedula = cedulaSinDigitoVerificador % 10;
            cedulaSinDigitoVerificador /= 10;

            int restoSemilla = semilla % 10;
            semilla /= 10;

            suma += restoCedula * restoSemilla;

            return DigitoVerificadorRecursivo(cedulaSinDigitoVerificador, semilla, suma);
        }
    }
}