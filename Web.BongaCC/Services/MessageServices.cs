using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using Web.BongaCC.Models;
using Microsoft.Extensions.Options;
using MimeKit.Text;

namespace App.Services
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly IOptions<appSettingsModel> appSettings;

        //private structUserMailIdx m_eSender;

        public AuthMessageSender(IOptions<appSettingsModel> app)
        {
            appSettings = app;
            //m_eSender = _eSender;
        }

        public async Task SendEmailAsync(structUserMailIdx mailFrom, structUserMailIdx mailTo, structUserMailIdx cCopy, string subject, string message)
        {
            try
            {
                string BodyContent = message;

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(mailFrom.m_sUserName, mailFrom.m_sUserMail));
                mimeMessage.To.Add(new MailboxAddress(mailTo.m_sUserName, mailTo.m_sUserMail));
                mimeMessage.To.Add(new MailboxAddress(cCopy.m_sUserName, cCopy.m_sUserMail));

                mimeMessage.Subject = subject;
                mimeMessage.Body = new TextPart(TextFormat.Html)
                {
                    Text = BodyContent
                };

                await Sender(mimeMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendEmailAsync(structUserMailIdx mailFrom, List<structUserMailIdx> mailTo, List<structUserMailIdx> cCopy, string subject, string message)
        {
            try
            {
                string BodyContent = message;

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(mailFrom.m_sUserName, mailFrom.m_sUserMail));
                foreach (var to in mailTo) mimeMessage.To.Add(new MailboxAddress(to.m_sUserName, to.m_sUserMail));
                foreach (var copy in cCopy) mimeMessage.To.Add(new MailboxAddress(copy.m_sUserName, copy.m_sUserMail));

                mimeMessage.Subject = subject;
                mimeMessage.Body = new TextPart(TextFormat.Html)
                {
                    Text = BodyContent
                };

                await Sender(mimeMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendEmailAsync(structUserMailIdx mailFrom, structUserMailIdx mailTo, List<structUserMailIdx> cCopy, string subject, string message)
        {
            try
            {
                string BodyContent = message;

                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(mailFrom.m_sUserName, mailFrom.m_sUserMail));
                mimeMessage.To.Add(new MailboxAddress(mailTo.m_sUserName, mailTo.m_sUserMail));
                foreach (var copy in cCopy) mimeMessage.To.Add(new MailboxAddress(copy.m_sUserName, copy.m_sUserMail));
                
                mimeMessage.Subject = subject;
                mimeMessage.Body = new TextPart(TextFormat.Html)
                {
                    Text = BodyContent
                };

                await Sender(mimeMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendEmailAsync(string mailFrom, string subject, string message)
        {
            try
            {
                string BodyContent = message;

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(mailFrom, mailFrom));
                mimeMessage.To.Add(new MailboxAddress(mailFrom, mailFrom));

                mimeMessage.Subject = subject;
                mimeMessage.Body = new TextPart(TextFormat.Html)
                {
                    Text = BodyContent
                };

                await Sender(mimeMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //throw new NotImplementedException();
        }

        public async Task Sender(MimeMessage mimeMessage)
        {
            try
            {
                // configure and send email
                using (var client = new SmtpClient())
                {
                    ///TODO:https://www.c-sharpcorner.com/article/reading-values-from-appsettings-json-in-asp-net-core/
                    string tSmtp = appSettings.Value.SmtpServer;
                    client.Connect(tSmtp);
                    await client.SendAsync(mimeMessage);
                    Console.WriteLine("The mail has been sent successfully !!");
                    Console.ReadLine();
                    await client.DisconnectAsync(true);
                }

                //using (var client = new SmtpClient())
                //{
                //    client.Connect(SmtpServer, SmtpPortNumber, false);
                //    client.Authenticate("myemail@mydomain.com", "MyPassword");
                //    await client.SendAsync(mimeMessage);
                //    Console.WriteLine("The mail has been sent successfully !!");
                //    Console.ReadLine();
                //    await client.DisconnectAsync(true);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
