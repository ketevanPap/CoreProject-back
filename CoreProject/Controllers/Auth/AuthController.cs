using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.DomainService;
using ProjectCore.Entity.Model;
using ProjectCore.Entity.Model.ApplicationClasses.Auth;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace CoreProject.Controllers.Auth
{
    [Route("api/[controller]")]
    //[EnableCors("AllowOrigin")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            Result result = await _authRepository.Login(model);

            if (result.Success == true)
            {
                //roles
                LoginData data = (LoginData)result.Data;

                var role = data.Role;
                var claims = data.Claims;
                var token = data.Token;

                //roles
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    statuses = Statuses.Get()
                });
            }
            else
            {
                return Ok(new
                {
                    token = string.Empty,
                    expiration = DateTime.Now,
                    statuses = Statuses.Get(),
                    errorMsg = result.ErrorMessage
                });
            }
        }
    }
}
