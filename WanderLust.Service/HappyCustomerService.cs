using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WanderLust.Data;
using WanderLust.Models.CommonModels;
using WanderLust.Models.DataModels;
using WanderLust.Models.ViewModels;

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
    }
}
