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
        List<LinkTime> list_all;
        List<LinkTime> list_identical;
         

        HashSet<string> main_Xml;
        HashSet<string> main_Html;
        HashSet<string> temp_identical;

        private int XmlCount;
        private int HtmlCount;

        public MethodsForReferences(HashSet<string> mainXml, HashSet<string> mainHtml)
        {
            list_xml = new List<LinkTime>();
            list_html = new List<LinkTime>();
            list_identical = new List<LinkTime>();
            main_Xml = mainXml;
            main_Html = mainHtml;
        }

        private void ToGetDifferentReferencesOnly()
        {
            HtmlCount = main_Html.Count;
            XmlCount = main_Xml.Count;

            temp_identical = new HashSet<string>(main_Xml);
            temp_identical.IntersectWith(main_Html);

            HashSet<string> temp = new HashSet<string>(main_Xml); 
            

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
            foreach (var item in temp_identical)
            {
                list_identical.Add(new LinkTime() { link = item, time = Timer.TimeofResponse(item) });

            }
            Console.WriteLine("Time Xml");
            foreach (var item in main_Xml)
            {
                list_xml.Add(new LinkTime() { link = item, time = Timer.TimeofResponse(item) });
                
            }
            
            Console.WriteLine("Time Html");
            foreach (var item in main_Html)
            {
                list_html.Add(new LinkTime() { link = item, time = Timer.TimeofResponse(item) });
               
            }

            list_all = list_html.Concat(list_xml).Concat(list_identical).ToList();
            SortingAtTimes(ref list_all);
        }

        public void ShowParce()
        {
            ToGetDifferentReferencesOnly();// takes away identical elements in a list

            CteateListOfLinks();//carries from hash in list and also knows time of response
            Console.WriteLine("Press any key");
            Console.ReadKey();
            Console.Clear();
            int i = 1;
            Console.WriteLine("If time = -1, site cant be opened");
            if (list_xml.Count == 0)
                Console.WriteLine("in sitemap 0 links or sitemap cant be opened");
            else
            {
                Console.WriteLine($"\n\n\nUrls FOUNDED IN SITEMAP.XML but not founded after crawling a web site \n\n");
                
                foreach (var item in list_xml)
                {
                    Console.WriteLine($"{i}){item.link}");
                    i++;
                }
            }
            Console.WriteLine("\n\n\nUrls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml\n\n");
            if (list_html.Count == 0)
                Console.WriteLine("in html 0 links or html cant be opened");
            else
            {
                i = 1;
                foreach (var item in list_html)
                {
                    Console.WriteLine($"{i}){item.link}");
                    i++;
                }
            }
            i = 1;
            
            
            Console.WriteLine("\n\n\n\n\nTiming\nUrl\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tTiming(ms)");
            foreach (var item in list_all)
            {
                Console.WriteLine($"{i}){item.link, -135}{item.time, 13}");
                i++;
            }
            

            Console.WriteLine($"\n\nUrls(html documents) found after crawling a website: {HtmlCount}");
            Console.WriteLine($"\n\nUrls found in sitemap: {XmlCount}");

           
            
        }
    }
}
