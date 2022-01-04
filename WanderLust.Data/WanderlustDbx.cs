
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using WanderLust.Models.CommonModels;
using WanderLust.Models.DataModels;
using WanderLust.Common;

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
            mb.Entity<AspNetUser>(entity => {
                entity.HasKey(e => e.Id);
            });

            mb.Entity<Content>(entity =>
            {
                entity.HasKey(e => new { e.ContentId });
                entity.HasOne(p => p.Home)
                .WithMany(p => p.Content)
                .HasForeignKey(p => p.HomeIdFK)
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
                entity.HasKey(e => e.HappyCustomerId);
                entity.Property(x => x.HappyCustomerId).ValueGeneratedOnAdd();
                entity.HasOne(x => x.AspNetUser)
                .WithMany()
                .HasForeignKey(x => x.RatingBy);
            });

            mb.Entity<ContactUs>(e => {
                e.HasKey(k => k.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            mb.Entity<GalleryImages>(entity =>
            {
                entity.HasKey(k => k.ImageId);
            });

            mb.Entity<Category>(e =>
            {
                e.HasKey(k => k.CategoryId);
            });
            mb.Entity<AboutOurCompany>(e =>
            {
                e.HasKey(k => k.AboutOurCompanyId);
            });
            mb.Entity<OurTeam>(e =>
            {
                e.HasKey(k => k.OurTeamId);
            });
        }


        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<Home> Home { get; set; }
        public DbSet<Content> Content { get; set; }
        public DbSet<HappyCustomer> HappyCustomer { get; set; }

        public DbSet<ContactUs> ContactUs { get; set; }

        public DbSet<GalleryImages> GalleryImages { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<AboutOurCompany>AboutOurCompany { get; set; }
        public DbSet<OurTeam> OurTeam { get; set; }
        public DbSet<TeamMember> TeamMember { get; set; }

    }
}
