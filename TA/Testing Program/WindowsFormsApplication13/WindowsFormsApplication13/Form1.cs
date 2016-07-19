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
        string[] kamus;
        public string[] kalimat;

        #region design
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
            if (materialRadioButton2.Checked == true && materialRadioButton1.Checked == false)
            {
                //Tokenization
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
                listBox1.Text.ToLower();
                listBox2.Text.ToLower();
                Char chr = richTextBox1.Text[0];
                string[] word = richTextBox1.Text.Split('.');
                string[] word1 = richTextBox2.Text.Split('.');
                string[,] listkata = new string[0, 0];
                foreach (string item in word)
                {
                    listBox1.Items.Add(item);

                }
                foreach (string item in word1)
                {
                    listBox8.Items.Add(item);
                }

                //Proses Stopwords Disini
                sql = "SELECT List FROM StopWord_List";
                conn = new OleDbConnection(link);
                stoplist(sql);
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader reader = com.ExecuteReader();
                string[] temp = new string[listBox1.Items.Count];
                string regexCode = string.Format(@"\s?\b(?:{0})\b\s?", string.Join("|", list));

                Regex regex = new Regex(regexCode, RegexOptions.Singleline | RegexOptions.IgnoreCase);

                Regex removeDoubleSpace = new Regex(@"\s{2,}", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    temp[i] = regex.Replace(listBox1.Items[i].ToString(), " ");
                    temp[i] = removeDoubleSpace.Replace(temp[i].ToString(), " ");
                    listBox3.Items.Add(RemoveChars(temp[i]));
                }
                for (int i = 0; i < listBox8.Items.Count; i++)
                {
                    temp[i] = regex.Replace(listBox8.Items[i].ToString(), " ");
                    temp[i] = removeDoubleSpace.Replace(temp[i].ToString(), " ");
                    listBox6.Items.Add(RemoveChars(temp[i]));
                }
                conn.Close();
                //Proses Stemming Disini
                sql = "SELECT List FROM tb_rootword";
                conn = new OleDbConnection(link);

                string[] temp1 = new string[listBox3.Items.Count];
                listKamus();
                for (int i = 0; i < listBox3.Items.Count; i++)
                {

                    string[] kata = listBox3.Items[i].ToString().ToLower().Split(' ');
                    foreach (string j in kata)
                    {
                        if (!string.IsNullOrWhiteSpace(j))
                            listBox2.Items.Add(Stemming(j));
                    }

                }
                for (int i = 0; i < listBox6.Items.Count; i++)
                {

                    string[] kata = listBox6.Items[i].ToString().ToLower().Split(' ');
                    foreach (string j in kata)
                    {
                        if (!string.IsNullOrWhiteSpace(j))
                            listBox7.Items.Add(Stemming(j));
                    }

                }
                //Proses levenstein disini
                foreach (string cek in listBox2.Items)
                {
                    int cost = LevenshteinDistance.Compute(cek, listBox7.Items.ToString());

                    listBox4.Items.Add(cost);
                }
                foreach (string cek1 in listBox7.Items)
                {
                    int cost1 = LevenshteinDistance.Compute(cek1, listBox2.Items.ToString());

                    listBox5.Items.Add(cost1);
                }
                //Proses Synonim disini
                conn = new OleDbConnection();
                conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Kamus.accdb; Persist Security Info=False;";
                conn.Open();

                OleDbDataAdapter da;
                //DataTable dt = new DataTable();
                string[] tempo = new string[147];
                int pos = 0;
                int nilai = 0;
                for (int i = 0; i < listBox2.Items.Count - 1; i++)
                {
                    sql = string.Empty; Array.Clear(tempo, 0, tempo.Length); pos = 0;
                    sql = "SELECT * FROM Kamus_Tesaurus Where Kata_u LIKE  '%" + listBox2.Items[i].ToString() + "%'";
                    da = new OleDbDataAdapter(sql, conn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "persamaan");
                    //da.Fill(dt, "persamaan");
                    while (pos < ds.Tables["persamaan"].Columns.Count - 1)
                    {
                        tempo[pos] = ds.Tables["persamaan"].Rows[0][pos].ToString();
                        pos++;
                    }
                    for (int j = 0; j < listBox7.Items.Count - 1; j++)
                    {
                        for (int k = 0; k < tempo.Length; k++)
                        {
                            if (tempo[k] == listBox7.Items[j].ToString())
                                nilai += 1;
                        }
                    }
                    ds.Clear();
                }
                listBox9.Items.Add(nilai);
                nilai = 0;
                for (int i = 0; i < listBox7.Items.Count - 1; i++)
                {
                    sql = string.Empty; Array.Clear(tempo, 0, tempo.Length); pos = 0;
                    sql = "SELECT * FROM Kamus_Tesaurus Where Kata_u LIKE  '%" + listBox7.Items[i].ToString() + "%'";
                    da = new OleDbDataAdapter(sql, conn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "persamaan");
                    //da.Fill(dt, "persamaan");
                    while (pos < ds.Tables["persamaan"].Columns.Count - 1)
                    {
                        tempo[pos] = ds.Tables["persamaan"].Rows[0][pos].ToString();
                        pos++;
                    }
                    for (int j = 0; j < listBox2.Items.Count - 1; j++)
                    {
                        for (int k = 0; k < tempo.Length; k++)
                        {
                            if (tempo[k] == listBox2.Items[j].ToString())
                                nilai += 1;
                        }
                    }
                    ds.Clear();
                }
                listBox10.Items.Add(nilai);
                conn.Close();
            }
            else if (materialRadioButton1.Checked == true && materialRadioButton2.Checked == false)
            {
                //Tokenization
                #region
                label3.Hide();
                listBox1.Show();
                listBox2.Show();
                listBox3.Show();
                listBox4.Hide();
                listBox5.Hide();
                listBox6.Show();
                listBox7.Show();
                listBox8.Show();
                listBox9.Hide();
                listBox10.Hide();
                materialLabel1.Show();
                materialLabel4.Hide();
                materialLabel2.Show();
                materialLabel3.Hide();
                materialLabel5.Show();
                materialLabel5.Text = "Levensthein Distance";
                this.materialLabel5.Location = new Point(588, 154);
                materialLabel2.Text = "Synonym";
                #endregion
                this.timer1.Start();
                metroProgressBar1.Show();
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                listBox1.Text.ToLower();
                listBox2.Text.ToLower();
                Char chr = richTextBox1.Text[0];
                string[] word = richTextBox1.Text.Split(' ');
                string[] word1 = richTextBox2.Text.Split(' ');
                string[,] listkata = new string[0, 0];
                foreach (string item in word)
                {
                    RemoveChars(item);
                     listBox1.Items.Add(item);
                    
                }
                foreach (string item in word1)
                {
                    RemoveChars(item);
                    listBox8.Items.Add(item);
                }
                //Proses levenstein disini
                foreach (string cek in listBox1.Items)
                {
                    int cost = LevenshteinDistance.Compute(cek, listBox8.Items.ToString());

                    listBox3.Items.Add(cost);
                }
                foreach (string cek1 in listBox8.Items)
                {
                    int cost1 = LevenshteinDistance.Compute(cek1, listBox1.Items.ToString());

                    listBox6.Items.Add(cost1);
                }
                //Proses Synonim disini
                conn = new OleDbConnection();
                conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Kamus.accdb; Persist Security Info=False;";
                conn.Open();

                OleDbDataAdapter da;
                //DataTable dt = new DataTable();
                string[] tempo = new string[147];
                int pos = 0;
                int nilai = 0;
                for (int i = 0; i < listBox1.Items.Count - 1; i++)
                {
                    sql = string.Empty; Array.Clear(tempo, 0, tempo.Length); pos = 0;
                    sql = "SELECT * FROM Kamus_Tesaurus Where Kata_u LIKE  '%" + listBox1.Items[i].ToString() + "%'";
                    da = new OleDbDataAdapter(sql, conn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "persamaan");
                    //da.Fill(dt, "persamaan");
                    while (pos < ds.Tables["persamaan"].Columns.Count - 1)
                    {
                        tempo[pos] = ds.Tables["persamaan"].Rows[0][pos].ToString();
                        pos++;
                    }
                    for (int j = 0; j < listBox8.Items.Count - 1; j++)
                    {
                        for (int k = 0; k < tempo.Length; k++)
                        {
                            if (tempo[k] == listBox8.Items[j].ToString())
                                nilai += 1;
                        }
                    }
                    ds.Clear();
                }
                listBox2.Items.Add(nilai);
                nilai = 0;
                for (int i = 0; i < listBox8.Items.Count - 1; i++)
                {
                    sql = string.Empty; Array.Clear(tempo, 0, tempo.Length); pos = 0;
                    sql = "SELECT * FROM Kamus_Tesaurus Where Kata_u LIKE  '%" + listBox8.Items[i].ToString() + "%'";
                    da = new OleDbDataAdapter(sql, conn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "persamaan");
                    //da.Fill(dt, "persamaan");
                    while (pos < ds.Tables["persamaan"].Columns.Count - 1)
                    {
                        tempo[pos] = ds.Tables["persamaan"].Rows[0][pos].ToString();
                        pos++;
                    }
                    for (int j = 0; j < listBox1.Items.Count - 1; j++)
                    {
                        for (int k = 0; k < tempo.Length; k++)
                        {
                            if (tempo[k] == listBox1.Items[j].ToString())
                                nilai += 1;
                        }
                    }
                    ds.Clear();
                }
                listBox7.Items.Add(nilai);
                conn.Close();
            }
            //bool temu = false;
            //while (pos < dataGridView1.Columns.Count & !temu)
            //{
            //    if ((string)dataGridView1[pos, 0].Value.ToString().Trim() == "perubahan")
            //    {
            //        temu = true;
            //        label1.Text = "ketemu";
            //    }
            //    else
            //        pos++;
            //}
            //cmd.CommandText = sql;
            //OleDbDataReader reader = cmd.ExecuteReader();

            //while (pos < ds.Tables["Test"].Columns.Count)
            //{
            //    Array.Resize(ref tempo, pos + 1);
            //    temp[pos] = ds.Tables["Test"].Rows[0][pos].ToString();
            //    pos++;

            //}
            //foreach (string i in temp)
            //{
            //    listBox3.Items.Add(i);
            //}


            //Array.Resize(ref temp, k + 1);
            //temp[k] = reader.GetString(0).ToString();
            //if (temp[k] == "Kemajuan")
            //    label1.Text = "true";

        }
        //static string ConverStringArrayToString(string[] array)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    foreach(string value in array)
        //    {
        //        builder.Append(value);
        //        builder.Append('.');
        //    }
        //    return builder.ToString();
        //}
        //static string ConvertStringArrayToStringJoin (string[]array)
        //{
        //    string result = string.Join(".", array);
        //    return result;
        //}
        public string RemoveChars(string str)
        {
            string[] chars = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "-", "_", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0",";",":","(",")","[","]","{","}","=","+","?","`"};
            for (int i = 0; i < chars.Length; i++)
            {
                if (str.Contains(chars[i]))
                {
                    str = str.Replace(chars[i], "");
                }
            }
            return str;
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
                timer1.Stop();
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
        //Leveinstein here
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }
//Stemming Procedure Starts Here
        private void listKamus()
        {
            OleDbConnection db = new OleDbConnection(link);
            db.Open();
            OleDbCommand cmd = db.CreateCommand();
            string sql = "SELECT rootword FROM tb_rootword";
            cmd.CommandText = sql;
            OleDbDataReader reader = cmd.ExecuteReader();
            int banyak = jlhlist("SELECT count(*) FROM tb_rootword");
            kamus = new String[banyak];
            int i = 0;
            while (reader.Read())
            {
                kamus[i] = reader.GetString(0).ToString();
                i++;
            }
            db.Close();

        }
        private bool cekKataDasar(string kata)
        {
            int banyak = jlhlist("SELECT count(*) FROM tb_rootword");
            //MessageBox.Show(banyak.ToString());
            for (int i = 0; i < banyak - 1; i++)
            {
                if (kata == kamus[i])
                {
                    return true;
                }
            }
            return false;
        }
        private string HapusAkhiran(string kata)
        {

            Match matchkahlah = Regex.Match(kata, @"([A-Za-z]+)([klt]ah|pun|ku|mu|nya)$", RegexOptions.IgnoreCase);
            if (matchkahlah.Success)
            {
                string key = matchkahlah.Groups[1].Value;

                return HapusAkhiranKepunyaan(key);
            }

            return HapusAkhiranKepunyaan(kata);
        }

        private string HapusAkhiranKepunyaan(string kata)
        {
            Match matchkahlah = Regex.Match(kata, @"([A-Za-z]+)(nya|[km]u)$", RegexOptions.IgnoreCase);
            if (matchkahlah.Success)
            {
                string key = matchkahlah.Groups[1].Value;

                return key;
            }
            return kata;
        }

        private string HapusAkhiranIAnKan(string kata)
        {
            string kataasal = kata;

            if (Regex.IsMatch(kata, "(kan)$"))
            {
                string kata1 = Regex.Replace(kata, "(kan)$", "");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }

            }

            if (Regex.IsMatch(kata, "(i|an)$"))
            {
                string kata2 = Regex.Replace(kata, "(i|an)$", "");
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }

            }
            return kataasal;
        }


        private string hapus_derivation_prefix(string kata)
        {
            string kataasal = kata;
            if (Regex.IsMatch(kata, "^(di|[ks]e)"))
            {
                string kata1 = Regex.Replace(kata, "^(di|[ks]e)", "");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }

                string kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }
            }


            #region cek te- me- be- pe-
            if (Regex.IsMatch(kata, "^([tmbp]e)")) //cek te- me- be- pe-
            {
                #region awalan be-

                if (Regex.IsMatch(kataasal, "^(be)"))
                {
                    if (Regex.IsMatch(kataasal, "^(ber)[aiueo]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(ber)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                        kata1 = Regex.Replace(kata, "^(ber)", "r");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }

                    if (Regex.IsMatch(kata, "^(ber)[^aiueor][A-Za-z](?!er)"))
                    {
                        string kata1 = Regex.Replace(kata, "^(ber)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }

                    if (Regex.IsMatch(kata, "^(ber)[^aiueor][A-Za-z]er[aiueo]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(ber)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }

                    if (Regex.IsMatch(kata, "^(belajar)"))
                    {
                        string kata1 = Regex.Replace(kata, "^(bel)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(ber)[^aiueor]er[^aiueo]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(be)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }


                }

                #endregion

                #region awalan te-
                if (Regex.IsMatch(kata, "^(te)"))
                {
                    if (Regex.IsMatch(kata, "^(terr)"))
                    {
                        return kata;
                    }
                    if (Regex.IsMatch(kata, "^(ter)[aioue]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(ter)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                        kata1 = Regex.Replace(kata, "^(ter)", "r");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    //teko kene
                }
                if (Regex.IsMatch(kata, "^(ter)[^aiueor]er[aiueo]"))
                {
                    string kata1 = Regex.Replace(kata, "^(ter)", "");
                    if (cekKataDasar(kata1))
                    {
                        return kata1;
                    }
                    string kata2 = HapusAkhiranIAnKan(kata1);
                    if (cekKataDasar(kata2))
                    {
                        return kata2;
                    }
                }
                if (Regex.IsMatch(kata, "^(ter)[^aiueor](?!er)"))
                {
                    string kata1 = Regex.Replace(kata, "^(ter)", "");
                    if (cekKataDasar(kata1))
                    {
                        return kata1;
                    }
                    string kata2 = HapusAkhiranIAnKan(kata1);
                    if (cekKataDasar(kata2))
                    {
                        return kata2;
                    }
                }
                if (Regex.IsMatch(kata, "^(te)[^aiueor]er[aiueo]"))
                {
                    string kata1 = Regex.Replace(kata, "^(te)", "");
                    if (cekKataDasar(kata1))
                    {
                        return kata1;
                    }
                    string kata2 = HapusAkhiranIAnKan(kata1);
                    if (cekKataDasar(kata2))
                    {
                        return kata2;
                    }
                }
                if (Regex.IsMatch(kata, "^(ter)[^aiueor]er[^aiueo]"))
                {
                    string kata1 = Regex.Replace(kata, "^(ter)", "");
                    if (cekKataDasar(kata1))
                    {
                        return kata1;
                    }
                    string kata2 = HapusAkhiranIAnKan(kata1);
                    if (cekKataDasar(kata2))
                    {
                        return kata2;
                    }
                }

                #endregion

                #region awalan me-
                if (Regex.IsMatch(kata, "^(me)"))
                {
                    if (Regex.IsMatch(kata, "^(me)[lrwyv][aiueo]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(me)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(mem)[bfvp]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(mem)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(mem)((r[aiueo])|[aiueo])"))
                    {
                        string kata1 = Regex.Replace(kata, "^(mem)", "m");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                        kata1 = Regex.Replace(kata, "^(mem)", "p");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }

                    }
                    if (Regex.IsMatch(kata, "^(men)[cdjszt]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(men)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(men)[aiueo]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(men)", "n");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                        kata1 = Regex.Replace(kata, "^(men)", "t");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(meng)[ghqk]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(meng)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(meng)[aiueo]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(meng)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                        kata1 = Regex.Replace(kata, "^(meng)", "k");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                        kata1 = Regex.Replace(kata, "^(menge)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(meny)[aiueo]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(meny)", "s");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                        kata1 = Regex.Replace(kata, "^(me)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                }
                #endregion

                #region awalan pe-
                if (Regex.IsMatch(kata, "^(pe)"))
                {
                    if (Regex.IsMatch(kata, "^(pe)[wy]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(pe)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(per)[aiueo]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(per)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                        kata1 = Regex.Replace(kata, "^(per)", "r");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(per)[^aiueor][A-Za-z](?!er)"))
                    {
                        string kata1 = Regex.Replace(kata, "^(per)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(per)[^aiueor][A-Za-z](er)[aiueo]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(per)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(pembelajaran)"))
                    {
                        string kata1 = Regex.Replace(kata, "^(pembelajaran)", "ajar");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(pem)[bfv]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(pem)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(pem)(r[aiueo]|[aiueo])"))
                    {
                        string kata1 = Regex.Replace(kata, "^(pem)", "m");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                        kata1 = Regex.Replace(kata, "^(pem)", "p");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(pen)[cdjzt]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(pen)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(pen)[aiueo]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(pen)", "n");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                        kata1 = Regex.Replace(kata, "^(pen)", "t");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(peng)[^aiueo]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(peng)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(peng)[aiueo]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(peng)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                        kata1 = Regex.Replace(kata, "^(peng)", "k");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                        kata1 = Regex.Replace(kata, "^(penge)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(peny)[aiueo]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(peny)", "s");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                        kata1 = Regex.Replace(kata, "^(pe)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(pel)[aiueo]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(pel)", "l");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(pelajar)"))
                    {
                        string kata1 = Regex.Replace(kata, "^(pel)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(pe)[^rwylmn]er[aiueo]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(pe)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(pe)[^rwylmn](?!er)"))
                    {
                        string kata1 = Regex.Replace(kata, "^(pe)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                    if (Regex.IsMatch(kata, "^(pe)[^aiueo]er[^aiueo]"))
                    {
                        string kata1 = Regex.Replace(kata, "^(pe)", "");
                        if (cekKataDasar(kata1))
                        {
                            return kata1;
                        }
                        string kata2 = HapusAkhiranIAnKan(kata1);
                        if (cekKataDasar(kata2))
                        {
                            return kata2;
                        }
                    }
                }
            }

            #endregion
            #endregion

            #region memper- dkk

            if (Regex.IsMatch(kata, "^(memper)"))
            {
                string kata1 = Regex.Replace(kata, "^(memper)", "");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                string kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }
                kata1 = Regex.Replace(kata, "^(memper)", "r");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }
            }

            if (Regex.IsMatch(kata, "^(mempel)"))
            {
                string kata1 = Regex.Replace(kata, "^(mempel)", "");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                string kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }
                kata1 = Regex.Replace(kata, "^(mempel)", "l");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }

            }
            if (Regex.IsMatch(kata, "^(menter)"))
            {
                string kata1 = Regex.Replace(kata, "^(menter)", "");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                string kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }
                kata1 = Regex.Replace(kata, "^(menter)", "r");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }

            }
            if (Regex.IsMatch(kata, "^(member)"))
            {
                string kata1 = Regex.Replace(kata, "^(member)", "");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                string kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }
                kata1 = Regex.Replace(kata, "^(member)", "r");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }

            }

            #endregion

            #region diper-
            if (Regex.IsMatch(kata, "^(diper)"))
            {
                string kata1 = Regex.Replace(kata, "^(diper)", "");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                string kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }
                kata1 = Regex.Replace(kata, "^(diper)", "r");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }
            }
            #endregion

            #region diter-
            if (Regex.IsMatch(kata, "^(diter)"))
            {
                string kata1 = Regex.Replace(kata, "^(diter)", "");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                string kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }
                kata1 = Regex.Replace(kata, "^(diter)", "r");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }
            }
            #endregion

            #region dipel-
            if (Regex.IsMatch(kata, "^(dipel)"))
            {
                string kata1 = Regex.Replace(kata, "^(dipel)", "");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                string kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }
                kata1 = Regex.Replace(kata, "^(dipel)", "l");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }
            }
            #endregion

            #region diber-
            if (Regex.IsMatch(kata, "^(diber)"))
            {
                string kata1 = Regex.Replace(kata, "^(diber)", "");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                string kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }
                kata1 = Regex.Replace(kata, "^(diber)", "r");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }
            }
            #endregion

            #region keber-
            if (Regex.IsMatch(kata, "^(keber)"))
            {
                string kata1 = Regex.Replace(kata, "^(keber)", "");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                string kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }
                kata1 = Regex.Replace(kata, "^(keber)", "r");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }
            }
            #endregion

            #region keter-
            if (Regex.IsMatch(kata, "^(keter)"))
            {
                string kata1 = Regex.Replace(kata, "^(keter)", "");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                string kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }
                kata1 = Regex.Replace(kata, "^(keter)", "r");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }
            }
            #endregion

            #region berke-
            if (Regex.IsMatch(kata, "^(keter)"))
            {
                string kata1 = Regex.Replace(kata, "^(berke)", "");
                if (cekKataDasar(kata1))
                {
                    return kata1;
                }
                string kata2 = HapusAkhiranIAnKan(kata1);
                if (cekKataDasar(kata2))
                {
                    return kata2;
                }

            }
            #endregion

            //cek awalan di ke se te be me
            if (Regex.IsMatch(kata, "^(di|[kstbmp]e)") == false)
            {
                return kataasal;
            }
            return kataasal;


        }

        private string Stemming(string kata)
        {
            if (cekKataDasar(kata))
            {
                return kata;
            }

            kata = HapusAkhiran(kata);

            kata = HapusAkhiranIAnKan(kata);

            kata = hapus_derivation_prefix(kata);

            return kata;
        }
        static class LevenshteinDistance
        {
            /// <summary>
            /// Compute the distance between two strings.
            /// </summary>
            public static int Compute(string s, string t)
            {
                int n = s.Length;
                int m = t.Length;
                int[,] d = new int[n + 1, m + 1];

                // Step 1
                if (n == 0)
                {
                    return m;
                }

                if (m == 0)
                {
                    return n;
                }

                // Step 2
                for (int i = 0; i <= n; d[i, 0] = i++)
                {
                }

                for (int j = 0; j <= m; d[0, j] = j++)
                {
                }

                // Step 3
                for (int i = 1; i <= n; i++)
                {
                    //Step 4
                    for (int j = 1; j <= m; j++)
                    {
                        // Step 5
                        int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                        // Step 6
                        d[i, j] = Math.Min(
                            Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                            d[i - 1, j - 1] + cost);
                    }
                }
                // Step 7
                return d[n, m];
            }
        }

        private void materialRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            metroTile3.Show();
        }

        private void materialRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            metroTile3.Show();
        }
    }

}