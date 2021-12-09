using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDownloader
{
    interface IDownloadService
    {
        public Task WebSiteProcessor();
        public Task SiteIterator(string url, string path, Queue<Node> urlQueue);
        public Task SiteDownloader(string url, string path);
        public Task SiteDownloaderProgressBar();

    }
}
