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
    public class ContentController : ControllerBase
    {
        WanderlustDbx db;
        private readonly IOptions<AppSettings> appSettings;
        private readonly ContentService contentService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public ContentController(IOptions<AppSettings> _appSettings,
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

        }

        [HttpPost]
        [Route("AddContent")]
        public async Task<ApiResponse> AddContent(Content content)
        {
            try
            {

                content.Home = null;
                var result = await contentService.AddContent(content);

                if (result)
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

        [HttpPost]
        [Route("UpdateContent")]
        public async Task<Content> UpdateSection(int id, Content content)
        {
            content.Home = null;
            var result = await contentService.FindContentById(id);
            if (result != null)
            {
                return await contentService.UpdateContent(id, content);
            }
            return null;
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