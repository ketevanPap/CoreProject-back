using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Infrastructure.DatabaseContext;
using ProjectCore.DomainService;
using ProjectCore.Entity.Model;
using ProjectCore.Entity.Model.ApplicationClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _context;
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UserRepository(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Result> CreateAsync(User user)
        {
            user.Id = Guid.NewGuid().ToString();
            user.CreatedAt = DateTime.Now;
            user.IsDeleted = false;

            try
            {
                IdentityResult success = await _userManager.CreateAsync(user, user.Password);

                if (!success.Succeeded)
                {
                    string error = "";

                    foreach (var er in success.Errors)
                    {
                        error += er.Description + Environment.NewLine;
                    }
                    return new Result
                    {
                        Success = false,
                        Data = user,
                        ErrorMessage = error
                    };
                }

                string role = user.Role.NormalizedName;

                Result result = await AddRoleAsync(user.UserName, role);

                if (result.Success)
                {
                    return new Result
                    {
                        Data = user,
                        Success = true
                    };
                }
                else
                {
                    return new Result
                    {
                        Success = false,
                        Data = user,
                        ErrorMessage = "Error"
                    };
                }
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Data = user,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<Result> UpdateAsync(User user)
        {
            User currentUser = (User)GetById(user.UserName).Result.Data;

            // Remove old Roles 
            string role = currentUser.Role.NormalizedName;

            if (role != null)
            {
                await RemoveRoleAsync(currentUser.UserName, role);
            }
            //

            User oldUser = (User)GetById(user.UserName).Result.Data;  

            oldUser.FirstName = user.FirstName;
            oldUser.LastName = user.LastName;
            oldUser.Password = user.Password;
            oldUser.Birthday = user.Birthday;
            oldUser.UpdatedAt = DateTime.Now;
            oldUser.Role = user.Role;

            if (!string.IsNullOrEmpty(user.Password))
            {
                var hashedNewPassword = _userManager.PasswordHasher.HashPassword(oldUser, user.Password);

                UserStore<User> store = new UserStore<User>(_context);

                await store.SetPasswordHashAsync(oldUser, hashedNewPassword);
            }

            await _userManager.UpdateAsync(oldUser);

            // Add New Roles

            if (user.Role != null)
            {
                string newRole = user.Role.NormalizedName;

                await _userManager.AddToRoleAsync(oldUser, newRole);
            }

            //

            return new Result
            {
                Data = oldUser
            };
        }

        public async Task<Result> DeleteAsync(string userName)
        {
            try
            {
                User user = (User)GetById(userName).Result.Data;

                await _userManager.DeleteAsync(user);

                return new Result
                {
                    Success = true,
                    Data = user
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<Result> GetAll()
        {
            List<User> users = _userManager.Users.ToList();
            long userCount = users.Count;

            try
            {
                foreach (var user in users)
                {
                    var roleList = await _userManager.GetRolesAsync(user);

                    foreach (var role in roleList)
                    {
                        user.Role = await _roleManager.FindByNameAsync(role);
                    }
                }

                return new Result
                {
                    Success = true,
                    Data = users,
                    ListCount = userCount
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<Result> GetById(string userName)
        {
            User user = await _userManager.Users.FirstOrDefaultAsync(user => user.UserName == userName);

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                user.Role = await _roleManager.FindByNameAsync(role);
            }

            return new Result
            {
                Data = user,
                Success = true
            };
        }

        public Result GetRoles()
        {
            return new Result
            {
                Data = _roleManager.Roles
            };
        }
        
        public async Task<Result> AddRoleAsync(string userName, string role)
        {
            try
            {
                var userResult = await GetById(userName);

                User user = (User)userResult.Data;

                var result = await _userManager.AddToRoleAsync(user, role);

                if (result.Succeeded)
                {
                    // should be return new User data
                }

                return new Result
                {
                    Data = user,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<Result> RemoveRoleAsync(string userName, string role)
        {
            try
            {
                User user = (User)GetById(userName).Result.Data;

                var result = await _userManager.RemoveFromRoleAsync(user, role);

                if (result.Succeeded)
                {
                    // should be return new User data
                }

                return new Result
                {
                    Success = true,
                    Data = user
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<Result> UpdatePasswordAsync(string userName, string oldPassword, string newPassword)
        {
            try
            {
                User user = (User)GetById(userName).Result.Data;

                await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

                return new Result
                {
                    Success = true,
                    Data = user
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
