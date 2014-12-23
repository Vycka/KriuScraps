using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery;
using CsQuery.ExtensionMethods.Internal;

namespace KriuClient
{
    public static class Kriu
    {
        public const string Host = "http://zodynas.kriu.lt/";
        public static int GetPageCount(char indexLetter)
        {
            Uri url = new Uri(Host + "raide/" + indexLetter.ToLower());
            CQ dom = RequestForPageDom(url);

            // Page by default loads on last page
            CQ pageElements = dom[".activepage"];

            int maxPage = Int32.Parse(pageElements.FirstElement().InnerHTML);

            return maxPage;
        }

        public static IEnumerable<string> GetKeyWords(char indexLetter, int page)
        {
            Uri url = new Uri(Host + "raide/" + indexLetter.ToLower() + "/?page=" + page);
            CQ dom = RequestForPageDom(url);

            CQ keyElements = dom[".zodis a"];

            return keyElements.Select(element => element.GetAttribute("href").Replace("http://zodynas.kriu.lt/zodis/",""));

        }

        public static WordDefinition GetDefinition(string keyWord)
        {
            Uri url = new Uri(Host + "zodis/" + keyWord);
            CQ dom = RequestForPageDom(url);

            return new WordDefinition
            {
                Name = dom[".word-title"].FirstElement().InnerHTML,
                Translation = dom[".vertimas"].FirstElement().InnerHTML,
                Definition = dom[".pavyzdys"].FirstElement().InnerHTML
            };
        }
        
        private static string DecodeHtmlChars(string aText)
        {
            string[] parts = aText.Split(new string[]{"&#x"}, StringSplitOptions.None);
            for (int i = 1; i < parts.Length; i++)
            {
                int n = parts[i].IndexOf(';');
                string number = parts[i].Substring(0,n);
                try
                {
                    int unicode = Convert.ToInt32(number, 16);
                    parts[i] = ((char)unicode) + parts[i].Substring(n+1);
                } catch {}
            }
            return String.Join("",parts);
        }

        private static CQ RequestForPageDom(Uri pageUri)
        {
            return CQ.CreateFromUrl(pageUri.ToString());
        }
    }
}
