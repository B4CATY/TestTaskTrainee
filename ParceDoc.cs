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
    public class ParceDoc
    {
        private HashSet<string> xml_hash;
        private HashSet<string> html_hash;


        private int html_count;
        private int xml_count;

        public HashSet<string> XmlLinks { get => xml_hash; }
        public HashSet<string> HtmlLinks { get => html_hash; }

        public int HtmlCount { get => html_count; }
        public int XmlCount { get => xml_count; }

        public ParceDoc()
        {
            xml_hash = new HashSet<string>();
            html_hash = new HashSet<string>();
            html_count = 0;
            xml_count = 0;
        }
        public void StartParceDocs(string DownloadXml, string DownloadHtml, string url)
        {
            try
            {
                ParceXml(DownloadXml);
                ParceHtml(DownloadHtml, url);


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
        private void ParceXml(string DownloadXml)
        {
            Console.WriteLine("Parcing Xml:");
            XmlDocument urldoc = new XmlDocument();

            urldoc.LoadXml(DownloadXml);

            XmlNodeList xmlSitemapList = urldoc.GetElementsByTagName("loc");
            Console.WriteLine("Xml:");
            foreach (XmlNode node in xmlSitemapList)
            {
                xml_hash.Add(node.InnerText);

                Console.WriteLine($"link = {node.InnerText}");
            }
            xml_count = xml_hash.Count;
            if (xml_count == 0) throw new Exception("There is not a xml document on this web-site, or a web-site forbade access to him.");
        }

        private void ParceHtml(string DownloadHtml, string url)
        {
            string temprary;
            Console.WriteLine("Parcing Html:");
            var links = Regex.Matches(DownloadHtml, "<a(.*?) href=\"(.*?)\"").Cast<Match>().Select(x => x.Groups[2].Value);
           

            var uriI = new Uri(url);
            url = Uri.UriSchemeHttps + "://" + uriI.Host;

            Console.WriteLine("Html:");
            foreach (var item in links)
            {
                try
                {
                    if (item.Contains("https") || item.Contains("http") || item.Contains("www"))
                    {
                        if(item[0] =='/' && item[1] == '/')
                        {
                            temprary = "https:" + item;
                            var temp_uriI = new Uri(temprary);

                            if (uriI.Host == temp_uriI.Host)
                            {
                                html_hash.Add(temprary);
                                
                            }
                        }
                        else
                        {
                            var temp_uriI = new Uri(item);
                            if (uriI.Host == temp_uriI.Host)
                            {
                                html_hash.Add(item);
                                
                            }
                            
                        }
                        
                    }
                    else if (item[0] == '/' || item.Contains(url) || item[0] == '#')
                    {
                        
                        if (item.Contains(url) || item.Contains(uriI.Host))
                        {
                            html_hash.Add(item);
                            
                        }
                        else if (item[0] == '#')
                        {
                            html_hash.Add(url +"/" + item);
                            
                        }


                        else
                        {
                            html_hash.Add(url + item);
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    
                }
               
                

                

                
            }
            html_count = html_hash.Count;
            if (html_count == 0) throw new Exception("Ban html document");
        }

    }
}
