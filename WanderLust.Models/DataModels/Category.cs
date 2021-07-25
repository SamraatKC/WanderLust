using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using WanderLust.Models.ViewModels;

namespace WanderLust.Models.DataModels
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public static implicit operator Category(CategoryViewModel vm)
        {
            Category m = new Category();
            m.CategoryId = vm.CategoryId;
            m.CategoryName = vm.CategoryName;
            return m;
        }
    }
}
