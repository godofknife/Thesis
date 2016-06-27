//Written By Ian McCullough
//03/19/2013

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        String Dictionary, MisspelledString;
        Dictionary<int, string> dictionary;
        Dictionary<KeyValuePair<String, int>, List<int>> results;
        public Form1()
        {
            InitializeComponent();
            suggestionbox.MouseClick += new MouseEventHandler(suggestionbox_MouseClick);
        }

        void suggestionbox_MouseClick(object sender, MouseEventArgs e)
        {
               
                String theString = suggestionbox.SelectedItem.ToString();
                string[] t = theString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (!theString.Contains("\t"))
                {
                    richTextBox1.Select(Convert.ToInt32(t[1]), t[0].Length);
                    richTextBox1.Focus();
                }
                
           // MessageBox.Show();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            checkButton.Enabled = true;
            DictionaryFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (DictionaryFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(DictionaryFileDialog.FileName);
                //MessageBox.Show(sr.ReadToEnd());
                Dictionary = sr.ReadToEnd();
                sr.Close();
                DictionaryTextBox.Text = DictionaryFileDialog.FileName;
                CreateDictionary(Dictionary);
            }
        }
        private void CreateDictionary(String ASCII)
        {
            string[] t = ASCII.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            dictionary = new Dictionary<int, string>();
            for (int i = 0; i < t.Length; i++)
            {
                t[i] = stripSymbolsFromString(t[i]);

                if (!dictionary.ContainsValue(t[i]) && t[i] != "")
                {
                    dictionary.Add(i, t[i]);
                    listView1.Items.Add(new ListViewItem(t[i]));
                }

            }
        }

        private Dictionary<int, String> makeDictionary(String ASCII)
        {
            Dictionary<int, String> words = new Dictionary<int, String>();
            MatchCollection matches = Regex.Matches(ASCII, "[^( !|\"|#|$|%|&|'|*|+|,|.|/|:|;|<|=|>|?|@|\\|^|_|`|{|}|~|-|\r|\n)]+");
            foreach (Match m in matches)
            {
                words.Add(m.Index, m.Value);
            }
            return words;

            // MessageBox.Show(printDictionary(dictionary));
        }

        private String printDictionary(Dictionary<int, string> d)
        {
            String built = "";
            foreach (KeyValuePair<int, String> entry in d)
            {
                built += entry.Key + ": " + entry.Value + Environment.NewLine;
            }
            return built;
        }
        private String printDictionary(Dictionary<String, List<int> > d)
        {
            String built = "";
            foreach (KeyValuePair<String, List<int>> entry in d)
            {
                built += entry.Key + ": "; 
                for (int i=0; i< entry.Value.Count; i++)
                {
                    built += entry.Value.ElementAt(i)+" ";
                }
                built += Environment.NewLine;
            }
            return built;
        }

        private void checkButton_Click(object sender, EventArgs e)
        {
            SuggestingButton.Enabled = true;
            label2.Visible = true;
            openFileDialog2.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog2.FileName);
                //MessageBox.Show(sr.ReadToEnd());
                MisspelledString = sr.ReadToEnd();
                sr.Close();
                richTextBox1.Text = MisspelledString;
                textBox1.Text = openFileDialog2.FileName;
                //CreateDictionary(Dictionary);
                checkMisspelledWords();
            }
        }

        private void checkMisspelledWords()
        {
            results = new Dictionary<KeyValuePair<String, int>, List<int>>(); //keyvalue contains the word and its index, the list of ints is the possible dictionary keys
            List<int> dictionaryKeys = new List<int>();
            Dictionary<int, String> wordsToCheck = makeDictionary(richTextBox1.Text); //all words and all the indexes they're found at in the String
            foreach (KeyValuePair<int, String> word in wordsToCheck) //look through the the string to be checked
            {
                if ((!dictionary.ContainsValue(word.Value))) //if the word isn't currently in the dictionary
                {
                    dictionaryKeys = new List<int>();
                    foreach (KeyValuePair<int, String> dictionaryKey in dictionary) //look through the dictionary
                    {
                        if (LevenshteinDistance(word.Value, dictionaryKey.Value) <= 1)
                        {
                           // MessageBox.Show("The Word: " + word.Value + ", Dictionary:" + dictionaryKey.Value );
                            dictionaryKeys.Add(dictionaryKey.Key);
                        }
                    }
                    KeyValuePair<String, int> wordToCheckPair = new KeyValuePair<String, int>( word.Value, word.Key);
                    results.Add(wordToCheckPair, dictionaryKeys); //the results dictionary will have all words with possible fixes
                }             
            }
        }

        // <summary>
        // Calculates the Levenshtein distance between two strings--the number of changes that need to be made for the first string to become the second.
        // </summary>
       // <param name="first">The first string, used as a source.</param>
        // <param name="second">The second string, used as a target.</param>
        // <returns>The number of changes that need to be made to convert the first string to the second.</returns>
        // <remarks>
        // From http://www.merriampark.com/ldcsharp.htm
        // </remarks>
        public static int LevenshteinDistance(string first, string second)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }

            int n = first.Length;
            int m = second.Length;
            var d = new int[n + 1, m + 1]; // matrix

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            for (int i = 1; i <= n; i++)
            {

                for (int j = 1; j <= m; j++)
                {
                    int cost = (second.Substring(j - 1, 1) == first.Substring(i - 1, 1) ? 0 : 1); // cost
                    d[i, j] = Math.Min(
                        Math.Min(
                            d[i - 1, j] + 1,
                            d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }
        public String stripSymbolsFromString(String aString)
        {
            char[] arr = aString.ToCharArray();

            arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c) || c.Equals(".")))); //remove nonletters
            return new string(arr);

        }
        public Regex makeRegex(String[] words)
        {
            String allWords = "";
            foreach (String w in words)
            {
                allWords += w + "|";
            }

            return new Regex(allWords);
        }

        private void SuggetionButton_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<KeyValuePair<String, int>, List<int>> k in results)
            {
                richTextBox1.Select(k.Key.Value, k.Key.Key.Length);
                richTextBox1.SelectionColor = Color.Red;

                String theString = k.Key.Key+" "+k.Key.Value+" : "+Environment.NewLine;
                suggestionbox.Items.Add(theString);
                foreach (int index in k.Value)
                {
                    theString = "\t" + dictionary[index] + Environment.NewLine;
                    suggestionbox.Items.Add(theString);
                }
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            listView1.Items.Add(addWordTextbox.Text);
        }
    }
}
