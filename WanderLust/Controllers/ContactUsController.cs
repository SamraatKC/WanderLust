using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WanderLust.Service;

using WanderLust.Models.ViewModels;
using AutoWrapper.Wrappers;

namespace WanderLust.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private ContactUsService service;
        public ContactUsController(ContactUsService _service)
        {
            this.service = _service;
        }

        [HttpPost]
        [Route("saveorupdate_contactus_info")]
        public async Task<ApiResponse> SaveOrUpdate(ContactUsViewModel vm)
        {
            try
            {
                bool isSaved = await this.service.AddOrUpdateContactUsInformation(vm);
                if (isSaved)
                    return new ApiResponse("Changes successfully saved", vm, StatusCodes.Status200OK);
                else
                    return new ApiResponse("Unable to save changes", null);
            }
            catch (Exception ex)
            {
                return new ApiResponse(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }

        [HttpGet]
        [Route("get_contact_info")]
        public async Task<ApiResponse> GetContactInfo()
        {
            try
            {
                ContactUsViewModel vm = await this.service.GetContactInfo();
                return new ApiResponse(vm, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {

                return new ApiResponse(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}