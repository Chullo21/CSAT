using PIMES_DMS.Data;
using System.Net;
using System.Net.Mail;
using PIMES_DMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace PIMES_DMS.Controllers
{
    public class NotifController : Controller
    {
        private readonly AppDbContext _Db;

        public NotifController(AppDbContext db)
        {
            _Db = db;
        }

        public IActionResult ShowNotif()
        {
            return View(_Db.NotifDb.OrderByDescending(j => j.DateCreated));
        }

        public void RandomSMTPandCallSendEmail(string sendTo, string subj, string body)
        {
            sendTo = "toledojgabby@gmail.com";
            subj = "Try Button";
            body = "pls work you piece of shii";

            Random random = new Random();
            List<SmtpEmailsModel> mod = new List<SmtpEmailsModel>
            {
                //new SmtpEmailsModel
                //{
                //    Email = "atsnoreply01@gmail.com",
                //    Password = "atsnoreply01111",
                //    Port = 587,
                //    SmtpServer = "smtp.gmail.com"
                //},
                new SmtpEmailsModel
                {
                    Email = "noreplyats1@gmail.com",
                    Password = "mxmppmodmskwwzhv",
                    Port = 587,
                    SmtpServer = "smtp.gmail.com"
                },
                new SmtpEmailsModel
                {
                    Email = "noreplyATS3@gmail.com",
                    Password = "hvieofknaxeretfy",
                    Port = 587,
                    SmtpServer = "smtp.gmail.com"
                }
            };

            SmtpEmailsModel selectedMod = mod[random.Next(0, mod.Count)];

            SendEmail(selectedMod, sendTo, subj, body);
        }

        public void SendEmail(SmtpEmailsModel mod, string sendTo, string subject, string body)
        {
            try
            {
                using (MailMessage message = new MailMessage())
                {
                    SmtpClient smtpServer = new SmtpClient(mod.SmtpServer);
                    smtpServer.Port = mod.Port;
                    smtpServer.Credentials = new NetworkCredential(mod.Email, mod.Password);
                    smtpServer.EnableSsl = true;
                    smtpServer.Timeout = 10000;           

                    message.From = new MailAddress(mod.Email!);
                    message.To.Add(sendTo);
                    message.Subject = subject;
                    message.Body = body;

                    smtpServer.Send(message);
                }
            }
            catch (Exception ex)
            {
                // Log the exception details
                LogException(ex);

                // Handle the exception or rethrow it to propagate it further
                throw;
            }
        }


        public void LogException(Exception ex)
        {
            // Here, you can implement your logging mechanism.
            // You can choose to log the exception to a file, database, or other logging systems.
            // For simplicity, this example will write the exception details to the console.
            Console.WriteLine("An exception occurred:");
            Console.WriteLine(ex.ToString());
        }

    }
}


