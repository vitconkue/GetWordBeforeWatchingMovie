using System.Collections.Generic;

namespace GetWordBeforeWatchingMovie.Models
{
    public class WordViewModel
    {
        public List<string> Words { get; set; }

        public WordViewModel(List<string> initial)
        {
            Words = new List<string>();
            Words.AddRange(initial);
        }
    }
}