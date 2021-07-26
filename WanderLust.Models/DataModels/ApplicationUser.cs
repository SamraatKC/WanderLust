using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace WanderLust.Models.DataModels
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsFirstLogin { get; set; }
        public bool IsPasswordReset { get; set; }
    }
}
