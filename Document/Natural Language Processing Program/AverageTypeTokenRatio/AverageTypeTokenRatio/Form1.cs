/**
 * Find the average type token ratio of the text entered by user
 * or read from a selected file.
 * 
 * Author: Jiayun Han
 * http://www.@nlpdotnet.com
 */ 

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace AverageTypeTokenRatio
{
    public partial class Form1 : Form
    {

        #region Form-wide variables

        // Do nothing if there are less than 100 tokens
        const int MIN_WINDOW_SIZE = 100;

        // Assume we allow maximally 100 clicks on this control in one direction.
        const int NUMERIC_UP_DOWN_CLICKS = 20;

        string[] tokens;
        HashSet<string>
        types;

        double generalTypeTokenRatio;

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        #region Methods

        // This method is called after user selects a file or after user's mouse leaves the input
        // textbox. It sets the maximum and increment values of the NumericUpDown control based on
        // the number of the tokens and the pre-defined minimum and clicks values. It also fills
        // the textbox for the number of the tokens.
        private void SetNumericUpDownControl()
        {
            // The text in the input textbox may have been manually pasted or read from a file. In
            // any case, if there is no text in the input textbox, do nothing.
            string text = textBoxInput.Text.Trim();
            if (text != string.Empty)
            {
                // Tokenize text using the improved greedy tokenizer
                tokens = Tokenize(text);

                // For this application, we are interested in texts with at least MIN_WINDOW_SIZE tokens
                if (tokens.Length < MIN_WINDOW_SIZE)
                {
                    MessageBox.Show(
                         string.Format("Too few tokens. Try text with more than {0} tokens.", MIN_WINDOW_SIZE));
                }
                else
                {
                    // Fill up the tokens count textbox
                    textBoxTokenCount.Text = tokens.Length.ToString();

                    // Dynamically set the maximum, the minimum, and the increment values
                    numericUpDownWindowSize.Maximum = tokens.Length;
                    numericUpDownWindowSize.Increment
                             = (tokens.Length - MIN_WINDOW_SIZE) / NUMERIC_UP_DOWN_CLICKS;
                    numericUpDownWindowSize.Minimum = MIN_WINDOW_SIZE;
                }
            }
        }


        /// <summary>
        /// Tokenizes a text into an array of tokens, using the improved tokenizer with the
        /// discard-digit option.
        /// </summary>
        /// <param name="text">the text to tokenize</param>
        private static string[] Tokenize(string text)
        {
            // Discard digits
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

                // Change token only when it starts and/or ends with "'" and it has at least 2 characters.
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
        /// Computes the average type token ratio of an array of tokens, based on the window size.
        /// Algorithm:
        /// If windowSize &gt;= tokens.Length, average type token ratio is the general type token
        /// ratio of all tokens.
        /// </summary>
        /// <param name="tokens">the array of the tokens to calculate the average type token ratio</param>
        /// <param name="windowSize">the number of the tokens per window</param>
        private static double GetAverageTypeTokenRatio(string[] tokens, int windowSize)
        {
            LinkedList<string>
            movingWindow = new LinkedList<string>();

            int index = 0;
            while (index < windowSize)
            {
                movingWindow.AddLast(tokens[index]);
                index++;
            }

            // Build frequency table of this window of tokens
            Dictionary<string, int>
            movingFreqTable = BuildFreqTable(movingWindow);

            // This type token ratio keeps changing
            double finalTTR = (double)movingFreqTable.Count / movingWindow.Count;

            int windowCount = 1;

            // Now index stops at windowSize position of the tokens.
            while (index < tokens.Length)
            {
                // Check the first token of the moving window of tokens and remove it from the moving window.
                string firstToken = movingWindow.First.Value;
                movingWindow.RemoveFirst();

                // Check its frequency in the frequency table. If it is 1, it means that this token
                // occurs in the moving window only once, so we can safely remove it from the moving
                // window; otherwise, it appears more than once, so we cannot delete it but we can
                // reduce its frequency by 1.
                if (movingFreqTable[firstToken] == 1)
                    movingFreqTable.Remove(firstToken);
                else
                    movingFreqTable[firstToken]--;

                // Find the next available token. If it is in the moving frequency table, increase its
                // frequency value by 1; otherwise, add it as a new entry and set its frequency to 1.
                string newToken = tokens[index];

                if (movingFreqTable.ContainsKey(newToken))
                    movingFreqTable[newToken]++;
                else
                    movingFreqTable.Add(newToken, 1);

                // Add this word to the moving window so that the window always has the same number of tokens.
                movingWindow.AddLast(newToken);

                // Re-compute the type token ratio of this changed window.
                double thisTTR = (double)movingFreqTable.Count / windowSize;

                // Add this new type token ratio to the final type token ratio.
                finalTTR += thisTTR;

                // Update index position and window counters
                index++;
                windowCount++;
            }

            // We need to divided the final type token ratio by the number of windows
            finalTTR = finalTTR / windowCount;

            return finalTTR;
        }

        /// <summary>
        /// Compute the general type token ratio of an array of tokens.
        /// </summary>
        /// <param name="tokens">the array of tokens whose type and type token ratio values are computed </param>
        /// <param name="types">the HashSet to be updated</param>
        /// <param name="typeTokenRatio">the type token ratio to be updated</param>
        private static void GetGeneralTypeTokenRatio(string[] tokens, out HashSet<string> types, out double typeTokenRatio)
        {
            // Dump array of tokens into a HashSet of string. By definition, HashSet has no duplicates,
            // which is the what type means.
            types = new HashSet<string>();

            foreach (string token in tokens)
                types.Add(token);

            // A sanity check: if types set is empty, set typeTokenRatio = double.NaN,
            // i.e. Not a Number; otherwise, we'll get a "divided by 0" Exception.
            if (types.Count == 0)
            {
                typeTokenRatio = double.NaN;
            }
            else
            {
                // Be very aware that you need to cast either types.Count or tokens.Length
                // into double type; otherwise you'll always get 0 as the result.
                typeTokenRatio = (double)types.Count / tokens.Length;
            }

            // After the completion of this method, a new value of types and typeTokenRatio
            // will be sent back to the caller, which is what 'out parameter' means.
        }


        /// <summary>
        /// Create a string-integer dictionary out of a linked list of tokens.
        /// </summary>
        /// <param name="tokens"> the tokens to create the frequency table. For this
        /// method, there is no difference between List and LinkedList types. </param>
        private static Dictionary<string, int> BuildFreqTable(LinkedList<string> tokens)
        {
            Dictionary<string, int> token_freq_table = new Dictionary<string, int>();
            foreach (string token in tokens)
            {
                if (token_freq_table.ContainsKey(token))
                    token_freq_table[token]++;
                else
                    token_freq_table.Add(token, 1);
            }

            return token_freq_table;
        }

        #endregion


        #region event-handlers

        // This event handler is invoked after the Select File button is clicked.
        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            // Programmatically create an open file dialog and make it open *.txt files only.
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Text files|*.txt";

            // If OK button of the open file dialog is clicked, put the text of
            // the selected file in the input box
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                // This deals with the exception caused by files that have
                // .txt extensions but are actually not text files.
                try
                {
                    textBoxInput.Text = File.ReadAllText(openDlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                // Set numeric up down control
                SetNumericUpDownControl();
            }
        }

        // This event handler is invoked after user's mouse leaves the input textbox.
        private void textBoxInput_MouseLeave(object sender, EventArgs e)
        {
            // Set numeric up down control
            SetNumericUpDownControl();
        }


        private void btnGo_Click(object sender, EventArgs e)
        {
            // Make sure some text has been put in the input textbox.
            if (tokens != null && tokens.Length > 0)
            {
                // Compute general type token ratio
                GetGeneralTypeTokenRatio(tokens, out types, out generalTypeTokenRatio);

                textBoxTypeCount.Text = types.Count.ToString();
                textBoxTTR.Text = string.Format("{0:0.###}", generalTypeTokenRatio);

                // Compute average type token ratio.
                // Note that the Value of a NumericUpDown control is of decimal type
                // so we need to convert it to int type to fit the called method.
                double averageTypeTokenRatio
                    = GetAverageTypeTokenRatio(tokens, decimal.ToInt32(numericUpDownWindowSize.Value));

                textBoxAverageTTR.Text = string.Format("{0:0.###}", averageTypeTokenRatio);
            }
        }

        #endregion
    }
}