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
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Data.OleDb;

namespace WindowsFormsApplication13
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        
        public Form1()
        {
           
            InitializeComponent();
            metroTextBox2.Hide();
            richTextBox2.Hide();
            metroTile2.Hide();
            metroProgressBar1.Hide();
            groupBox1.Hide();
            metroTile3.Hide();
        }
        public OleDbConnection conn;
        public string link = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Kamus.accdb; Persist Security Info=False;";
        public string sql;
        public string[] list;

        #region design
        private void materialCheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox3.Checked)
            {
                materialCheckBox1.Checked = true;
                materialCheckBox2.Checked = true;
            }
            if (materialCheckBox3.Checked == false)
            {
                materialCheckBox1.Checked = false;
                materialCheckBox2.Checked = false;
            }
        }
        private void materialCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            metroTile3.Show();
        }

        private void materialCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            metroTile3.Show();
        }
        private void metroTile1_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "PDF Files(*.PDF)|*.PDF|ALL Files(*.*)|*.*";

            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                metroTextBox2.Show();
                richTextBox2.Show();
                metroTile2.Show();
                string filePath = openDlg.FileName.ToString();
                filePath = openDlg.FileName.ToString();
                metroTextBox1.Text = filePath;
                metroTextBox1.Enabled = false;
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

        private void metroTile2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "PDF Files(*.PDF)|*.PDF|ALL Files(*.*)|*.*";

            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                groupBox1.Show();
                string filePath = openDlg.FileName.ToString();
                filePath = openDlg.FileName.ToString();
                metroTextBox2.Text = filePath;
                metroTextBox2.Enabled = false;
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

        private void metroTile3_Click(object sender, EventArgs e)
        {
            #region
            label3.Hide();
            listBox1.Show();
            listBox2.Show();
            listBox3.Show();
            listBox4.Show();
            listBox5.Show();
            listBox6.Show();
            listBox7.Show();
            listBox8.Show();
            listBox9.Show();
            listBox10.Show();
            materialLabel1.Show();
            materialLabel4.Show();
            materialLabel2.Show();
            materialLabel3.Show();
            materialLabel5.Show();
            
            #endregion
            this.timer1.Start();
            metroProgressBar1.Show();
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            Char chr = richTextBox1.Text[0];
            string[] word = richTextBox1.Text.Split('.');
            string[] word1 = richTextBox2.Text.Split('.');
            foreach(string item in word)
            {
                listBox1.Items.Add(item);
                
            }
            foreach(string item in word1)
            {
                listBox8.Items.Add(item);
            }
            sql = "SELECT List FROM StopWord_List";
            conn = new OleDbConnection(link);
            stoplist(sql);
            conn.Open();
            OleDbCommand com = new OleDbCommand(sql, conn);
            OleDbDataReader reader = com.ExecuteReader();
            //int z = word.Length;
            string[] temp = new string[listBox1.Items.Count];
            string regexCode = string.Format(@"\s?\b(?:{0})\b\s?", string.Join("|", list));

            Regex regex = new Regex(regexCode, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            Regex removeDoubleSpace = new Regex(@"\s{2,}", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                temp[i] = regex.Replace(listBox1.Items[i].ToString(), " ");
                temp[i] = removeDoubleSpace.Replace(temp[i].ToString(), " ");
                listBox3.Items.Add(temp[i]);
            }
            for (int i = 0; i < listBox8.Items.Count; i++)
            {
                temp[i] = regex.Replace(listBox8.Items[i].ToString(), " ");
                temp[i] = removeDoubleSpace.Replace(temp[i].ToString(), " ");
                listBox6.Items.Add(temp[i]);
            }
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
        
        #endregion
        private void timer1_Tick(object sender, EventArgs e)
        {
            metroProgressBar1.Maximum = 1000;
            metroProgressBar1.PerformStep();
            if(metroProgressBar1.Value==1000)
            {
                TabPage t = tabControl1.TabPages[1];
                tabControl1.SelectedTab = t;
                label1.Hide();
            }
               
        }
        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.Image = WindowsFormsApplication13.Properties.Resources.info2;
        }
        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Image = WindowsFormsApplication13.Properties.Resources.info;
        }
        private void metroTile4_Click(object sender, EventArgs e)
        {
            TabPage t = tabControl1.TabPages[0];
            tabControl1.SelectedTab = t;
            metroTextBox1.Clear();
            metroTextBox2.Clear();
            richTextBox1.Clear();
            richTextBox2.Clear();
            metroProgressBar1.Hide();
            materialCheckBox1.Checked = false;
            materialCheckBox2.Checked = false;
            materialCheckBox3.Checked = false;
        }
        private void metroTile5_Click(object sender, EventArgs e)
        {
            
        }

        private void tabControl1_ParentChanged(object sender, EventArgs e)
        {
            listBox1.Hide();
            listBox2.Hide();
            listBox3.Hide();
            listBox4.Hide();
            listBox5.Hide();
            listBox6.Hide();
            listBox7.Hide();
            listBox8.Hide();
            listBox9.Hide();
            listBox10.Hide();
            materialLabel1.Hide();
            materialLabel2.Hide();
            materialLabel3.Hide();
            materialLabel4.Hide();
            materialLabel5.Hide();
        } 
        
        
    }
}