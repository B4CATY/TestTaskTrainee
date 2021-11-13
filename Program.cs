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
                //string url = Console.ReadLine();
                string url = "https://translate.meta.ua";
                if (Timer.TimeofResponse(url) == -1) throw new Exception();
                DownloadFile downloadFile = new DownloadFile(url);
                downloadFile.DownloadHtmlDoc();
                downloadFile.DownloadXmlDoc();
                ParceDoc parceDoc = new ParceDoc();
                parceDoc.StartParceDocs(downloadFile.XmlDoc, downloadFile.HtmlDoc, url);
                MethodsForReferences methods = new MethodsForReferences(parceDoc.XmlLinks, parceDoc.HtmlLinks);
                methods.ShowParce();
            }
            catch (Exception)
            {

                Console.WriteLine("erorr link or a web-site a block stands from parcing, restart pls");
            }
            Console.WriteLine("\n\nEnd work");
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