using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderLust.Common;
using WanderLust.Models.CommonModels;
using WanderLust.Models.ViewModels;
using WanderLust.Service;

namespace WanderLust.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OurTeamController : ControllerBase
    {
        private IWebHostEnvironment webHostEnvironment;
        private OurTeamService ourTeamService;
        private IOptions<AppSettings> appSettings;
        public OurTeamController(IWebHostEnvironment _webHostEnvironment,
            OurTeamService _ourTeamService,
            IOptions<AppSettings> _appSettings,
            ILogger<OurTeamController> logger)
        {
            webHostEnvironment = _webHostEnvironment;
            appSettings = _appSettings;
            ourTeamService = _ourTeamService;
        }

        [HttpPost]
        [Route("AddOurTeam")]
        public async Task<ApiResponse> AddOurTeam(OurTeamViewModel ourTeamViewModel)
        {
            try
            {
                await ourTeamService.AddOurTeam(ourTeamViewModel);
                return new ApiResponse("Our Team Added", StatusCodes.Status200OK);

            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message, ex, StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("UpdateOurTeam")]
        public async Task<ApiResponse> UpdateOurTeam(OurTeamViewModel ourTeamViewModel)
        {
            try
            {
                await ourTeamService.UpdateOurTeam(ourTeamViewModel);
                return new ApiResponse("Our Team updated", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message, ex, StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetAllOurTeam")]
        public async Task<ApiResponse> GetAllOurTeam()
        {
            try
            {
                return new ApiResponse(await ourTeamService.GetAllOurTeam(), StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return new ApiResponse(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet]
        //[Authorize]
        [Route("GetOurTeamById")]
        public async Task<ApiResponse> GetOurTeamById(int id)
        {
            try
            {
                var ourTeamById = await ourTeamService.GetOurTeamById(id);
                if (ourTeamById != null)
                {
                    return new ApiResponse(ourTeamById, 200);
                }
                return new ApiResponse(CustomResponseMessage.UnableToFindInformation, StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
            }

        }

        [HttpPost]
        [Route("DeleteOurTeamById")]
        public async Task<ApiResponse> DeleteOurTeamById(int id)
        {
            try
            {
               
                    var result = await ourTeamService.DeleteOurTeamById(id);
                    if (result)
                    {

                        return new ApiResponse(CustomResponseMessage.OurTeamDeleted);
                    }
                

                return new ApiResponse(CustomResponseMessage.OurTeamDeletionError);
            }
            catch (Exception ex)
            {
                return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
            }

        }

    }
}
