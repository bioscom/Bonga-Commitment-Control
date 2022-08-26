using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.Models
{
    public class appSettingsModel
    {
        public string DbConnection { get; set; }
        public string Email { get; set; }
        public string ErrorDeliveryEmailAddress { get; set; }
        public string SMTPPort { get; set; }
        public string SmtpServer { get; set; }
        public decimal Threshold { get; set; }
    }
}
