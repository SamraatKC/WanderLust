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
using WanderLust.Models.ViewModels;
using WanderLust.Common;

namespace WanderLust.Service
{
    public class HappyCustomerService
    {
        WanderlustDbx db;
        IOptions<AppSettings> appSettings;
        public HappyCustomerService(IOptions<AppSettings> _appSettings)
        {
            appSettings = _appSettings;
            db = new WanderlustDbx(_appSettings);
        }

        public async Task<bool> AddHappyCustomer(HappyCustomerViewModel happyCustomerViewModel)
        {
            HappyCustomer happyCustomer = new HappyCustomer()
            {
                Title = happyCustomerViewModel.Title,
                SubTitle = happyCustomerViewModel.SubTitle,
                Description = happyCustomerViewModel.Description,
                Flights = happyCustomerViewModel.Flights,
                Hotels = happyCustomerViewModel.Hotels,
                Cars = happyCustomerViewModel.Hotels,
                Cruises = happyCustomerViewModel.Cruises
            };
            await db.HappyCustomer.AddAsync(happyCustomer);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateHappyCustomer(HappyCustomerViewModel happyCustomerViewModel)
        {
            HappyCustomer happyCustomer = await db.HappyCustomer.FindAsync(happyCustomerViewModel.HappyCustomerId);
            if (happyCustomer != null)
            {
                happyCustomer.Title = happyCustomerViewModel.Title;
                happyCustomer.SubTitle = happyCustomerViewModel.SubTitle;
                happyCustomer.Description = happyCustomerViewModel.Description;
                happyCustomer.Flights = happyCustomerViewModel.Flights;
                happyCustomer.Hotels = happyCustomerViewModel.Hotels;
                happyCustomer.Cars = happyCustomerViewModel.Hotels;
                happyCustomer.Cruises = happyCustomerViewModel.Cruises;
                await db.SaveChangesAsync();
                return true;
            };
            return false;
        }

        public async Task<List<HappyCustomerViewModel>> GetHappyCustomer()
        {
            //List<HappyCustomerViewModel> vlm = await db.HappyCustomer.Select(x => (HappyCustomerViewModel)x).ToListAsync();
            //return vlm;
            List<HappyCustomerViewModel> vlm = await db.HappyCustomer.Include(x=>x.AspNetUser).Select(x=>(HappyCustomerViewModel)x).ToListAsync();
            return vlm;
        }
    }
}
