using System;
using System.Diagnostics;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace FBru.Repository.Helpers
{
    public class Utilities
    {
        public static string EncryptStringToMd5(string input)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder newStr = new StringBuilder();
            foreach (byte b in data)
            {
                newStr.Append(b.ToString("x2"));
            }
            return newStr.ToString();
        }

        public static string GenerateRandomString(int number)
        {
            StringBuilder rdStr = new StringBuilder();
            string libraryCharacter = "1234567890abcdefghiklmnopqwertyuizxABCDEFGHIKLMNOPQWRTZX";
            Random rd = new Random();
            for (int i = 0; i < number; i++)
            {
                int index = rd.Next(libraryCharacter.Length);
                rdStr.Append(libraryCharacter[index]);
            }
            return rdStr.ToString();
        }

        public static bool SendEmail(string receiverEmail, string subject, string message)
        {
            if (receiverEmail != null && subject != null && message != null)
            {

                string senderEmail = "";
                string senderPassword = "";
                string senderName = "FBru - Food Brunei";
                MailMessage m = new MailMessage(new MailAddress(senderEmail, senderName), new MailAddress(receiverEmail))
                {
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };
                SmtpClient smtp = new SmtpClient("")
                {
                    Credentials = new System.Net.NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true,
                    Port = 25
                };
                try
                {
                    smtp.Send(m);
                    return true;
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    for (int i = 0; i < ex.InnerExceptions.Length; i++)
                    {
                        SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                        if (status == SmtpStatusCode.MailboxBusy ||
                            status == SmtpStatusCode.MailboxUnavailable)
                        {
                            Console.WriteLine(@"Delivery failed - retrying in 5 seconds.");
                            System.Threading.Thread.Sleep(5000);
                            try
                            {
                                smtp.Send(m);
                                return true;
                            }
                            catch (Exception ex1) { Debug.WriteLine(ex1.Message); }
                        }
                        else
                        {
                            Debug.WriteLine(@"Failed to deliver message to {0}",
                                ex.InnerExceptions[i].FailedRecipient);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"Exception caught in RetryIfBusy(): {0}",
                            ex.ToString());
                }
            }
            return false;
        }
    }
}
