using System;
using System.Collections.Generic;
using System.Text;

namespace WanderLust.Common
{
    public class AppSettings
    {
        public string DefaultConnectionString { get; set; }
        public string JwtKey { get; set; }
        public string JwtExpireDays { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
        public string UploadPath { get; set; }
        public string VerificationLink { get; set; }
        public string ActivatedPostUrl { get; set; }
        public string AllowedOrigin { get; set; }
        public string AdminCode { get; set; }
        public string UploadImagePath { get; set; }
        public string UploadGalleryImagePath { get; set; }

        //EmailSenderHelper
        public string EmailSenderAddress { get; set; }
        public string EmailSenderDisplayName { get; set; }
        public string SMTPHost { get; set; }
        public int SMTPPort { get; set; }
        public string NetworkCredentialUserName { get; set; }
        public string NetworkCredentialPassword { get; set; }
        public string EmailTemplate_AccountVerification { get; set; }
    }
}