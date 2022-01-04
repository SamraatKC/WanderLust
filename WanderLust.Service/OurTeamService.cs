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
    public class OurTeamService
    {
        WanderlustDbx db;
        IOptions<AppSettings> appSettings;
        private IWebHostEnvironment webHostEnvironment;

        public OurTeamService(IOptions<AppSettings> _appSettings, IWebHostEnvironment _webHostEnvironment, WanderlustDbx _db)
        {
            appSettings = _appSettings;
            db = _db;
            webHostEnvironment = _webHostEnvironment;
        }

        public async Task<bool> AddOurTeam(OurTeamViewModel ourTeamViewModel)
        {
            OurTeam model = (OurTeam)ourTeamViewModel;
            await db.OurTeam.AddAsync(model);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateOurTeam(OurTeamViewModel ourTeamViewModel)
        {
            OurTeam ourTeam = await db.OurTeam.Where(x => x.OurTeamId == ourTeamViewModel.OurTeamId).FirstOrDefaultAsync();
            if (ourTeam != null)
            {
                ourTeam.Title = ourTeamViewModel.Title;
                ourTeam.Description = ourTeamViewModel.Description;
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<OurTeamViewModel>> GetAllOurTeam()
        {
            List<OurTeamViewModel> ourTeam = await db.OurTeam
                .OrderByDescending(x => x.OurTeamId)
                .Select<OurTeam, OurTeamViewModel>(x => x)
                .ToListAsync();
            return ourTeam;

           
        }

        public async Task<OurTeam> GetOurTeamById(int id)
        {
            var res = await db.OurTeam.FindAsync(id);
            return res;

        }

        public async Task<bool> DeleteOurTeamById(int id)
        {
            var ourTeamId = db.OurTeam.OrderBy(e => e.OurTeamId).Where(a => a.OurTeamId == id).FirstOrDefault();
            db.OurTeam.Remove(ourTeamId);
            await db.SaveChangesAsync();
            return true;

        }

    }
}
