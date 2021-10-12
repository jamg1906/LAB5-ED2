using System;
using SDESLibrary;
namespace SDESConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string texto = "in my head i do everything right when you call i forgive and not" +
               " fight all the moments i play in the dark we were wild and flourescent come " +
               "home to my heart, uh, in your car the radio up, in your car the radio up, we" +
               " keep tryin' to talk about us. Melodrama forever. 2017.";

            SDESLibrary.Cipher.CipherSDES sDES = new SDESLibrary.Cipher.CipherSDES();

            Console.WriteLine("Cifrado SDES, clave: 1017");

            sDES.SetKeys(1017);

            byte[] result_encrypt2 = sDES.EncryptText(ConvertToByte(texto));
            Console.WriteLine(ConvertToChar(result_encrypt2));

            byte[] result_decrypt2 = sDES.DecryptText(result_encrypt2);
            Console.WriteLine(ConvertToChar(result_decrypt2));

            Console.ReadKey();
        }

        public static byte[] ConvertToByte(string contenido)
        {
            byte[] arreglo = new byte[contenido.Length];
            for (int i = 0; i < arreglo.Length; i++)
            {
                arreglo[i] = Convert.ToByte(contenido[i]);
            }
            return arreglo;
        }

        public static char[] ConvertToChar(byte[] contenido)
        {
            char[] arreglo = new char[contenido.Length];
            for (int i = 0; i < arreglo.Length; i++)
            {
                arreglo[i] = Convert.ToChar(contenido[i]);
            }
            return arreglo;
        }
    }
}
