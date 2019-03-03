using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration configuration;

        public TokenController(IConfiguration config)
        {
            configuration = config;
        }

        [HttpGet("CreateToken")]
        public IActionResult Index(string username = "faruk", string password = "faruk")
        {
            IActionResult response  = Unauthorized();
            if (username.Equals(password))
            {
                var token = JwtTokenBuilder();

                response = Ok(new { access_token = token });
            }
            return response;
        }

        private string JwtTokenBuilder()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                configuration["JWT:key"]
                ));

            var credintials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(issuer: configuration["JWT:issuer"],
                audience: configuration["JWT:audience"],
                signingCredentials: credintials,
                expires: DateTime.Now.AddDays(30),
                claims: new List<Claim>() {new Claim("name","faruk"),new Claim("system_role","admin") }
                );           

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}