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

namespace Plagiarism_Checker
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {

        public Form1()
        {
            InitializeComponent();
            metroTextBox2.Hide();
            textBox2.Hide();
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

        public string RemoveChars(string str)
        {
            string[] chars = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "-", "_", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", ";", ":", "(", ")", "[", "]", "{", "}", "=", "+", "?", "`" };
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

        private void tabControl1_ParentChanged(object sender, EventArgs e)
        {
            listBox1.Hide();
            listBox2.Hide();
            listBox3.Hide();

            listBox5.Hide();
            listBox6.Hide();
            listBox7.Hide();
            listBox8.Hide();

            listBox10.Hide();
            materialLabel1.Hide();
            materialLabel2.Hide();
            materialLabel3.Hide();
            materialLabel4.Hide();
            materialLabel5.Hide();
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
                        d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
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

        private void metroTile3_Click_1(object sender, EventArgs e)
        {
            metroTile3.Enabled = false;
            if (materialRadioButton2.Checked == true && materialRadioButton1.Checked == false)
            {
                storevariable.cekstemmingresult = true;
                //Tokenization
                #region
                
                listBox10.Items.Clear();
                label3.Hide();
                listBox1.Show();
                listBox2.Show();
                listBox3.Show();

                listBox5.Show();
                listBox6.Show();
                listBox7.Show();
                listBox8.Show();
                listBox11.Show();
                listBox13.Show();
                listBox10.Show();
                materialLabel1.Show();
                materialLabel4.Show();
                materialLabel2.Show();
                materialLabel3.Show();
                materialLabel5.Show();
                metroTile5.Show();
                this.materialLabel3.Location = new Point(308, 376);
                this.materialLabel4.Location = new Point(349, 497);
                this.listBox5.Location = new Point(487, 347);
                this.listBox10.Location = new Point(487, 456);
                #endregion
                timer1.Start();
                metroProgressBar1.Show();
                metroTile4.Show();

                listBox1.Items.Clear();
                listBox2.Items.Clear();
                listBox1.Text.ToLower();
                listBox2.Text.ToLower();
                Char chr = textBox1.Text[0];
                string[] word = textBox1.Text.Split('.');
                string[] word1 = textBox2.Text.Split('.');
                foreach (string item in word)
                {
                    if (!string.IsNullOrWhiteSpace(item.ToString()))
                        listBox1.Items.Add(item);

                }
                foreach (string item in word1)
                {
                    if (!string.IsNullOrWhiteSpace(item.ToString()))
                        listBox8.Items.Add(item);
                }

                //Proses Stopwords Disini
                sql = "SELECT List FROM StopWord_List";
                conn = new OleDbConnection(link);
                stoplist(sql);
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                OleDbDataReader reader = com.ExecuteReader();
                string[] temp = new string[0];
                string regexCode = string.Format(@"\s?\b(?:{0})\b\s?", string.Join("|", list));

                Regex regex = new Regex(regexCode, RegexOptions.Singleline | RegexOptions.IgnoreCase);

                Regex removeDoubleSpace = new Regex(@"\s{2,}", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    Array.Resize(ref temp, i + 1);
                    temp[i] = regex.Replace(listBox1.Items[i].ToString(), " ");
                    temp[i] = removeDoubleSpace.Replace(temp[i].ToString(), " ");
                    listBox3.Items.Add(RemoveChars(temp[i]));
                    //listBox9.Items.Add(RemoveChars(temp[i]));
                }
                temp = new string[0];
                for (int i = 0; i < listBox8.Items.Count; i++)
                {
                    Array.Resize(ref temp, i + 1);
                    temp[i] = regex.Replace(listBox8.Items[i].ToString(), " ");
                    temp[i] = removeDoubleSpace.Replace(temp[i].ToString(), " ");
                    listBox6.Items.Add(RemoveChars(temp[i]));
                    //listBox12.Items.Add(RemoveChars(temp[i]));
                }
                conn.Close();
                //Proses Stemming Disini
                sql = "SELECT List FROM tb_rootword";
                conn = new OleDbConnection(link);

                string[] temp1 = new string[listBox3.Items.Count];
                listKamus();
                for (int i = 0; i < listBox3.Items.Count; i++)
                {

                    string[] kata = listBox3.Items[i].ToString().ToLower().Trim().Split(' ');
                    string[] temp2 = new string[0];
                    for (int j = 0; j < kata.Length; j++)
                    {
                        if (!string.IsNullOrWhiteSpace(kata[j]))
                        {
                            Array.Resize(ref temp2, j + 1);
                            temp2[j] = Stemming(kata[j]);
                        }
                    }
                    listBox11.Items.Add(string.Join(" ", temp2));

                    foreach (string j in kata)
                    {
                        if (!string.IsNullOrWhiteSpace(j) && !listBox2.Items.Contains(Stemming(j)))
                        {
                            listBox2.Items.Add(Stemming(j));
                            //listBox11.Items.Add(Stemming(j));
                        }
                    }
                }
                for (int i = 0; i < listBox6.Items.Count; i++)
                {
                    string[] kata = listBox6.Items[i].ToString().ToLower().Trim().Split(' ');
                    string[] temp2 = new string[0];
                    for (int j = 0; j < kata.Length; j++)
                    {
                        if (!string.IsNullOrWhiteSpace(kata[j]))
                        {
                            Array.Resize(ref temp2, j + 1);
                            temp2[j] = Stemming(kata[j]);
                        }
                    }
                    listBox13.Items.Add(string.Join(" ", temp2));
                    foreach (string j in kata)
                    {
                        if (!string.IsNullOrWhiteSpace(j) && !listBox7.Items.Contains(Stemming(j)))
                            listBox7.Items.Add(Stemming(j));
                        //listBox13.Items.Add(Stemming(j));
                    }
                }
                //Proses levenstein disini         
                for (int i = 0; i < listBox7.Items.Count; i++)
                {
                    for (int j = 0; j < listBox2.Items.Count; j++)
                    {
                        int cost1 = LevenshteinDistance.Compute(listBox7.Items[i].ToString(), listBox2.Items[j].ToString());
                        listBox5.Items.Add(cost1);
                    }
                }
                //Proses Synonim disini
                conn = new OleDbConnection();
                conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Kamus.accdb; Persist Security Info=False;";
                conn.Open();
                OleDbDataAdapter da;

                string[] tempo = new string[147];
                int pos = 0;
                int nilai = 0;

                for (int i = 0; i < listBox13.Items.Count; i++)
                {
                    Boolean cek1 = true;
                    string[] tmp1 = new string[0];
                    foreach (string j in listBox13.Items[i].ToString().Split(' '))
                    {
                        if (!string.IsNullOrWhiteSpace(j))
                        {
                            Array.Resize(ref tmp1, pos + 1);
                            tmp1[pos] = j; pos++;
                        }
                    }
                    pos = 0;
                    if (cek1 == true)
                    {
                        for (int j = 0; j < tmp1.Length; j++)
                        {
                            sql = string.Empty; Array.Clear(tempo, 0, tempo.Length); pos = 0;
                            string cek = "SELECT count(*) FROM Kamus_Tesaurus Where Kata_u LIKE  '" + tmp1[j] + "%'";
                            sql = "SELECT * FROM Kamus_Tesaurus Where Kata_u LIKE  '" + tmp1[j] + "%'";
                            OleDbCommand cmd = new OleDbCommand(cek, conn);
                            int count = (int)cmd.ExecuteScalar();
                            if (count > 0)
                            {
                                da = new OleDbDataAdapter(sql, conn);
                                DataSet ds = new DataSet();
                                da.Fill(ds, "persamaan");
                                while (pos < ds.Tables["persamaan"].Columns.Count)
                                {
                                    tempo[pos] = ds.Tables["persamaan"].Rows[0][pos].ToString();
                                    pos++;
                                }
                                pos = 0;
                                string[] tmp2 = new string[0];
                                for (int k = 0; k < listBox11.Items.Count; k++)
                                {
                                    foreach (string l in listBox11.Items[k].ToString().Split(' '))
                                    {
                                        if (!string.IsNullOrWhiteSpace(l))
                                        {
                                            Array.Resize(ref tmp2, pos + 1);
                                            tmp2[pos] = l; pos++;
                                        }
                                    }
                                    //pos = 0;
                                }
                                for (int l = 0; l < tmp2.Length; l++)
                                {
                                    //cek1 = true;
                                    for (int m = 1; m < tempo.Length; m++)
                                    {
                                        if (tempo[m].Trim().ToLower() == tmp2[l] && cek1 == true)
                                        {
                                            nilai += 1;
                                            cek1 = false;
                                        }
                                        //cek1 = false;
                                    }
                                }
                                ds.Clear();
                            }
                            else
                            {
                                cek = "SELECT count(*) FROM Unneeded Where Added LIKE  '" + tmp1[j] + "%'";
                                sql = "SELECT * FROM Unneeded Where Added LIKE  '" + tmp1[j] + "%'";
                                cmd = new OleDbCommand(cek, conn);
                                count = (int)cmd.ExecuteScalar();
                                if (count > 0)
                                {
                                }
                                else
                                {
                                    string add = "insert into Unneeded ([Added]) values (@1)";
                                    OleDbCommand command = new OleDbCommand(add, conn);
                                    command.Parameters.AddWithValue("@1", tmp1[j]);
                                    command.ExecuteNonQuery();
                                }
                            }

                        }
                    }

                }
                listBox10.Items.Add(nilai);
                conn.Close();
                //for (int i = 0; i < listBox7.Items.Count; i++)
                //{
                //    sql = string.Empty; Array.Clear(tempo, 0, tempo.Length); pos = 0;
                //    string cek = "SELECT count(*) FROM Kamus_Tesaurus Where Kata_u LIKE  '%" + listBox7.Items[i].ToString() + "%'";
                //    sql = "SELECT * FROM Kamus_Tesaurus Where Kata_u LIKE  '%" + listBox7.Items[i].ToString() + "%'";
                //    //listBox13.Items[i].ToString().Split(' ');
                //    OleDbCommand cmd = new OleDbCommand(cek, conn);
                //    int count = (int)cmd.ExecuteScalar();
                //    if (count > 0)
                //    {
                //        da = new OleDbDataAdapter(sql, conn);
                //        DataSet ds = new DataSet();
                //        da.Fill(ds, "persamaan");
                //        //da.Fill(dt, "persamaan");
                //            while (pos < ds.Tables["persamaan"].Columns.Count)
                //            {
                //                tempo[pos] = ds.Tables["persamaan"].Rows[0][pos].ToString();
                //                pos++;
                //            }
                //        for (int j = 0; j < listBox2.Items.Count; j++)
                //        {
                //            for (int k = 0; k < tempo.Length; k++)
                //            {
                //                if (listBox2.Items[j].ToString() == tempo[k].Trim().ToLower())
                //                    nilai += 1;
                //            }
                //        }
                //        ds.Clear();
                //    }
                //    else
                //    {
                //        string add = "insert into Kamus_Tesaurus ([Kata_u]) values (@1)";
                //        OleDbCommand command = new OleDbCommand(add, conn);
                //        command.Parameters.AddWithValue("@1", listBox7.Items[i].ToString());
                //        command.ExecuteNonQuery();
                //    }

                //}
                //listBox10.Items.Add(nilai);
                //conn.Close();
            }
            else if (materialRadioButton1.Checked == true && materialRadioButton2.Checked == false)
            {
                storevariable.cekstemmingresult = false;

                //Tokenization
                #region
                listBox2.Hide();
                listBox7.Hide();
                listBox3.Show();
                listBox6.Show();
                metroTile5.Show();
                label3.Hide();
                listBox1.Show();
                listBox5.Show();
                listBox8.Show();
                listBox10.Show();
                materialLabel1.Show();
                materialLabel4.Hide();
                materialLabel2.Show();
                materialLabel3.Hide();
                materialLabel5.Show();
                materialLabel5.Text = "Levensthein Distance";
                this.materialLabel5.Location = new Point(188, 280);
                materialLabel2.Text = "Sinonim";
                this.materialLabel2.Location = new Point(188, 380);
                this.listBox5.Location = new Point(359, 250);
                this.listBox10.Location = new Point(359, 350);
                #endregion
                this.timer1.Start();
                metroProgressBar1.Show();
                metroTile4.Show();
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                listBox8.Items.Clear();
                listBox5.Items.Clear();
                listBox10.Items.Clear();
                listBox1.Text.ToLower();
                listBox2.Text.ToLower();
                listBox11.Hide();
                listBox13.Hide();
                Char chr = textBox1.Text[0];
                string[] word = textBox1.Text.Split(' ');
                string[] word1 = textBox2.Text.Split(' ');
                string[] word2 = textBox1.Text.Split('.');
                string[] word3 = textBox2.Text.Split('.');
                //string[,] listkata = new string[0, 0];
                string[] temp = new string[0];
                //for(int i = 0; i < word.Length; i++)
                //{
                //    Array.Resize(ref temp, i + 1);
                //    temp[i] = RemoveChars( word[i]);
                //}

                foreach (string item in word2)
                {
                    if (!string.IsNullOrWhiteSpace(RemoveChars(item)))
                    {
                        listBox1.Items.Add(RemoveChars(item));
                    }

                }
                foreach (string item in word3)
                {
                    if (!string.IsNullOrWhiteSpace(RemoveChars(item)))
                    {
                        listBox8.Items.Add(RemoveChars(item));
                    }

                }
                foreach (string item in word)
                {
                    if (!string.IsNullOrWhiteSpace(RemoveChars(item)) && !listBox3.Items.Contains(item))
                    {
                        listBox3.Items.Add(RemoveChars(item));
                    }

                }
                foreach (string item in word1)
                {
                    if (!string.IsNullOrWhiteSpace(RemoveChars(item)) && !listBox6.Items.Contains(item))
                    {
                        listBox6.Items.Add(RemoveChars(item));
                    }

                }
                //Proses levenstein disini

                for (int i = 0; i < listBox6.Items.Count; i++)
                {
                    for (int j = 0; j < listBox3.Items.Count; j++)
                    {
                        int cost1 = LevenshteinDistance.Compute(listBox6.Items[i].ToString(), listBox3.Items[j].ToString());
                        listBox5.Items.Add(cost1);
                    }
                }
                //Proses Synonim disini

                conn = new OleDbConnection();
                conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Kamus.accdb; Persist Security Info=False;";
                conn.Open();
                OleDbDataAdapter da;
                string[] tempo = new string[147];
                int pos = 0;
                int nilai = 0;
                for (int i = 0; i < listBox8.Items.Count; i++)
                {
                    Boolean cek1 = true;
                    string[] tmp1 = new string[0];
                    foreach (string j in listBox8.Items[i].ToString().Split(' '))
                    {
                        if (!string.IsNullOrWhiteSpace(j))
                        {
                            Array.Resize(ref tmp1, pos + 1);
                            tmp1[pos] = j; pos++;
                        }
                    }
                    pos = 0;
                    if (cek1 == true)
                    {
                        for (int j = 0; j < tmp1.Length; j++)
                        {
                            sql = string.Empty; Array.Clear(tempo, 0, tempo.Length); pos = 0;
                            string cek = "SELECT count(*) FROM Kamus_Tesaurus Where Kata_u LIKE  '" + tmp1[j] + "%'";
                            sql = "SELECT * FROM Kamus_Tesaurus Where Kata_u LIKE  '" + tmp1[j] + "%'";
                            OleDbCommand cmd = new OleDbCommand(cek, conn);
                            int count = (int)cmd.ExecuteScalar();
                            if (count > 0)
                            {
                                da = new OleDbDataAdapter(sql, conn);
                                DataSet ds = new DataSet();
                                da.Fill(ds, "persamaan");
                                while (pos < ds.Tables["persamaan"].Columns.Count)
                                {
                                    tempo[pos] = ds.Tables["persamaan"].Rows[0][pos].ToString();
                                    pos++;
                                }
                                pos = 0;
                                string[] tmp2 = new string[0];
                                for (int k = 0; k < listBox1.Items.Count; k++)
                                {
                                    foreach (string l in listBox1.Items[k].ToString().Split(' '))
                                    {
                                        if (!string.IsNullOrWhiteSpace(l))
                                        {
                                            Array.Resize(ref tmp2, pos + 1);
                                            tmp2[pos] = l; pos++;

                                        }
                                    }
                                }
                                for (int l = 0; l < tmp2.Length; l++)
                                {
                                    for (int m = 1; m < tempo.Length; m++)
                                    {
                                        if (tempo[m].Trim().ToLower() == tmp2[l] && cek1 == true)
                                        {
                                            nilai += 1;
                                            cek1 = false;
                                        }
                                    }
                                }
                                ds.Clear();
                            }
                            else
                            {
                                cek = "SELECT count(*) FROM Unneeded Where Added LIKE  '" + tmp1[j] + "%'";
                                sql = "SELECT * FROM Unneeded Where Added LIKE  '" + tmp1[j] + "%'";
                                cmd = new OleDbCommand(cek, conn);
                                count = (int)cmd.ExecuteScalar();
                                if (count > 0)
                                {
                                }
                                else
                                {
                                    string add = "insert into Unneeded ([Added]) values (@1)";
                                    OleDbCommand command = new OleDbCommand(add, conn);
                                    command.Parameters.AddWithValue("@1", tmp1[j]);
                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                listBox10.Items.Add(nilai);
                conn.Close();
            }
        }

        private void metroTile2_Click_1(object sender, EventArgs e)
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
                FileInfo inf = new FileInfo(filePath);
                long lengthinf = inf.Length / 1024;
                string fordislay = lengthinf.ToString("N0") + "KB";
                textBox36.Text = fordislay;
                textBox37.Text = filePath;
                textBox38.Text = System.IO.Path.GetFileName(filePath);
                string strText = string.Empty;
                try
                {

                    PdfReader read = new PdfReader(filePath);
                    for (int page = 1; page <= read.NumberOfPages; page++)
                    {
                        ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();
                        String s = PdfTextExtractor.GetTextFromPage(read, page, its);

                        s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
                        strText = strText + s;
                        textBox2.Text = strText;
                    }
                    read.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void materialRadioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            metroTile3.Show();
        }

        private void materialRadioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            metroTile3.Show();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            metroProgressBar1.Maximum = 1000;
            metroProgressBar1.PerformStep();
            if (metroProgressBar1.Value == 1000)
            {
                TabPage t = tabControl1.TabPages[1];
                tabControl1.SelectedTab = t;
                label1.Hide();
                timer1.Stop();
            }

        }
        private void button1_Click_2(object sender, EventArgs e)
        {
            label14.Show();
            label15.Show();
            label16.Show();
            label17.Show();
            label18.Show();
            label19.Show();
            textBox8.Show();
            textBox9.Show();
            textBox10.Show();
            label35.Show();
            label36.Show();
            label37.Show();
            label38.Show();
            textBox17.Show();

            label14.Visible = true;
            label15.Visible = true;
            label16.Visible = true;
            label17.Visible = true;
            label18.Visible = true;
            label19.Visible = true;
            label38.Visible = true;
            label37.Visible = true;
            textBox8.Visible = true;
            textBox9.Visible = true;
            textBox10.Visible = true;
            textBox17.Visible = true;
            label36.Visible = true;
            label35.Visible = true;
            label14.Show();
            label15.Show();
            label16.Show();
            label17.Show();
            label18.Show();
            label19.Show();
            label37.Show();
            label38.Show();
            textBox8.Show();
            textBox9.Show();
            textBox10.Show();
            textBox17.Show();
      

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            metroTile4.Hide();
            metroTile7.Hide();
            listBox1.Hide();
            listBox2.Hide();
            listBox3.Hide();
            listBox5.Hide();
            listBox6.Hide();
            listBox7.Hide();
            listBox8.Hide();
            listBox10.Hide();
            listBox11.Hide();
            listBox13.Hide();
            materialLabel2.Hide();
            materialLabel3.Hide();
            materialLabel4.Hide();
            materialLabel5.Hide();
            metroTile5.Hide();
            metroTile6.Hide();
            listBox4.Hide();
            materialLabel1.Hide();

        }

        private void metroTile5_Click(object sender, EventArgs e)
        {
            storevariable variable = new storevariable();
            TabPage t = tabControl1.TabPages[2];
            tabControl1.SelectedTab = t;
            groupBox2.Show();
            metroTile6.Show();
            groupBox7.Show();
            textBox24.Show();
            textBox25.Show();
            metroTile7.Show();
            label36.Show();
            label35.Show();
            if (storevariable.cekstemmingresult == true)
            {
                groupBox5.Show();
                groupBox6.Show();
                label2.Hide();
                label4.Hide();
                label5.Hide();

                //levensthein
                float hasiltotalleven = 0;
                for (int i = 0; i < listBox5.Items.Count; i++)
                {
                    if ((int)listBox5.Items[i] == 0)
                        hasiltotalleven += 1;
                }
                float persenleven;
                persenleven = (hasiltotalleven / Math.Max(listBox2.Items.Count, listBox7.Items.Count) * 100f);
                textBox5.Text = persenleven.ToString();
                textBox8.Text = hasiltotalleven.ToString();

                //sinonim
                float hasilsinonim = ((float.Parse(listBox10.Items[0].ToString())) / listBox13.Items.Count) * 100f;
                textBox6.Text = hasilsinonim.ToString();

                ////Counting Total words in Textbox
                String countingwords = textBox1.Text.Trim();
                string countingwords2 = textBox2.Text.Trim();
                int wordCount = 0, index = 0;
                int wordCount2 = 0, index2 = 0;
                while (index < countingwords.Length)
                {
                    // check if current char is part of a word
                    while (index < countingwords.Length && Char.IsWhiteSpace(countingwords[index]) == false)
                        index++;

                    wordCount++;

                    // skip whitespace until next word
                    while (index < countingwords.Length && Char.IsWhiteSpace(countingwords[index]) == true)
                        index++;
                }
                textBox10.Text = wordCount.ToString();
                while (index2 < countingwords2.Length)
                {
                    // check if current char is part of a word
                    while (index2 < countingwords2.Length && Char.IsWhiteSpace(countingwords2[index2]) == false)
                        index2++;

                    wordCount2++;

                    // skip whitespace until next word
                    while (index2 < countingwords2.Length && Char.IsWhiteSpace(countingwords2[index2]) == true)
                        index2++;
                }
                textBox17.Text = wordCount2.ToString();
                ////Ambil nilai dari listbox sinonim untuk Stemming

                int angka2 = int.Parse(listBox10.Items[0].ToString());
                textBox9.Text = angka2.ToString();

                //Penentuan Akhir
                if (persenleven >= 1 && persenleven <= 30)
                {
                    label36.Text = "Plagiat Rendah";
                    label36.ForeColor = Color.GreenYellow;
                }
                else if (persenleven >= 31 && persenleven <= 70)
                {
                    label36.Text = "Plagiat Menengah";
                    label36.ForeColor = Color.Yellow;
                }
                else if (persenleven >= 71 && persenleven <= 100)
                {
                    label36.Text = "Plagiat Tinggi";
                    label36.ForeColor = Color.Red;
                }
                else if (persenleven == 0)
                {
                    label36.Text = "Tidak ada Plagiat";
                    label36.ForeColor = Color.White;
                }

                if (hasilsinonim >= 1 && hasilsinonim <= 30)
                {
                    label35.Text = "Plagiat Rendah";
                    label35.ForeColor = Color.GreenYellow;
                }
                else if (hasilsinonim >= 31 && hasilsinonim <= 70)
                {
                    label35.Text = "Plagiat Menengah";
                    label35.ForeColor = Color.Yellow;
                }
                else if (hasilsinonim >= 71 && hasilsinonim <= 100)
                {
                    label35.Text = "Plagiat Tinggi";
                    label35.ForeColor = Color.Red;
                }
                else if (hasilsinonim == 0)
                {
                    label35.Text = "Tidak ada Plagiat";
                    label35.ForeColor = Color.White;
                }

            }
            else if (storevariable.cekstemmingresult == false)
            {
                metroTile6.Show();
                groupBox5.Show();
                groupBox6.Show();
                label2.Hide();
                label4.Hide();
                label5.Hide();

                //levensthein
                float hasiltotalleven = 0;
                for (int i = 0; i < listBox5.Items.Count; i++)
                {
                    if ((int)listBox5.Items[i] == 0)
                        hasiltotalleven += 1;
                }
                float persenleven;
                persenleven = (hasiltotalleven / Math.Max(listBox3.Items.Count, listBox6.Items.Count) * 100f);
                textBox5.Text = persenleven.ToString();
                textBox8.Text = hasiltotalleven.ToString();
                //sinonim
                float hasilsinonim = ((float.Parse(listBox10.Items[0].ToString())) / listBox8.Items.Count) * 100f;
                textBox6.Text = hasilsinonim.ToString();
                ////Counting Total words in Textbox
                String countingwords = textBox1.Text.Trim();
                string countingwords2 = textBox2.Text.Trim();
                int wordCount = 0, index = 0;
                int wordCount2 = 0, index2 = 0;
                while (index < countingwords.Length)
                {
                    // check if current char is part of a word
                    while (index < countingwords.Length && Char.IsWhiteSpace(countingwords[index]) == false)
                        index++;

                    wordCount++;

                    // skip whitespace until next word
                    while (index < countingwords.Length && Char.IsWhiteSpace(countingwords[index]) == true)
                        index++;
                }
                textBox10.Text = wordCount.ToString();
                while (index2 < countingwords2.Length)
                {
                    // check if current char is part of a word
                    while (index2 < countingwords2.Length && Char.IsWhiteSpace(countingwords2[index2]) == false)
                        index2++;

                    wordCount2++;

                    // skip whitespace until next word
                    while (index2 < countingwords2.Length && Char.IsWhiteSpace(countingwords2[index2]) == true)
                        index2++;
                }
                textBox17.Text = wordCount2.ToString();
                //Ambil nilai dari listbox sinonim untuk Stemming
                int angka2 = int.Parse(listBox10.Items[0].ToString());
                textBox9.Text = angka2.ToString();
                //Penentuan Akhir
                if (persenleven >= 1 && persenleven <= 30)
                {
                    label36.Text = "Plagiat Rendah";
                    label36.ForeColor = Color.GreenYellow;
                }
                else if (persenleven >= 31 && persenleven <= 70)
                {
                    label36.Text = "Plagiat Menengah";
                    label36.ForeColor = Color.Yellow;
                }
                else if (persenleven >= 71 && persenleven <= 100)
                {
                    label36.Text = "Plagit Tinggi";
                    label36.ForeColor = Color.Red;
                }
                else if (persenleven == 0)
                {
                    label36.Text = "Tidak ada Plagiat";
                    label36.ForeColor = Color.Green;
                }

                if (hasilsinonim >= 1 && hasilsinonim <= 30)
                {
                    label35.Text = "Plagiat Rendah";
                    label35.ForeColor = Color.GreenYellow;
                }
                else if (hasilsinonim >= 31 && hasilsinonim <= 70)
                {
                    label35.Text = "Plagiat Menengah";
                    label35.ForeColor = Color.Yellow;
                }
                else if (hasilsinonim >= 71 && hasilsinonim <= 100)
                {
                    label35.Text = "Plagiat Tinggi";
                    label35.ForeColor = Color.Red;
                }
                else if (hasilsinonim == 0)
                {
                    label35.Text = "Tidak ada Plagiat";
                    label35.ForeColor = Color.Green;
                }
            }
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "PDF Files(*.PDF)|*.PDF|ALL Files(*.*)|*.*";

            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                metroTextBox2.Show();
                textBox2.Show();
                metroTile2.Show();
                string filePath = openDlg.FileName.ToString();
                filePath = openDlg.FileName.ToString();
                metroTextBox1.Text = filePath;
                metroTextBox1.Enabled = false;
                FileInfo inf = new FileInfo(filePath);
                long lengthinf = inf.Length / 1024;
                string fordislay = lengthinf.ToString("N0") + "KB";
                textBox35.Text = fordislay;
                textBox34.Text = filePath;
                textBox33.Text = System.IO.Path.GetFileName(filePath);
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


        private void metroTile6_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("You will Check new file. Continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                label5.Show();
                label2.Show();
                metroTile2.Hide();
                metroTextBox2.Hide();
                textBox2.Hide();
                groupBox1.Hide();
                materialRadioButton1.Checked = false;
                materialRadioButton2.Checked = false;
                metroTile3.Hide();
                metroTile3.Enabled = true;
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                listBox3.Items.Clear();

                listBox5.Items.Clear();
                listBox6.Items.Clear();
                listBox7.Items.Clear();
                listBox8.Items.Clear();

                listBox10.Items.Clear();
                listBox11.Items.Clear();
                listBox13.Items.Clear();
                listBox1.Hide();
                listBox2.Hide();
                listBox3.Hide();

                listBox5.Hide();
                listBox6.Hide();
                listBox7.Hide();
                listBox8.Hide();

                listBox10.Hide();
                materialLabel1.Hide();
                materialLabel2.Hide();
                materialLabel3.Hide();
                materialLabel4.Hide();
                materialLabel5.Hide();
                label3.Show();
                label1.Show();
                groupBox2.Hide();
                groupBox5.Hide();
                groupBox6.Hide();
                groupBox7.Hide();
                metroTile6.Hide();
                metroTile7.Hide();
                metroTile4.Hide();
                metroTile5.Hide();
                TabPage t = tabControl1.TabPages[0];
                tabControl1.SelectedTab = t;
                metroTextBox1.Clear();
                metroTextBox2.Clear();
                textBox1.Clear();
                textBox2.Clear();
                metroProgressBar1.Hide();
                metroProgressBar1.Value = 0;
                label35.Hide();
                label36.Hide();
                label14.Hide();
                label15.Hide();
                label16.Hide();
                label17.Hide();
                label18.Hide();
                label19.Hide();
                label37.Hide();
                label38.Hide();
                textBox8.Hide();
                textBox9.Hide();
                textBox10.Hide();
                textBox17.Hide();
                listBox11.Hide();
                listBox13.Hide();
            }
            else
            {

            }
        }
        private void metroTile7_Click(object sender, EventArgs e)
        {
            TabPage t = tabControl1.TabPages[1];
            tabControl1.SelectedTab = t;
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.Image = Plagiarism_Checker.Properties.Resources.info2;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Image = Plagiarism_Checker.Properties.Resources.info;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            About ab = new About();
            ab.Show();
        }

        private void metroTile4_Click(object sender, EventArgs e)
        {
            #region
            DialogResult dialogResult = MessageBox.Show("If you go back, all process will be resetted and previous file be load back. Continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                label5.Show();
                label2.Show();
                metroTile2.Hide();
                metroTextBox2.Hide();
                textBox2.Hide();
                groupBox1.Hide();
                materialRadioButton1.Checked = false;
                materialRadioButton2.Checked = false;
                metroTile3.Enabled = true;
                metroTile3.Hide();
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                listBox3.Items.Clear();

                listBox5.Items.Clear();
                listBox6.Items.Clear();
                listBox7.Items.Clear();
                listBox8.Items.Clear();

                listBox10.Items.Clear();
                listBox11.Items.Clear();
                listBox13.Items.Clear();
                listBox1.Hide();
                listBox2.Hide();
                listBox3.Hide();

                listBox5.Hide();
                listBox6.Hide();
                listBox7.Hide();
                listBox8.Hide();

                listBox10.Hide();
                materialLabel1.Hide();
                materialLabel2.Hide();
                materialLabel3.Hide();
                materialLabel4.Hide();
                materialLabel5.Hide();
                label3.Show();
                label1.Show();
                groupBox2.Hide();
                groupBox5.Hide();
                groupBox6.Hide();
                groupBox7.Hide();
                metroTile6.Hide();
                metroTile7.Hide();
                metroTile4.Hide();
                metroTile5.Hide();
                TabPage t = tabControl1.TabPages[0];
                tabControl1.SelectedTab = t;
                metroTextBox1.Clear();
                metroTextBox2.Clear();
                textBox1.Clear();
                textBox2.Clear();
                metroProgressBar1.Hide();
            #endregion
            }
            else
            {

            }

        }
    }

}
