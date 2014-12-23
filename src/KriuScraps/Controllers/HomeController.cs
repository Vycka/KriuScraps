using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KriuClient;

namespace KriuScraps.Controllers
{
    public class HomeController : Controller
    {
        private static readonly Random Random = new Random();

        //
        // GET: /Home/

        public ActionResult Index()
        {
            char letter = GetRandomLetter();
            int pageCount = Kriu.GetPageCount(letter);

            List<string> keyWords = Kriu.GetKeyWords(letter, Random.Next(1, pageCount)).ToList();
            int randomKeyWordIndex = Random.Next(0, keyWords.Count - 1);

            WordDefinition result = Kriu.GetDefinition(keyWords[randomKeyWordIndex]);
            return View(result);
        }

        private char GetRandomLetter()
        {
            int num = Random.Next(0, 26); // Zero to 25
            char letter = (char) ('a' + num);

            return letter;
        }

    }
}
