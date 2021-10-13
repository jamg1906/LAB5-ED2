using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using API_LAB5.Models;

namespace API_LAB5.Controllers
{
    [Route("api/sdes/")]
    [ApiController]
    public class CifradosController : Controller
    {
        [HttpPost("cipher/{name}")]
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

        [HttpPost("decipher")]
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


    }
}
