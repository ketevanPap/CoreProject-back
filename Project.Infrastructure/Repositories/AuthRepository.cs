using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ProjectCore.DomainService;
using ProjectCore.Entity.Model;
using ProjectCore.Entity.Model.ApplicationClasses;
using ProjectCore.Entity.Model.ApplicationClasses.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {

        private UserManager<User> _userManager;

        public AuthRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> Login(LoginModel model)
        {
            Result result = new Result();

            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                var check = _userManager.CheckPasswordAsync(user, model.Password);

                if (user != null && user.StatusId == 2 && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    IList<string> roles = await _userManager.GetRolesAsync(user);

                    string[] rolesforprincipal = new List<string>(roles).ToArray();

                    string rolestoarray = "";

                    foreach (var item in roles)
                    {
                        rolestoarray = String.Format(rolestoarray + ", " + "{0}", item);
                    }

                    rolestoarray = rolestoarray.Substring(2, rolestoarray.Length - 2);

                    //var FullName = user.FullName != null ? user.FullName : user.UserName;

                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        //new Claim("Name", FullName),
                        new Claim("UserName", user.UserName),
                        new Claim("UserId", user.Id)
                    };

                    //roles
                    foreach (var userRole in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, userRole));

                        var role = await _userManager.FindByNameAsync(userRole);

                        if (role != null)
                        {
                            var roleClaims = await _userManager.GetClaimsAsync(role);

                            foreach (Claim roleClaim in roleClaims)
                            {
                                claims.Add(roleClaim);
                            }
                        }
                    }

                    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecurityKey"));

                    var token = new JwtSecurityToken(
                         issuer: "http://oec.com",
                         audience: "http://oec.com",
                         expires: DateTime.Now.AddDays(1),
                         claims: claims,
                         signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

                    result.Success = true;

                    result.Data = new LoginData
                    {
                        Token = token,
                        ValidTo = token.ValidTo,
                        Role = roles.FirstOrDefault(),
                        Claims = claims
                    };

                    return result;
                }

                result.Success = false;
                result.ErrorMessage = "Username or password is incorect";
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return result;
            }
        }
    }
}
