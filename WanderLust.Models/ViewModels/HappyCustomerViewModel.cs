using System;
using System.Collections.Generic;
using System.Text;

namespace WanderLust.Models.ViewModels
{
    public class HappyCustomerViewModel
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public int Flights { get; set; }
        public int Hotels { get; set; }
        public int Cars { get; set; }
        public int Cruises { get; set; }
    }
}
