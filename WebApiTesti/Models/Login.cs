using System;
using System.Collections.Generic;

#nullable disable

namespace WebApiTesti.Models
{
    public partial class Login
    {
        public int LoginId { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public int AccessLevelId { get; set; }
        public string Token { get; set; }
    }
}
