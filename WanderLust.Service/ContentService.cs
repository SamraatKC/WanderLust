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

        public async Task<bool> AddContent(Content content)
        {
            await db.Content.AddAsync(content);
            return db.SaveChanges() > 0;

        }

        public async Task<Content> UpdateContent(int id, Content content)
        {

            var result = await db.Content.FirstOrDefaultAsync(e => e.ContentId == id);
            if (result != null)
            {

                result.Title = content.Title;
                result.SubTitle = content.SubTitle;
                result.Description = content.Description;
                result.HomeIdFK = content.HomeIdFK;
                result.Graphics = content.Graphics;
                result.SubsectionName = content.SubsectionName;
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
