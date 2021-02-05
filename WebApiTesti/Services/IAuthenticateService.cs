using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTesti.Models;

namespace WebApiTesti.Services
{
    public interface IAuthenticateService
    {
        Login Authenticate(string userName, string password);
    }
}
