using System;
using System.Collections.Generic;
using System.Text;
using WanderLust.Models.ViewModels;

namespace WanderLust.Models.DataModels
{
    public class OurTeam
    {
        public int OurTeamId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public static implicit operator OurTeam(OurTeamViewModel vm)
        {
            OurTeam m = new OurTeam();
            m.OurTeamId = vm.OurTeamId;
            m.Title = vm.Title;
            m.Description = vm.Description;
            return m;
        }
    }
}
