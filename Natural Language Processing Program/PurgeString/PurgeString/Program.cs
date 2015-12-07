/**
 * Author: Jiayun Han
 * http://wwww.nlpdotnet.com
 */ 
using System;

namespace PurgeString
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = string.Empty;

            Console.WriteLine("\r\n***** Non-Alphanumeric Starting and Ending Character Remover ******\r\n\r\n");

            // Loop forever until user types 'q' or "Q".
            while(true)
            {
                Console.Write("Type a test string or Q to quit: ");
                input = Console.ReadLine();
                
                if (input.ToLower() == "q")
                {
                    break;
                }

                string output = RemovePuncts(input);
                Console.WriteLine(output + "\r\n");
            }
        }

        /// <summary>
        /// Strip off the starting non-alphanumeric character and the ending non-alphanumeric character from a string.
        /// </summary>
        /// <param name="str">the string to process</param>
        /// <returns>the string with the starting and ending non-alphanumeric characters stripped</returns>
        private static string RemovePuncts(string str)
        {
            string newStr = str;
            if (newStr != string.Empty)
            {
                int strLength = newStr.Length;

                bool punctHead = !char.IsLetterOrDigit(newStr[0]);                
                bool punctEnd = !char.IsLetterOrDigit(newStr[strLength - 1]);

                //if at least one end is a non-alphanumeric character...
                if (punctHead || punctEnd)
                {                    
                    if (punctHead && punctEnd && strLength >= 2)
                    {
                        //if both ends are a non-alphanumeric character, remove the first and last last characters.
                        newStr = newStr.Substring(1, strLength - 2);
                    }
                    else if (punctHead)
                    {
                        //if it stars with a non-alphanumeric character, remove the first character.
                        newStr = newStr.Substring(1);
                    }
                    else
                    {
                        //if it ends with a non-alphanumeric character, remove the last character
                        newStr = newStr.Substring(0, strLength - 1);
                    }
                }
            }

            return newStr;
        }
    }
}
