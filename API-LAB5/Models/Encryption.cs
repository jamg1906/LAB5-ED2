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
            path = Directory.GetCurrentDirectory() + "\\LlavesRSA";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Directory.GetCurrentDirectory() + "\\ResultadoRSA";
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

        public static void LlavesRSA(string p, string q, string path)
        {
            RSALibrary.Parametros parametros = new RSALibrary.Parametros();

            try
            {
                if (File.Exists($"{path}/../keys.zip"))
                    File.Delete($"{path}/../keys.zip");
                if (File.Exists(path + "\\public.key"))
                    File.Delete(path + "\\public.key");
                if (File.Exists(path + "\\private.key"))
                    File.Delete(path + "\\private.key");

                parametros.p = int.Parse(p);
                parametros.q = int.Parse(q);

                RSALibrary.Algoritmo.CifradoRSA temporal = new RSALibrary.Algoritmo.CifradoRSA();
                parametros = temporal.ObtenerLlave(parametros);

                FileStream publickeys = new FileStream((path + "\\public.key"), FileMode.Create);
                publickeys.Close();
                FileStream privatekeys = new FileStream((path + "\\private.key"), FileMode.Create);
                privatekeys.Close();

                using (StreamWriter writer = new StreamWriter(path + "\\public.key"))
                {
                    writer.WriteLine(parametros.n);
                    writer.WriteLine(parametros.e);
                }

                using (StreamWriter writer = new StreamWriter(path + "\\private.key"))
                {
                    writer.WriteLine(parametros.n);
                    writer.WriteLine(parametros.d);
                }
            }
            catch
            {
                throw new Exception();
            }
        }

        public static string[] CifrarDescifrarRSA(string path, string file_name, string key_name, string nameOut)
        {

            byte[] buffer;

            string file_path = path + "\\Temp\\" + file_name;
            using (FileStream fs = new FileStream(file_path, FileMode.OpenOrCreate))
            {
                buffer = new byte[fs.Length];
                using (var br = new BinaryReader(fs))
                {
                    br.Read(buffer, 0, buffer.Length);
                }
            }

            RSALibrary.Parametros parametros = new RSALibrary.Parametros();
            string file_path_key = path + "\\Temp\\" + key_name;
            string[] nameKey = key_name.Split(".");
            using (StreamReader keys = new StreamReader(file_path_key))
            {
                parametros.n = int.Parse(keys.ReadLine());
                if (nameKey[0] == "private")
                {
                    parametros.d = int.Parse(keys.ReadLine());
                    parametros.e = parametros.d;
                }
                else if (nameKey[0] == "public")
                {
                    parametros.e = int.Parse(keys.ReadLine());
                    parametros.d = parametros.e;
                }

            }

            byte[] result;
            string extension = file_name.Split(".")[1];
            string OutFilePath = "";
            string OutFileName = "";
            switch (extension)
            {
                case "txt":
                    result = new RSALibrary.Algoritmo.CifradoRSA().Cifrar(parametros, buffer);
                    OutFileName = nameOut + ".rsa";
                    OutFilePath = path + $"\\ResultadoRSA\\" + OutFileName;
                    break;
                case "rsa":
                    result = new RSALibrary.Algoritmo.CifradoRSA().Descifrar(parametros, buffer);
                    OutFileName = nameOut + ".txt";
                    OutFilePath = path + $"\\ResultadoRSA\\" + OutFileName;
                    break;
                default: throw new Exception();
            }
            using (var fs = new FileStream(OutFilePath, FileMode.OpenOrCreate))
            {
                fs.Write(result, 0, result.Length);
            }
            string [] retorno = new string[2];
            retorno[0] = OutFilePath;
            retorno[1] = OutFileName;

            return retorno;
        }

    }
}
