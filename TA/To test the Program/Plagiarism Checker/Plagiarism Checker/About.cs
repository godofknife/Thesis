using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Plagiarism_Checker
{
    public partial class About : MetroFramework.Forms.MetroForm
    {
        public About()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            groupBox1.Show();
            groupBox2.Hide();
            textBox1.Hide();
            textBox1.Clear();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            groupBox1.Hide();
            groupBox2.Hide();
            textBox1.Show();
            textBox1.Clear();
            textBox1.Text = Properties.Resources.dokumentasi;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            groupBox2.Show();
            groupBox1.Hide();
            textBox1.Hide();
            


        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            groupBox1.Hide();
            groupBox2.Hide();
            textBox1.Show();
            textBox1.Text = Properties.Resources.info1;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
