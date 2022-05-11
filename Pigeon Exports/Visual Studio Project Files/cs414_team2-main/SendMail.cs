using CS414_Team2.Data;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

namespace CS414_Team2
{
    public class SendMail
    {
        public static string SendConfirmationMail(string email, string username)
        {
            string passCodeValue = DataProvider.ReturnPasscode(email, username);
            string subject = "Email Confirmation Code.";
            string body = $"Hello {username},\r\nUse the temporary Accsess key {passCodeValue} to confirm your email address." +
                            "\r\nVisit http://csmain/cs414/cs414_team2/ConfirmEmail.aspx if you have left the confirm email " +
                            "page or would like to access the page from the network on another device.";

            string response = BuildEmail(email, username, subject, body);
            return response;
        }
        public static string SendResetPasswordMail(string email, string username)
        {
            DataProvider.UpdatePasscode(email, username);
            string passCodeValue = DataProvider.ReturnPasscode(email, username);
            string subject = "Forgotten Password.";
            string body = $"Hello {username},\r\nUse the temporary Accsess key {passCodeValue} to change your password.";

            string response = BuildEmail(email, username, subject, body);
            return response;
        }
        public static string BuildEmail(string email, string username, string subject, string body)
        {
            try
            {
                string mailServerUsername = DataProvider.GetMessage(62, 7587, 220);
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress("mason.phillips@students.pcci.edu", "Pigeon Support", System.Text.Encoding.UTF8);
                mail.Subject = subject;
                mail.Body = body;
                SmtpClient client = new SmtpClient();
                mail.Priority = MailPriority.High;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("136745", mailServerUsername);
                client.Host = "students.pcci.edu";
                client.EnableSsl = true;
                try
                {
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in btn_forgot_pass_Click(): {0}", ex.ToString());
                }
            }
            catch (OracleException oEx)
            {
                switch (oEx.Number)
                {
                    case 20002:
                        return "Username is not correct.";
                    case 20003:
                        return "Email is not correct.";
                    default:
                        throw oEx;
                }
            }
            return null;
        }
    }
}