using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WanderLust.Models.DataModels
{
    public class Content
    {
        [Key]
        public int ContentId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public int HomeIdFK { get; set; }
        public string Graphics { get; set; }
        public string SubsectionName { get; set; }
        public Home Home { get; set; }
    }
}
