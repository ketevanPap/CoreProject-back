using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ProjectCore.Entity.Model.ApplicationClasses.Auth
{
    public class LoginData
    {
        public SecurityToken Token { get; set; }
        public DateTime ValidTo { get; set; }
        public string Role { get; set; }
        public IList<Claim> Claims { get; set; }
    }
}
