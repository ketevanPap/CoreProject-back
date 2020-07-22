using ProjectCore.Entity.Model;
using ProjectCore.Entity.Model.ApplicationClasses.Auth;
using System.Threading.Tasks;

namespace ProjectCore.DomainService
{
    public interface IAuthRepository
    {
        Task<Result> Login(LoginModel model);
    }
}
