using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WanderLust.Common;
using WanderLust.Data;
using WanderLust.Models.CommonModels;
using WanderLust.Models.DataModels;
using WanderLust.Models.ViewModels;
using WanderLust.Service;

namespace WanderLust.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutOurCompanyController : ControllerBase
    {
        WanderlustDbx db;
        private readonly IOptions<AppSettings> appSettings;
        private readonly AboutOurCompanyService aboutOurCompanyService;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private IWebHostEnvironment webHostEnvironment;
        public AboutOurCompanyController(IWebHostEnvironment _webHostEnvironment, IOptions<AppSettings> _appSettings,
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
            RoleManager<IdentityRole> _roleManager,
            AboutOurCompanyService _aboutOurCompanyService)
        {
            appSettings = _appSettings;
            aboutOurCompanyService = _aboutOurCompanyService;
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            db = new WanderlustDbx(_appSettings);
            webHostEnvironment = _webHostEnvironment;
        }

        [HttpPost]
        [Route("saveorupdate_aboutourcompany")]
        public async Task<ApiResponse> SaveOrUpdateAboutOurCompany([FromForm] AboutOurCompanyViewModel aboutOurCompanyViewModel)
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
                                //aboutOurCompanyViewModel.ImageId = Guid.NewGuid().ToString();
                                aboutOurCompanyViewModel.ImageURL = fileName;
                            }
                        }
                    }
                }
                #endregion
                bool isSaved = await this.aboutOurCompanyService.AddOrUpdateAboutOurCompany(aboutOurCompanyViewModel);
                if (isSaved)
                    return new ApiResponse("Changes successfully saved", aboutOurCompanyViewModel, StatusCodes.Status200OK);
                else
                    return new ApiResponse("Unable to save changes", null);
            }
            catch (Exception ex)
            {
                return new ApiResponse(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet]
        [Route("GetAllAboutOurCompany")]
        public async Task<ApiResponse> GetAllAboutOurCompany()
        {
            try
            {
                return new ApiResponse(await aboutOurCompanyService.GetAllAboutOurCompany(), StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return new ApiResponse(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet]
        //[Authorize]
        [Route("GetAboutOurCompanyById")]
        public async Task<ApiResponse> GetAboutOurCompanyById(int id)
        {
            try
            {
                var aboutOurCompanyById = await aboutOurCompanyService.GetAboutOurCompanyById(id);
                if (aboutOurCompanyById != null)
                {
                    return new ApiResponse(aboutOurCompanyById, 200);
                }
                return new ApiResponse(CustomResponseMessage.UnableToFindInformation, StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
            }

        }

        [HttpPost]
        [Route("DeleteAboutOurCompanyById")]
        public async Task<ApiResponse> DeleteAboutOurCompanyById(int id)
        {
            try
            {

                var result = await aboutOurCompanyService.DeleteAboutOurCompanyById(id);
                if (result)
                {

                    return new ApiResponse(CustomResponseMessage.AboutOurCompanyDeleted);
                }


                return new ApiResponse(CustomResponseMessage.AboutOurCompanyDeletionError);
            }
            catch (Exception ex)
            {
                return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
            }

        }
    }
}
