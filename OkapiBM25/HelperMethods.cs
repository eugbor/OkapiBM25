namespace OkapiBM25
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    static class HelperMethods
    {
        // frequency of words in the document
        private static int GetTermFrequency(string doc, string word)
        {
            doc = doc.ToLower();
            word = word.ToLower();
            string[] words = doc.Split(' ', '.', ',', '!', '?', '-', ':');
            var i = (from w in words where w == word select w).Count();
            return i;
        }

        // document length (number of words in it)
        private static int GetDoclenght(string doc)
        {
            doc = doc.ToLower();
            string[] words = doc.Split(' ', '.', ',', '!', '?', '-', ':');
            return words.Count();
        }

        // average length of the document in the collection
        private static double GetAvarageDocLenght(List<string> docs)
        {
            List<int> amountOfWord = new List<int>();
            foreach (string doc in docs)
            {
                string[] words = doc.Split(' ', '.', ',', '!', '?', '-', ':');
                var i = (from w in words select w).Count();
                amountOfWord.Add(i);
            }
            var avarengeCount = (from cow in amountOfWord select cow).Average();
            return avarengeCount;
        }

        // number of documents containing "word"
        private static int GetAmountOfDocsConteinedWord(List<string> docs, string word)
        {
            var i = (from doc in docs
                     where doc.IndexOf(word) > 0
                     select doc).Count();
            return i;
        }

        // calculating frequency feedback documents IDF
        private static double GetIDF(List<string> docs, string word)
        {
            int N = docs.Count();
            int n = GetAmountOfDocsConteinedWord(docs, word);
            double idf = Math.Log(((N - n + 0.5) / (n + 0.5)), 2);
            return idf;
        }

        // calculating function BM25
        public static double BM25(List<string> docs, string doc, string query)
        {
            string[] words = query.Split(' ', '.', ',', '!', '?', '-', ':');
            double rankIndex = 0;
            double k = 2;
            double b = 0.75;
            foreach (string word in words)
            {
                double rankOfWord = GetIDF(docs, word) * (GetTermFrequency(doc, word) * (k + 1)) /
                                    (GetTermFrequency(doc, word) + k * (1 - b + b * (GetDoclenght(doc) /
                                    GetAvarageDocLenght(docs))));
                rankIndex += rankOfWord;
            }

            return rankIndex;
        }

        // calculating rank of word
        public static double GetRankOfWord(List<string> docs, string word)
        {
            double k = 2;
            double b = 0.75;
            double rankOfWord = 0;
            foreach (var doc in docs)
            {
                rankOfWord += GetIDF(docs, word) * (GetTermFrequency(doc, word) * (k + 1)) /
                              (GetTermFrequency(doc, word) + k * (1 - b + b * (GetDoclenght(doc) /
                              GetAvarageDocLenght(docs))));
            }
            return rankOfWord;
        }

        // splits an input string into an array of substrings at the positions
        //defined by the specified regular expression pattern
        public static string[] SplitTextOnSentences(string text)
        {
            return Regex.Split(text, "(?<=[\\.!\\?])\\s+");
        }
    }
}
