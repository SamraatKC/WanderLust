using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WanderLust.Common;
using WanderLust.Data;
using WanderLust.Models.CommonModels;
using WanderLust.Models.DataModels;
using WanderLust.Models.ViewModels;

namespace WanderLust.Service
{
    public class GalleryService
    {
        WanderlustDbx db;
        IOptions<AppSettings> appSettings;
        private IWebHostEnvironment webHostEnvironment;

        public GalleryService(IOptions<AppSettings> _appSettings, IWebHostEnvironment _webHostEnvironment, WanderlustDbx _db)
        {
            appSettings = _appSettings;
            db = _db;
            webHostEnvironment = _webHostEnvironment;
        }

        public async Task<bool> UpdateImageFileName(GalleryImageViewModel vm)
        {
            GalleryImages model = (GalleryImages)vm;
            await db.GalleryImages.AddAsync(model);
            await db.SaveChangesAsync();
            vm.ImageId = model.ImageId.ToString();
            return true;
        }

        public async Task<bool> DeleteFileRecord(List<string> ImageId)
        {
            List<GalleryImages> images = await db.GalleryImages.Where(x => ImageId.Contains(x.ImageId.ToString())).ToListAsync();
            if (images != null)
            {
                if (images.Count > 0)
                {
                    foreach (GalleryImages img in images)
                        db.GalleryImages.Remove(img);
                    db.SaveChanges();
                }
                return true;
            }
            return false;
        }
        public async Task<List<string>> GetFileNameById(List<string> imageIds)
        {
            List<string> filenames = await db.GalleryImages.Where(x => imageIds.Contains(x.ImageId.ToString())).Select(x => x.FileName).ToListAsync();
            return filenames;
        }

        public async Task<List<GalleryImageViewModel>> GetAllGalleryImages()
        {
            List<GalleryImageViewModel> images = await db.GalleryImages.Select<GalleryImages, GalleryImageViewModel>(x => x).ToListAsync();
            return images;
        }

        public async Task<bool> UpdateImageCaption(GalleryImageViewModel vm)
        {
            GalleryImages m = await db.GalleryImages.FindAsync(vm.ImageId);
            if (m != null)
            {
                m.Caption = vm.Caption;
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
