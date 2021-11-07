using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace RSALibrary.Algoritmo
{
    public class CifradoRSA : Interfaces.ICifradoAsimetrico
    {
        private int Fi { get; set; }
        private int P { get; set; }
        private int Q { get; set; }
        private int E { get; set; }
        private int D { get; set; }
        private int N { get; set; }

        List<long> salidaRSACifrado = new List<long>();
        List<byte> salidaRSADescifrado = new List<byte>();

        public byte[] Descifrar(Parametros data, byte[] bufer)
        {
            throw new NotImplementedException();
        }

        //Generación de llaves

        public Parametros ObtenerLlave(Parametros data)
        {
            P = data.p;
            Q = data.q;
            N = P * Q;
            if (RevisarNumeroPrimo(P) && RevisarNumeroPrimo(Q))
            {
                Parametros llaves = new Parametros() { p = P, q = Q };
                llaves.n = P * Q;
                Fi = (P - 1) * (Q - 1);
                ObtenerE();
                D = ObtenerD();

                if (D == E)
                {
                    D += Fi;
                }

                llaves.e = E;
                llaves.d = D;
                return llaves;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        int ObtenerD()
        {
            List<int> filaA = new List<int>();
            List<int> filaB = new List<int>();
            int auxSalida = 0;
            int auxDivisor;
            int newA;
            int newB;

            filaA.Add(Fi);
            filaB.Add(Fi);
            filaA.Add(E);
            filaB.Add(1);

            for (int i = 0; auxSalida != 1; i++)
            {
                auxDivisor = filaA[i] / filaA[i + 1];
                newA = filaA[i] - (auxDivisor * filaA[i + 1]);
                newB = filaB[i] - (auxDivisor * filaB[i + 1]);

                while (newB < 0)
                {
                    newB = ((newB % Fi) + Fi) % Fi;
                }
                filaA.Add(newA);
                filaB.Add(newB);
                auxSalida = newA;
            }
            return filaB[filaB.Count - 1];
        }
        void ObtenerE()
        {
            List<int> randoms = new List<int>();
            Random elRandom = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            int e = 1;
            do
            {
                e++;

            } while (!RevisarE(e) && e < Fi);
            E = e;
        }

        bool RevisarE(int e)
        {

            if (RevisarNumeroPrimo(e))
            {
                List<int> multiplosFi = ObtenerMultiplos(Fi);
                List<int> multiplosE = ObtenerMultiplos(e);
                List<int> multiplosN = ObtenerMultiplos(N);

                foreach (var item in multiplosFi)
                {
                    if (multiplosE.Contains(item))
                    {
                        return false;
                    }
                }
                foreach (var item in multiplosN)
                {
                    if (multiplosE.Contains(item))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }


        List<int> ObtenerMultiplos(int valor)
        {
            List<int> multiplos = new List<int>();
            for (int i = 1; i <= valor; i++)
            {
                if (valor % i == 0 && i > 1) multiplos.Add(i);
            }
            return multiplos;
        }


        bool RevisarNumeroPrimo(int valor)
        {
            int contador = 0;
            for (int i = 1; i < (valor + 1); i++)
            {
                if (valor % i == 0)
                {
                    contador++;
                }
            }
            if (contador != 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //cifrado

        public byte[] Cifrar(Parametros data, byte[] bufer)
        {
            foreach (var item in bufer)
            {
                salidaRSACifrado.Add((long)BigInteger.ModPow(item, data.e, data.n));
            }
            return ConvertirByte();
        }
        byte[] ConvertirByte()
        {
            List<byte> validarBytes = new List<byte>();
            string CodigoBinario = "";
            int maximoBinario = Convert.ToString(salidaRSACifrado.Max(), 2).Length;
            foreach (var item in salidaRSACifrado)
            {
                CodigoBinario += Convert.ToString(item, 2).PadLeft(maximoBinario, '0');
                while (CodigoBinario.Length > 8)
                {
                    validarBytes.Add(Convert.ToByte(CodigoBinario.Substring(0, 8), 2));
                    CodigoBinario = CodigoBinario.Remove(0, 8);
                }
            }
            if (CodigoBinario.Length > 0 && CodigoBinario.Length < 8)
            {

                validarBytes.Add(Convert.ToByte(CodigoBinario.PadRight(8, '0'), 2));
                CodigoBinario = "";
            }
            else if (CodigoBinario.Length == 8)
            {
                validarBytes.Add(Convert.ToByte(CodigoBinario, 2));
            }
            validarBytes.Insert(0, Convert.ToByte(maximoBinario));
            return validarBytes.ToArray();
        } // Pasa a bytes todos los bytes cifrados por rsa

    }
}
