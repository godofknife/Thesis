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
using System.Collections;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        string[] kamus;
        string connection = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=katadasar.accdb; Persist Security Info=False;";
        public Form1()
        {
            InitializeComponent();
        }
        private int menghitungjumlah(string Query)
        {
            int jumlah = 0;
            OleDbConnection db = new OleDbConnection(connection);
            db.Open();
            OleDbCommand cmd = db.CreateCommand();
            string sql = Query;
            cmd.CommandText = sql;
            OleDbDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                jumlah = Convert.ToInt32(reader.GetString(0));
             
            }
            db.Close();
            return jumlah;
        }
        
        private void listkamus()
        {
            OleDbConnection db = new OleDbConnection(connection);
            db.Open();
            OleDbCommand olcmd = db.CreateCommand();
            string sql = "SELECT * From Kata";
            olcmd.CommandText = sql;
            OleDbDataReader reader = olcmd.ExecuteReader();
            int banyak = menghitungjumlah("SELECT count(*)FROM Kata");
            kamus = new String[banyak];
            int i = 0;
            while(reader.Read())
            {
                kamus[i] = reader.GetString(0).ToString();
                i++;
            }
            db.Close();
        }

        private bool cekkatadasar(string kata)
        {
            int banyak = menghitungjumlah("SELECT count(*) FROM tb_Kata");
            for(int i=0; i<banyak -1; i++)
            {
                if(kata==kamus[i])
                {
                    return true;
                }
            }
            return false;
        }

        private string HapusAkhiranKepunyaan(string kata)
        {
            Match mth = Regex.Match(kata, @"([A-Za-z]+)([klt]ah|pun|ku|mu|nya)$", RegexOptions.IgnoreCase);
            if(mth.Success)
            {
                string key = mth.Groups[1].Value;
                return HapusAkhiranKepunyaan(key);
            }
            return HapusAkhiranKepunyaan(kata);
        }
        private string hapus_derivation_prefix(string kata)
        {
            string kataasal = kata;
            if(Regex.IsMatch(kata,"(me)$"))
            {
                string kata1 = Regex.Replace(kata, "(me)$", "");
                if(cekkatadasar(kata1))
                {
                    return kata1;
                }
            }
            if (Regex.IsMatch(kata, "(di)$"))
            {
                string kata1 = Regex.Replace(kata, "(di)$", "");
                if (cekkatadasar(kata1))
                {
                    return kata1;
                }
            }
            if (Regex.IsMatch(kata, "(ke)$"))
            {
                string kata1 = Regex.Replace(kata, "(ke)$", "");
                if (cekkatadasar(kata1))
                {
                    return kata1;
                }
            }
            return kataasal;
        }

        private string HapusAkhiranIanKan(string kata)
        {
            string kataasal = kata;
            if(Regex.IsMatch(kata,"(kan)$"))
            {
                string kata1 = Regex.Replace(kata, "(kan)$", "");
                if(cekkatadasar(kata1))
                {
                    return kata1;
                }
            }
            if(Regex.IsMatch(kata,"(i|an)$"))
            {
                string kata2 = Regex.Replace(kata, "(i|an)$", "");
                if(cekkatadasar(kata2))
                {
                    return kata2;
                }
            }
            return kataasal;
        }

        private string stemming(string kata)
        {
            if(cekkatadasar(kata))
            {
                return kata;
            }
            kata = HapusAkhiranKepunyaan(kata);
            kata = HapusAkhiranIanKan(kata);
            kata = hapus_derivation_prefix(kata);
            return kata;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listkamus();
            string kata;
            kata = textBox1.Text;
            kata = stemming(kata);
            textBox2.Text = kata;
        }
        
    }
}
