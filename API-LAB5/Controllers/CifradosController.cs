using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using API_LAB5.Models;
using System.IO.Compression;

namespace API_LAB5.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CifradosController : Controller
    {
        [HttpPost("sdes/cipher/{name}")]
        public async Task<IActionResult> OnPostUploadAsync([FromForm] IFormFile file, [FromForm] string key, [FromRoute] string name)
        {
            try
            {
                Encryption.DirectoryCreation();
                var filePath = Path.GetFullPath(Directory.GetCurrentDirectory() + "\\Temp\\" + file.FileName);
                if (file != null)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else { return StatusCode(500); }

                string finalName = Encryption.Encrypt(filePath, name, "sdes", key);
                FileStream Sender = new FileStream(Directory.GetCurrentDirectory() + "\\Cifrados\\" + finalName, FileMode.OpenOrCreate);
                return File(Sender, "text/plain", finalName);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost("sdes/decipher")]
        public async Task<IActionResult> OnPostUploadAsync([FromForm] IFormFile file, [FromForm] string key)
        {
            try
            {
                Encryption.DirectoryCreation();
                var filePath = Path.GetFullPath(Directory.GetCurrentDirectory() + "\\Temp\\" + file.FileName);
                if (file != null)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else { return StatusCode(500); }

                string[] fileName = file.FileName.Split('.');
                string method = fileName[1];
                switch (method)
                {
                    case "sdes":
                        method = "sdes";
                        break;
                    default:
                        throw new Exception();
                }

                string finalName = Encryption.Decrypt(filePath, fileName[0], method, key);
                FileStream Sender = new FileStream(Directory.GetCurrentDirectory() + "\\Descifrados\\" + finalName, FileMode.OpenOrCreate);
                return File(Sender, "text/plain", finalName);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("rsa/keys/{p}/{q}")]
        public ActionResult GetKeys([FromRoute] string p, [FromRoute] string q)
        {
            try
            {
                Encryption.DirectoryCreation();
                var filePath = Path.GetFullPath(Directory.GetCurrentDirectory() + "\\LlavesRSA");

                Encryption.LlavesRSA(p, q, filePath);

                ZipFile.CreateFromDirectory($"{filePath}", $"{filePath}/../keys.zip");

                FileStream Sender = new FileStream($"{filePath}/../keys.zip", FileMode.Open);
                return File(Sender, "application/zip", "keys.zip");
            }
            catch
            {
                return StatusCode(500); 
            }
        }

        [HttpPost("rsa/{nombre}")]
        public async Task<IActionResult> EncryptOrDecryptFile([FromForm] IFormFile file, [FromForm] IFormFile key, string nombre)
        {
            try
            {
                Encryption.DirectoryCreation();
                string filePath = Directory.GetCurrentDirectory();
                string file_name = file.FileName;
                string key_name = key.FileName;

                if (file != null)
                {
                    using (var stream = new FileStream(filePath + "\\Temp\\" + file_name, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else { return StatusCode(500); }

                if (key != null)
                {
                    using (var stream = new FileStream(filePath + "\\Temp\\" + key_name, FileMode.Create))
                    {
                        await key.CopyToAsync(stream);
                    }
                }
                else { return StatusCode(500); }

                string[] retorno = Encryption.CifrarDescifrarRSA(filePath, file_name, key_name, nombre);

                FileStream result = new FileStream(retorno[0], FileMode.Open);
                return File(result, "text/plain", retorno[1]);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
