using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WanderLust.Models.ViewModels
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public string PhoneNumber { get; set; }
        public string RoleName
        {
            get; set;

            //[Required]
            //public string CountryCode { get; set; }
            //[Required]
            //[StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 9)]
            //public string PhoneNumber { get; set; }
            //public string RoleId { get; set; }
            //public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }

        }
    }
}
