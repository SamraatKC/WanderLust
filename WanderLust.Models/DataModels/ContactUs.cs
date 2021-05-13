using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WanderLust.Models.ViewModels;

namespace WanderLust.Models.DataModels
{
    public class ContactUs
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Byte Id { get; set; }
        [MaxLength(500)]
        public string Introduction { get; set; }
        [MaxLength(50)]
        public string StreetAddress { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
        [MaxLength(10)]
        public string ZipCode { get; set; }
        [MaxLength(50)]
        public string Country { get; set; }
        [MaxLength(15)]
        public string Telephone1 { get; set; }
        [MaxLength(15)]
        public string Telephone2 { get; set; }
        [MaxLength(100)]
        public string EmailAddress { get; set; }

        public static implicit operator ContactUs(ContactUsViewModel vm)
        {
            ContactUs model = new ContactUs();
            model.Id = (Byte)vm.Id;
            model.Introduction = vm.Introduction;
            model.StreetAddress = vm.StreetAddress;
            model.City = vm.City;
            model.State = vm.State;
            model.ZipCode = vm.ZipCode;
            model.Country = vm.Country;
            model.Telephone1 = vm.Telephone1;
            model.Telephone2 = vm.Telephone2;
            model.EmailAddress = vm.EmailAddress;
            return model;
        }

    }
}
