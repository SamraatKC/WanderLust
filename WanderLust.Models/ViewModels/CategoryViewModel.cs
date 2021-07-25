using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WanderLust.Models.DataModels;

namespace WanderLust.Models.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public static implicit operator CategoryViewModel(Category category)
        {
            CategoryViewModel vm = new CategoryViewModel();
            if (category != null)
            {
                vm = new CategoryViewModel();
                vm.CategoryId = category.CategoryId;
                vm.CategoryName = category.CategoryName;
                return vm;
            }
            return null;
        }
    }
}
