using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GetWordBeforeWatchingMovie.Core
{
    public class DictionaryHandler
    {
        private DictionaryHandler()
        {
            A1WordList = File.ReadAllLines("./Core//A1.txt");
            A2WordList = File.ReadAllLines("./Core/A2.txt");
            B1WordList = File.ReadAllLines("./Core/B1.txt");
            B2WordList = File.ReadAllLines("./Core/B2.txt");
            C1WordList = File.ReadAllLines("./Core/C1.txt");
        }
        private static readonly DictionaryHandler instance = new DictionaryHandler();
        private string[] A1WordList;
        private string[] A2WordList;
        private string[] B1WordList;
        private string[] B2WordList;
        private string[] C1WordList ;

        public static DictionaryHandler GetInstance()
        {
            return instance;
        }
        
        public List<string> GetWordsToLearn(Level userLevel, List<string> movieWordList)
        {
            string[] chosenList = userLevel switch
            {
                Level.A1 => A2WordList.Concat(A1WordList).ToArray(),
                Level.A2 => B1WordList,
                Level.B1 => B2WordList,
                Level.B2 => C1WordList,
                Level.C1 => C1WordList
            };

            return
                chosenList
                    .SelectMany(WordTransformHandler.GetAllWordForms)
                    .Where(movieWordList.Contains)
                    .ToList();
          
        }
    }
}