using System;
using System.Collections.Generic;
using System.Linq;

namespace GetWordBeforeWatchingMovie.Core
{
    public static class WordProcessor
    {

        private static bool IsBelongToSentence(string line) => line.StartsWith("<i>") || line.EndsWith("</i>");
        private static List<string> ProcessLineInSrtFile(List<string> lines)
        {

            var processedLines = new List<string>();
            foreach (var line in lines)
            {
                var subProcessedLine = line;
                if (line.StartsWith("<i>"))
                {
                    subProcessedLine = subProcessedLine.Remove(0, 3); 
                }

                if (line.EndsWith("</i>"))
                {
                    subProcessedLine = subProcessedLine.Remove(subProcessedLine.Length - 5, 5); 
                }
                
                processedLines.Add(subProcessedLine);
                    
            }
            return processedLines;
        }

        public static List<string> BuildWords(string[] lines)
        {
            // tokenize 
            var wordList = new List<string>();
            var processedLines = ProcessLineInSrtFile
                (lines.ToList());
            foreach (var line in processedLines)
            {
                var normalized = line.Normalize();
                var words =
                    normalized.Split(new[] { ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);

                wordList.AddRange(words);
            }
            // Remove ..., !, ?, (, )
            var punctuations = new[] { '!', '.', '?', '(', ')', '\'' };
            for(var i = 0; i < wordList.Count; i++)
            {
                var word = wordList[i];
                var lastCharacter = word[^1];
                if (punctuations.Contains(lastCharacter))
                {
                    wordList[i] = word.Remove(word.Length - 1);
                }
            }

            return wordList.Select(w => w.Trim()).Where(w => !string.IsNullOrWhiteSpace(w)).ToList(); 

        }
    }
}