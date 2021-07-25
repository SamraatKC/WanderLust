using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WanderLust.Common
{
    public class EmailHelper
    {
        private  AppSettings appSettings;
        public EmailHelper(IOptions<AppSettings> _appSettings)
        {
            appSettings = _appSettings.Value;
        }

        public  EmailSentStatus SendEmail(string subject, string receiverEmailAddress, string body, List<string> cc = null, List<string> attachmentFilesLocation = null)
        {
            try
            {
                MailMessage mailMsg = new MailMessage();
                MailAddress fromAddress = new MailAddress(appSettings.EmailSenderAddress, appSettings.EmailSenderDisplayName);
                MailAddress toAddress = new MailAddress(receiverEmailAddress);
                mailMsg.From = fromAddress;
                mailMsg.To.Add(toAddress);
                if (cc != null)
                    foreach (string _cc in cc)
                        mailMsg.CC.Add(_cc);
                mailMsg.Subject = subject;
                mailMsg.IsBodyHtml = true;
                mailMsg.Body = body;
                if (attachmentFilesLocation != null)
                    foreach (string fileLocation in attachmentFilesLocation)
                        mailMsg.Attachments.Add(new Attachment(fileLocation));


                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                smtp.Host = appSettings.SMTPHost;
                smtp.Port = appSettings.SMTPPort;
                smtp.EnableSsl = true;
                
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(appSettings.NetworkCredentialUserName, appSettings.NetworkCredentialPassword);
                smtp.SendCompleted += Smtp_SendCompleted;
                smtp.SendMailAsync(mailMsg);
                
            }
            catch (SmtpFailedRecipientsException ex)
            {
                new EmailSentStatus(false, ex.Message);
            }
            return new EmailSentStatus(true, "Email Sent");

        }

        private static void Smtp_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if(e.Error != null)
            {
                //log Error;
            }
            else
            {
                //log success;
            }
        }

        public string GetEmailBody(string fileName)
        {
            var runningDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string htmlEmailBody;
            fileName = string.Concat(runningDirectory, fileName);
            using(StreamReader file = new StreamReader(fileName) ){
                htmlEmailBody = file.ReadToEnd();
            }
            return htmlEmailBody;
        }
    }
}
