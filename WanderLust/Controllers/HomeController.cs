using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WanderLust.Data;
using WanderLust.Models.CommonModels;
using WanderLust.Models.DataModels;
using WanderLust.Service;

namespace WanderLust.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        WanderlustDbx db;
        private readonly IOptions<AppSettings> appSettings;
        private readonly HomeService services;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public HomeController(IOptions<AppSettings> _appSettings,
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
            RoleManager<IdentityRole> _roleManager)
        {
            appSettings = _appSettings;
            services = new HomeService(_appSettings);
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            db = new WanderlustDbx(_appSettings);

        }
        [HttpPost]
        [Route("AddSection")]
        public async Task<ApiResponse> AddSection(Home home)
        {
            try
            {

                home.Content = null;
                
                var result = await services.AddSection(home);

                if (result)
                {
                  
                    return new ApiResponse(CustomResponseMessage.SectionAdded);
                }
                return new ApiResponse(CustomResponseMessage.InternalServerError);
            }
            catch (Exception ex)
            {
                return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
            }

        }

        [HttpGet]
        [Route("GetAllSection")]
        public async Task<List<Home>> GetAllSection()
        {
            return await services.GetAllSection();
        }

        [HttpGet]
        //[Authorize]
        [Route("FindSectionById")]
        public async Task<ApiResponse> FindSectionById(int id)
        {
            try
            {
                var sectionDetail = await services.FindSectionById(id);
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
        [Route("DeleteSectionById")]
        public async Task<ApiResponse> DeleteSectionById(int id)
        {
            try
            {

               // home.Content = null;

                var result  = await services.DeleteSectionById(id);

                if (result)
                {

                    return new ApiResponse(CustomResponseMessage.SectionDeleted);
                }
                return new ApiResponse(CustomResponseMessage.InternalServerError);
            }
            catch (Exception ex)
            {
                return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
            }

        }
    }
}