using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using WanderLust.Models.ViewModels;

namespace WanderLust.Models.DataModels
{
    public class GalleryImages
    {
        public Guid ImageId { get; set; }
        public int CategoryId { get; set; }
        public string FileName { get; set; }
        public DateTime? UploadedDate { get; set; }

        public string Caption { get; set; }

        public static implicit operator GalleryImages(GalleryImageViewModel vm)
        {
            GalleryImages m = new GalleryImages();
            m.ImageId = Guid.Parse( vm.ImageId);
            m.CategoryId = vm.CategoryId;
            m.FileName = vm.FileName;
            m.UploadedDate = System.DateTime.Now;
            m.Caption = vm.Caption;
            return m;
        }
    }
}
