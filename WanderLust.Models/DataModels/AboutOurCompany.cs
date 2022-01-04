using System;
using System.Collections.Generic;
using System.Text;
using WanderLust.Models.ViewModels;

namespace WanderLust.Models.DataModels
{
    public class AboutOurCompany
    {
        public int AboutOurCompanyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
    

    public static implicit operator AboutOurCompany(AboutOurCompanyViewModel vm)
    {
        AboutOurCompany model = new AboutOurCompany();
        model.AboutOurCompanyId = (Byte)vm.AboutOurCompanyId;
        model.Title = vm.Title;
        model.Description = vm.Description;
        model.ImageURL = vm.ImageURL;
      
        return model;
    }
    }
}
