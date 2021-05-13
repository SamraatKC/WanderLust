using System;
using System.Collections.Generic;
using System.Text;
using WanderLust.Models.DataModels;

namespace WanderLust.Models.ViewModels
{
    public class ContactUsViewModel
    {
        public byte Id { get; set; }
        public string Introduction { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string EmailAddress { get; set; }

        public static implicit operator ContactUsViewModel(ContactUs model)
        {
            if (model == null) return null;
            ContactUsViewModel vm = new ContactUsViewModel();

            vm.Id = model.Id;
            vm.Introduction = model.Introduction;
            vm.StreetAddress = model.StreetAddress;
            vm.City = model.City;
            vm.State = model.State;
            vm.ZipCode = model.ZipCode;
            vm.Country = model.Country;
            vm.Telephone1 = model.Telephone1;
            vm.Telephone2 = model.Telephone2;
            vm.EmailAddress = model.EmailAddress;
            return vm;
        }
    }
}
