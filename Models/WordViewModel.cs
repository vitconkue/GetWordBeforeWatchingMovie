using System.Collections.Generic;

namespace GetWordBeforeWatchingMovie.Models
{
    public class WordViewModel
    {
        public List<string> Words { get; set; }
        public string Level { get; set; }

        public WordViewModel(List<string> initial, string level)
        {
            Words = new List<string>();
            Words.AddRange(initial);
            Level = level;
        }
    }
}