using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WanderLust.Common;
using WanderLust.Models.CommonModels;
using WanderLust.Models.ViewModels;
using WanderLust.Service;

namespace WanderLust.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private IWebHostEnvironment webHostEnvironment;
        private CategoryService categoryService;
        private IOptions<AppSettings> appSettings;
        public CategoryController(IWebHostEnvironment _webHostEnvironment, 
            CategoryService _categoryService, 
            IOptions<AppSettings> _appSettings,
            ILogger<CategoryController> logger)
        {
            webHostEnvironment = _webHostEnvironment;
            appSettings = _appSettings;
            categoryService = _categoryService;
        }

        [HttpPost]
        [Route("NewCategory")]
        public async Task<ApiResponse> NewCategory(CategoryViewModel category)
        {
            try
            {
               await categoryService.NewCategory(category);
                return new ApiResponse("New category created", StatusCodes.Status200OK);

            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message, ex, StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("UpdateCategory")]
        public async Task<ApiResponse> UpdateCategory(CategoryViewModel category)
        {
            try
            {
                await categoryService.UpdateCategory(category);
                return new ApiResponse("Category updated", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message, ex, StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetAllCategory")]
        public async Task<ApiResponse> GetAllGallery()
        {
            try
            {
                List<CategoryViewModel> category = await categoryService.GetAllCategory();
                return new ApiResponse(category, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message, ex, StatusCodes.Status500InternalServerError);
            }
        }
    }
}