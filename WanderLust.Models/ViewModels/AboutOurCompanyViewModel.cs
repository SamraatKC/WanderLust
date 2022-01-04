using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using WanderLust.Models.DataModels;

namespace WanderLust.Models.ViewModels
{
    public class AboutOurCompanyViewModel
    {
        public int AboutOurCompanyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile AboutOurCompanyImage { get; set; }
        public string ImageURL { get; set; }

        public static implicit operator AboutOurCompanyViewModel(AboutOurCompany model)
        {
            if (model == null) return null;
            AboutOurCompanyViewModel vm = new AboutOurCompanyViewModel();

            vm.AboutOurCompanyId = model.AboutOurCompanyId;
            vm.Title = model.Title;
            vm.Description = model.Description;
            vm.ImageURL = model.ImageURL;
           return vm;
        }
    }
}
