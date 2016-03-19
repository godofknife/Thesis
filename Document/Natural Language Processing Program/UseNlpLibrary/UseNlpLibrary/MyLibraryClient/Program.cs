using System;
using System.Collections.Generic;
using System.Text;

// Need this directive to use our library
using MyLibrary;

namespace MyLibraryClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //string sentence = "Dr. Harper said that it's a mistake that " + 
            //                  "Dr. Covington didn't lend $5.34 to Dr. Pager yesterday.";
            string sentence;
            sentence = Console.ReadLine();

            // Use the instance methods of the Tokenizer class
            Tokenizer tokenizer = new Tokenizer();

            // use greedy tokenization
            string[] tokens = tokenizer.GreedyTokenize(sentence);
            DisplayTokens("Result of greedy tokenization", tokens);

            // use improved tokenization, keeping digits
            tokens = tokenizer.Tokenize(sentence, true);
            DisplayTokens("Result of improved tokenization, keeping digits", tokens);

            // use improved tokenization, throwing away digits
            tokens = tokenizer.Tokenize(sentence, false);
            DisplayTokens("Result of imporved tokenization, throwing away digits", tokens);

            // Use the static methods of the Tool class
            //

            // Get word frequency table
            Dictionary<string, int> wordFreq = Tool.ToStrIntDict(sentence);
            DisplayDictionary("Result of word frequency", wordFreq);

            // List words by frequency
            wordFreq = Tool.ListWordsByFreq(wordFreq, true);
            DisplayDictionary("List words by frequency", wordFreq);

            // Get type token ratio
            Double ttr = Tool.GetTypeTokenRatio(sentence);
            Console.WriteLine("\r\nType token ratio = {0}\r\n", ttr);
            Console.ReadKey();
        }

        private static void DisplayDictionary(string title, Dictionary<string, int> dict)
        {
            Console.WriteLine("\r\n\r\n\r\n========={0}===========\r\n", title);
            foreach (KeyValuePair<string, int> kv in dict)
            {
                Console.WriteLine(kv.Key + "\t" + kv.Value);
            }
        }

        private static void DisplayTokens(string title, string[] tokens)
        {
            Console.WriteLine("\r\n\r\n\r\n========={0}===========\r\n", title);
            foreach (string token in tokens)
            {
                Console.Write(token + " | ");
            }
        }
    }
}
