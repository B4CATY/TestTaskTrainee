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
        private HashSet<string> html_hash_temp;
        private List<string> list_html_temp;

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

            list_html_temp = new List<string>();
            html_hash_temp = new HashSet<string>();

            html_count = 0;
            xml_count = 0;
        }
        public async Task StartParceDocs(string DownloadXml, string DownloadHtml, string url)
        {
            try
            {
                ParceXml(DownloadXml);
                await ParceHtml(DownloadHtml, url);


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
            
            foreach (XmlNode node in xmlSitemapList)
            {
                xml_hash.Add(node.InnerText);

               
            }
            xml_count = xml_hash.Count;
            if (xml_count == 0) throw new Exception("There is not a xml document on this web-site, or a web-site forbade access to him.");
        }

        private async Task ParceHtml(string DownloadHtml, string url)
        {
            Console.WriteLine("Download Html Doc");
            
            Console.WriteLine("Parcing Html:");
            var links = Regex.Matches(DownloadHtml, "<a(.*?) href=\"(.*?)\"").Cast<Match>().Select(x => x.Groups[2].Value);


            var uriI = new Uri(url);
            url = Uri.UriSchemeHttps + "://" + uriI.Host;

            
            #region foreach
            foreach (var item in links)
            {
                ForEachHtml(item, url, uriI, ref html_hash);
            }
            list_html_temp = html_hash.ToList();
            #endregion
            int i = 0;
            bool IsEnd = false;
            
            while (!IsEnd)
            {
                html_count = list_html_temp.Count;
                for (; i < html_count; i++)
                {


                    DownloadFile downloadFile = new DownloadFile(list_html_temp[i]);
                    await downloadFile.DownloadHtmlDoc();
                    links = Regex.Matches(downloadFile.HtmlDoc, "<a(.*?) href=\"(.*?)\"").Cast<Match>().Select(x => x.Groups[2].Value);
                    foreach (var itemsHtml in links)
                    {
                        ForEachHtml(itemsHtml, url, uriI, ref html_hash_temp);
                    }


                }
                html_hash.UnionWith(html_hash_temp);
                list_html_temp = html_hash.ToList();
                if (html_count == html_hash.Count)
                    IsEnd = true;
            }
            
            html_count = html_hash.Count;
            if (html_count == 0) throw new Exception("Ban html document");


        }




        private void ForEachHtml(string item, string url, Uri uriI, ref HashSet<string> html)
        {
           
            string temprary;

            try
            {
                if(item.Contains(" ")|| item.Contains("/#"))
                {
                    return;
                }
                else if (item.Contains("https") || item.Contains("http") || item.Contains("www"))
                {
                    if (item[0] == '/' && item[1] == '/')
                    {
                        temprary = "https:" + item;
                        var temp_uriI = new Uri(temprary);

                        if (uriI.Host == temp_uriI.Host)
                        {
                            html.Add(temprary);

                        }
                    }
                    else
                    {
                        var temp_uriI = new Uri(item);
                        if (uriI.Host == temp_uriI.Host)
                        {
                            html.Add(item);

                        }

                    }

                }

                else if (item[0] == '/' || item.Contains(url))
                {

                    if (item.Contains(url) || item.Contains(uriI.Host))
                    {
                        html.Add(item);

                    }
                    else
                    {
                        html.Add(url + item);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            
        }
    }
}
