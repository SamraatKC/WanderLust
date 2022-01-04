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
    public class TeamMemberController : ControllerBase
    {
        WanderlustDbx db;
        private readonly IOptions<AppSettings> appSettings;
        private readonly TeamMemberService teamMemberService;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private IWebHostEnvironment webHostEnvironment;
        public TeamMemberController(IWebHostEnvironment _webHostEnvironment, IOptions<AppSettings> _appSettings,
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
            RoleManager<IdentityRole> _roleManager,
            TeamMemberService _teamMemberService)
        {
            appSettings = _appSettings;
            teamMemberService = _teamMemberService;
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            db = new WanderlustDbx(_appSettings);
            webHostEnvironment = _webHostEnvironment;
        }

        [HttpPost]
        [Route("saveorupdate_teammember")]
        public async Task<ApiResponse> SaveOrUpdateTeamMember([FromForm] TeamMemberViewModel teamMemberViewModel)
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
                                teamMemberViewModel.ImageURL = fileName;
                            }
                        }
                    }
                }
                #endregion
                bool isSaved = await this.teamMemberService.AddOrUpdateTeamMember(teamMemberViewModel);
                if (isSaved)
                    return new ApiResponse("Changes successfully saved", teamMemberViewModel, StatusCodes.Status200OK);
                else
                    return new ApiResponse("Unable to save changes", null);
            }
            catch (Exception ex)
            {
                return new ApiResponse(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet]
        [Route("GetAllTeamMember")]
        public async Task<ApiResponse> GetAllTeamMember()
        {
            try
            {
                return new ApiResponse(await teamMemberService.GetAllTeamMember(), StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return new ApiResponse(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet]
        //[Authorize]
        [Route("GetTeamMemberById")]
        public async Task<ApiResponse> GetTeamMemberById(int id)
        {
            try
            {
                var aboutOurCompanyById = await teamMemberService.GetTeamMemberById(id);
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
        [Route("DeleteTeamMemberById")]
        public async Task<ApiResponse> DeleteTeamMemberById(int id)
        {
            try
            {

                var result = await teamMemberService.DeleteTeamMemberById(id);
                if (result)
                {

                    return new ApiResponse(CustomResponseMessage.TeamMemberDeleted);
                }


                return new ApiResponse(CustomResponseMessage.TeamMemberDeletionError);
            }
            catch (Exception ex)
            {
                return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
            }

        }
    }
}
