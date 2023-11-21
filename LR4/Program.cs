using System;
using System.Net;
using System.Linq;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace LR4
{
    public class WebScanner : IDisposable
    {
        private readonly HashSet<Uri> _procLinks = new HashSet<Uri>();
        private readonly WebClient _webClient = new WebClient();
        private readonly HashSet<string> _ignoreFiles = new HashSet<string>() { ".ico", "xml" };

        public event Action<Uri, Uri[]> TargetFound;

        private void OnTargetFound(Uri page, Uri[] links)
        {
            TargetFound?.Invoke(page, links);
        }
        private void Process(string domain, Uri page, int count)
        {
            if (count <= 0) return;
            if (_procLinks.Contains(page)) return;

            _procLinks.Add(page);
            string html = _webClient.DownloadString(page);

            var hrefs = (from href in Regex.Matches(html, @"href=""[\/\w -\.:]+""").Cast<Match>()
                         let url = href.Value.Replace("href=", "").Trim('"')
                         let loc = url.StartsWith("/")
                         select new
                         {
                             Ref = loc ? $"https://mangalib.me{url}" : url,
                             IsLocal = loc || url.StartsWith(domain)
                         }
                         ).ToList();

            var externals = (from href in hrefs where !href.IsLocal select href.Ref).ToArray();
            if (externals.Length > 0) OnTargetFound(page, externals);
            var locals = (from href in hrefs where href.IsLocal select href.Ref).ToList();

            foreach (var href in locals)
            {
                string fielEx = Path.GetExtension(href.LocalPath).ToLower();
                if (_ignoreFiles.Contains(fielEx)) continue;

                Process(domain, href, --count);
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
        static void Main()
        {
            Uri startPage = new Uri("https://mangalib.me/");
            using (WebScanner scanner = new WebScanner())
            {
                scanner.TargetFound += (page, links) =>
                {
                    Console.WriteLine($"\nPage:\n\t{page}\nLinks:");
                    foreach (var link in links)
                        Console.WriteLine($"\t{link}");
                };
                scanner.Scan(startPage, 4);
                Console.WriteLine("Done.");
            }
        }
    }
}