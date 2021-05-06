using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Text;
using WanderLust.Models.ViewModels;

namespace WanderLust.Models.DataModels
{
    public class Content
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContentId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public int? HomeIdFK { get; set; }      
        public string GraphicsURL { get; set; }
        [NotMapped]
        public IFormFile Graphics { get; set; }       
        public string SubsectionName { get; set; }
        public string ContentType { get; set; }
        public int? Ratings { get; set; }
        public Home Home { get; set; }

        public static implicit operator Content(ContentViewModel vm)
        {
            Content c = new Content();
            c.ContentId = vm.ContentId;
            c.Description = vm.Description;
            //c.GraphicsURL = vm.GraphicsURL;
            c.HomeIdFK = vm.HomeIdFK;
            c.SubTitle = vm.SubTitle;
            c.Title = vm.Title;
            return c;
        }
    }
}
