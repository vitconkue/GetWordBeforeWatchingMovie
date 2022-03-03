using System;
using System.Collections.Generic;

namespace GetWordBeforeWatchingMovie.Core
{
    public class WordTransformHandler
    {
        private static readonly List<char> _vowels = new() { 'a', 'o', 'i', 'u', 'e' };

        private static List<string> GetAllSAndESForm(string word)
        {
            if (string.IsNullOrWhiteSpace(word) || string.IsNullOrWhiteSpace(word))
            {
                throw new ArgumentException($" e-es-: Throw at word: {word}"); 
            }
            var result = new List<string>();
            result.Add(word + "s");
            // -s, -es
            if (word[^1] == 'y')
                result.Add(word.Remove(word.Length - 1) + "ies");
            if (word.EndsWith("ch"))
                result.Add(word + "es");
       
            return result;
        }

        private static List<string> GetAllEDAndINGForm(string word)
        {
            var result = new List<string>();
            if (string.IsNullOrEmpty(word))
                throw new ArgumentException($"Ed-ing: throws at {word}");

            result.Add(word + "ing");
            result.Add(word + "ed");
            // -ing and -ed 
            if (word.Length >= 2 && _vowels.Contains(word[^2]))
            {
                result.Add(word + word[^1] + "ing");
                result.Add(word + word[^1] + "ed");
            }
            else if (_vowels.Contains(word[^1]))
            {
                result.Add(word.Remove(word.Length - 1) + "ing");
                if (word[^1] == 'e')
                    result.Add(word + "d");
            }
            else if (word[^1] == 'y')
            {
                result.Add(word.Remove(word.Length - 1) + "ied");
            }

            return result;
        }

        private static List<string> GetAllVerbAndNounTransformation(string word)
        {
            word = word.Trim();
            var result = new List<string>();
            result.AddRange(GetAllSAndESForm(word));
            // -ing and -ed 
            result.AddRange(GetAllEDAndINGForm(word));
            result.Add(word);
            return result;
        }

        private static List<string> GetAllAdjTransformation(string word)
        {
            word = word.Trim();
            var result = new List<string>();
            result.Add(word + "ly");
            if (word.Length >= 2 && _vowels.Contains(word[^2]))
            {
                result.Add(word + "lly");
            }

            return result;
        }

        public static List<string> GetAllWordForms(string word)
        {
            var result = new List<string>() { word };
            if (string.IsNullOrWhiteSpace(word))
            {
                return result;
            }
            result.AddRange(GetAllVerbAndNounTransformation(word));
            result.AddRange(GetAllAdjTransformation(word));

            return result;

        }
    }
}