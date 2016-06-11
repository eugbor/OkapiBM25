namespace OkapiBM25
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;

    class Program
    {
        static void Main()
        {
            //Console.Write("Enter the path to file:");
            //string filePath = Console.ReadLine();
            string filePath = @"D:\C# project\OkapiBM25\text.txt";

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found");
                return;
            }

            string text;

            // reads a string of characters from the current position
            //to the end of the stream and returns the data as a string
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                text = streamReader.ReadToEnd();
            }

            // list sentences
            List<string> sentences = HelperMethods.SplitTextOnSentences(text).ToList();
            string[] words = text.Split(' ', '!', '.', ',', '?').Where(word => word != string.Empty).Distinct().ToArray();

            // list words
            List<OkapiWord> okapiWords = new List<OkapiWord>();
            foreach (var word in words)
            {
                okapiWords.Add(new OkapiWord() { Word = word, Rank = HelperMethods.GetRankOfWord(sentences, word) });
            }

            // top 10
            List<OkapiWord> frequencyWords = okapiWords.OrderByDescending(ow => ow.Rank).Take(10).ToList();


            foreach (var word in frequencyWords)
            {
                Console.WriteLine(word.Word);
            }
           
            /*
            string searchQuery = "The";

            var searchResult = (from doc in sentences
                               where HelperMethods.BM25(sentences, doc, searchQuery) != 0
                               orderby HelperMethods.BM25(sentences, doc, searchQuery)
                               select doc).Take(5).ToList();*/

            Console.ReadLine();
        }
    }
}