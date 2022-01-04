using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WanderLust.Common;
using WanderLust.Data;
using WanderLust.Models.DataModels;
using WanderLust.Models.ViewModels;

namespace WanderLust.Service
{
    public class TeamMemberService
    {
        WanderlustDbx db;
        IOptions<AppSettings> appSettings;
        private IWebHostEnvironment webHostEnvironment;

        public TeamMemberService(IOptions<AppSettings> _appSettings, IWebHostEnvironment _webHostEnvironment, WanderlustDbx _db)
        {
            appSettings = _appSettings;
            db = _db;
            webHostEnvironment = _webHostEnvironment;
        }

        public async Task<Boolean> AddOrUpdateTeamMember(TeamMemberViewModel teamMemberViewModel)
        {
            if (teamMemberViewModel.TeamMemberId > 0)
            {
                TeamMember model = db.TeamMember.FindAsync(teamMemberViewModel.TeamMemberId).Result;
                model.OurTeamIdFK = teamMemberViewModel.OurTeamIdFK;
                model.FirstName = teamMemberViewModel.FirstName;
                model.LastName = teamMemberViewModel.LastName;
                model.Designation = teamMemberViewModel.Designation;
                model.ImageURL = teamMemberViewModel.ImageURL;
                model.Description = teamMemberViewModel.Description;
                model.FacebookURL = teamMemberViewModel.FacebookURL;
                model.InstagramURL = teamMemberViewModel.InstagramURL;
                model.LinkedInURL = teamMemberViewModel.LinkedInURL;
                model.TwitterURL = teamMemberViewModel.TwitterURL;

                await db.SaveChangesAsync();
                return true;
            }
            else
            {
                TeamMember model = (TeamMember)teamMemberViewModel;
                await db.TeamMember.AddAsync(model);
                await db.SaveChangesAsync();
                teamMemberViewModel.TeamMemberId = model.TeamMemberId;
                return true;
            }
        }

        public async Task<List<TeamMemberViewModel>> GetAllTeamMember()
        {
            List<TeamMemberViewModel> teamMember = await db.TeamMember
                .OrderByDescending(x => x.TeamMemberId)
                .Select<TeamMember, TeamMemberViewModel>(x => x)
                .ToListAsync();
            return teamMember;


        }

        public async Task<TeamMember> GetTeamMemberById(int id)
        {
            var res = await db.TeamMember.FindAsync(id);
            return res;

        }

        public async Task<bool> DeleteTeamMemberById(int id)
        {
            var teamMemberId = db.TeamMember.OrderBy(e => e.TeamMemberId).Where(a => a.TeamMemberId == id).FirstOrDefault();
            db.TeamMember.Remove(teamMemberId);
            await db.SaveChangesAsync();
            return true;

        }
    }
}
