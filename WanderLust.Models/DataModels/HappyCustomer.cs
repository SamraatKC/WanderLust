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
        public string RatingBy { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public byte Flights { get; set; }
        public byte Hotels { get; set; }
        public byte Cars { get; set; }
        public byte Cruises { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }

    }
}