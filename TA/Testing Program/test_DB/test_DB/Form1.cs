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
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace test_DB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        public OleDbConnection conn;
        public string link= @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Kamus.accdb; Persist Security Info=False;";
        public string[] kalimat;
        private void Form1_Load(object sender, EventArgs e)
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
            kalimat = richTextBox1.Text.Split('.');
            foreach(string i in kalimat)
            {
                listBox1.Items.Add(i);
            }
            string[] kata;
            
            for (int i=0; i < kalimat.Length - 1; i++)
            {
                kata = listBox1.Items[i].ToString().ToLower().Split(' ');
                for(int j=0; j<kata.Length; j++)
                {
                    listBox2.Items.Add(kata[j]);
                }
            }
            conn = new OleDbConnection();
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Kamus.accdb; Persist Security Info=False;";
            conn.Open();
            //OleDbCommand cmd = conn.CreateCommand();
            OleDbDataAdapter da;
            DataSet ds = new DataSet() ;
            string sql = "SELECT * FROM Kamus_Tesaurus Where Kata_u = '" + listBox2.Items[0] + "'";
            //cmd.CommandText = sql;
            //OleDbDataReader reader = cmd.ExecuteReader();
            //string[] temp = new string[0];
            da = new OleDbDataAdapter(sql, conn);
            da.Fill(ds, "Test");
            dataGridView1.DataSource = ds.Tables["Test"];
            bool temu = false;
            int pos = 0;
            while (pos < dataGridView1.Columns.Count & !temu)
            {
                if ((string)dataGridView1[pos, 0].Value.ToString().Trim() == "kemajuan")
                {
                    temu = true;
                    label1.Text = "ketemu";
                }
                else
                    pos++;
            }


            //Array.Resize(ref temp, k + 1);
            //temp[k] = reader.GetString(0).ToString();
            //if (temp[k] == "Kemajuan")
            //    label1.Text = "true";
            conn.Close();

        }
    }
}
