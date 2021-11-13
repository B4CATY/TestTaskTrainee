using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Web;
using System.Net.Http;
using System.Diagnostics;
using System.Xml;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;

namespace TestTaskTrainee
{
    public class DownloadFile
    {
        private string link_;
        private string DownloadXml;
        private string DownloadHtml;

        public string URL { get => link_; }
        public string XmlDoc { get => DownloadXml; }
        public string HtmlDoc { get => DownloadHtml; }

        public DownloadFile(string link)
        {
            link_ = link;
        }
        

        public void DownloadXmlDoc()
        {
            Console.WriteLine("Download Xml Doc");
            if (link_ == null || link_ == "") link_ = "https://www.ukad-group.com";

            var uriI = new Uri(link_);
            link_ = Uri.UriSchemeHttps + "://" + uriI.Host + "/sitemap.xml";

            try
            {
                WebClient wc = new WebClient();

                wc.Encoding = System.Text.Encoding.UTF8;

                DownloadXml = wc.DownloadString(link_);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
               
            }
            

        }
        public void DownloadHtmlDoc()
        {
            Console.WriteLine("Download Html Doc");
            if (link_ == null || link_ == "") link_ = "https://www.ukad-group.com";
            HttpClient client = new HttpClient();

            HttpResponseMessage response = client.GetAsync(link_).Result;

            HttpContent content = response.Content;

            DownloadHtml = content.ReadAsStringAsync().Result;
            
        } 
    }
}
