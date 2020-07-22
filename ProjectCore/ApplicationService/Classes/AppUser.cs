using ProjectCore.ApplicationService.Interfaces;
using ProjectCore.DomainService;
using ProjectCore.Entity.Model;
using ProjectCore.Entity.Model.ApplicationClasses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectCore.ApplicationService.Classes
{
    public class AppUser : IUser
    {
        private IUserRepository _repository;

        public AppUser(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> CreateAsync(User user)
        {
            return await _repository.CreateAsync(user);
        }

        public async Task<Result> UpdateAsync(User user)
        {
            return await _repository.UpdateAsync(user);
        }

        public async Task<Result> DeleteAsync(string userName)
        {
            return await _repository.DeleteAsync(userName);
        }

        public async Task<Result> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Result> GetById(string userName)
        {
            return await _repository.GetById(userName);
        }

        public Result GetRoles()
        {
            return _repository.GetRoles();
        }

        public async Task<Result> AddRoleAsync(string userName, string role)
        {
            return await _repository.AddRoleAsync(userName, role);
        }

        public async Task<Result> RemoveRoleAsync(string userName, string role)
        {
            return await _repository.RemoveRoleAsync(userName, role);
        }

        public async Task<Result> UpdatePasswordAsync(string userName, string oldPassword, string newPassword)
        {
            return await _repository.UpdatePasswordAsync(userName, oldPassword, newPassword);
        }
    }
}
