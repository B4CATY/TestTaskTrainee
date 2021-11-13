using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskTrainee
{
    public class MethodsForReferences
    {
        List<LinkTime> list_xml;
        List<LinkTime> list_html;

        HashSet<string> main_Xml;
        HashSet<string> main_Html;

        private int XmlCount;
        private int HtmlCount;

        public MethodsForReferences(HashSet<string> mainXml, HashSet<string> mainHtml)
        {
            list_xml = new List<LinkTime>();
            list_html = new List<LinkTime>();
            main_Xml = mainXml;
            main_Html = mainHtml;
        }

        private void ToGetDifferentReferencesOnly()
        {
            HtmlCount = main_Html.Count;
            XmlCount = main_Xml.Count;
            HashSet<string> temp = main_Xml;
            main_Xml.ExceptWith(main_Html);
            main_Html.ExceptWith(temp);
        }

        private void SortingAtTimes(ref List<LinkTime> list)
        {   
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[i].time < list[j].time)
                    {
                        var temp = list[i];
                        list[i] = list[j];
                        list[j] = temp;

                    }
                }
            }
            
        }

        private void CteateListOfLinks()
        {
            foreach (var item in main_Xml)
            {
                list_xml.Add(new LinkTime() { link = item, time = Timer.TimeofResponse(item) });
            }
            SortingAtTimes(ref list_xml);
            foreach (var item in main_Html)
            {
                list_html.Add(new LinkTime() { link = item, time = Timer.TimeofResponse(item) });
            }
            SortingAtTimes(ref list_html);
        }

        public void ShowParce()
        {
            ToGetDifferentReferencesOnly();
            CteateListOfLinks();
            Console.WriteLine("If time = -1, site cant be opened");
            Console.WriteLine("\n\n\nUrls FOUNDED IN SITEMAP.XML but not founded after crawling a web site\n\n");
            foreach (var item in list_xml)
            {
                Console.WriteLine($"Links: {item.link},\tTime ms: {item.time}");
            }
            Console.WriteLine("\n\n\nUrls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml\n\n");
            foreach (var item in list_html)
            {
                Console.WriteLine($"Links: {item.link},\tTime ms: {item.time}");
            }

            Console.WriteLine($"\n\nUrls(html documents) found after crawling a website: {HtmlCount}");
            Console.WriteLine($"\n\nUrlsUrls found in sitemap: {XmlCount}");
        }
    }
}
