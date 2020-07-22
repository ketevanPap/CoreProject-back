using ProjectCore.Entity.Model;
using ProjectCore.Entity.Model.ApplicationClasses;
using System.Threading.Tasks;

namespace ProjectCore.ApplicationService.Interfaces
{
    public interface IUser
    {
        Task<Result> CreateAsync(User user);

        Task<Result> UpdateAsync(User user);

        Task<Result> DeleteAsync(string userName);

        Task<Result> GetAll();

        Task<Result> GetById(string userName);

        Result GetRoles();

        Task<Result> AddRoleAsync(string userName, string role);

        Task<Result> RemoveRoleAsync(string userName, string role);

        Task<Result> UpdatePasswordAsync(string userName, string oldPassword, string newPassword);
    }
}
