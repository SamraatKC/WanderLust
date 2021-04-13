using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WanderLust.Data;
using WanderLust.Models.CommonModels;
using WanderLust.Models.DataModels;
using WanderLust.Models.ViewModels;
using WanderLust.Service;

namespace WanderLust.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        WanderlustDbx db;
        private readonly IOptions<AppSettings> appSettings;
        private readonly ContentService contentService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private IWebHostEnvironment webHostEnvironment;
        public ContentController(IWebHostEnvironment _webHostEnvironment, IOptions<AppSettings> _appSettings,
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
            RoleManager<IdentityRole> _roleManager)
        {
            appSettings = _appSettings;
            contentService = new ContentService(_appSettings);
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            db = new WanderlustDbx(_appSettings);
            webHostEnvironment = _webHostEnvironment;

        }

        //[HttpPost]
        //[Route("AddContent")]
        //public async Task<ApiResponse> AddContent([FromForm]ContentViewModel contentViewModel)
        //{
        //    try
        //    {
                
        //        //content.Home = null;
        //        #region saveimage
        //        if()
        //        var graphics = HttpContext.Request.Form.Files;       
        //        foreach (var Graphics in graphics)
        //        {
        //            if (Graphics != null && Graphics.Length > 0)
        //            {
        //                var file = Graphics;
        //                //There is an error here
        //                var uploads = webHostEnvironment.WebRootPath + "\\Uploads\\";
        //                if (file.Length > 0)
        //                {
        //                    var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
        //                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
        //                    {
        //                        await file.CopyToAsync(fileStream);
        //                        string filePath = "\\Uploads\\" + fileName;
        //                        string baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
        //                        contentViewModel.GraphicsURL = fileName;
        //                    }

        //                }
        //            }
        //        }
        //        #endregion
        //        var result = await contentService.AddContent(contentViewModel);
        //        if (result==true)
        //        {
        //            return new ApiResponse(CustomResponseMessage.ContentAdded);
        //        }
              
        //        return new ApiResponse(CustomResponseMessage.InternalServerError);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
        //    }

        //}

        [HttpPost]
        [Route("UpdateContent")]
        public async Task<ApiResponse> UpdateContent([FromForm] Content content)
        {
            content.Home = null;
            int id = content.ContentId;
            var result = await contentService.FindContentById(id);
            if (result != null)
            {
                var graphics = HttpContext.Request.Form.Files;
                foreach (var Graphics in graphics)
                {
                    if (Graphics != null && Graphics.Length > 0)
                    {
                        var file = Graphics;
                        //There is an error here
                        var uploads = webHostEnvironment.WebRootPath + "\\Uploads\\";
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                string filePath = "\\Uploads\\" + fileName;
                                string baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
                                content.GraphicsURL = fileName;
                            }

                        }
                    }
                }
                await contentService.UpdateContent(content);
            }
            return new ApiResponse(CustomResponseMessage.ContentUpdated);
        }

        [HttpGet]
        [Route("GetAllContent")]
        public async Task<List<Content>> GetAllContent()
        {
            return await contentService.GetAllContent();
        }

        [HttpGet]
        //[Authorize]
        [Route("FindContentById")]
        public async Task<ApiResponse> FindContentById(int id)
        {
            try
            {
                var sectionDetail = await contentService.FindContentById(id);
                if (sectionDetail != null)
                {
                    return new ApiResponse(sectionDetail, 200);
                }
                return new ApiResponse(CustomResponseMessage.UnableToFindInformation, StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
            }

        }

        [HttpPost]
        [Route("DeleteContentById")]
        public async Task<ApiResponse> DeleteContentById(int id)
        {
            try
            {
                bool checkDependency = contentService.CheckContentDependencies(id);
                if (checkDependency == true)
                {
                    var result = await contentService.DeleteContentById(id);

                    if (result)
                    {

                        return new ApiResponse(CustomResponseMessage.ContentDeleted);
                    }
                }
                return new ApiResponse(CustomResponseMessage.ContentDeletionError);
            }
            catch (Exception ex)
            {
                return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
            }

        }
    }
}