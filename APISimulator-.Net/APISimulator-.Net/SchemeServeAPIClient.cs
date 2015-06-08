using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.Text;

namespace SSClient
{
    public class SchemeServeAPIClient
    {
        public string Token { get; set; }
        public DateTime TokenExpiryTimestamp { get; set; }

        private string RequestBodyFilePath { get; set; }
        private string ResultFilePath { get; set; }

        public SchemeServeAPIClient(string requestBodyFilePath, string resultFilePath)
        {
            this.RequestBodyFilePath = requestBodyFilePath;
            this.ResultFilePath = resultFilePath;

            this.Token = String.Empty;
            this.TokenExpiryTimestamp = DateTime.MinValue;
        }

        public void LoadToken(string tokenURL)
        {
            string tokenRaw = String.Empty;

            try
            {
                tokenRaw = this.PerformGetRequest(tokenURL);

                XDocument doc = XDocument.Parse(tokenRaw);

                this.Token = doc.Element("access").Element("token").Value;
                long ExpiresNum = Convert.ToInt64(doc.Element("access").Element("expires").Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SubmitCase(string url)
        {
            string requestBody = File.ReadAllText(this.RequestBodyFilePath);

            url = String.Format("{0}?token={1}", url, this.Token);

            HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(url);
            HttpWebResponse response = null;

            request.Method = "POST";
            request.Timeout = 20000;

            request.Accept = "application/xml";
            request.ContentType = "application/xml";
            request.ContentLength = requestBody.Length;

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(requestBody);

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
                stream.Close();
            }

            string result = String.Empty;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                Encoding respEncoding = Encoding.GetEncoding(response.CharacterSet);

                using (Stream ReceiveStream = response.GetResponseStream())
                {
                    using (StreamReader readStream = new StreamReader(ReceiveStream, respEncoding))
                    {
                        result = readStream.ReadToEnd();
                    }
                }
            }
            catch (WebException Ex)
            {
                throw Ex;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            File.WriteAllText(this.ResultFilePath, result);
        }

        public void GetCase(string getCaseURL, long caseID) {
            this.PerformGetRequest(getCaseURL + "/" + caseID);
        }

        private string PerformGetRequest(string url)
        {
            url = String.Format("{0}?token={1}", url, this.Token);

            HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(url);
            HttpWebResponse response = null;

            request.Method = "GET";
            request.Timeout = 20000;

            string result = String.Empty;

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                Encoding respEncoding = Encoding.GetEncoding(response.CharacterSet);

                using (Stream ReceiveStream = response.GetResponseStream())
                {
                    using (StreamReader readStream = new StreamReader(ReceiveStream, respEncoding))
                    {
                        result = readStream.ReadToEnd();
                    }
                }
            }
            catch (WebException Ex)
            {
                throw Ex;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            File.WriteAllText(this.ResultFilePath, result);

            return result;
        }

    }
}