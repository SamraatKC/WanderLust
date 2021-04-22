using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WanderLust.Models.DataModels
{
    public class HappyCustomer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HappyCustomerId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public int Flights { get; set; }
        public int Hotels { get; set; }
        public int Cars { get; set; }
        public int Cruises { get; set; }

    }
}
