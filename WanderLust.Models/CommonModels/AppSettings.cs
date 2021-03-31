using System;
using System.Collections.Generic;
using System.Text;

namespace WanderLust.Models.CommonModels
{
    public class AppSettings
    {
        public string DefaultConnectionString { get; set; }
        public string JwtKey { get; set; }
        public string JwtExpireDays { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }


    }
}
