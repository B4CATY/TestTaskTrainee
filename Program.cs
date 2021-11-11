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
using System.Threading.Tasks;


namespace TestTaskTrainee
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                string url = "https://basicweb.ru";
                CorrectUri(url);
                Parcer parcer = new Parcer(url);
                parcer.Start();
                parcer.ShowInfo();
            }
            catch (Exception)
            {

                Console.WriteLine("erorr link, restart pls");
            }
            Console.WriteLine("End work");
        }
        private static void CorrectUri(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            System.Diagnostics.Stopwatch timer = new Stopwatch();
            HttpWebResponse response1 = (HttpWebResponse)request.GetResponse();
        }
    }
}
#region 12
/*string sitemapURL = "https://seoagilitytools.com/sitemap.xml";

WebClient wc = new WebClient();

wc.Encoding = System.Text.Encoding.UTF8;

string sitemapString = wc.DownloadString(sitemapURL);

XmlDocument urldoc = new XmlDocument();

urldoc.LoadXml(sitemapString);

XmlNodeList xmlSitemapList = urldoc.GetElementsByTagName("url");

foreach (XmlNode node in xmlSitemapList)
{
    if (node["loc"] != null)
    {
        Console.WriteLine("url " + node["loc"].InnerText);
    }
    *//*if (node["priority"] != null)
    {
        Console.WriteLine("priority " + node["priority"].InnerText);
    }
    if (node["lastmod"] != null)
    {
        Console.WriteLine("last modified " + node["lastmod"].InnerText);
    }
    if (node["changefreq"] != null)
    {
        Console.WriteLine("change frequency " + node["changefreq"].InnerText);
    }*//*
    //Console.WriteLine(Environment.NewLine);
}*/
#endregion