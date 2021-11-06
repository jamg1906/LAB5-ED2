using System;
using System.Collections.Generic;
using System.Text;

namespace RSALibrary.Interfaces
{
    interface ICifradoAsimetrico
    {
        public Parametros ObtenerLlave(Parametros data);
        public byte[] Cifrar(Parametros data, byte[] bufer);
        public byte[] Descifrar(Parametros data, byte[] bufer);
    }
}
