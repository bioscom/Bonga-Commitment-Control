using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendEmail.Services;

namespace EF.Utility
{
    public class appMonitor
    {
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;

        public appMonitor()
        {
           
        }

        public appMonitor(IEmailSender emailSender, ISmsSender smsSender)
        {
            _emailSender = emailSender;
            _smsSender = smsSender;
        }

        public enum appDeployMode
        {
            localMachine = 1,
            testServer = 2,
            liveServer = 3
        };

        public appDeployMode appRunningServer()
        {
            appDeployMode eRet = appDeployMode.localMachine;
            try
            {
                //eRet = (appDeployMode)  CType(webConfig.sGetAppSettingValue("cpdms.deployMode"), appDeployMode)
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
            }
            return eRet;
        }

        public bool logAppExceptions(Exception eErr, string FromEmail, string ManifestNumber)
        {
            try
            {
                //System.Diagnostics.Debug.Fail(eErr.TargetSite.Name + "\n \n" + eErr.Message.ToString() + "\n \n" + eErr.StackTrace);
                string sErr = "";
                if (eErr.InnerException != null)
                {
                    System.Diagnostics.Debug.Fail(eErr.InnerException.TargetSite.Name + "\n \n" + eErr.InnerException.Message + "\n \n" + eErr.InnerException.StackTrace);
                    sErr = "Inner Exception Information=================" + "\n \n" + eErr.InnerException.TargetSite.Name + "\n \n" + "\n \n" + eErr.InnerException.Message + "\n \n" + "\n \n" + eErr.InnerException.StackTrace + "\n \n";
                }
                //sErr = sErr + "\n \n" + "This Exception Information================= \n\n Method@ " + eErr.TargetSite.Name + "\n \n" + "\n\n Message@ " + eErr.Message + "\n \n" + "\n\n Stack Trace@ \n\n" + eErr.StackTrace + "\n \n Source@ " + eErr.Source;


                //HttpContext context = HttpContext.Current;
                sErr += "\n\n Page location: "; //+ context.Request.RawUrl;

                // build the error message
                sErr += "\n\n Message: " + eErr.Message;
                sErr += "\n\n Source: " + eErr.Source;
                sErr += "\n\n Method: " + eErr.TargetSite;
                sErr += "\n\n Stack Trace: \n\n" + eErr.StackTrace;

                //emailClient oMail = new emailClient(true);
                //oMail.sendAdminAppMail(AppConfiguration.appNameId + " Application Error Log", sErr);

                _emailSender.SendEmailAsync(FromEmail, ManifestNumber + " - Manifest Pending your action", $"Message!!!");
                //'LUXOR uncomment
                //'Call windowsEventLog.writeWindowsEventLog(c_sEventLogSource, sErr, EventLogEntryType.Error, eCategory)
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
            }
            return true;
        }

        public bool logAppExceptions(string sSubject, string sErr, string FromEmail, string ManifestNumber)
        {
            try
            {
                _emailSender.SendEmailAsync(FromEmail, ManifestNumber + " - Manifest Pending your action", $"Message!!!");
                //emailClient oMail = new emailClient(true);
                //oMail.sendAdminAppMail(AppConfiguration.appNameId + " " + sSubject, sErr);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
            }
            return true;
        }
    }
}