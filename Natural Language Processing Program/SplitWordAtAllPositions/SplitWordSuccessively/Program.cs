/**
 * Author: Jiayun Han
 * http://wwww.nlpdotnet.com
 */ 

using System;

namespace SplitWordSuccessively
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = string.Empty;

            Console.WriteLine("\r\n**** Split a word into pairs at all possible positions ****\r\n\r\n");

            // Loop forever until user types 'q' or "Q".
            while (true)
            {
                Console.Write("Type a word to split or Q or q to quit: ");
                input = Console.ReadLine();

                if (input.ToLower() == "q")
                {
                    break;
                }

                // Make sure the word has at least 2 letters to make sense of our program
                if (input.Length < 2)
                {
                    Console.WriteLine("The word is too short to split.");
                }
                else
                {
                    string[] parts = SplitWord(input);

                    Console.WriteLine();

                    for (int i = 0; i < parts.Length - 1; i += 2)
                    {
                        Console.WriteLine(string.Format("{0} {1}", parts[i], parts[i + 1]));
                    }
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Splits an n-letter word into a string array. The array's size will be (n-1) * 2. 
        /// For example, 'carried', a 7-letter word, will be splitted into a string array
        /// containing the following (7-1) * 2 = 12 elements:
        /// {c, arried, ca, rried, car, ried, carr, ied, carri, ed, carri, d}.
        /// </summary>
        /// <param name="word">the word to split</param>
        /// <returns>all pairs of strings which the word can be split into</returns>
        private static string[] SplitWord(string word)
        {
            //The word must have at least 2 letters
            if (word.Length < 2)
                return null;

            int arrSize = (word.Length - 1) * 2;
            string[] strArr = new string[arrSize];
            
            int index = 0;

            for (int cutPosition = 1; cutPosition < word.Length; cutPosition++)
            {
                strArr[index] = word.Substring(0, cutPosition);
                strArr[index + 1] = word.Substring(cutPosition, word.Length - cutPosition);

                //Since we collect two elements per time, we need to increase the index by 2.
                index += 2;
            }
            return strArr;
        }
    }
}
