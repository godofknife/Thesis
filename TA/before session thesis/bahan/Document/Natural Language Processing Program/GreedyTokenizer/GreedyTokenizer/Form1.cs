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

namespace GreedyTokenizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            string[] tokens = GreedyTokenize(text);

            foreach (var token in tokens)
            {
                textBox2.AppendText(token + "\r\n");
            }
            
        }

        /// <summary>
        /// Tokenizes text into an array of words, using whitespace and
        /// all punctuation as delimiters.
        /// </summary>
        /// <param name="text">the text to tokenize</param>
        /// <returns>an array of resulted tokens</returns>
        public static string[] GreedyTokenize(string text)
        {
            char[] delimiters = new char[] {
            '{', '}', '(', ')', '[', ']', '>', '<','-', '_', '=', '+',
            '|', '\\', ':', ';', '"', '\'', ',', '.', '/', '?', '~', '!',
            '@', '#', '$', '%', '^', '&', '*', ' ', '\r', '\n', '\t'};

            return text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
