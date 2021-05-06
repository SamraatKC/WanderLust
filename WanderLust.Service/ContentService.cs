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
        private IWebHostEnvironment webHostEnvironment;

        public ContentService(IOptions<AppSettings> _appSettings, IWebHostEnvironment _webHostEnvironment)
        {
            appSettings = _appSettings;
            db = new WanderlustDbx(_appSettings);
            webHostEnvironment = _webHostEnvironment;
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
                        //Delete Old file


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


        public async Task<ContentViewModel> UpdateContent(ContentViewModel contentViewModel)
        {           
            if (contentViewModel.ContentId > 0)
            {
                var result = await db.Content.FirstOrDefaultAsync(e => e.ContentId == contentViewModel.ContentId);
                result.Title = contentViewModel.Title;
                result.SubTitle = contentViewModel.SubTitle;
                result.Description = contentViewModel.Description;
                if(await db.SaveChangesAsync() > 0)
                    return contentViewModel;
                else
                {
                    //-1 meaning not able to update the record.
                    contentViewModel.ContentId = -1;
                    return contentViewModel;
                }
            }
            else
            {
                Content content = new Content();
                content = (Content)contentViewModel;
                db.Content.Add(content);
                await db.SaveChangesAsync();
                return (ContentViewModel)content;
            }
        }

        public async Task<ContentViewModel> SaveOrUpdateContent([FromForm]ContentViewModel contentViewModel)
        {
            if (contentViewModel.ContentId > 0)
            {
                var result = await db.Content.FirstOrDefaultAsync(e => e.ContentId == contentViewModel.ContentId);
                result.Title = contentViewModel.Title;
                result.SubTitle = contentViewModel.SubTitle;
                result.Description = contentViewModel.Description;

                if (result.GraphicsURL != contentViewModel.GraphicsURL)
                {
                    // Saving Image on Server
                    if (contentViewModel.Graphics.Length > 0)
                    {
                        var uploads = webHostEnvironment.WebRootPath + appSettings.Value.UploadPath;
                        var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(contentViewModel.Graphics.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await contentViewModel.Graphics.CopyToAsync(fileStream);
                            result.GraphicsURL = fileName;
                            contentViewModel.GraphicsURL = fileName;
                        }
                    }
                }

                if (await db.SaveChangesAsync() > 0)
                    return contentViewModel;
                else
                {
                    //-1 meaning not able to update the record.
                    contentViewModel.ContentId = -1;
                    return contentViewModel;
                }
            }
            else
            {
                Content content = new Content();
                content = (Content)contentViewModel;
                if (contentViewModel.Graphics.Length > 0)
                {
                    var uploads = webHostEnvironment.WebRootPath + appSettings.Value.UploadPath;
                    var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(contentViewModel.Graphics.FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.OpenOrCreate))
                    {
                        await contentViewModel.Graphics.CopyToAsync(fileStream);
                        content.GraphicsURL = fileName;
                    }
                }
                db.Content.Add(content);
                await db.SaveChangesAsync();
                return (ContentViewModel)content;
            }
        }


        public async Task<List<ContentViewModel>> GetAllContent()
        {
            return await db.Content.Select(x=> (ContentViewModel)x).ToListAsync();
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
            //when checking dependency to owns table must avoid checking self record.
            var count = db.Content.Count(u => u.ContentId == id);
            //what the **** is going on here, if count is equal to zero, it is not dependent but if count value is greater than zero then this record is dependent to other.
            if (count == 0)
            {
                return true;
            }
            return false;
        }
    }
}
