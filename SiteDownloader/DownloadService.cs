using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDownloader
{
    class DownloadService : IDownloadService
    {
        string baseDirectoyPath = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\Root";
        HashSet<string> uniqueUrlList = new HashSet<string>();
        StringBuilder siteDownloadProgress = new StringBuilder("#");
        string baseUrl;

        public DownloadService(string url)
        {
            baseUrl = url;
        }

        public async Task WebSiteProcessor()
        {
            Console.WriteLine("Downloading Service Has Started, it may take around 2-3 mins....");
            Console.WriteLine("Downloading " + baseUrl);

            Queue<Node> urlQueue = new Queue<Node>();
            urlQueue.Enqueue(new Node { directoyPath = baseDirectoyPath, url = baseUrl });

            while (urlQueue.Count > 0)
            {
                int currSize = urlQueue.Count;
                List<Task> tasks = new List<Task>();

                for (int i = 0; i < currSize; i++)
                {
                    Node currentNode = urlQueue.Dequeue();
                    if(currentNode != null)
                        tasks.Add(SiteIterator(currentNode.url, currentNode.directoyPath, urlQueue));
                    tasks.Add(SiteDownloaderProgressBar());
                }

                await Task.WhenAll(tasks);
            }

            Console.WriteLine("Downloading completed, You may find your downloaded file at: " + baseDirectoyPath);
        }
        public async Task SiteIterator(string url, string path, Queue<Node> urlQueue)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = await web.LoadFromWebAsync(url);

            await SiteDownloader(url, path);

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                HtmlAttribute att = link.Attributes["href"];
                string currentUrl = baseUrl + att.Value;

                if (!uniqueUrlList.Contains(currentUrl) && att.Value[0] == '/')
                {
                    string directoyPath = baseDirectoyPath + att.Value.Replace("/", "\\");
                    if(currentUrl != null && directoyPath != null)
                        urlQueue.Enqueue(new Node { url = currentUrl, directoyPath = directoyPath });
                    uniqueUrlList.Add(currentUrl);
                }
            }

        }

        public async Task SiteDownloader(string url, string path)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument docContent = await web.LoadFromWebAsync(url);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            docContent.Save(path + ".htm") ;
        }

        public async Task SiteDownloaderProgressBar()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, 3);
            Console.WriteLine(siteDownloadProgress);
            siteDownloadProgress = siteDownloadProgress.Append("#");
        }

    }
}
