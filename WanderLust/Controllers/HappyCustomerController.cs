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
using WanderLust.Models.ViewModels;
using WanderLust.Service;

namespace WanderLust.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class HappyCustomerController : ControllerBase
    {
        WanderlustDbx db;
        private readonly IOptions<AppSettings> appSettings;
        private readonly HappyCustomerService happyCustomerService;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public HappyCustomerController(IOptions<AppSettings> _appSettings, HappyCustomerService _happyCustomerService, WanderlustDbx _db)
        {
            appSettings = _appSettings;
            happyCustomerService = _happyCustomerService;
            db = _db;
        }

        [HttpPost]
        [Route("SaveHappyCustomer")]
        public async Task<ApiResponse> SaveHappyCustomer(HappyCustomerViewModel happyCustomerViewModel)
        {
            try
            {
                var result = await happyCustomerService.AddHappyCustomer(happyCustomerViewModel);
                if (result == true)
                {
                    return new ApiResponse(CustomResponseMessage.HappyCustomerAdded);
                }
                else
                {
                    return new ApiResponse(CustomResponseMessage.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
            }

        }

        [HttpPost]
        [Route("UpdateHappyCustomer")]
        public async Task<ApiResponse> UpdateHappyCustomer(HappyCustomerViewModel happyCustomerViewModel)
        {
            try
            {
                var result = await happyCustomerService.UpdateHappyCustomer(happyCustomerViewModel);
                if (result == true)
                {
                    return new ApiResponse(CustomResponseMessage.HappyCustomerAdded);
                }
                else
                {
                    return new ApiResponse(CustomResponseMessage.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
            }

        }

        [HttpGet]
        [Route("GetHappyCustomerInfo")]
        public async Task<ApiResponse> GetHappyCustomerInfo()
        {
            try
            {
                var result = await happyCustomerService.GetHappyCustomer();
                return new ApiResponse(result, StatusCodes.Status200OK);

            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message, StatusCodes.Status500InternalServerError);
            }

        }
    }
}