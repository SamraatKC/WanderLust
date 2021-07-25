﻿using System;
using System.Collections.Generic;
using System.Text;
using WanderLust.Models.DataModels;

namespace WanderLust.Models.ViewModels
{
    public class HappyCustomerViewModel
    {
        public int HappyCustomerId { get; set; }
        public string RatingBy { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public byte Flights { get; set; }
        public byte Hotels { get; set; }
        public byte Cars { get; set; }
        public byte Cruises { get; set; }
        public UserViewModel RatingUser { get; set; }

        public static implicit operator HappyCustomerViewModel(HappyCustomer model)
        {
            HappyCustomerViewModel vm = new HappyCustomerViewModel();
            vm.HappyCustomerId = model.HappyCustomerId;
            vm.RatingBy = model.RatingBy;
            vm.Title = model.Title;
            vm.SubTitle = model.SubTitle;
            vm.Description = model.Description;
            vm.Flights = model.Flights;
            vm.Hotels = model.Hotels;
            vm.Cars = model.Cars;
            vm.Cruises = model.Cruises;
            vm.RatingUser = model.AspNetUser == null ? null :  new UserViewModel()
            {
                Email = model.AspNetUser.Email,
                FirstName = model.AspNetUser.FirstName,
                LastName = model.AspNetUser.LastName,

            };
            return vm;

        }

    }
}