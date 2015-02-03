using System;
using System.Net.Mail;
using System.IO;
using DFramework;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DFramework.Utilities
{
    public static class EmailHelper
    {
        static bool _configed = false;
        static string _emailServer;
        static string _webmasterEmail;
        static string _emailAccount;
        static string _emailPassword;

        public static void Config(string emailServer, string webMasterEmail, string emailAccount, string emailPassword)
        {
            Check.Argument.IsNotEmpty(emailServer, "emailServer");
            Check.Argument.IsNotEmpty(emailAccount, "emailAccount");
            Check.Argument.IsNotEmpty(emailPassword, "emailPassword");

            _emailServer = emailServer;
            _webmasterEmail = string.IsNullOrEmpty(webMasterEmail) ? emailAccount : webMasterEmail;
            _emailAccount = emailAccount;
            _emailPassword = emailPassword;
            _configed = true;
        }


        public static void SendMail(string toAddress, string subject, string body)
        {
            using (MailMessage mail = BuildMessageWith(_webmasterEmail, toAddress, subject, body))
            {
                SendMail(mail);
            }
        }

        public static void SendMail(MailMessage mail)
        {
            if (!_configed)
                throw new Exception("Email Sender must config before use.");

            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = _emailServer,
                    EnableSsl = false
                };

                smtp.Credentials = new NetworkCredential(_emailAccount, _emailPassword);

                smtp.Send(mail);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void SendMailAsync(string toAddress, string subject, string body)
        {
            Task.Factory.StartNew(() => SendMail(toAddress, subject, body))
                        .ContinueWith(t =>
                        {
                            Log.Error("   email to {0} failed，title {1}-the email content is '{2}'.", toAddress, subject, body);
                        }, TaskContinuationOptions.OnlyOnFaulted);
        }

        #region 私有方法
        //private string PrepareMailBodyWith(string templateName, params string[] pairs)
        //{
        //    string body = GetMailBodyOfTemplate(templateName);

        //    for (var i = 0; i < pairs.Length; i += 2)
        //    {
        //        body = body.Replace("<%={0}%>".FormatWith(pairs[i]), pairs[i + 1]);
        //    }

        //    return body;
        //}

        //private string GetMailBodyOfTemplate(string templateName)
        //{
        //    string cacheKey = string.Concat("mailTemplate:", templateName);
        //    string body;

        //    Cache.TryGet(cacheKey, out body);

        //    if (string.IsNullOrEmpty(body))
        //    {
        //        body = ReadFileFrom(templateName);

        //        if ((!string.IsNullOrEmpty(body)) && (!Cache.Contains(cacheKey)))
        //        {
        //            Cache.Add(cacheKey, body);
        //        }
        //    }

        //    return body;
        //}

        //private string ReadFileFrom(string templateName)
        //{
        //    string filePath = string.Concat(Path.Combine(_settings.TemplateDirectory, templateName), ".txt");

        //    string body = File.ReadAllText(filePath);

        //    return body;
        //}



        private static MailMessage BuildMessageWith(string fromAddress, string toAddress, string subject, string body)
        {
            MailMessage message = new MailMessage
            {
                Sender = new MailAddress(_webmasterEmail),
                From = new MailAddress(fromAddress),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            string[] tos = toAddress.Split(';');

            foreach (string to in tos)
            {
                message.To.Add(new MailAddress(to));
            }

            return message;
        }
        #endregion

    }
}