using System;
using System.Collections.Generic;
using System.Text;

namespace SDESLibrary.Interfaces
{
    interface IAlgoritmoCifrado
    {
        public byte[] EncryptText(byte[] content);

        public byte[] DecryptText(byte[] content);

    }
}
