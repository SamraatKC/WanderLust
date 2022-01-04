using System;
using System.Collections.Generic;
using System.Text;
using WanderLust.Models.DataModels;

namespace WanderLust.Models.ViewModels
{
    public class OurTeamViewModel
    {
        public int OurTeamId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public static implicit operator OurTeamViewModel(OurTeam ourTeam)
        {
            OurTeamViewModel ourTeamViewModel = new OurTeamViewModel();
            ourTeamViewModel.OurTeamId = ourTeam.OurTeamId;
            ourTeamViewModel.Title = ourTeam.Title;
            ourTeamViewModel.Description = ourTeam.Description;
            return ourTeamViewModel;
        }
    }
}
