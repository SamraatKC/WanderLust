using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WanderLust.Models.ViewModels
{
   public class ImageViewModel
    {
      
        public IFormFile Graphics { get; set; }
        public string GraphicsURL { get; set; }
    }
}
