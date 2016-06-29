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
                richTextBox1.Enabled = false;
                richTextBox2.Enabled = false;
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
        
        
    }
}