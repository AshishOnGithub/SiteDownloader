using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SiteDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            // Startting point of the application
            string currentUrl = "https://tretton37.com";
            DownloadService service = new DownloadService(currentUrl);
            service.WebSiteProcessor().GetAwaiter().GetResult();
            Console.ReadLine();
        }
    }
}
