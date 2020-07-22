using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.ApplicationService.Interfaces;
using ProjectCore.Entity.Model;
using ProjectCore.Entity.Model.PostEntities;
using System.Threading.Tasks;

namespace CoreProject.Controllers.User
{
    [Route("api/[controller]")]
    //[Authorize(Roles = "admin")]
    //[EnableCors("AllowOrigin")]
    public class UserController : Controller
    {
        private IUser _user;

        public UserController(IUser user)
        {
            _user = user;
        }

        [Route("user")]
        [HttpPost]
        public async Task<Result> CreateUser([FromBody] ProjectCore.Entity.Model.ApplicationClasses.User user)
        {
            return await _user.CreateAsync(user);
        }

        [Route("user")]
        [HttpPut]
        public async Task<Result> UpdateUser([FromBody] ProjectCore.Entity.Model.ApplicationClasses.User user)
        {
            return await _user.UpdateAsync(user);
        }

        [Route("user")]
        [HttpDelete]
        public async Task<Result> DeleteUser(string userName)
        {
            return await _user.DeleteAsync(userName);
        }

        [AllowAnonymous]
        [Route("user")]
        [HttpGet]
        public async Task<Result> GetUser(string userName)
        {
            return await _user.GetById(userName);
        }

        [AllowAnonymous]
        [Route("users")]
        [HttpGet]
        public async Task<Result> GetUsers()
        {
            return await _user.GetAll();
        }

        [AllowAnonymous]
        [Route("roles")]
        [HttpGet]
        public Result GetRoles()
        {
            return _user.GetRoles();
        }

        [Route("roles")]
        [HttpPost]
        public async Task<Result> AddRoles([FromBody] UserAddRolesEntity model)
        {
            return await _user.AddRoleAsync(model.UserName, model.Role);
        }

        [Route("roles")]
        [HttpDelete]
        public async Task<Result> RemoveRoles([FromBody] UserAddRolesEntity model)
        {
            return await _user.RemoveRoleAsync(model.UserName, model.Role);
        }
    }
}
