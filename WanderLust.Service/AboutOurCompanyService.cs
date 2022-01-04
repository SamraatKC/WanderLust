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
    public class AboutOurCompanyService
    {
        WanderlustDbx db;
        IOptions<AppSettings> appSettings;
        private IWebHostEnvironment webHostEnvironment;

        public AboutOurCompanyService(IOptions<AppSettings> _appSettings, IWebHostEnvironment _webHostEnvironment, WanderlustDbx _db)
        {
            appSettings = _appSettings;
            db = _db;
            webHostEnvironment = _webHostEnvironment;
        }

        public async Task<Boolean> AddOrUpdateAboutOurCompany(AboutOurCompanyViewModel aboutOurCompanyViewModel)
        {
            if (aboutOurCompanyViewModel.AboutOurCompanyId > 0)
            {
                AboutOurCompany model = db.AboutOurCompany.FindAsync(aboutOurCompanyViewModel.AboutOurCompanyId).Result;
                model.Title = aboutOurCompanyViewModel.Title;
                model.Description = aboutOurCompanyViewModel.Description;
                model.ImageURL = aboutOurCompanyViewModel.ImageURL;
               
                await db.SaveChangesAsync();
                return true;
            }
            else
            {
                AboutOurCompany model = (AboutOurCompany)aboutOurCompanyViewModel;
                await db.AboutOurCompany.AddAsync(model);
                await db.SaveChangesAsync();
                aboutOurCompanyViewModel.AboutOurCompanyId = model.AboutOurCompanyId;
                return true;
            }
        }

        public async Task<List<AboutOurCompanyViewModel>> GetAllAboutOurCompany()
        {
            List<AboutOurCompanyViewModel> aboutOurCompany = await db.AboutOurCompany
                .OrderByDescending(x => x.AboutOurCompanyId)
                .Select<AboutOurCompany, AboutOurCompanyViewModel>(x => x)
                .ToListAsync();
            return aboutOurCompany;


        }

        public async Task<AboutOurCompany> GetAboutOurCompanyById(int id)
        {
            var res = await db.AboutOurCompany.FindAsync(id);
            return res;

        }

        public async Task<bool> DeleteAboutOurCompanyById(int id)
        {
            var aboutOurCompanyId = db.AboutOurCompany.OrderBy(e => e.AboutOurCompanyId).Where(a => a.AboutOurCompanyId == id).FirstOrDefault();
            db.AboutOurCompany.Remove(aboutOurCompanyId);
            await db.SaveChangesAsync();
            return true;

        }
    }
}
