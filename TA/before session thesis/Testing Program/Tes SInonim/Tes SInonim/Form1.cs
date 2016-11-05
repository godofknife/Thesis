using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;


namespace Tes_SInonim
{
    public partial class Form1 : Form
    {
        public OleDbConnection connection= new OleDbConnection();
       
        public Form1()
        {
            InitializeComponent();
            connection.ConnectionString= @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Kamus.accdb; Persist Security Info=False;";
        }

        private void button1_Click(object sender, EventArgs e)
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
                    //textBoxInput.Text = File.ReadAllText(openDlg.FileName);
                    PdfReader read = new PdfReader(filePath);
                    for (int page = 1; page <= read.NumberOfPages; page++)
                    {
                        ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();
                        String s = PdfTextExtractor.GetTextFromPage(read, page, its);

                        s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
                        strText = strText + s;
                        richTextBox1.Text = strText;
                    }
                    read.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
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
                    //textBoxInput.Text = File.ReadAllText(openDlg.FileName);
                    PdfReader read = new PdfReader(filePath);
                    for (int page = 1; page <= read.NumberOfPages; page++)
                    {
                        ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();
                        String s = PdfTextExtractor.GetTextFromPage(read, page, its);

                        s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
                        strText = strText + s;
                        richTextBox2.Text = strText;
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
            string[] array = richTextBox1.Text.Split('.');
            string[] array1 = richTextBox2.Text.Split('.');
            for (int i = 0; i < array.Length-1; i++)
            {
                string[] word = array[i].ToString().Split(' ');
                foreach(string j in word)
                {
                    listBox1.Items.Add(j);
                }    
            }
            for (int k = 0; k < array1.Length - 1; k++)
            {
                string[] word = array1[k].ToString().Split(' ');
                foreach (string j in word)
                {
                    listBox2.Items.Add(j);
                }
            }
            OleDbCommand comm;
            foreach (string i in listBox1.Items)
            {
                connection.Open();
                //string sql = "Select * FROM Kamus_Tesaurus WHERE [Kata_u] == @1";
                comm = new OleDbCommand("Select Kamus_Tesaurus.Kata_u from Kamus_Tesaurus where Kata_u = '" + i + "'", connection);
                //comm.Parameters.AddWithValue("@1",i.ToLower().Trim());
                //string kata = (string)comm.ExecuteScalar();
                //label1.Text = kata;

                foreach (string j in listBox2.Items)
                {
                    comm = new OleDbCommand("Select * from Kamus_Tesaurus where Kata_u = '" + j + "'", connection);
                    string kata = (string)comm.ExecuteScalar();
                    label1.Text = kata;
                    break;
                }


                connection.Close();

            }
           
        }
    }
}
