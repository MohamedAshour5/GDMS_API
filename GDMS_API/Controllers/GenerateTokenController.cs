using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using GDMS_API.HelperFunction;
using GDMS_API.Interfaces;
using GDMS_API.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GDMS_API.Controllers
{
    [Route("api")]
    [ApiController]

    public class GenerateTokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICommonHelperFunction _commonHelperFunction;
        public GenerateTokenController(IConfiguration configuration, ICommonHelperFunction commonHelperFunction)
        {
            _configuration = configuration;
            _commonHelperFunction = commonHelperFunction;
        }
        [HttpPost]
        [Route("GenerateToken")]
        public IActionResult GenerateJwtToken(TokenInfo tokenInfo)
        {
            try
            {
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenInfo.client_secret));
                string? DepartementKey = _commonHelperFunction.GetDepartementKeyFromValue(tokenInfo.client_secret);
                string? DepartementKey_ClientID = _commonHelperFunction.GetDepartementKeyFromValue(tokenInfo.client_id);
                if (DepartementKey_ClientID == null && DepartementKey == null)
                    return Ok(new ErrorCredentials() { error = "InValid credential", error_description = "client_id1 and client_secret is not correct " });
                else if (DepartementKey_ClientID == null)
                    return Ok(new ErrorCredentials() { error = "InValid credential", error_description = "client_id is not correct " });
                else if (DepartementKey == null)
                    return Ok(new ErrorCredentials() { error = "InValid credential", error_description = "client_secret is not correct " });
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]{
                                    new Claim(JwtRegisteredClaimNames.Sub, tokenInfo.client_id),
                                    new Claim("grant_type", tokenInfo.grant_type),
                                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                    };
                var Roles = _configuration.GetSection("JWTSettings").GetSection("ROles").GetSection(DepartementKey).Get<List<string>>().ToArray();
                foreach (var item in Roles)
                {
                    claims = claims.Append(new Claim(ClaimTypes.Role, item.ToString())).ToArray();

                }
                var token = new JwtSecurityToken(
                    issuer: "admin",
                    audience: "admin",
                    claims: claims,
                    expires: DateTime.Now.AddSeconds(int.Parse(_configuration.GetSection("ExpirationToken").Value)),
                    signingCredentials: credentials);
                return Ok(new TokenResponce()
                {
                    access_token = new JwtSecurityTokenHandler().WriteToken(token),
                    expires_in = int.Parse(_configuration.GetSection("ExpirationToken").Value),
                    scope = DepartementKey,
                    token_type = "Bearer"
                });
            }
            catch (Exception ex)
            {
                return null;

            }
        }
    }
}
