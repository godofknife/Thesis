/**
 * Author: Jiayun Han
 * http://wwww.nlpdotnet.com
 */ 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace TypeTokenRatio
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // When the go button is clicked, show the Open file dialog.
        private void btnGo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Open a text file.");
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            // Limit to file with .txt extention only.
            string file = openFileDialog1.FileName;
            if (Path.GetExtension(file) != ".txt")
            {
                MessageBox.Show("Select text file only!");
                return;
            }            

            // The length of this array is the number of tokens of the file.
            string[] tokens = null;

            // This deals with the exception caused by fils with .txt extentions
            // but they are acutally not text files.
            try
            {
                string text = File.ReadAllText(file);
                if (text == string.Empty)
                {
                    MessageBox.Show("This is an empty file. No type token ratio can be computed.");
                    return;
                }

                tokens = Tokenize(text);                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            textBoxFile.Text = Path.GetFileName(file);

            // HashSet has the feature that only unique items are accepted, which is exactly what we want for types.
            HashSet<string> types;
            double typeTokenRatio; 

            // The out keyword indicate that the parameter will be instantiated by the called method.
            // In our case, the HashSet and the typeTokenRation that will be given a new value after
            // the invocation of GetTypeTokenTation method is completed.
            GetTypeTokenRatio(tokens, out types, out typeTokenRatio);

            // Fill in the text boxes
            textBoxTokens.Text = tokens.Length.ToString();
            textBoxTypes.Text = types.Count.ToString();

            // How to read the format "{0:0.###}": 
            // the first 0 is a placeholder for the actual value (in our case, 
            //     the type token ratio number with a lot of digits); 
            // the second 0 tells the system to keep the 0 before the decimal point;
            // ### means keep only three digits after the decimal point.
            //
            textBoxTTR.Text = string.Format("{0:0.###}", typeTokenRatio);            
        }

        private static void GetTypeTokenRatio(string[] tokens, out HashSet<string> types, out double typeTokenRatio)
        {
            // dump array of words into a HashSet of string. 
            types = new HashSet<string>();

            // HashSet ignores duplicated elements which ensures for us that duplicated words be counted only once.
            foreach (string token in tokens)
            {
                types.Add(token);
            }

            // A sanity check: if types set is empty, set typeTokenRatio = double.NaN, i.e. Not a Number. 
            // Otherwise, we'll get a "divided by 0" Exception.

            if (types.Count == 0)
            {
                typeTokenRatio = double.NaN;
            }
            else
            {
                // Be very aware that you need to cast either types.Count or tokens.Length into 
                // double type; otherwise you'll always get 0 as the result.
                typeTokenRatio = (double)types.Count / tokens.Length;
            }
        }

        /// <summary>
        /// Tokenizes a text into an array of words, using the improved
        /// tokenizer with the discard-digit option.
        /// </summary>
        /// <param name="text">the text to tokenize</param>
        private static string[] Tokenize(string text)
        {
             // This will discard digits
            char[] delimiters_no_digits = new char[] {
            '{', '}', '(', ')', '[', ']', '>', '<','-', '_', '=', '+',
            '|', '\\', ':', ';', '"', ',', '.', '/', '?', '~', '!',
            '@', '#', '$', '%', '^', '&', '*', ' ', '\r', '\n', '\t',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

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
