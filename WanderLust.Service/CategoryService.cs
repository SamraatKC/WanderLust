using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WanderLust.Common;
using WanderLust.Data;
using WanderLust.Models.CommonModels;
using WanderLust.Models.DataModels;
using WanderLust.Models.ViewModels;

namespace WanderLust.Service
{
    public class CategoryService
    {
        WanderlustDbx db;
        IOptions<AppSettings> appSettings;
        private IWebHostEnvironment webHostEnvironment;

        public CategoryService(IOptions<AppSettings> _appSettings, IWebHostEnvironment _webHostEnvironment, WanderlustDbx _db)
        {
            appSettings = _appSettings;
            db = _db;
            webHostEnvironment = _webHostEnvironment;
        }

        public async Task<bool> NewCategory(CategoryViewModel vm)
        { 
            Category model = (Category)vm;
            await db.Category.AddAsync(model);
            await db.SaveChangesAsync();
            return true;
        }

    
        public async Task<bool> UpdateCategory(CategoryViewModel vm)
        {
            Category category = await db.Category.Where(x => x.CategoryId == vm.CategoryId).FirstOrDefaultAsync();
            if (category != null)
            {
                category.CategoryName = vm.CategoryName;
                await db.SaveChangesAsync();
                return true;
            }
            return false ;
        }

        public async Task<List<CategoryViewModel>> GetAllCategory()
        {
            List<CategoryViewModel> categories = await db.Category
                .OrderByDescending(x => x.CategoryId)
                .Select<Category, CategoryViewModel>(x=>x)
                .ToListAsync();
            return categories;
        }
    }
}
