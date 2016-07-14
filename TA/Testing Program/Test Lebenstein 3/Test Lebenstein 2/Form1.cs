using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text.RegularExpressions;

namespace Test_Lebenstein_2
{
    public partial class Form1 : Form
    {
        String Dictionary;
        Dictionary<int, string> dictionary;
        Dictionary<KeyValuePair<String, int>, List<int>> results;
        public Form1()
        {
            InitializeComponent();
        }
        private void DictionaryButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog1.FileName);
                //MessageBox.Show(sr.ReadToEnd());
                Dictionary = sr.ReadToEnd();
                sr.Close();
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
            Dictionary<int, String> wordsToCheck = makeDictionary(richTextBox2.Text); //all words and all the indexes they're found at in the String
            foreach (KeyValuePair<int, String> word in wordsToCheck) //look through the the string to be checked
            {
                if ((!dictionary.ContainsValue(word.Value))) //if the word isn't currently in the dictionary
                {
                    dictionaryKeys = new List<int>();
                    foreach (KeyValuePair<int, String> dictionaryKey in dictionary) //look through the dictionary
                    {
                        if (LevenshteinDistance.Compute(word.Value, dictionaryKey.Value) <= 1)
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
      
        public String stripSymbolsFromString(String aString)
        {
            char[] arr = aString.ToCharArray();

            arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c) || c.Equals(".")))); //remove nonletters
            return new string(arr);

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {


        }
        static class LevenshteinDistance
        {
            /// <summary>
            /// Compute the distance between two strings.
            /// </summary>
            public static int Compute(string s, string t)
            {
                int n = s.Length;
                int m = t.Length;
                int[,] d = new int[n + 1, m + 1];

                // Step 1
                if (n == 0)
                {
                    return m;
                }

                if (m == 0)
                {
                    return n;
                }

                // Step 2
                for (int i = 0; i <= n; d[i, 0] = i++)
                {
                }

                for (int j = 0; j <= m; d[0, j] = j++)
                {
                }

                // Step 3
                for (int i = 1; i <= n; i++)
                {
                    //Step 4
                    for (int j = 1; j <= m; j++)
                    {
                        // Step 5
                        int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                        // Step 6
                        d[i, j] = Math.Min(
                            Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                            d[i - 1, j - 1] + cost);
                    }
                }
                // Step 7
                return d[n, m];
            }
        }
 

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog1.FileName);
                //MessageBox.Show(sr.ReadToEnd());
                Dictionary = sr.ReadToEnd();
                sr.Close();

                CreateDictionary(Dictionary);
            }
            //OpenFileDialog openDlg = new OpenFileDialog();
            //openDlg.Filter = "PDF Files(*.PDF)|*.PDF|ALL Files(*.*)|*.*";

            //if (openDlg.ShowDialog() == DialogResult.OK)
            //{

            //    string filePath = openDlg.FileName.ToString();
            //    filePath = openDlg.FileName.ToString();
            //    string strText = string.Empty;
            //    try
            //    {

            //        PdfReader read = new PdfReader(filePath);
            //        for (int page = 1; page <= read.NumberOfPages; page++)
            //        {
            //            ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();
            //            String s = PdfTextExtractor.GetTextFromPage(read, page, its);

            //            s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
            //            strText = strText + s;
            //            richTextBox1.Text = strText;

            //        }
            //        read.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "PDF Files(*.PDF)|*.PDF|ALL Files(*.*)|*.*";

            if (openDlg.ShowDialog() == DialogResult.OK)
            {

                string filePath = openDlg.FileName.ToString();
                filePath = openDlg.FileName.ToString();
                string strText = string.Empty;
                try
                {

                    PdfReader read = new PdfReader(filePath);
                    for (int page = 1; page <= read.NumberOfPages; page++)
                    {
                        ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();
                        String s = PdfTextExtractor.GetTextFromPage(read, page, its);

                        s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
                        strText = strText + s;
                        richTextBox2.Text = strText;
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
      

        private void button3_Click(object sender, EventArgs e)
        {
            //string[] array = richTextBox1.Text.Split('.');
            string[] array1 = richTextBox2.Text.Split('.');
            //foreach (string j in array)
            //{
            //    listBox1.Items.Add(j);
            //}
            foreach (string j in array1)
            {
                listBox2.Items.Add(j);
            }
            for (int intCount = 0; intCount < listView1.Items.Count; intCount++)
            {
                listBox1.Items.Add(listView1.Items[intCount].Text);
                //lvEmpDetails.SelectedItems[intCount].Remove();
                ////Every time remove item, reduce the index           
                //intCount--;
            }
            foreach (string cek in listBox1.Items)
            {
                int cost=LevenshteinDistance.Compute(cek, listBox2.Items.ToString());
                listBox3.Items.Add(cost.ToString());
            }
            foreach (string cek1 in listBox2.Items)
            {
                int cost = LevenshteinDistance.Compute(cek1, listBox1.Items.ToString());
                listBox4.Items.Add(cost.ToString());
            }
            foreach (KeyValuePair<KeyValuePair<String, int>, List<int>> k in results)
            {
                richTextBox2.Select(k.Key.Value, k.Key.Key.Length);
                richTextBox2.SelectionColor = Color.Red;

                String theString = k.Key.Key + " " + k.Key.Value + " : " + Environment.NewLine;
  
                foreach (int index in k.Value)
                {
                    theString = "\t" + dictionary[index] + Environment.NewLine;

                }
            }


            //foreach (string[] a in l)
            //{
            //    int cost = LevenshteinDistance.Compute(a[0], a[1]);

            //    //Console.WriteLine("{0} -> {1} = {2}",
            //    //    a[0],
            //    //    a[1],
            //    //    cost);
            //}

        }
    }
}
