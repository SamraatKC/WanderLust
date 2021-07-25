using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WanderLust.Data;
using WanderLust.Models.CommonModels;
using WanderLust.Models.DataModels;
using WanderLust.Models.ViewModels;
using WanderLust.Common;

namespace WanderLust.Service
{
    public  class HomeService
    {
      
        WanderlustDbx db;
        IOptions<AppSettings> appSettings;
        private List<Home> _home;
        public HomeService(IOptions<AppSettings> _appSettings)
        {
            appSettings = _appSettings;
            db = new WanderlustDbx(_appSettings);
            _home = new List<Home>();
        }



        public async Task<bool> AddSection(HomeViewModel homeViewModel)
        {
            Home home = new Home();
             home.Content = null;
            home.Section = homeViewModel.Section;
            home.ThemeType = homeViewModel.ThemeType;
            await db.Home.AddAsync(home);
            return db.SaveChanges() > 0;
           
        }

        public async Task<bool> UpdateSection(int id,HomeViewModel homeViewModel)
        {

            //Home home = new Home();
            var homeId = await db.Home.FirstOrDefaultAsync(e => e.HomeId == id);
            if (homeId != null)
            {
                homeId.Content = null;
                homeId.Section = homeViewModel.Section;
                homeId.ThemeType = homeViewModel.ThemeType;
                return  db.SaveChanges()>0;
                //return home;
            }
            return false;
        }

        public async Task<List<Home>> GetAllSection()
        {
            return await db.Home.ToListAsync();
        }

       
        public async Task<Home> FindSectionById(int id)
        {
            var res = await db.Home.FindAsync(id);
            return res;
        }

        public async Task<bool>DeleteSectionById(int id)
        {
          
                var homeId = db.Home.OrderBy(e => e.HomeId).Include(e => e.Content).Where(a => a.HomeId == id).FirstOrDefault();
                db.Home.Remove(homeId);
                await db.SaveChangesAsync();
                return true;
          
        }

        public bool CheckSectionDependencies(int id)
        {
            var count = db.Home.Count(u => u.HomeId == id);
            if (count == 0)
            {
                return true;
            }
            return false;
        }



    }
}
