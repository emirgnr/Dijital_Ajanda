using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GORSELP_PROJE
{
    public partial class AnaSayfa : Form
    {
        public AnaSayfa()
        {
            InitializeComponent();
        }

        int Move;
        int Mouse_X;
        int Mouse_Y;
        public string sessionKAdi;

        SqlConnection baglanti;
        SqlDataReader okuyucu;
        SqlCommand komut;

        string baglantiDizesi = "Data Source=LENOVO;Initial Catalog=DB_AJANDA;Integrated Security=True";

        private void btn_programKapat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_asagiIndir_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void AnaSayfa_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
        }

        private void AnaSayfa_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }

        private void AnaSayfa_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }

        private void AnaSayfa_Load(object sender, EventArgs e)
        {
            lbl_sessionKAdi.Text = sessionKAdi;
            baglanti = new SqlConnection(baglantiDizesi);

            baglanti.Open();

            komut = new SqlCommand();
            komut.Connection = baglanti;

            komut.CommandText = "SELECT * FROM yapilacaklarim where kullanici_adi = @k1";
            komut.Parameters.AddWithValue("@k1", sessionKAdi);
            okuyucu = komut.ExecuteReader();

            while (okuyucu.Read())
            {
                lst_yapilacaklar.Items.Add(okuyucu[2]);
            }

            okuyucu.Close();

            komut.CommandText = "SELECT * FROM tamamlananlarim where kullanici_adi = @k2";
            komut.Parameters.AddWithValue("@k2", sessionKAdi);
            okuyucu = komut.ExecuteReader();

            while (okuyucu.Read())
            {
                lst_tamamlananlar.Items.Add(okuyucu[2]);
            }

            okuyucu.Close();

            baglanti.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form1 giris = new Form1();
            giris.Show();
            this.Hide();
        }

        private void btn_gorsellerim_Click(object sender, EventArgs e)
        {
            Gorsellerim gorselSayfa = new Gorsellerim();
            gorselSayfa.sessionKAdi = sessionKAdi;
            gorselSayfa.Show();
            this.Hide();
        }

        private void btn_hesapMakinesi_Click(object sender, EventArgs e)
        {
            HesapMakinesi hesapMSayfa = new HesapMakinesi();
            hesapMSayfa.sessionKAdi = sessionKAdi;
            hesapMSayfa.Show();
            this.Hide();
        }

        private void btn_seslerim_Click(object sender, EventArgs e)
        {
            Seslerim seslerimSayfa = new Seslerim();
            seslerimSayfa.sessionKAdi = sessionKAdi;
            seslerimSayfa.Show();
            this.Hide();
        }

        private void btn_dosyaIslemleri_Click(object sender, EventArgs e)
        {
            DosyaIslemleri dosyaIslemSayfa = new DosyaIslemleri();
            dosyaIslemSayfa.sessionKAdi = sessionKAdi;
            dosyaIslemSayfa.Show();
            this.Hide();
        }

        private void btn_gunluk_Click(object sender, EventArgs e)
        {
            Gunluk gunlukSayfa = new Gunluk();
            gunlukSayfa.sessionKAdi = sessionKAdi;
            gunlukSayfa.Show();
            this.Hide();
        }

        private void tamamlandi_btn_Click(object sender, EventArgs e)
        {
            if (lst_yapilacaklar.SelectedIndex != -1)
            {
                string secilenYapilacak = lst_yapilacaklar.SelectedItem.ToString();

                using (baglanti = new SqlConnection(baglantiDizesi))
                {
                    baglanti.Open();

                    SqlCommand komut = new SqlCommand("SELECT * FROM yapilacaklarim WHERE kullanici_adi = @p1 and yapilacak_metni = @p2", baglanti);
                    komut.Parameters.AddWithValue("@p1", sessionKAdi);
                    komut.Parameters.AddWithValue("@p2", secilenYapilacak);

                    using (SqlDataReader okuyucu = komut.ExecuteReader())
                    {
                        if (okuyucu.Read())
                        {
                            string kullaniciAdi = okuyucu[1].ToString();
                            string yapilacakMetni = okuyucu[2].ToString();

                            okuyucu.Close();

                            SqlCommand insertKomut = new SqlCommand("INSERT INTO tamamlananlarim (kullanici_adi, tamamlanan_metni) VALUES (@p1, @p2)", baglanti);
                            insertKomut.Parameters.AddWithValue("@p1", kullaniciAdi);
                            insertKomut.Parameters.AddWithValue("@p2", yapilacakMetni);

                            insertKomut.ExecuteNonQuery();

                            SqlCommand deleteKomut = new SqlCommand("DELETE FROM yapilacaklarim WHERE kullanici_adi = @p1 and yapilacak_metni = @p2", baglanti);
                            deleteKomut.Parameters.AddWithValue("@p1", sessionKAdi);
                            deleteKomut.Parameters.AddWithValue("@p2", secilenYapilacak);
                            deleteKomut.ExecuteNonQuery();
                        }
                    }

                    komut = new SqlCommand("SELECT * FROM yapilacaklarim WHERE kullanici_adi = @p1", baglanti);
                    komut.Parameters.AddWithValue("@p1", sessionKAdi);
                    using (SqlDataReader yapilacaklarOkuyucu = komut.ExecuteReader())
                    {
                        lst_yapilacaklar.Items.Clear();
                        while (yapilacaklarOkuyucu.Read())
                        {
                            lst_yapilacaklar.Items.Add(yapilacaklarOkuyucu.GetString(2));
                        }
                    }

                    komut = new SqlCommand("SELECT * FROM tamamlananlarim WHERE kullanici_adi = @p1", baglanti);
                    komut.Parameters.AddWithValue("@p1", sessionKAdi);
                    using (SqlDataReader tamamlananOkuyucu = komut.ExecuteReader())
                    {
                        lst_tamamlananlar.Items.Clear();
                        while (tamamlananOkuyucu.Read())
                        {
                            lst_tamamlananlar.Items.Add(tamamlananOkuyucu.GetString(2));
                        }
                    }
                }
            }
        }

        private void txt_yapilacak_TextChanged(object sender, EventArgs e)
        {
            int MaxKarakterSayisi = 50;

            int girilenKarakter = txt_yapilacak.Text.Length;

            int kalanKarakter = MaxKarakterSayisi - girilenKarakter;

            lbl_kalanKarakter.Text = string.Format("{0}", kalanKarakter);
        }

        private void btn_yapilacakEkle_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(txt_yapilacak.Text))
            {
                if (txt_yapilacak.Text.Length <= 50)
                {
                    if (!lst_yapilacaklar.Items.Cast<string>().Any(x => x.Equals(txt_yapilacak.Text, StringComparison.OrdinalIgnoreCase)))
                    {
                        using (SqlConnection baglanti = new SqlConnection(baglantiDizesi))
                        {
                            baglanti.Open();

                            using (SqlCommand komut = new SqlCommand())
                            {
                                komut.Connection = baglanti;
                                komut.CommandText = "INSERT INTO yapilacaklarim (kullanici_adi, yapilacak_metni) values (@p1, @p2)";
                                komut.Parameters.AddWithValue("@p1", sessionKAdi);
                                komut.Parameters.AddWithValue("@p2", txt_yapilacak.Text);
                                komut.ExecuteNonQuery();
                            }

                            lst_yapilacaklar.Items.Clear();

                            using (SqlCommand komut = new SqlCommand())
                            {
                                komut.Connection = baglanti;
                                komut.CommandText = "SELECT * FROM yapilacaklarim where kullanici_adi = @kullanici_adi";
                                komut.Parameters.AddWithValue("@kullanici_adi", sessionKAdi);

                                using (SqlDataReader okuyucu = komut.ExecuteReader())
                                {
                                    while (okuyucu.Read())
                                    {
                                        lst_yapilacaklar.Items.Add(okuyucu[2]);
                                    }
                                }
                            }
                        }
                        txt_yapilacak.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Böyle bir yapılacak zaten var lütfen farklı bir şey giriniz!", "Yapılacak Hatası");
                    }
                }
                else
                {
                    MessageBox.Show("Girilen metin 50 karakterden uzun olamaz!", "Karakter Uzunluğu Hatası");
                }
            }
            else
            {
                MessageBox.Show("Metin kısmı boş bırakılamaz lütfen birkaç metin giriniz!", "Metin Hatası");
            }

        }

        private void btn_yapilacakSil_Click(object sender, EventArgs e)
        {
            if (lst_yapilacaklar.SelectedIndex != -1)
            {

                string selectedYapilacak = lst_yapilacaklar.SelectedItem.ToString();
                string sorgu = "DELETE FROM yapilacaklarim WHERE yapilacak_metni = @p1";

                using (SqlConnection baglanti = new SqlConnection(baglantiDizesi))
                {
                    baglanti.Open();

                    using (SqlCommand komut = new SqlCommand(sorgu, baglanti))
                    {
                        komut.Parameters.AddWithValue("@p1", selectedYapilacak);
                        komut.ExecuteNonQuery();


                        lst_yapilacaklar.Items.Clear();

                        komut.CommandText = "SELECT * FROM yapilacaklarim where kullanici_adi = @k1";
                        komut.Parameters.Clear();
                        komut.Parameters.AddWithValue("@k1", sessionKAdi);
                        SqlDataReader okuyucu = komut.ExecuteReader();

                        while (okuyucu.Read())
                        {
                            lst_yapilacaklar.Items.Add(okuyucu.GetString(2));
                        }
                    }
                }
            }
        }
    }
}
