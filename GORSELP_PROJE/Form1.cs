using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace GORSELP_PROJE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection baglanti;
        SqlDataReader okuyucu;
        SqlCommand komut;

        string baglantiDizesi = "Data Source=LENOVO;Initial Catalog=DB_AJANDA;Integrated Security=True";

        private void Form1_Load(object sender, EventArgs e)
        {
            txt_sifre.PasswordChar = '*';
            baglanti = new SqlConnection(baglantiDizesi);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txt_sifre.PasswordChar = '\0';
            }
            else
            {
                txt_sifre.PasswordChar = '*';
            }
        }

        private void btn_giris_Click(object sender, EventArgs e)
        {

            baglanti.Open();

            string kullaniciAdi = txt_kullaniciAdi.Text;
            string sifre = txt_sifre.Text;

            komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "SELECT * FROM kullanicilar where kullanici_adi =@kullanici_adi";
            komut.Parameters.AddWithValue("@kullanici_adi", kullaniciAdi);

            okuyucu = komut.ExecuteReader();

            if (okuyucu.Read())
            {
                if (okuyucu["kullanici_adi"].ToString()== kullaniciAdi && okuyucu["sifre"].ToString() == sifre)
                {
                    
                    AnaSayfa anaSayfa = new AnaSayfa();
                    anaSayfa.sessionKAdi = kullaniciAdi;
                    anaSayfa.Show();
                    this.Hide();
                    
                }
                else
                {
                    MessageBox.Show("Hatalı giriş yaptınız lütfen kullanıcı adı ya da şifrenizi kontrol ediniz!");
                }
                
            }
            else
            {
                MessageBox.Show("Böyle bir kullanıcı bulunamadı!");
            }

            baglanti.Close();
        }
    }
}
