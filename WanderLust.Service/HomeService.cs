using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WanderLust.Data;
using WanderLust.Models.CommonModels;
using WanderLust.Models.DataModels;

namespace WanderLust.Service
{
    public class HomeService
    {
        WanderlustDbx db;
        IOptions<AppSettings> appSettings;
        public HomeService(IOptions<AppSettings> _appSettings)
        {
            appSettings = _appSettings;
            db = new WanderlustDbx(_appSettings);
        }

        public async Task<bool> AddSection(Home home)
        {
           await db.Home.AddAsync(home);
            return db.SaveChanges() > 0;
           
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

        

    }
}
