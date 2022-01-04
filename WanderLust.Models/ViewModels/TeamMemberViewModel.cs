using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using WanderLust.Models.DataModels;

namespace WanderLust.Models.ViewModels
{
    public class TeamMemberViewModel
    {
        public int TeamMemberId { get; set; }
        public int OurTeamIdFK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public IFormFile TeamMemberImage { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
        public string FacebookURL { get; set; }
        public string InstagramURL { get; set; }
        public string TwitterURL { get; set; }
        public string LinkedInURL { get; set; }

        public static implicit operator TeamMemberViewModel(TeamMember model)
        {
            if (model == null) return null;
            TeamMemberViewModel vm = new TeamMemberViewModel();

            vm.OurTeamIdFK = model.OurTeamIdFK;
            vm.FirstName = model.FirstName;
            vm.LastName = model.LastName;
            vm.Designation = model.Designation;
            vm.ImageURL = model.ImageURL;
            vm.Description = model.Description;
            vm.FacebookURL = model.FacebookURL;
            vm.InstagramURL = model.InstagramURL;
            vm.LinkedInURL = model.LinkedInURL;
            vm.TwitterURL = model.TwitterURL;
            return vm;
        }
    }
}
