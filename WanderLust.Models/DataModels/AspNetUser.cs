using System;
using System.Collections.Generic;
using System.Text;

namespace WanderLust.Models.DataModels
{
    public class AspNetUser
    {
        //public AspNetUser()
        //{
        //    this.AspNetUserRoles = new HashSet<AspNetUserRole>();
        //}

        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTimeOffset> LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsPasswordReset { get; set; }
        public bool IsFirstLogin { get; set; }

        //public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
    }
}
