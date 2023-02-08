using Core.PersonRepository.Interfaces;
using Microsoft.Extensions.Configuration;
using Data.Model;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Security.Claims;

namespace Core.PersonRepository.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _config;
        public AuthRepository(IConfiguration config)
        {
            _config = config;
        }
        public string RequestToken(LoginModel login)
        {
            if (VerifyUserCredentials(login))
            {
                var claims = new[]
                {
                new Claim(ClaimTypes.Name, login.Username)
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetKey()));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var jwtToken = new JwtSecurityToken(GetIssuer(),
                                                    GetIssuer(),
                                                    claims,
                                                    expires: DateTime.Now.AddMinutes(30),
                                                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(jwtToken);
            }

            return string.Empty;
        }
        public bool VerifyUserCredentials(LoginModel login)
        {
            // aca podria implementar la verificación de credenciales
            // en función de los requisitos, por ejemplo, consultando
            // a una base de datos.
            return login.Username == "user" && login.Password == "password";
        }
        public string GetKey()
        {
            return _config["Jwt:Key"];
        }

        public string GetIssuer()
        {
            return _config["Jwt:Issuer"];
        }
    }
}