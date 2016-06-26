using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using iTextSharp.text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Data.OleDb;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public OleDbConnection conn;
        public string link = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Kamus.accdb; Persist Security Info=False;";
        public string sql;
        public string[] list;
        private void Form3_Load(object sender, EventArgs e)
        {
            //AllocConsole();
        }
        //[DllImport("kernel32.dll", SetLastError = true)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //static extern bool AllocConsole();
        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Char chr = textBox1.Text[0];
            string[] word = textBox1.Text.Split('.');
            
            foreach (string item in word)
            {
                listBox1.Items.Add(item);
            }
            sql = "SELECT List FROM StopWord_List";
            conn = new OleDbConnection(link);
            stoplist(sql);
            conn.Open();
            OleDbCommand com = new OleDbCommand(sql, conn);
            OleDbDataReader reader = com.ExecuteReader();
            //int z = word.Length;
            string[] temp= new string[listBox1.Items.Count];
            string regexCode = string.Format(@"\s?\b(?:{0})\b\s?", string.Join("|", list));
            
            Regex regex = new Regex(regexCode, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            Regex removeDoubleSpace = new Regex(@"\s{2,}", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            for (int i = 0;i < listBox1.Items.Count; i++)
            {
                temp[i] = regex.Replace(listBox1.Items[i].ToString(), " ");
                temp[i] = removeDoubleSpace.Replace(temp[i].ToString(), " ");
                listBox2.Items.Add(temp[i]);
            }
            
            //temp = removeDoubleSpace.Replace(cleaned, " ");


            conn.Close();
        }
        public int jlhlist(string query)
        {
            conn = new OleDbConnection(link);
            conn.Open();
            OleDbCommand comm = conn.CreateCommand();
            sql = query;
            comm.CommandText = sql;
            int jlh = (Int32)comm.ExecuteScalar();
            conn.Close();
            return jlh;
        }
        public void stoplist(string sql)
        {
            conn = new OleDbConnection(link);
            conn.Open();
            OleDbCommand comm = conn.CreateCommand();
            sql = "SELECT List FROM StopWord_List";
            comm.CommandText = sql;
            OleDbDataReader reader = comm.ExecuteReader();
            int jlh = jlhlist("SELECT COUNT (List) FROM StopWord_List");
            list = new String[jlh];
            int i = 0;
            while (reader.Read())
            {
                list[i] = reader.GetString(0).ToString();
                i++;
            }
            conn.Close();
        }
        public string[] get()
        {
            string[] arr = new string[listBox1.Items.Count];
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                arr[i] = listBox1.Items[i].ToString();
            }
            return arr;
        }
        private void button2_Click(object sender, EventArgs e)
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
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        
        
    }
}
