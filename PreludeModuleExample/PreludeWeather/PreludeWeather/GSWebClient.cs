using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;

namespace PreludeAddons
{
    /// <summary>
    /// This GSWebClient class is a lightweight wrapper around the WebClient class of the .NET framework.
    /// </summary>
    public class GSWebClient : WebClient
    {
        private System.Net.CookieContainer cookieContainer;
        private string userAgent;
        private int timeout;
        private Uri _responseUri;

        /// <summary>
        /// CookieContainer implementation
        /// </summary>
        public System.Net.CookieContainer CookieContainer
        {
            get { return cookieContainer; }
            set { cookieContainer = value; }
        }

        /// <summary>
        /// UserAgent implementation
        /// </summary>
        public string UserAgent
        {
            get { return userAgent; }
            set { userAgent = value; }
        }

        /// <summary>
        /// Custom Timeout implementation
        /// </summary>
        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        public GSWebClient()
        {
            timeout = 180000;
            userAgent = @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/3.0.195.27 Safari/532.0";
            cookieContainer = new CookieContainer();
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);

            if (request.GetType() == typeof(HttpWebRequest))
            {
                ((HttpWebRequest)request).CookieContainer = cookieContainer;
                ((HttpWebRequest)request).UserAgent = userAgent;
                ((HttpWebRequest)request).Timeout = timeout;
                ((HttpWebRequest)request).AllowAutoRedirect = true;
                ((HttpWebRequest)request).ReadWriteTimeout = 360000;
                ((HttpWebRequest)request).UnsafeAuthenticatedConnectionSharing = true;
                ((HttpWebRequest)request).KeepAlive = false;
                ((HttpWebRequest)request).Accept = "*/*";
                //((HttpWebRequest)request).MaximumAutomaticRedirections = ;
            }
            
            return request;
        }

        //protected override WebResponse GetWebResponse(WebRequest request)
        //{
        //    WebResponse resp = base.GetWebResponse(request);
        //    StreamReader streamRead = new StreamReader(resp.GetResponseStream());
        //    string myResponse = streamRead.ReadToEnd();
        //    if(myResponse.ToLower().Contains("hide my"
        //    // do something with type
        //    return resp;
        //}


        public Uri ResponseUri
        {
            get { return _responseUri; }
        }

        //protected override WebResponse GetWebResponse(WebRequest request)
        //{
        //    try
        //    {
        //        WebResponse response = base.GetWebResponse(request);
        //        _responseUri = response.ResponseUri;
        //        return response;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
}
