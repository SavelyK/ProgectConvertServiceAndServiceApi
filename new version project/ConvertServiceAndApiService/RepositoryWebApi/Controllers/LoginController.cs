using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryWebApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace RepositoryWebApi.Controllers
{
    [ApiVersion("1.0")]
  
    //[ApiVersionNeutral]
    [Produces("application/json")]
    [Route("api/{version:apiVersion}[controller]")]
    public class LoginController : BaseController
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }
        [HttpGet]
        public IActionResult Login(string username, string pass)
        {
            UserModel login = new UserModel();
            login.UserName = username;
            login.Password = pass;
            IActionResult response = Unauthorized();

            var user = AuthenticateUser(login);
            if (user != null)
            {
                var tokenStr = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenStr });
            }

            return response;
        }
        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;
            if (login.UserName == "name" && login.Password == "123")
            {
                user = new UserModel { UserName = "name", EmailAddress = "ashproghelp@gmail.com", Password = "123" };
            }
            return user;
        }
        private string GenerateJSONWebToken(UserModel userinfo)
        {
            var secutityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(secutityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userinfo.UserName),
                new Claim(JwtRegisteredClaimNames.Email,userinfo.EmailAddress),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);
            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;
        }
        [Authorize]
        [HttpPost("Post")]
        public string Post()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var userName = claim[0].Value;
            return "Welcome To: " + userName;
        }
        [Authorize]
        [HttpGet("GetValue")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Value1", "Value2", "Value3" };
        }
    }
}
