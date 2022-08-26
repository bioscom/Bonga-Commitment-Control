using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Services
{
    public interface IEmailSender
    {
        //Task SendEmailAsync(string email, string subject, string message);
        Task SendEmailAsync(structUserMailIdx mailFrom, structUserMailIdx mailTo, structUserMailIdx cCopy, string subject, string message);
        Task SendEmailAsync(structUserMailIdx mailFrom, structUserMailIdx mailTo, List<structUserMailIdx> cCopy, string subject, string message);
        Task SendEmailAsync(structUserMailIdx mailFrom, List<structUserMailIdx> mailTo, List<structUserMailIdx> cCopy, string subject, string message);
        Task SendEmailAsync(string mailFrom, string subject, string message);
    }
}
