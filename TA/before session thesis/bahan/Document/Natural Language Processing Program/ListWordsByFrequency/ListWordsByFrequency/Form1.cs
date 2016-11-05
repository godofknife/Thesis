/**
 * Author: Jiayun Han
 * http://wwww.nlpdotnet.com
 */ 

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ListWordsByFrequency
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// An enumeration of sorting options to be used.
        /// </summary>
        private enum SortOrder
        {
            Ascending, // from small to big numbers or alphabetically.
            Descending // from big to small number or reversed alphabetical order
        }

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

        /// <summary>
        /// Tokenizes a text into an array of words, using the improved
        /// tokenizer with the discard-digit option.
        /// </summary>
        /// <param name="text">the text to tokenize</param>
        private string[] Tokenize(string text)
        {
            string[] tokens = text.Split(delimiters_no_digits, StringSplitOptions.RemoveEmptyEntries);

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

        /// <summary>
        /// Make a string-integer dictionary out of an array of words.
        /// </summary>
        /// <param name="words">the words out of which to make the dictionary</param>
        /// <returns>a string-integer dictionary</returns>
        private Dictionary<string, int> ToStrIntDict(string[] words)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
                        
            foreach (string word in words)
            {
                // if the word is in the dictionary, increment its freq.
                if (dict.ContainsKey(word))
                {
                    dict[word]++;
                }
                // if not, add it to the dictionary and set its freq = 1
                else
                {
                    dict.Add(word, 1);
                }
            }

            return dict;
        }

        /// <summary>
        /// Sort a string-int dictionary by its entries' values.
        /// </summary>
        /// <param name="strIntDict">a string-int dictionary to sort</param>
        /// <param name="sortOrder">one of the two enumerations: Ascening and Descending</param>
        /// <returns>a string-integer dictionary sorted by integer values</returns>
        private Dictionary<string, int> ListWordsByFreq(Dictionary<string, int> strIntDict, SortOrder sortOrder)
        {
            // Copy keys and values to two arrays
            string[] words = new string[strIntDict.Keys.Count];
            strIntDict.Keys.CopyTo(words, 0);

            int[] freqs = new int[strIntDict.Values.Count];
            strIntDict.Values.CopyTo(freqs, 0);

            //Sort by freqs: it sorts the freqs array, but it also rearranges
            //the words array's elements accordingly (not sorting)
            Array.Sort(freqs, words);

            // If sort order is descending, reverse the sorted arrays.
            if (sortOrder == SortOrder.Descending)
            {
                //reverse both arrays
                Array.Reverse(freqs);
                Array.Reverse(words);
            }

            //Copy freqs and words to a new Dictionary<string, int>
            Dictionary<string, int> dictByFreq = new Dictionary<string, int>();

            for (int i = 0; i < freqs.Length; i++)
            {
                dictByFreq.Add(words[i], freqs[i]);
            }

            return dictByFreq;
        }

        // Load file to input box
        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                textBoxInput.Text = File.ReadAllText(openDlg.FileName);
            }
        }

        // Process input box text and display result in output box
        private void btnGo_Click(object sender, EventArgs e)
        {
            if (textBoxInput.Text != string.Empty)
            {
                // Split text into array of words
                string[] words = Tokenize(textBoxInput.Text);

                if (words.Length > 0)
                {
                    // Make a string-int dictionary out of the array of words 
                    Dictionary<string, int> dict = ToStrIntDict(words);
                                        
                    // Set SortOrder based on user's choice. 
                    //
                    // The follow code is the compact form of:
                    //  if radioButtonAscending is Checked, sortOrder = SortOrder.Ascending
                    //  else sortOrder = SortOrder.Descending
                    //
                    SortOrder sortOrder = radioButtonAscending.Checked ? SortOrder.Ascending : SortOrder.Descending;

                    // Sort dict by values
                    dict = ListWordsByFreq(dict, sortOrder);

                    // Dump dict entries to a StrinbBuilder for efficiency.
                    StringBuilder resultSb = new StringBuilder(dict.Count * 9);
                    foreach (KeyValuePair<string, int> entry in dict)
                        resultSb.AppendLine(string.Format("{0} [{1}]", entry.Key, entry.Value));

                    // Put the content of the StringBuilder in the Output box
                    textBoxOutput.Text = resultSb.ToString();
                }
            }
        }
    }
}
