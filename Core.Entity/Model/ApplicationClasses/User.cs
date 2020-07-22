using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ProjectCore.Entity.Model.ApplicationClasses
{
    public class User: IdentityUser
    {
        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Password { get; set; }

        public virtual DateTime Birthday { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual DateTime UpdatedAt { get; set; }

        public virtual DateTime? DeletedAt { get; set; }

        public virtual int StatusId { get; set; }

        public virtual Status Status { get; set; }

        public List<IdentityRole> Role = new List<IdentityRole>();
    }
}
