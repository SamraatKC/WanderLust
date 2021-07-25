using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WanderLust.Common;
using WanderLust.Data;
using WanderLust.Models.CommonModels;
using WanderLust.Models.DataModels;
using WanderLust.Models.ViewModels;
using WanderLust.Service;

namespace WanderLust.Controllers
{
    
    [Route("api/[controller]")]
    //[EnableCors("CorsPolicy")]
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
            RoleManager<IdentityRole> _roleManager,
            ContentService _contentService)
        {
            appSettings = _appSettings;
            contentService = _contentService;
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            db = new WanderlustDbx(_appSettings);
            webHostEnvironment = _webHostEnvironment;
        }

        //[EnableCors("CorsPolicy")]
        [HttpPost, Produces("application/json")]
        [Route("AddContent")]
        public async Task<ApiResponse> AddContent(ContentViewModel contentViewModel)
        {
            try
            {
                //content.Home = null;
                #region saveimage

                var graphics = HttpContext.Request.Form.Files;
                foreach (var Graphics in graphics)
                {
                    if (Graphics != null && Graphics.Length > 0)
                    {
                        var file = Graphics;
                        //There is an error here
                        var uploads = webHostEnvironment.WebRootPath + appSettings.Value.UploadImagePath;
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                string filePath = appSettings.Value.UploadImagePath + fileName;
                                string baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
                                contentViewModel.GraphicsURL = fileName;
                            }
                        }
                    }
                }
                #endregion
                var result = await contentService.AddContent(contentViewModel);
                if (result == true)
                {
                    return new ApiResponse(CustomResponseMessage.ContentAdded);
                }

                return new ApiResponse(CustomResponseMessage.InternalServerError);
            }
            catch (Exception ex)
            {
                return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
            }

        }
        
        [HttpPost, Produces("application/json")]
        [Route("UpdateContent")]
        public async Task<ApiResponse> UpdateContent(ContentViewModel contentViewModel)
        {
            try
            {
                var response = await contentService.UpdateContent(contentViewModel);
                if (response.ContentId > 0)
                    return new ApiResponse(CustomResponseMessage.ContentUpdated);
                else
                    return new ApiResponse(StatusCodes.Status204NoContent, CustomResponseMessage.ContentUpdateFailed);
            }
            catch (Exception ex)
            {
                return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost, Produces("application/json")]
        [Route("SaveOrUpdateContent")]
        public async Task<ApiResponse>SaveOrUpdateContent([FromForm]ContentViewModel contentViewModel)
        {
            try
            {
                var response = await contentService.SaveOrUpdateContent(contentViewModel);
                if (response.ContentId > 0)
                    return new ApiResponse(response, StatusCodes.Status200OK);
                else
                    return new ApiResponse(StatusCodes.Status204NoContent, CustomResponseMessage.ContentUpdateFailed);
            }
            catch (Exception ex)
            {
                return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost, Produces("application/json")]
        [Route("UploadFile")]
        public async Task<ApiResponse> UploadFile([FromForm] ImageViewModel imageViewModel)
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
                        var uploads = webHostEnvironment.WebRootPath + appSettings.Value.UploadImagePath;
                        //var uploads = Path.Combine(Directory.GetCurrentDirectory(), "~\\Uploads\\");
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                string filePath = appSettings.Value.UploadImagePath + fileName;
                                string baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
                                imageViewModel.GraphicsURL = fileName;
                            }
                        }
                    }
                }
                #endregion

               var result= await contentService.UploadFile(imageViewModel);

                if (result == true)
                {
                    return new ApiResponse(new { CustomResponseMessage.FileUploaded }, StatusCodes.Status200OK);
                }

                return new ApiResponse(new { CustomResponseMessage.InternalServerError }, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetAllContent")]
        public async Task<ApiResponse> GetAllContent()
        {
            try
            {
                return new ApiResponse(await contentService.GetAllContent(), StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return new ApiResponse(StatusCodes.Status500InternalServerError, ex);
            }
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
        [Route("DeleteContentById/{id:int}")]
        public async Task<ApiResponse> DeleteContentById(int id)
        {
            try
            {
                int contentId = int.Parse(id.ToString());
                //for now only dependency check is check as there are improper logic in the method.
                //bool checkDependency = contentService.CheckContentDependencies(contentId);
                bool checkDependency = false;
                if (!checkDependency)
                {
                    var result = await contentService.DeleteContentById(contentId);

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