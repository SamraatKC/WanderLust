
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using WanderLust.Models.CommonModels;
using WanderLust.Models.DataModels;

namespace WanderLust.Data
{
    public class WanderlustDbx:DbContext
    {
        private readonly AppSettings appSettings;

        public WanderlustDbx(IOptions<AppSettings> appSettingsOptions)
        {
            appSettings = appSettingsOptions.Value;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(appSettings.DefaultConnectionString);
        }


        public DbSet<AspNetUser> AspNetUsers { get; set; }

    }
}
