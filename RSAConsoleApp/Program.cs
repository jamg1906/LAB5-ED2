using System;
using RSALibrary;

namespace RSAConsoleApp
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Laboratorio 6. \n Javier Morales - 1210219 \n Mario Roldan - 1117517\n\n");
            RSALibrary.Algoritmo.CifradoRSA CifradoRSA = new RSALibrary.Algoritmo.CifradoRSA();
            RSALibrary.Parametros ParametroRSA = new RSALibrary.Parametros() { p = 9601, q =8731};
            string texto = "in my head i do everything right when you call i forgive and not" +
              " fight all the moments i play in the dark we were wild and flourescent come " +
              "home to my heart, uh, in your car the radio up, in your car the radio up, we" +
              " keep tryin' to talk about us. Melodrama forever. 2017.";

            Console.WriteLine("Texto original: \n" + texto + "\n");
            ParametroRSA = CifradoRSA.ObtenerLlave(ParametroRSA);

            Console.WriteLine("Cifrado RSA");

            Console.WriteLine("\nLaves\n valor N: " + ParametroRSA.n);
            Console.WriteLine("valor D: " + ParametroRSA.d);
            Console.WriteLine("valor E: " + ParametroRSA.e);

            Console.WriteLine("Texto cifrado: \n");

            byte[] encriptadoRSA = CifradoRSA.Cifrar(ParametroRSA, ConvertToByte(texto));
            Console.WriteLine(ConvertToChar(encriptadoRSA));


            Console.WriteLine("\nTexto descifrado: \n");
            byte[] descrifradoRSA = CifradoRSA.Descifrar(ParametroRSA, encriptadoRSA);
            Console.WriteLine(ConvertToChar(descrifradoRSA));

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
