/**
 * Author: Jiayun Han
 * http://wwww.nlpdotnet.com
 */ 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ImprovedTokenizer
{
    public partial class Form1 : Form
    {
        // Use this to keep digits.
        private static char[] delimiters_keep_digits = new char[] {
            '{', '}', '(', ')', '[', ']', '>', '<','-', '_', '=', '+',
            '|', '\\', ':', ';', '"', ',', '.', '/', '?', '~', '!',
            '@', '#', '$', '%', '^', '&', '*', ' ', '\r', '\n', '\t'};

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
            string text = textBoxInput.Text;

            // Call the improved tokenizer
            string[] tokens = Tokenize(text, radioKeepDigits.Checked);

            // This time, we first put the result in a StringBuilder for the sake of efficiency;
            // then we dump the content of the StringBuilder to the text box.
            // We assume each word is 4 letters on the average.

            StringBuilder sb = new StringBuilder(tokens.Length * 4);

            foreach (string token in tokens)
            {
                sb.AppendLine(token);                
            }

            textBoxOutput.Text = sb.ToString();
        }

        /// <summary>
        /// Tokenizes a text into an array of words, using whitespace and
        /// all punctuation except the apostrophe "'" as delimiters. Digits
	    /// are handled based on user choice.
        /// </summary>
        /// <param name="text">the text to tokenize</param>
        /// <param name="keepDigits">true to keep digits; false to discard digits.</param>
        /// <returns>an array of resulted tokens</returns>
        public static string[] Tokenize(string text, bool keepDigits)
        {
            string[] tokens = null;

            if (keepDigits)
                tokens = text.Split(delimiters_keep_digits, StringSplitOptions.RemoveEmptyEntries);
            else
                tokens = text.Split(delimiters_no_digits, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < tokens.Length; i++ )
            {
                string token = tokens[i];

                // Change token only when it starts and/or ends with "'" and the
                // toekn has at least 2 characters.
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
