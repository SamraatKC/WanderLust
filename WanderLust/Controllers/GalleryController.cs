using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WanderLust.Models.CommonModels;
using WanderLust.Models.ViewModels;
using WanderLust.Service;
using WanderLust.Common;

namespace WanderLust.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private IWebHostEnvironment webHostEnvironment;
        private GalleryService gallerySerivce;
        private IOptions<AppSettings> appSettings;
        public GalleryController(IWebHostEnvironment _webHostEnvironment, GalleryService _galleryService, IOptions<AppSettings> _appSettings)
        {
            webHostEnvironment = _webHostEnvironment;
            appSettings = _appSettings;
            gallerySerivce = _galleryService;
        }

        [HttpPost, Produces("application/json")]
        [Route("UploadFile")]
        public async Task<ApiResponse> UploadFile([FromForm] GalleryImageViewModel vm)
        {
            try
            {

                #region saveimage
                var graphics = HttpContext.Request.Form.Files;
                foreach (var Graphics in graphics)
                {
                    if (Graphics != null && Graphics.Length > 0)
                    {
                        var file = Graphics;
                        var uploads = webHostEnvironment.WebRootPath + appSettings.Value.UploadGalleryImagePath;
                        //var uploads = Path.Combine(Directory.GetCurrentDirectory(), "~\\Uploads\\");
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                string filePath = appSettings.Value.UploadGalleryImagePath + fileName;
                                string baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
                                vm.ImageId = Guid.NewGuid().ToString();
                                vm.FileName = fileName;
                            }
                        }
                    }
                }
                #endregion
                var result = await gallerySerivce.UpdateImageFileName(vm);

                if (result == true)
                {
                    return new ApiResponse(new { CustomResponseMessage.FileUploaded }, StatusCodes.Status200OK);
                }

                return new ApiResponse(new { CustomResponseMessage.InternalServerError }, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message, ex, StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("RemoveGalleryImage")]
        public async Task<ApiResponse> RemoveGalleryImage(List<string> galleryImagesIds)
        {
            try
            {
                await gallerySerivce.DeleteFileRecord(galleryImagesIds);
                List<string> imageFileNames = await gallerySerivce.GetFileNameById(galleryImagesIds);
                if (imageFileNames != null)
                {
                    var uploads = webHostEnvironment.WebRootPath + appSettings.Value.UploadGalleryImagePath;
                    foreach (string fileName in imageFileNames)
                        System.IO.File.Delete(string.Concat(uploads, fileName));
                }

            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message, ex, StatusCodes.Status500InternalServerError);
            }
            return new ApiResponse("Image removed successfully", StatusCodes.Status200OK);
        }

        [HttpGet]
        [Route("GetAllGallery")]
        public async Task<ApiResponse> GetAllGallery()
        {
            try
            {
                List<GalleryImageViewModel> images = await gallerySerivce.GetAllGalleryImages();
                return new ApiResponse(images, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message, ex, StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("UpdateGalleryCaption")]
        public async Task<ApiResponse> UpdateGalleryCaption(GalleryImageViewModel vm)
        {
            try
            {
                await gallerySerivce.UpdateImageCaption(vm);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message, ex, StatusCodes.Status500InternalServerError);
            }
            return new ApiResponse("Image caption updated successfully", StatusCodes.Status200OK);
        }
    }
}