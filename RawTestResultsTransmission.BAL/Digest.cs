using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Configuration;
using NLog;
using System.Text;
using System.Threading.Tasks;

namespace RawTestResultsTransmission.BAL
{
    public class Digest
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private string EmailBody { get; set; }

        public void PrepareBody(List<TestResult> testResults)
        {
            string body = "<div style='font-family:arial, helvetica, sans-serif;'>" +
                          "<h3 style='margin-bottom: 0px;'>California Standards - Based Test in Spanish (STS)</h3>" +
                          "<h4 style='margin-top: 0px; font-weight: normal;'>Test Results Transmission (TRT) to ETS status</h4>";
            if (testResults.Count == 0)
            {
                body += "<p style='font-size:10pt;margin-bottom:0px;'>No TRT results found on: <b>" + DateTime.Now.DayOfWeek + ", " + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year + "</b></p><br>" +
                        "<p style='font-size:8pt;margin-top: 5px;color:#778899'>" +
                        "<span>" + ConfigurationManager.AppSettings["AppName"] + "</span><br>" +
                        "<span>version: " + ConfigurationManager.AppSettings["version"] + "</span><br>" +
                        "<span>released: " + ConfigurationManager.AppSettings["releaseDate"] + "</span>" +
                        "</p>" +
                        "</div>";
            }
            else
            {                
                body += "<p style='font-size:10pt;margin-bottom:0px;'>transmitted on: <b>" + DateTime.Now.DayOfWeek + ", " + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year + "</b></p>" +
                        "<p style='font-size:8pt;margin-top: 5px;color:#778899'>" +
                        "<table style='border: 1px solid black; border-collapse:collapse;'>" +
                        "<thead>" +
                        "<th style='font-family:arial, helvetica, sans-serif;border: 1px solid black;font-size:10pt;color:white;background-color:#246a34;padding:5px'>#</th>" +
                        "<th style='font-family:arial, helvetica, sans-serif;border: 1px solid black;font-size:10pt;color:white;background-color:#246a34;padding:5px'>SSID</th>" +
                        "<th style='font-family:arial, helvetica, sans-serif;border: 1px solid black;font-size:10pt;color:white;background-color:#246a34;padding:5px'>OppId</th>" +
                        "<th style='font-family:arial, helvetica, sans-serif;border: 1px solid black;font-size:10pt;color:white;background-color:#246a34;padding:5px'>TRT Transmitted</th>" +
                        "</thead>";
                int i = 1;
                foreach(TestResult result in testResults)
                {
                    if (i % 2 == 0){
                        body += "<tr style='background-color:#F5F5F5;'>";
                    }
                    else
                    {
                        body += "<tr>";
                    }
                    body += "<td style='font-family:arial, helvetica, sans-serif;border: 1px solid black;font-size:10pt;padding:5px'>" + i.ToString() + "</td>" +
                            "<td style='font-family:arial, helvetica, sans-serif;border: 1px solid black;font-size:10pt;padding:5px'>" + result.SSID.ToString() + "</td>" +
                            "<td style='font-family:arial, helvetica, sans-serif;border: 1px solid black;font-size:10pt;padding:5px'>" + result.OppId.ToString() + "</td>";
                    if (result.TRTSent)
                    {
                        body += "<td style='font-family:arial, helvetica, sans-serif;border: 1px solid black;font-size:10pt;padding:5px;color:green;text-align:center;'>Yes</td>";
                    }
                    else
                    {
                        body += "<td style='font-family:arial, helvetica, sans-serif;border: 1px solid black;font-size:10pt;padding:5px;color:red;text-align:center;'>No</td>";
                    }
                    body += "</tr>";
                    i++;
                }

                body += "</table>" +
                        "<br>" +
                        "<span>" + ConfigurationManager.AppSettings["AppName"] + "</span><br>" +
                        "<span>version: " + ConfigurationManager.AppSettings["version"] + "</span><br>" +
                        "<span>released: " + ConfigurationManager.AppSettings["releaseDate"] + "</span>" +
                        "</p>" +
                        "</div>";
            }
            EmailBody = body;
        }

        public void Send()
        {
            try
            {
                SmtpClient mySmtpClient = new SmtpClient(ConfigurationManager.AppSettings["mailServer"]);

                // set smtp-client with basicAuthentication
                mySmtpClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential basicAuthenticationInfo = new
                   System.Net.NetworkCredential(ConfigurationManager.AppSettings["mailUsername"],
                                                ConfigurationManager.AppSettings["mailPassword"]);
                mySmtpClient.Credentials = basicAuthenticationInfo;
                mySmtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["mailPort"]);
                mySmtpClient.EnableSsl = true;

                // add from,to mailaddresses
                //MailAddress from = new MailAddress(ConfigurationManager.AppSettings["mailFromAddress"]);
                //MailAddress to = new MailAddress(ConfigurationManager.AppSettings["mailToAddress"]);
                //MailMessage myMail = new System.Net.Mail.MailMessage(from, to);
                MailMessage myMail = new MailMessage();
                myMail.To.Add(ConfigurationManager.AppSettings["mailToAddress"]);
                myMail.From = new MailAddress(ConfigurationManager.AppSettings["mailFromAddress"]);

                // add ReplyTo
                //MailAddress replyto = new MailAddress("reply@example.com");
                //myMail.ReplyToList.Add(replyTo);

                // add CC
                //MailAddress ccTo = new MailAddress(ConfigurationManager.AppSettings["mailCCAddress"]);
                //myMail.CC.Add(ccTo);

                // set subject and encoding
                myMail.Subject = ConfigurationManager.AppSettings["mailSubject"] + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year;
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                // set body-message and encoding
                myMail.Body = EmailBody;
                myMail.BodyEncoding = System.Text.Encoding.UTF8;
                // text or html
                myMail.IsBodyHtml = true;

                // attachments
                //if (System.IO.File.Exists(newValidFile))
                //{
                //    Attachment attachment = new Attachment(newValidFile);
                //    myMail.Attachments.Add(attachment);
                //}
                //if (System.IO.File.Exists(newInvalidFile))
                //{
                //    Attachment attachment = new Attachment(newInvalidFile);
                //    myMail.Attachments.Add(attachment);
                //}
                //if (System.IO.File.Exists(assessmentReportFile))
                //{
                //    Attachment attachment = new Attachment(assessmentReportFile);
                //    myMail.Attachments.Add(attachment);
                //}

                mySmtpClient.Send(myMail);
                _logger.Info("Email sent.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
