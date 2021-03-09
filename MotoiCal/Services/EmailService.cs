using MotoiCal.Interfaces;
using MotoiCal.Models;
using MotoiCal.Models.FileManagement;

using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MotoiCal.Services
{
    class EmailService
    {
        private readonly SmtpClient smtp;
        private readonly MailMessage mail;
        private readonly EmailModel emailModel;
        private readonly EncryptionManager encryptionManager;

        private string apiSender = "motoical@outlook.com";
        private string apiKeyLocation => ".\\apikey";

        public EmailService()
        {
            this.smtp = new SmtpClient();
            this.mail = new MailMessage();
            this.emailModel = new EmailModel();
            this.encryptionManager = new EncryptionManager();
        }


        public async Task<string> SendCalendar<T>(T motorSport) where T : IRaceTimeTable, ICalendarEvent // Add another method with options for attachment etc
        {
            if (this.encryptionManager.IsFileCreated(motorSport.FilePath))
            {
                if (string.IsNullOrWhiteSpace(this.emailModel.To))
                {
                    return "Email address is empty, Configure in settings.";
                }

                return await this.Email(motorSport);
            }
   
            else
            {
                return "Generate Ical file to email.";
            }
        }

        private string GetApiOrUserPassword()
        {
            if (this.emailModel.From.Equals(this.apiSender) && string.IsNullOrWhiteSpace(this.emailModel.Password))
            {
                return this.encryptionManager.GetDecryptedFileContents(this.encryptionManager.EncryptionKey, this.apiKeyLocation);
            }
            else
            {
                return this.emailModel.Password;
            }
        }

        private async Task<string> Email<T>(T motorSport) where T : IRaceTimeTable, ICalendarEvent
        {
            // Set smtp-client with basicAuthentication.
            this.smtp.Host = this.emailModel.Host;
            this.smtp.Port = this.emailModel.Port;

            // Must be set before NetworkCredentials.
            this.smtp.UseDefaultCredentials = false;
            this.smtp.Credentials = new NetworkCredential(this.emailModel.UserName, this.GetApiOrUserPassword());
            this.smtp.EnableSsl = this.emailModel.IsSSL;

            // Add from & to mailaddresses.
            this.mail.From = new MailAddress(this.emailModel.From);
            this.mail.To.Add(this.emailModel.To);

            // Set subject & encoding.
            this.mail.Subject = $"{motorSport.SportIdentifier} Calendar";
            this.mail.SubjectEncoding = System.Text.Encoding.UTF8;

            // Set body-message & encoding.
            this.mail.Body = "Calendar attached";
            this.mail.BodyEncoding = System.Text.Encoding.UTF8;

            // Set attachment.
            Attachment data = new Attachment(motorSport.FilePath);
            this.mail.Attachments.Add(data);

            try
            {
                // Send Mail.
                await this.smtp.SendMailAsync(this.mail);
                return "Email sent sucessfully.";
            }
            catch (SmtpException ex)
            {
                return "SmtpException has occured: " + ex.Message;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
