using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WanderLust.Models.DataModels;

namespace WanderLust.Models.ViewModels
{
    public class GalleryImageViewModel
    {
        public string ImageId { get; set; }
        public IFormFile Graphics { get; set; }
        public string FileName { get; set; }
        public int CategoryId { get; set; }

        public string Caption { get; set; }

        public static implicit operator GalleryImageViewModel(GalleryImages galleryImages)
        {
            GalleryImageViewModel vm = new GalleryImageViewModel();
            if (galleryImages != null)
            {
                vm = new GalleryImageViewModel();
                vm.CategoryId = galleryImages.CategoryId;
                vm.FileName = galleryImages.FileName;
                vm.ImageId = galleryImages.ImageId.ToString();
                vm.Caption = galleryImages.Caption;
                return vm;
            }
            return null;
        }
    }
}
