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
        public HappyCustomerController(IOptions<AppSettings> _appSettings,
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
            RoleManager<IdentityRole> _roleManager)
        {
            appSettings = _appSettings;
            happyCustomerService = new HappyCustomerService(_appSettings);
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            db = new WanderlustDbx(_appSettings);
        }

        [HttpPost]
        [Route("AddHappyCustomer")]
        public async Task<ApiResponse>AddHappyCustomer(HappyCustomerViewModel happyCustomerViewModel)
        {
         try
            {
                var result = await happyCustomerService.AddHappyCustomer(happyCustomerViewModel);
                if(result==true)
                {
                    return new ApiResponse(CustomResponseMessage.HappyCustomerAdded);
                }
                else
                {
                    return new ApiResponse(CustomResponseMessage.InternalServerError);
                }
            }
            catch(Exception ex)
            {
                return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
            }

        }
    }
}