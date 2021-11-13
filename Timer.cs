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
    public static class Timer
    {
        public static double TimeofResponse(string link)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
            System.Diagnostics.Stopwatch timer = new Stopwatch();

            timer.Start();
            try
            {
                HttpWebResponse response1 = (HttpWebResponse)request.GetResponse();
                response1.Close();

                timer.Stop();
                return timer.Elapsed.TotalMilliseconds;
            }
            catch (WebException ex)
            {
              
                Console.WriteLine($"Site: {link},  {ex.Message}");
                //Console.WriteLine("Site cant be opened");
                timer.Stop();
                return -1;
            }
            //Console.WriteLine(timer.Elapsed.TotalMilliseconds);
        }
    }
}
