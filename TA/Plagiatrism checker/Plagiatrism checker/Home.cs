using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Plagiatrism_checker
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
            checkedListBox1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            progressBar1.Visible = false;
            label1.Visible = false;

            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PDF Files(*.pdf)|*.pdf;";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string path = ofd.FileName.ToString();
                textBox1.Text = path;
                textBox1.Enabled = false;
               
                checkedListBox1.Visible = true;
                button2.Visible = true;
                label1.Visible = true;
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog ofd = new OpenFileDialog();
            openFileDialog1.Multiselect = true;
            ofd.Filter = "PDF Files(*.pdf)|*.pdf;";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string path = ofd.FileName.ToString();
                checkedListBox1.Items.Add(path);
                checkedListBox1.Text = path;
                checkBox1.Visible = true;
                button2.Visible = true;
                ListBox lsb = new ListBox();
                CheckBox chk = new CheckBox();
                button3.Visible = true;
                
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }
            else
            {
                for(int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            Hasil frm = new Hasil();
            this.Hide();
            frm.Show();
        }
    }
}
            
               
                

                 
