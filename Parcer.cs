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
    struct LinkTime
    {
        public string link;
        public double time;
    }
    public class Parcer
    {
        private List<LinkTime> links_html;
        private List<LinkTime> links_xml;
        private LinkTime[] array_links_html;
        private LinkTime[] array_links_xml;

        private string url_;
        System.Diagnostics.Stopwatch timer;
        private LinkTime temp;
        private int xml_count;
        private int html_count;
        

        public string URL { get => url_; }
        public Parcer(string url)
        {
            url_ = url;
            timer = new Stopwatch();

        }
        public void Start()
        {
            try
            {
                AsyncParserHtml(url_).GetAwaiter().GetResult();
                AsyncParceXml(url_).GetAwaiter().GetResult();
                ToGetDifferentReferencesOnly();
                if (html_count == 0 && xml_count == 0) throw new Exception("Full ban site");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (WebException)
            {
                Console.WriteLine("Site cant be opened");
            }
            catch (UriFormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
            
            
        }
        #region Xml
        private async Task AsyncParceXml(string url)
        {
            await Task.Run(() => ParceXml(url));
        }
        private void ParceXml(string sitemapURL)
        {

            links_xml = new List<LinkTime>();

            if (sitemapURL == null) sitemapURL = "https://basicweb.ru";

            var uriI = new Uri(sitemapURL);
            sitemapURL = Uri.UriSchemeHttps + "://" + uriI.Host + "/sitemap.xml";

            WebClient wc = new WebClient();

            wc.Encoding = System.Text.Encoding.UTF8;

            string sitemapString = wc.DownloadString(sitemapURL);

            XmlDocument urldoc = new XmlDocument();

            urldoc.LoadXml(sitemapString);

            XmlNodeList xmlSitemapList = urldoc.GetElementsByTagName("loc");
            Console.WriteLine("Xml:");
            foreach (XmlNode node in xmlSitemapList)
            {
                    temp = new LinkTime();
                    temp.link = node.InnerText;
                    temp.time = TimeofResponse(temp);
                    links_xml.Add(temp);


                    //Console.WriteLine($"link = {temp.link}, time = {temp.time}");
            }
            xml_count = links_xml.Count;
            if (xml_count == 0) throw new Exception("There is not a xml document on this web-site, or a web-site forbade access to him.");
            /*Console.WriteLine(xml_count);*/


        }
        #endregion Xml

        #region Html
        private async Task AsyncParserHtml(string url)
        {
            await Task.Run(() => ParserHtml(url));
        }
        private void ParserHtml(string url)
        {
            links_html = new List<LinkTime>();
            if (url == null) url = "https://basicweb.ru";
            HttpClient client = new HttpClient();

            HttpResponseMessage response = client.GetAsync(url).Result;

            HttpContent content = response.Content;

            string result = content.ReadAsStringAsync().Result;
            Regex regex = new Regex(@"<link[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>");
            
            var links = Regex.Matches(result, "<a(.*?) href=\"(.*?)\"").Cast<Match>().Select(x => x.Groups[2].Value);

            Console.WriteLine("Html:");
            foreach (var item in links)
            {
                temp = new LinkTime();

                temp.link = url + item;
                temp.time = TimeofResponse(temp);
                links_html.Add(temp);
                
                
                //Console.WriteLine($"link = {temp.link}, time = {temp.time}");
            }
            html_count = links_html.Count;
            if (html_count == 0) throw new Exception("Ban html document");
            //Console.WriteLine(html_count);
        }
        #endregion Html
        #region Else
        private double TimeofResponse(LinkTime lt)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(lt.link);
            System.Diagnostics.Stopwatch timer = new Stopwatch();

            timer.Start();
            try
            {
                HttpWebResponse response1 = (HttpWebResponse)request.GetResponse();
                response1.Close();

                timer.Stop();
                temp.time = timer.Elapsed.TotalMilliseconds;
            }
            catch (WebException ex )
            {
                temp.time = -1;
                Console.WriteLine(ex.Message);
                //Console.WriteLine("Site cant be opened");
                timer.Stop();
            }
            //Console.WriteLine(timer.Elapsed.TotalMilliseconds);
            return temp.time;
        }
        private void ToGetDifferentReferencesOnly()
        {
             List<LinkTime> links_html_sorted = new List<LinkTime>();
             List<LinkTime> links_xml_sorted = new List<LinkTime>();

            bool IsEqual;
            for (int i = 0; i < links_html.Count; i++)
            {
                IsEqual = false;
                for (int j = 0; j < links_xml.Count; j++)
                {
                    if (links_html[i].link == links_xml[j].link) IsEqual = true;
                }

                if (!IsEqual)
                {
                    temp = new LinkTime();
                    temp.link = links_xml[i].link;
                    temp.time = links_xml[i].time;
                    links_html_sorted.Add(temp);
                }
            }
            for (int i = 0; i < links_xml.Count; i++)
            {
                IsEqual = false;
                for (int j = 0; j < links_html.Count; j++)
                {
                    if (links_xml[i].link == links_html[j].link) IsEqual = true;
                }

                if (!IsEqual)
                {
                    temp = new LinkTime();
                    temp.link = links_xml[i].link;
                    temp.time = links_xml[i].time;
                    links_xml_sorted.Add(temp);
                }
            }
            

            

        }
        public void ShowInfo()
        {
            SortingAtTimes();

            Console.WriteLine("If time = -1, site cant be opened");
            Console.WriteLine("\n\nUrls FOUNDED IN SITEMAP.XML but not founded after crawling a web site");
            foreach (var item in array_links_xml)
            {
                Console.WriteLine($"Links: {item.link},\tTime ms: {item.time}");
            }
            Console.WriteLine("Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml");
            foreach (var item in array_links_html)
            {
                Console.WriteLine($"Links: {item.link},\tTime ms: {item.time}");
            }

            Console.WriteLine($"\n\nUrls(html documents) found after crawling a website: {html_count}");
            Console.WriteLine($"\n\nUrlsUrls found in sitemap: {xml_count}");
        }
        private void SortingAtTimes()
        {
            array_links_html = links_html.ToArray();
            array_links_xml = links_xml.ToArray();
            for (int i = 0; i < array_links_html.Length; i++)
            {
                for (int j = 0; j < array_links_html.Length; j++)
                {
                    if(array_links_html[i].time < array_links_html[j].time)
                    {
                        temp.time = array_links_html[i].time;
                        array_links_html[i].time = array_links_html[j].time;
                        array_links_html[j].time = temp.time;

                        temp.link = array_links_html[i].link;
                        array_links_html[i].link = array_links_html[j].link;
                        array_links_html[j].link = temp.link;
                    }
                }
            }

            for (int i = 0; i < array_links_xml.Length; i++)
            {
                for (int j = 0; j < array_links_xml.Length; j++)
                {
                    if (array_links_xml[i].time < array_links_xml[j].time)
                    {
                        temp.time = array_links_xml[i].time;
                        array_links_xml[i].time = array_links_xml[j].time;
                        array_links_xml[j].time = temp.time;

                        temp.link = array_links_xml[i].link;
                        array_links_xml[i].link = array_links_xml[j].link;
                        array_links_xml[j].link = temp.link;
                    }
                }
            }
        }
        #endregion
    }

}
