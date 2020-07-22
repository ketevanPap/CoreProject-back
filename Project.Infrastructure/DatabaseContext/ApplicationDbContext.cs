using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectCore.Entity.Model.ApplicationClasses;

namespace Project.Infrastructure.DatabaseContext
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Status>().ToTable("Statuses");
        }

        public virtual DbSet<Status> Statuses { get; set; }
    }
}
