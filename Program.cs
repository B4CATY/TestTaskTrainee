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
        private static async Task Main()
        {
            try
            {
                Console.WriteLine("Enter your link: ");
                string url = Console.ReadLine();
                //string url = "https://www.ukad-group.com";

                if (Timer.TimeofResponse(url) == -1) throw new Exception();

                DownloadFile downloadFile = new DownloadFile(url);
                await downloadFile.DownloadHtmlDoc();
                await downloadFile.DownloadXmlDoc();
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
