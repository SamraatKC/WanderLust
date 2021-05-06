using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using WanderLust.Models.DataModels;

namespace WanderLust.Models.ViewModels
{
    public class ContentViewModel
    {
        public int ContentId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public int? HomeIdFK { get; set; }
        public IFormFile Graphics { get; set; }
        public string GraphicsURL { get; set; }

        public static implicit operator ContentViewModel(Content content)
        {
            ContentViewModel contentVM = new ContentViewModel();
            contentVM.ContentId = content.ContentId;
            contentVM.HomeIdFK = content.HomeIdFK;
            contentVM.Description = content.Description;
            contentVM.Graphics = content.Graphics;
            contentVM.Title = content.Title;
            contentVM.SubTitle = content.SubTitle;
            contentVM.GraphicsURL = content.GraphicsURL;

            return contentVM;
        }
    }
}
