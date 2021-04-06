using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WanderLust.Models.DataModels
{
    public class Home
    {
        [Key]
        public int HomeId { get; set; }
        public string Section { get; set; }
        public string ThemeType { get; set; }
        public List<Content> Content { get; set; }
    }
}
