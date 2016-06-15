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

namespace Plagiatrism_checker
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

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
                
                //Console.WriteLine(item.ToString());
                //MessageBox.Show(item.ToString());
                //richTextBox1.Text=(ret.ToString());
                listBox1.Items.Add(item);
            }

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

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Char chr = listBox1.Text[0];
            string[] word = textBox1.Text.Split(' ');
            foreach (string item in word)
            {

                //Console.WriteLine(item.ToString());
                //MessageBox.Show(item.ToString());
                //richTextBox1.Text=(ret.ToString());
                
                
            }
        }
}
    }
