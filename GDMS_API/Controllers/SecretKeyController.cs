using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GDMS_API.Models;
using GDMS_API.Services;

namespace GDMS_API.Controllers
{
    [Route("api/SecretKey")]
    [ApiController]
    public class SecretKeyController : ControllerBase
    {
        public static string GenerateSecretKey()
        {
            return Guid.NewGuid().ToString();
        }
        [HttpGet]
        [Route("SecretKey")]
        public async Task<IActionResult> SecretKey()
        {
            string secretKey = GenerateSecretKey();
            return Ok(secretKey);
        }
    }
}
