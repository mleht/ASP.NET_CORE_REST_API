using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTesti.Models;
using WebApiTesti.Services;

namespace WebApiTesti.Controllers
{
    [Route("api/[controller]")]  // api/Authentication
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticateService _authenticateService;   // dependency injection
        public AuthenticationController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [HttpPost]  // varsinainen metodi johon frontti ottaa yhteyden
        public IActionResult Post([FromBody] Models.Login login)
        {
            var user = _authenticateService.Authenticate(login.UserName, login.PassWord);  // hyödynnetään rajanpinnan palveluita

            if (user == null)
                return BadRequest(new { message = "Käyttäjätunnus tai salasana on virheellinen" });

            return Ok(user);
        }
    }
}
