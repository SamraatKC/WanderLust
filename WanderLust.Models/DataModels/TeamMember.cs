using System;
using System.Collections.Generic;
using System.Text;
using WanderLust.Models.ViewModels;

namespace WanderLust.Models.DataModels
{
    public class TeamMember
    {
        public int TeamMemberId { get; set; }
        public int OurTeamIdFK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
        public string FacebookURL { get; set; }
        public string InstagramURL { get; set; }
        public string TwitterURL { get; set; }
        public string LinkedInURL { get; set; }
        public static implicit operator TeamMember(TeamMemberViewModel vm)
        {
            TeamMember model = new TeamMember();
            model.OurTeamIdFK = vm.OurTeamIdFK;
            model.FirstName = vm.FirstName;
            model.LastName = vm.LastName;
            model.Designation = vm.Designation;
            model.ImageURL = vm.ImageURL;
            model.Description = vm.Description;
            model.FacebookURL = vm.FacebookURL;
            model.InstagramURL = vm.InstagramURL;
            model.LinkedInURL = vm.LinkedInURL;
            model.TwitterURL = vm.TwitterURL;

            return model;
        }
    }
}
