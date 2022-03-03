using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using GetWordBeforeWatchingMovie.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GetWordBeforeWatchingMovie.Models;
using Topdev.OpenSubtitles.Client;

namespace GetWordBeforeWatchingMovie.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private async Task<Subtitles[]> GetSubtitlesArray(string imdbId)
        {
            OpenSubtitlesClient client = new OpenSubtitlesClient();
            
            await client.LogInAsync("eng", "GetHardWordFromMovie v0.1", "vitconkute", "Vothien@123an");

            var actualId = imdbId;
            if (imdbId.StartsWith("tt"))
            {
                actualId = imdbId.Remove(0, 2); 
            }
            var foundList = await client.FindSubtitlesAsync(SearchMethod.IMDBId, actualId, "eng");

            return foundList;
        }

        private async Task<List<string>> GetWordLists(string imdbId, Level userEnglishLevel)
        {
            var subtitleList = await GetSubtitlesArray(imdbId);

            switch (subtitleList.Length)
            {
                case 0:
                    Console.WriteLine("Found no movie subtitles");
                    throw new ApplicationException("Cannot find the subtitle for the movie!");
                case >= 1:
                    Console.WriteLine($"Found {subtitleList.Length} movie subtitle(s) ");
                    break;
            }

            var client = new OpenSubtitlesClient();
            string movieName = subtitleList[0].MovieName;
            try
            {
                await client.DownloadSubtitleAsync(subtitleList[0], $"./Core/tempSubFiles/{movieName}.srt");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            var lines = await System.IO.File.ReadAllLinesAsync($"./Core/tempSubFiles/{movieName}.srt");
            var words = WordProcessor.BuildWords(lines);
            
            
            var wordsToLearn = DictionaryHandler.GetInstance().GetWordsToLearn(userEnglishLevel,words);
            
            // delete the temporary file
            System.IO.File.Delete($"./Core/tempSubFiles/{movieName}.srt");
            return wordsToLearn;
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> ReceiveRequest(string englishLevel, string imdbIdTextbox)
        {
            _logger.LogDebug(englishLevel + imdbIdTextbox);
            // Get the words
            Level userLevel = englishLevel switch
            {
                "A1" => Level.A1,
                "A2" => Level.A2,
                "B1" => Level.B1,
                "B2" => Level.B2,
                "C1" => Level.C1
            };
            var wordList = await GetWordLists(imdbIdTextbox, userLevel); 
            var model = new WordViewModel (wordList);
            return View("Index", model);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
