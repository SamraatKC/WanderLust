using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WanderLust.Models.ViewModels
{
    public class ContentViewModel
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public int HomeIdFK { get; set; }
        public IFormFile Graphics { get; set; }
        public string GraphicsURL { get; set; }
    }
}
