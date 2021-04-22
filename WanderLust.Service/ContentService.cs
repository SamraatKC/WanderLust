﻿using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WanderLust.Data;
using WanderLust.Models.CommonModels;
using WanderLust.Models.DataModels;
using WanderLust.Models.ViewModels;

namespace WanderLust.Service
{
    public class ContentService
    {
        WanderlustDbx db;
        IOptions<AppSettings> appSettings;
        public ContentService(IOptions<AppSettings> _appSettings)
        {
            appSettings = _appSettings;
            db = new WanderlustDbx(_appSettings);
        }

        public async Task<bool> AddContent(ContentViewModel contentViewModel)
        {
            Content result = new Content()
            {
                //ContentId=content.ContentId,
                Title = contentViewModel.Title,
                SubTitle = contentViewModel.SubTitle,
                Description = contentViewModel.Description,
                HomeIdFK = contentViewModel.HomeIdFK,
                GraphicsURL = contentViewModel.GraphicsURL,
                //SubsectionName = contentViewModel.SubsectionName,
                //ContentType= contentViewModel.ContentType
            };
            if (result.ContentId >= 0)
            {
                await db.Content.AddAsync(result);
                await db.SaveChangesAsync();
            }
            else
            {
                return false;
            }
            return true;

        }

        public async Task<bool> UploadFile(ImageViewModel imageViewModel)
        {
           
            var result = db.Content.Where(x=>x.Home.ThemeType == "Home" && x.Home.Section == "Root")
                .OrderBy(x=>x.ContentId).FirstOrDefault();
            if (result!=null)
            {
               try 
                {
                    if (result.ContentId > 0)
                    {
                        result.GraphicsURL = imageViewModel.GraphicsURL;
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        Content content = new Content();
                        content.GraphicsURL = imageViewModel.GraphicsURL;
                        await db.Content.AddAsync(content);
                        await db.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                Content content = new Content();
                content.GraphicsURL = imageViewModel.GraphicsURL;
                await db.Content.AddAsync(content);
                await db.SaveChangesAsync();
            }
            return true;

        }


        public async Task<Content> UpdateContent(ContentViewModel contentViewModel)
        {
            Content content = new Content();
            int id=content.ContentId;
            var result = await db.Content.FirstOrDefaultAsync(e => e.ContentId == id);
            if (result != null)
            {
                result.Title = contentViewModel.Title;
                result.SubTitle = contentViewModel.SubTitle;
                result.Description = contentViewModel.Description;
                result.HomeIdFK = contentViewModel.HomeIdFK;
                result.GraphicsURL = contentViewModel.GraphicsURL;
                //result.SubsectionName = contentViewModel.SubsectionName;
                //result.ContentType = contentViewModel.ContentType;
                await db.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<List<Content>> GetAllContent()
        {
            return await db.Content.ToListAsync();
        }


        public async Task<Content> FindContentById(int id)
        {
            var res = await db.Content.FindAsync(id);
            return res;
        }

        public async Task<bool> DeleteContentById(int id)
        {
            var contentId = db.Content.OrderBy(e => e.ContentId).Include(e => e.Home).Where(a => a.ContentId == id).FirstOrDefault();
            db.Content.Remove(contentId);
            await db.SaveChangesAsync();
            return true;

        }

        public bool CheckContentDependencies(int id)
        {
            var count = db.Content.Count(u => u.ContentId == id);
            if (count == 0)
            {
                return true;
            }
            return false;
        }
    }
}
