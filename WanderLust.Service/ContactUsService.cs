using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WanderLust.Data;
using WanderLust.Models.CommonModels;
using WanderLust.Models.DataModels;
using WanderLust.Models.ViewModels;
using WanderLust.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WanderLust.Service
{
    public class ContactUsService
    {
        private WanderlustDbx db;
        private IOptions<AppSettings> appSettings;
        public ContactUsService(WanderlustDbx _db, IOptions<AppSettings> _appSettings)
        {
            this.db = _db;
            this.appSettings = _appSettings;
        }

        public async Task<Boolean> AddOrUpdateContactUsInformation(ContactUsViewModel vm)
        {
            if (vm.Id > 0)
            {
                ContactUs model = db.ContactUs.FindAsync(vm.Id).Result;
                model.Introduction = vm.Introduction;
                model.StreetAddress = vm.StreetAddress;
                model.City = vm.City;
                model.State = vm.State;
                model.ZipCode = vm.ZipCode;
                model.Country = vm.Country;
                model.Telephone1 = vm.Telephone1;
                model.Telephone2 = vm.Telephone2;
                model.EmailAddress = vm.EmailAddress;
                await db.SaveChangesAsync();
                return true;
            }
            else
            {
                ContactUs model = (ContactUs)vm;
                await db.ContactUs.AddAsync(model);
                await db.SaveChangesAsync();
                vm.Id = model.Id;
                return true;
            }
        }

        public async Task<ContactUsViewModel> GetContactInfo()
        {
            var data = await this.db.ContactUs.FirstOrDefaultAsync();
            return (ContactUsViewModel)data;
        }
    }
}
