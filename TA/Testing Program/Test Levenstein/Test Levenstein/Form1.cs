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
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using iTextSharp.text;

namespace Test_Levenstein
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
        }

        private void DictionaryButton_Click(object sender, EventArgs e)
        {
            checkButton.Enabled = true;
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog1.FileName);
                //MessageBox.Show(sr.ReadToEnd());
                Dictionary = sr.ReadToEnd();
                sr.Close();
                DictionaryTextBox.Text = openFileDialog1.FileName;
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

        private void checkButton_Click(object sender, EventArgs e)
        {
            SuggestingButton.Enabled = true;
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "PDF Files(*.PDF)|*.PDF|ALL Files(*.*)|*.*";

            if (openDlg.ShowDialog() == DialogResult.OK)
            {
          
                string filePath = openDlg.FileName.ToString();
                filePath = openDlg.FileName.ToString();
                string strText = string.Empty;
                try
                {
                    //textBoxInput.Text = File.ReadAllText(openDlg.FileName);
                    PdfReader read = new PdfReader(filePath);
                    for (int page = 1; page <= read.NumberOfPages; page++)
                    {
                        ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();
                        String s = PdfTextExtractor.GetTextFromPage(read, page, its);

                        s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
                        strText = strText + s;
                        richTextBox1.Text = strText;
                        //MisspelledString = read.ReadToEnd();
                        //richTextBox1.Text = MisspelledString;
                        textBox1.Text = openDlg.FileName;
                        checkMisspelledWords();
                    }
                    read.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
            
               
                
            
        }

        private String printDictionary(Dictionary<String, List<int>> d)
        {
            String built = "";
            foreach (KeyValuePair<String, List<int>> entry in d)
            {
                built += entry.Key + ": ";
                for (int i = 0; i < entry.Value.Count; i++)
                {
                    built += entry.Value.ElementAt(i) + " ";
                }
                built += Environment.NewLine;
            }
            return built;
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
                    KeyValuePair<String, int> wordToCheckPair = new KeyValuePair<String, int>(word.Value, word.Key);
                    results.Add(wordToCheckPair, dictionaryKeys); //the results dictionary will have all words with possible fixes
                }
            }
        }
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

        private void SuggestingButton_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<KeyValuePair<String, int>, List<int>> k in results)
            {
                richTextBox1.Select(k.Key.Value, k.Key.Key.Length);
                richTextBox1.SelectionColor = Color.Red;

                String theString = k.Key.Key + " " + k.Key.Value + " : " + Environment.NewLine;
                suggestionbox.Items.Add(theString);
                foreach (int index in k.Value)
                {
                    theString = "\t" + dictionary[index] + Environment.NewLine;
                    suggestionbox.Items.Add(theString);
                }
            }
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
    }
}
