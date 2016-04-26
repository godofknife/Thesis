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

namespace Plagiatrism_checker
{
    public partial class Hasil : Form
    {
        private DataGridView dtg1 = new DataGridView();
        private LinkLabel lb1 = new LinkLabel();
        public Hasil()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            lb1.Text = "Details";
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].Name = "Nama File";
            dataGridView1.Columns[1].Name = "Ukuran";
            dataGridView1.Columns[2].Name = "Proses";
            dataGridView1.Columns[3].Name = "Ratio Kemiripan";
            dataGridView1.Columns[3].Name = "Info";
            dataGridView1.Rows[0].Cells[4].Value = lb1.Text;
            this.Controls.Add(dataGridView1);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Form1 frm1 = new Form1();
            this.Hide();
            frm1.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Home hm = new Home();
            this.Close();
            hm.Show();
        }

    }
}
