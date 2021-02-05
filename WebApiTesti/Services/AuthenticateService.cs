using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTesti.Models;
using Microsoft.IdentityModel.Tokens;

namespace WebApiTesti.Services
{
    public class AuthenticateService : IAuthenticateService // periytetään IAuthenticateService
    {
        private readonly AppSettings _appSettings;
        public AuthenticateService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;  // appSettings arvo _appSettings muuttujaan
        }

        private northwindContext db = new northwindContext();   

        public Login Authenticate(string userName, string passWord)
        {
            // var user = db.Logins.SingleOrDefault(x => x.UserName == userName && x.PassWord == passWord); // katsotaan onko Logins taulussa käyttäjää saaduilla parametreilla
            var user = db.Logins.SingleOrDefault(x => x.UserName == userName);  // katsotaan löytyykö haettu username
            if (user == null)
            {
                return null;
            }
            
            bool verified = BCrypt.Net.BCrypt.Verify(passWord, user.PassWord);  // katsotaan matsaako salasana hashattuun salanaan tietokannassa

            if (verified)  // jos salasana täsmää
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Key);  // appsettingsiin asetettu key enkoodattuna
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.LoginId.ToString()),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Version, "V5.0")
                    }),
                    Expires = DateTime.UtcNow.AddDays(1), // Tokenin voimassaolo päivinä

                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);

                user.PassWord = null;
                return user; // Palautetaan user kutsuvalle controllerimetodille ilman salasanaa (joka nullattu yläpuolella)
            }
            else // jos salasana ei täsmää
            {
                return null;
            }
            

        }
    }
}
