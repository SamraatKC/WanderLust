using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WanderLust.Data;
using WanderLust.Models.CommonModels;
using WanderLust.Models.DataModels;
using WanderLust.Common;

namespace WanderLust.Service
{
    public class UserService
    {
        WanderlustDbx db;
        IOptions<AppSettings> appSettings;

        public UserService(IOptions<AppSettings> _appSettings)
        {
            appSettings = _appSettings;
            db = new WanderlustDbx(_appSettings);
        }

        public async Task<List<AspNetUser>> GetAllUsers()
        {
            return await db.AspNetUsers.ToListAsync();
        }

        //public async Task<List<AspNetRole>> GetAllRoles()
        //{
        //    return await db.AspNetRoles.ToListAsync();
        //}

        //public async Task<List<AspNetUser>> GetAllUsersByRoleName(string roleName)
        //{
        //    return await db.AspNetUserRoles.Where(x => x.AspNetRole.Name == roleName).Select(x => x.AspNetUser).ToListAsync();
        //}


        public async Task<AspNetUser> FindUserById(string id)
        {
            var res = await db.AspNetUsers.FindAsync(id);
            return res;
        }
    }
}
