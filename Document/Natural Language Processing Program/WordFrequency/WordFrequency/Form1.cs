/**
 * Author: Jiayun Han
 * http://wwww.nlpdotnet.com
 */ 

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WordFrequency
{
    public partial class Form1 : Form
    {
        // This will discard digits
        private static char[] delimiters_no_digits = new char[] {
            '{', '}', '(', ')', '[', ']', '>', '<','-', '_', '=', '+',
            '|', '\\', ':', ';', '"', ',', '.', '/', '?', '~', '!',
            '@', '#', '$', '%', '^', '&', '*', ' ', '\r', '\n', '\t',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public Form1()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (textBoxInput.Text != string.Empty)
            {
                string[] words = Tokenize(textBoxInput.Text);
                if (words.Length > 0)
                {
                    SortedDictionary<string, int> dict
                        = new SortedDictionary<string, int>();

                    foreach (string word in words)
                    {
                        if (dict.ContainsKey(word))
                        {
                            dict[word]++;
                        }
                        else
                        {
                            dict.Add(word, 1);
                        }
                    }

                    // Dump out the dict entries to the output box.
                    // For efficiency, dump them to StringBuilder and set the
                    // capacity of the StringBuilder to the number of entries
                    // multipled by the average length of each entry plus 4 for
                    // [number]. For more details, see .NET Framework SDK 
                    // documentation for StringBuilder.
                    StringBuilder resultSb = new StringBuilder(dict.Count * 9);
                    foreach (KeyValuePair<string, int> entry in dict)
                    {
                        resultSb.AppendLine(string.Format("{0} [{1}]", entry.Key, entry.Value));
                    }

                    textBoxOutput.Text = resultSb.ToString();
                }
            }
        }

        /// <summary>
        /// Tokenizes a text into an array of words, using the improved
        /// tokenizer with the discard-digit option.
        /// </summary>
        /// <param name="text">the text to tokenize</param>
        public static string[] Tokenize(string text)
        {
            string[] tokens = text.Split(delimiters_no_digits, 
                                    StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];                               

                // Change token only when it starts and/or ends with "'" and 
                // it has at least 2 characters.

                if (token.Length > 1)
                {
                    if (token.StartsWith("'") && token.EndsWith("'"))
                        tokens[i] = token.Substring(1, token.Length - 2);  // remove the starting and ending "'"

                    else if (token.StartsWith("'"))
                        tokens[i] = token.Substring(1); // remove the starting "'"

                    else if (token.EndsWith("'"))
                        tokens[i] = token.Substring(0, token.Length - 1); // remove the last "'"
                }
            }

            return tokens;
        }
    }
}
