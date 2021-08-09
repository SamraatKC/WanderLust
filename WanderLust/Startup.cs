using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WanderLust.Data;
using WanderLust.MiddleWare;
using WanderLust.Models.CommonModels;
using WanderLust.Models.DataModels;
using WanderLust.Common;

namespace WanderLust
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var config = Configuration.GetSection("AppSettings").Get<AppSettings>();
            services.Configure<AppSettings>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<AppSettings>(Configuration.GetSection("appSettings"));
            #region CorsPolicy

            Debug.WriteLine("AllowedOrigin: " + config.AllowedOrigin);
            services.AddCors(options =>
            {
                options.AddPolicy(
                  "CorsPolicy",
                  builder => builder.WithOrigins(config.AllowedOrigin)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials());
            });
            
            services.AddAuthentication(IISDefaults.AuthenticationScheme);
            #endregion
            services.AddControllers(x => x.AllowEmptyInputInBodyModelBinding = true)
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                // add JWT Authentication on Swashbuckel API UI
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WanderLust API", Version = "v1" });
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {securityScheme, new string[] { }}
                    });
            });          
            string connectionString = Configuration.GetConnectionString("DefaultConnectionString");
            services.AddDbContext<ApplicationDbContext>(config =>
            {
                config.UseSqlServer(connectionString, b => b.MigrationsAssembly("WanderLust"));
            });
            #region configure AspNetIdentity

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = true;
                
            });

            #endregion
            #region configure JWT bearer configuration

            var key_phrase = Configuration.GetSection("AppSettings:JwtKey").Value;
            var key = Encoding.ASCII.GetBytes(key_phrase);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            #endregion


            #region injection

            services.AddScoped<WanderlustDbx>();
            services.AddScoped<Service.ContentService>();
            services.AddScoped<Service.ContactUsService>();
            services.AddScoped<Service.HappyCustomerService>();
            services.AddScoped<Service.GalleryService>();
            services.AddScoped<Service.CategoryService>();
            services.AddScoped<EmailHelper>();

            services.AddServiceExtension();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Redirect non api calls to angular app that will handle routing of the app.    
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                {
                    context.Request.Path = "/index.html";
                    context.Response.StatusCode = 200;
                    await next();
                }
                await next();
            });

            #region use swagger ui, to test the api endpoints
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wanderlust API");
            });
            #endregion
            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            #region Enable Cors
            app.UseCors("CorsPolicy");
            //app.UseMiddleware<CORMiddleware>();
            #endregion
            app.UseRouting();         
            app.UseAuthorization();                    
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //UpdateDatabase(app);
        }
        //private static void UpdateDatabase(IApplicationBuilder app)
        //{
        //    var serviceScope = app.ApplicationServices
        //        .GetRequiredService<IServiceScopeFactory>()
        //        .CreateScope();
        //    var context = serviceScope.ServiceProvider
        //        .GetService<ApplicationDbContext>();
        //    context.Database.Migrate();
        //}
    }
}
