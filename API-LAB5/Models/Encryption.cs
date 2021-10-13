using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API_LAB5.Models
{
    public class Encryption
    {
        public static void DirectoryCreation()
        {
            string path = Directory.GetCurrentDirectory() + "\\Temp";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Directory.GetCurrentDirectory() + "\\Cifrados";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Directory.GetCurrentDirectory() + "\\Descifrados";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }


        public static string Encrypt(string filePath, string fileName, string method, string key)
        {
            byte[] buffer;
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                buffer = new byte[fs.Length];
                using (var br = new BinaryReader(fs))
                {
                    br.Read(buffer, 0, (int)fs.Length);
                }
            }
            string ext = "";
            byte[] content = default;
            string final = "";
            switch (method.Trim())
            {
                case "sdes":
                    if (Convert.ToInt32(key) < 0 | Convert.ToInt32(key) > 1023)
                    {
                        throw new Exception();
                    }
                    SDESLibrary.Cipher.CipherSDES sDes = new SDESLibrary.Cipher.CipherSDES();
                    sDes.SetKeys(Convert.ToInt32(key));
                    content = sDes.EncryptText(buffer);
                    ext = ".sdes";
                    final = fileName + ext;
                    break;
                default:
                    throw new Exception();
            }

            string resultado = Directory.GetCurrentDirectory() + "\\Cifrados\\" + fileName + ext;
            using (var fs = new FileStream(resultado, FileMode.OpenOrCreate))
            {
                fs.Write(content, 0, content.Length);
            }
            return final;
        }


        public static string Decrypt(string filePath, string fileName, string method, string key)
        {
            byte[] buffer;
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                buffer = new byte[fs.Length];
                using (var br = new BinaryReader(fs))
                {
                    br.Read(buffer, 0, (int)fs.Length);
                }
            }
            string ext = ".txt";
            byte[] content = default;
            string final = "";
            switch (method.Trim())
            {
                case "sdes":
                    if (Convert.ToInt32(key) < 0 | Convert.ToInt32(key) > 1023)
                    {
                        throw new Exception();
                    }
                    SDESLibrary.Cipher.CipherSDES sDes = new SDESLibrary.Cipher.CipherSDES();
                    sDes.SetKeys(Convert.ToInt32(key));
                    content = sDes.DecryptText(buffer);
                    final = fileName + ext;
                    break;
                default:
                    throw new Exception();
            }

            string resultado = Directory.GetCurrentDirectory() + "\\Descifrados\\" + fileName + ext;
            using (var fs = new FileStream(resultado, FileMode.OpenOrCreate))
            {
                fs.Write(content, 0, content.Length);
            }
            return final;
        }


    }
}
