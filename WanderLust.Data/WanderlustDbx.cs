
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

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Content>(entity =>
            {
                entity.HasKey(e => new { e.ContentId });
                entity.Property(x => x.ContentId).ValueGeneratedOnAdd();
                entity.HasOne(p => p.Home)
                .WithMany(p => p.Content)
                .HasForeignKey(p=>p.HomeIdFK)
                //.IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);                
            });

            mb.Entity<Home>(entity =>
            {
                entity.HasKey(e => new { e.HomeId });
                entity.Property(x => x.HomeId).ValueGeneratedOnAdd();           
                //entity.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            });

            mb.Entity<HappyCustomer>(entity =>
            {
                entity.HasKey(e => new { e.HappyCustomerId });
                entity.Property(x => x.HappyCustomerId).ValueGeneratedOnAdd();
            });


        }


        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<Home> Home { get; set; }
        public DbSet<Content> Content { get; set; }
        public DbSet<HappyCustomer> HappyCustomer { get; set; }

    }
}
