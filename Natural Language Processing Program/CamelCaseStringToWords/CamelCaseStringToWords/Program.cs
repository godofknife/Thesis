/**
 * Author: Jiayun Han
 * http://wwww.nlpdotnet.com
 */ 

using System;
using System.Text.RegularExpressions;

namespace CamelCaseStringToWords
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = string.Empty;

            Console.WriteLine("\r\n**** Convert Camel Case String to Words ****\r\n\r\n");

            // Loop forever until user types 'q' or "Q".
            while (true)
            {
                Console.Write("Type a word to split or Q or q to quit: ");
                input = Console.ReadLine();

                if (input.ToLower() == "q")
                {
                    break;
                }

                // We use the static Replace method of the Regex class. It has several overloads.
                // For this particular overload, it accepts three parameters:
                // (1) The string to match against and replace for (the came case string, in our case)
                // (2) The regular expression to match for
                // (3) The string to replace with (a space, in our case)
                string newStr = Regex.Replace(input, "((?<=[a-z])(?=[A-Z]))|((?<=[A-Z])(?=[A-Z][a-z]))", " ");

                Console.WriteLine();
                Console.WriteLine(newStr);
                Console.WriteLine();
            }
        }
    }
}
