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
using System.Linq;
using Microsoft.AspNetCore.Identity;

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

       


        public async Task<AspNetUser> FindUserById(string id)
        {
            var res = await db.AspNetUsers.FindAsync(id);
            return res;
        }

        public async Task<AspNetUser> FindUserByEmail(string email)
        {
            var res= db.AspNetUsers.Where(x => x.Email == email).FirstOrDefault();
            return res; ;

        }

        public async Task<AspNetUser> IsPasswordReset(string email)
        {
            var res = db.AspNetUsers.Where(x => x.Email == email).FirstOrDefault();
            if(res!=null)
            {
                res.IsPasswordReset = true;
                await db.SaveChangesAsync();
            }
            return res;
        }

        public async Task<AspNetUser> ForgotPassword(string email,string newpassword)
        {
            var res = db.AspNetUsers.Where(x => x.Email == email).FirstOrDefault();
            if (res != null)
            {
                res.PasswordHash = newpassword;
                await db.SaveChangesAsync();
            }
            return res;
        }

    }
}
