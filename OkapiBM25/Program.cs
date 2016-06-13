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

            // List<OkapiWord> frequencyWords = okapiWords.OrderByDescending(ow => ow.Rank).Take(10).ToList();

            List<OkapiSentence> okapiSentences = new List<OkapiSentence>();
            foreach (var sentence in sentences)
            {
                // returns a string array that contains the substring of this instance,
                //separated by elements of a specified Unicode character array
                var wordsInSentence = sentence.Split(' ', ',', ':', ';', '.', '!', '?').Where(word => word != string.Empty).ToList();

                double rank = 1;
                foreach (var word in wordsInSentence)
                {
                    // returns the first element of a sequence or a default value
                    //if the sequence contains no elements
                    var foundOkapiWord = okapiWords.FirstOrDefault(w => w.Word.ToLower() == word.ToLower());
                    if (foundOkapiWord != null)
                    {
                        rank *= foundOkapiWord.Rank;
                    }
                }

                okapiSentences.Add(new OkapiSentence() { Sentence = sentence, Rank = rank });
            }

            // sorts the elements of a sequence in descending order
            var importantSentences = okapiSentences.OrderByDescending(sent => sent.Rank).Take(5).ToList();

            foreach (var impSent in importantSentences)
            {
                Console.WriteLine(impSent.Sentence);
            }

            Console.ReadLine();
        }
    }
}