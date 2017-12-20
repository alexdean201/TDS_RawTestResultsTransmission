using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using NLog;

namespace RawTestResultsTransmission.BAL
{
    public class EtsService
    {
        public string Message { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public bool CheckIfUrlIsValid()
        {
            return Uri.IsWellFormedUriString(Url, UriKind.Absolute) ? true : false;
        }

        public bool CheckIfConnectionIsGood()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return response.StatusCode == HttpStatusCode.OK ? true : false;
            }
            catch(Exception ex)
            {
                Message = "Exception in connecting to the ETS Service Endpoint. " + ex.Message;
                return false;
            }            
        }

        public HttpStatusCode PostResults(string TRT)
        {
            // send to POST REST
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(Username + ":" + Password)));
            byte[] bytes;
            bytes = Encoding.UTF8.GetBytes(TRT.ToString());
            request.ContentType = "application/xml";
            request.ContentLength = bytes.Length;
            request.Method = "POST";
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _logger.Info("Transmission successful");
                    return HttpStatusCode.OK;
                    // Console.WriteLine(response.ToString());
                    //Stream responseStream = response.GetResponseStream();
                    //string responseStr = new StreamReader(responseStream).ReadToEnd();
                }
                _logger.Info("Transmission failed: " + response.StatusCode);
                return HttpStatusCode.ExpectationFailed;
            }
            catch (WebException webex)
            {
                using (WebResponse resp = webex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)resp;
                    _logger.Error("Error code: {0}", httpResponse.StatusCode);
                    // Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    Stream data = resp.GetResponseStream();
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        _logger.Error(text);
                        // Console.WriteLine(text);
                    }
                }
                return HttpStatusCode.ExpectationFailed;
            }
        }
    }
}
