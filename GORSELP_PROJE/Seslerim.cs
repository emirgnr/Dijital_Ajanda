using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GORSELP_PROJE
{
    public partial class Seslerim : Form
    {
        public Seslerim()
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

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_asagiIndir_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form1 giris = new Form1();
            giris.Show();
            this.Hide();
        }

        private void Seslerim_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
        }

        private void Seslerim_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }

        private void Seslerim_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }

        private void Seslerim_Load(object sender, EventArgs e)
        {
            baglanti = new SqlConnection(baglantiDizesi);
            lbl_sessionKAdi.Text = sessionKAdi;
            lst_seslerim.Items.Clear();

            baglanti.Open();
            komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "SELECT * FROM seslerim where kullanici_adi = @kullanici_adi";
            komut.Parameters.AddWithValue("@kullanici_adi", sessionKAdi);
            okuyucu = komut.ExecuteReader();

            while (okuyucu.Read())
            {
                lst_seslerim.Items.Add(okuyucu[2]);
            }

            baglanti.Close();
        }

        private void btn_yapilacaklar_Click(object sender, EventArgs e)
        {
            AnaSayfa anaSayfa = new AnaSayfa();
            anaSayfa.sessionKAdi = sessionKAdi;
            anaSayfa.Show();
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

        private byte[] sesData;
        private string sesName;

        private void btn_sesEkle_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Ses Dosyaları|*.wav;*.mp3";
            openFileDialog.Title = "Ses Dosyası Seç";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                sesName = Path.GetFileName(openFileDialog.FileName);
                sesData = File.ReadAllBytes(openFileDialog.FileName);

                using (SqlConnection baglanti = new SqlConnection(baglantiDizesi))
                {
                    baglanti.Open();

                    using (SqlCommand komut = new SqlCommand())
                    {
                        komut.Connection = baglanti;

                        if (!lst_seslerim.Items.Cast<string>().Any(x => x.Equals(sesName, StringComparison.OrdinalIgnoreCase)))
                        {
                            komut.CommandText = "INSERT INTO seslerim (kullanici_adi, ses_adi, ses_Data) values (@p1, @p2, @p3)";
                            komut.Parameters.AddWithValue("@p1", sessionKAdi);
                            komut.Parameters.AddWithValue("@p2", sesName);
                            komut.Parameters.AddWithValue("@p3", sesData);
                            komut.ExecuteNonQuery();

                            lst_seslerim.Items.Clear();

                            komut.CommandText = "SELECT * FROM seslerim where kullanici_adi = @kullanici_adi";
                            komut.Parameters.AddWithValue("@kullanici_adi", sessionKAdi);

                            using (SqlDataReader okuyucu = komut.ExecuteReader())
                            {
                                while (okuyucu.Read())
                                {
                                    lst_seslerim.Items.Add(okuyucu[2]);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Seçtiğiniz ses adına sahip bir ses zaten bulunuyor lütfen ses dosyanızın adını değiştirin! ", "Ses Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void btn_sesSil_Click(object sender, EventArgs e)
        {
            if (lst_seslerim.SelectedIndex != -1)
            {

                string selectedSes = lst_seslerim.SelectedItem.ToString();
                string sorgu = "DELETE FROM seslerim WHERE ses_adi = @sesAdi";

                using (SqlConnection baglanti = new SqlConnection(baglantiDizesi))
                {
                    baglanti.Open();

                    using (SqlCommand komut = new SqlCommand(sorgu, baglanti))
                    {
                        komut.Parameters.AddWithValue("@sesAdi", selectedSes);
                        komut.ExecuteNonQuery();

                        lst_seslerim.Items.Clear();

                        komut.CommandText = "SELECT * FROM seslerim where kullanici_adi = @k1";
                        komut.Parameters.Clear();
                        komut.Parameters.AddWithValue("@k1", sessionKAdi);
                        SqlDataReader okuyucu = komut.ExecuteReader();

                        while (okuyucu.Read())
                        {
                            lst_seslerim.Items.Add(okuyucu.GetString(2));
                        }

                        axWindowsMediaPlayer1.URL = null;
                    }
                }
            }
        }

        private void lst_seslerim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lst_seslerim.SelectedIndex != -1)
            {

                string selectedSes = lst_seslerim.SelectedItem.ToString();

                using (baglanti = new SqlConnection(baglantiDizesi))
                {
                    baglanti.Open();

                    SqlCommand komut = new SqlCommand("SELECT * FROM seslerim WHERE kullanici_adi = @p1 and ses_adi = @p2", baglanti);
                    komut.Parameters.AddWithValue("@p1", sessionKAdi);
                    komut.Parameters.AddWithValue("@p2", selectedSes);

                    using (SqlDataReader okuyucu = komut.ExecuteReader())
                    {
                        if (okuyucu.Read())
                        {
                            string sesAdi = okuyucu[2].ToString();

                            byte[] audioBytes = sesDatasiGetir(selectedSes);

                            string tempFilePath = SaveAudioToFile(audioBytes);
                            axWindowsMediaPlayer1.URL = tempFilePath;
                            axWindowsMediaPlayer1.Ctlcontrols.play();

                            okuyucu.Close();

                        }
                    }

                    baglanti.Close();
                }
            }
        }

        private byte[] sesDatasiGetir(string selectedSes)
        {
            byte[] audioBytes = null;

            using (SqlConnection baglanti = new SqlConnection(baglantiDizesi))
            {
                baglanti.Open();
                string sorgu = "SELECT ses_Data FROM seslerim WHERE ses_adi = @p1 and  kullanici_adi = @p2";
                using (SqlCommand komut = new SqlCommand(sorgu, baglanti))
                {
                    komut.Parameters.AddWithValue("@p1", selectedSes);
                    komut.Parameters.AddWithValue("@p2", sessionKAdi);
                    using (SqlDataReader okuyucu = komut.ExecuteReader(CommandBehavior.SequentialAccess))
                    {
                        if (okuyucu.Read())
                        {
                            long bufferSize = okuyucu.GetBytes(0, 0, null, 0, 0);
                            audioBytes = new byte[bufferSize];
                            okuyucu.GetBytes(0, 0, audioBytes, 0, (int)bufferSize);
                        }
                    }
                }
                baglanti.Close();
            }

            return audioBytes;
        }

        private string SaveAudioToFile(byte[] audioBytes)
        {
            string tempFilePath = System.IO.Path.GetTempFileName();
            tempFilePath = System.IO.Path.ChangeExtension(tempFilePath, "mp3");

            System.IO.File.WriteAllBytes(tempFilePath, audioBytes);

            return tempFilePath;
        }
    }
}
