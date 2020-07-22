using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ProjectCore.Entity.Model.ApplicationClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.Infrastructure.DatabaseContext
{
    public class SeedDatabase
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {

                User user = new User()
                {
                    Email = "a@b.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "Ram"
                };

                userManager.CreateAsync(user, "Password@123");

                var adminRole = roleManager.CreateAsync(new IdentityRole()
                {
                    Id = "1",
                    Name = "RootAdmin",
                    NormalizedName = "ROOTADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                });

                userManager.AddToRoleAsync(user, "RootAdmin");
            }
        }
    }
}
