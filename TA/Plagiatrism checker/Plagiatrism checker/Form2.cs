using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using iTextSharp.text;
using System.Data.OleDb;

namespace Plagiatrism_checker
{
    public partial class Form2 : Form
    {
        #region Form-wide variables
        private OleDbConnection connection = new OleDbConnection();

        const int MIN_WINDOW_SIZE = 100;

  
        const int NUMERIC_UP_DOWN_CLICKS = 20;

        string[] tokens;
        HashSet<string>
        types;

        double generalTypeTokenRatio;

        #endregion
        
        public Form2()
        {
            InitializeComponent();
            connection.ConnectionString=@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\katadasar.accdb;Persist Security Info=False;";
        }
        #region Methods

        private void SetNumericUpDownControl()
        {

            string text = textBoxInput.Text.Trim();
            if (text != string.Empty)
            {
                tokens = Tokenize(text);
                if (tokens.Length < MIN_WINDOW_SIZE)
                {
                    MessageBox.Show(
                         string.Format("Too few tokens. Try text with more than {0} tokens.", MIN_WINDOW_SIZE));
                }
                else
                {
                    textBoxTokenCount.Text = tokens.Length.ToString();
                    numericUpDownWindowSize.Maximum = tokens.Length;
                    numericUpDownWindowSize.Increment
                             = (tokens.Length - MIN_WINDOW_SIZE) / NUMERIC_UP_DOWN_CLICKS;
                    numericUpDownWindowSize.Minimum = MIN_WINDOW_SIZE;
                }
            }
        }
        private void SetNumericUpDownControl2()
        {

            string text = textBox1.Text.Trim();
            if (text != string.Empty)
            {
                tokens = Tokenize(text);
                if (tokens.Length < MIN_WINDOW_SIZE)
                {
                    MessageBox.Show(
                         string.Format("Too few tokens. Try text with more than {0} tokens.", MIN_WINDOW_SIZE));
                }
                else
                {
                    textBox3.Text = tokens.Length.ToString();
                    numericUpDownWindowSize.Maximum = tokens.Length;
                    numericUpDownWindowSize.Increment
                             = (tokens.Length - MIN_WINDOW_SIZE) / NUMERIC_UP_DOWN_CLICKS;
                    numericUpDownWindowSize.Minimum = MIN_WINDOW_SIZE;
                }
            }
        }
        private static string[] Tokenize(string text)
        {
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
                if (token.Length > 1)
                {
                    if (token.StartsWith("'") && token.EndsWith("'"))
                        tokens[i] = token.Substring(1, token.Length - 2);  

                    else if (token.StartsWith("'"))
                        tokens[i] = token.Substring(1);

                    else if (token.EndsWith("'"))
                        tokens[i] = token.Substring(0, token.Length - 1); 
                }
            }

            return tokens;
        }
        private Dictionary<string, int> ToStrIntDict(string[] words)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

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

            return dict;
        }
        private Dictionary<string, int> ListWordsByFreq(Dictionary<string, int> strIntDict, SortOrder sortOrder)
        {
            string[] words = new string[strIntDict.Keys.Count];
            strIntDict.Keys.CopyTo(words, 0);

            int[] freqs = new int[strIntDict.Values.Count];
            strIntDict.Values.CopyTo(freqs, 0);
            Array.Sort(freqs, words);
            if (sortOrder == SortOrder.Descending)
            {
                Array.Reverse(freqs);
                Array.Reverse(words);
            }

            Dictionary<string, int> dictByFreq = new Dictionary<string, int>();

            for (int i = 0; i < freqs.Length; i++)
            {
                dictByFreq.Add(words[i], freqs[i]);
            }

            return dictByFreq;
        }
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

           
            Dictionary<string, int>
            movingFreqTable = BuildFreqTable(movingWindow);

 
            double finalTTR = (double)movingFreqTable.Count / movingWindow.Count;

            int windowCount = 1;


            while (index < tokens.Length)
            {
  
                string firstToken = movingWindow.First.Value;
                movingWindow.RemoveFirst();

                if (movingFreqTable[firstToken] == 1)
                    movingFreqTable.Remove(firstToken);
                else
                    movingFreqTable[firstToken]--;


                string newToken = tokens[index];

                if (movingFreqTable.ContainsKey(newToken))
                    movingFreqTable[newToken]++;
                else
                    movingFreqTable.Add(newToken, 1);

       
                movingWindow.AddLast(newToken);


                double thisTTR = (double)movingFreqTable.Count / windowSize;

          
                finalTTR += thisTTR;


                index++;
                windowCount++;
            }

  
            finalTTR = finalTTR / windowCount;

            return finalTTR;
        }
        private static void GetGeneralTypeTokenRatio(string[] tokens, out HashSet<string> types, out double typeTokenRatio)
        {
            types = new HashSet<string>();

            foreach (string token in tokens)
                types.Add(token);

   
            if (types.Count == 0)
            {
                typeTokenRatio = double.NaN;
            }
            else
            {

                typeTokenRatio = (double)types.Count / tokens.Length;
            }

        }
   
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

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                label10.Text = "Connection Successful";
                connection.Close();
            }
            catch(Exception)
            {
                label10.Text = "Connection Not Established";
            }
            connection.Close();
        }
        private bool buttonclick = false;
        private void btnGo_Click(object sender, EventArgs e)
        {
            
            if (tokens != null && tokens.Length > 0 && buttonclick==false)
            {
                buttonclick = true;
                GetGeneralTypeTokenRatio(tokens, out types, out generalTypeTokenRatio);
                textBoxTypeCount.Text = types.Count.ToString();
                textBoxTTR.Text = string.Format("{0:0.###}", generalTypeTokenRatio);

      
                double averageTypeTokenRatio
                    = GetAverageTypeTokenRatio(tokens, decimal.ToInt32(numericUpDownWindowSize.Value));

                textBoxAverageTTR.Text = string.Format("{0:0.###}", averageTypeTokenRatio);

            }
            if (tokens != null && tokens.Length > 0 && buttonclick == true)
            {
                GetGeneralTypeTokenRatio(tokens, out types, out generalTypeTokenRatio);

                textBox4.Text = types.Count.ToString();


                textBox6.Text = string.Format("{0:0.###}", generalTypeTokenRatio);
                double averageTypeTokenRatio
                    = GetAverageTypeTokenRatio(tokens, decimal.ToInt32(numericUpDownWindowSize.Value));

                textBox5.Text = string.Format("{0:0.###}", averageTypeTokenRatio);
            }
            if (textBoxInput.Text != string.Empty)
            {

                string[] words = Tokenize(textBoxInput.Text);

                if (words.Length > 0)
                {
  
                    Dictionary<string, int> dict = ToStrIntDict(words);


                    SortOrder sortOrder = radioButtonAscending.Checked ? SortOrder.Ascending : SortOrder.Descending;


                    dict = ListWordsByFreq(dict, sortOrder);

     
                    StringBuilder resultSb = new StringBuilder(dict.Count * 9);
                    foreach (KeyValuePair<string, int> entry in dict)
                        resultSb.AppendLine(string.Format("{0} [{1}]", entry.Key, entry.Value));

                    
                    textBoxOutput.Text = resultSb.ToString();
                    //string[] arrteammembers = new string[] { textBoxInput.Text };
                    //foreach (var value in arrteammembers)
                    //{
                    //    bool endsInPeriod = value.EndsWith(".");
                    //    Console.WriteLine("'{0}' ends in a period: {1}",
                    //                      value, endsInPeriod);
                    //}
                    string stringtocheck = ".";
                    string[] tobechecked = new string[] { textBoxInput.Text };
                    foreach(string x in tobechecked)
                    {
                        if(x.Contains(stringtocheck))
                        {
                            textBox7.Text = "True";
                        }
                    }
            
                    
                }
            }
            
            if (textBox1.Text != string.Empty)
            {

                string[] words = Tokenize(textBox1.Text);

                if (words.Length > 0)
                {

                    Dictionary<string, int> dict = ToStrIntDict(words);


                    SortOrder sortOrder = radioButtonAscending.Checked ? SortOrder.Ascending : SortOrder.Descending;


                    dict = ListWordsByFreq(dict, sortOrder);


                    StringBuilder resultSb = new StringBuilder(dict.Count * 9);
                    foreach (KeyValuePair<string, int> entry in dict)
                        resultSb.AppendLine(string.Format("{0} [{1}]", entry.Key, entry.Value));


                    textBox2.Text = resultSb.ToString();

                    string[] arrteammembers = new string[] { textBox1.Text };
                }
            }
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
   
            OpenFileDialog openDlg = new OpenFileDialog();
            string filePath;
            openDlg.Filter = "PDF Files(*.PDF)|*.PDF|ALL Files(*.*)|*.*";

            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                filePath =openDlg.FileName.ToString();
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
                        textBoxInput.Text = strText;
                    }
                    read.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            SetNumericUpDownControl();
            }
        }
        private void textBoxInput_MouseLeave(object sender, EventArgs e)
        {

            SetNumericUpDownControl();
        }
        #endregion

        private void textBoxOutput_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            string filePath;
            openDlg.Filter = "PDF Files(*.PDF)|*.PDF|ALL Files(*.*)|*.*";

            if (openDlg.ShowDialog() == DialogResult.OK)
            {
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
                        textBox1.Text = strText;
                    }
                    read.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                SetNumericUpDownControl2();
            }
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            SetNumericUpDownControl2();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
