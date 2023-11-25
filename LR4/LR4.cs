using System;
using System.Net;
using System.Linq;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Numerics;

namespace LR4
{
    public class WebScanner : IDisposable
    {
        private readonly HashSet<Uri> _procLinks = new HashSet<Uri>();
        private readonly WebClient _webClient = new WebClient();
        private readonly HashSet<string> _ignoreFiles = new HashSet<string>() { ".ico", "xml"};

        public event Action<Uri, List<string>, int> TargetFound;

        private void OnTargetFound(Uri page, List<string> links, int count)
        {
            TargetFound?.Invoke(page, links, count);
        }
        private void Process(string domain, Uri page, int count)
        {
            if (count <= 0) return;
            if (_procLinks.Contains(page)) return;
            if (!isReachable(page)) { return; }

            _procLinks.Add(page);

            string html = _webClient.DownloadString(page);

            var hrefs = (from href in Regex.Matches(html, @"href=""[\/\w-\.:]+""").Cast<Match>()
                         let url = href.Value.Replace("href=", "").Trim('"')
                         let loc = url.StartsWith("/")
                         select new
                         {
                             Ref = loc ? $"https://mangalib.me{url}" : url,
                             IsLocal = loc || url.StartsWith(domain)
                         }
                         ).ToList();

            var images = (from href in Regex.Matches(html, @"src="".*?\.(jpg|jpeg|png)""").Cast<Match>()
                         let url = href.Value.Replace("href=", "").Trim('"')
                         let loc = url.StartsWith("/")
                         select new
                         {
                             Ref = loc ? $"https://mangalib.me{url}" : url,
                             IsLocal = loc || url.StartsWith(domain)
                         }
                         ).ToList();

            List<string> externals = new List<string> { };
            foreach (var href in images) { if (!href.IsLocal) { externals.Add(href.Ref); } }
            if (externals.Count > 0) OnTargetFound(page, externals, count);
            List<string> locals = new List<string> { };
            foreach (var href in hrefs) { if (href.IsLocal) { locals.Add(href.Ref); } }

            foreach (var href in locals)
            {
                string fielEx = Path.GetExtension(href).ToLower();
                if (_ignoreFiles.Contains(fielEx)) continue;

                Process(domain, new Uri(href), --count);
            }
        }

        private bool isReachable(Uri url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 15000;
            request.Method = "HEAD";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }
        public void Scan(Uri startPage, int pageCount)
        {
            _procLinks.Clear();

            string domain = $"{startPage.Scheme}://{startPage.Host}";
            Process(domain, startPage, pageCount);
        }
        public void Dispose()
        {
            _webClient.Dispose();
        }
    }
    class LR4
    {
        public static void writeToCSV(List<List<string>> data, string path)
        {

        }
        static void Main()
        {
            List<List<string>> jopa = new List<List<string>> { };
            Uri startPage = new Uri("https://mangalib.me/");
            using (WebScanner scanner = new WebScanner())
            {
                scanner.TargetFound += (page, links, count) =>
                {
                    jopa.Add(new List<string> { page.ToString(), count.ToString() });
                    Console.WriteLine($"\nPage:\n\t{page}\nImages:");
                    foreach (var link in links)
                        Console.WriteLine($"\t{link}");
                };
                scanner.Scan(startPage, 30);
                writeToCSV(jopa, "C:\\Users\\gleb\\Desktop");
                Console.WriteLine("Done.");
            }
        }
    }
}