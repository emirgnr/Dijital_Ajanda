using System;
using System.Collections;
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
    public partial class Gunluk : Form
    {
        public Gunluk()
        {
            InitializeComponent();
        }

        DateTime bugun = DateTime.Today;

        int Move;
        int Mouse_X;
        int Mouse_Y;

        public string sessionKAdi;

        SqlConnection baglanti;
        SqlDataReader okuyucu;
        SqlCommand komut;

        string baglantiDizesi = "Data Source=LENOVO;Initial Catalog=DB_AJANDA;Integrated Security=True";

        private void Gunluk_Load(object sender, EventArgs e)
        {
            lbl_tarih.Text = bugun.ToString("d");
            lbl_sessionKAdi.Text = sessionKAdi;

            baglanti = new SqlConnection(baglantiDizesi);

            baglanti.Open();

            komut = new SqlCommand();
            komut.Connection = baglanti;

            komut.CommandText = "SELECT * FROM notlarim where kullanici_adi = @k1";
            komut.Parameters.AddWithValue("@k1", sessionKAdi);
            okuyucu = komut.ExecuteReader();

            while (okuyucu.Read())
            {
                lst_Notlarim.Items.Add(okuyucu[3]);
            }

            baglanti.Close();

            lbl_oynatilanGorselBaslik.Hide();
            lbl_oynatilanSesBaslik.Hide();
            lbl_notTarihiBaslik.Hide();

        }

        private void btn_programKapat_Click(object sender, EventArgs e)
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

        private void Gunluk_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
        }

        private void Gunluk_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }

        private void Gunluk_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
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

        private void btn_hesapMakinesi_Click(object sender, EventArgs e)
        {
            HesapMakinesi hesapMSayfa = new HesapMakinesi();
            hesapMSayfa.sessionKAdi = sessionKAdi;
            hesapMSayfa.Show();
            this.Hide();
        }

        private byte[] imageData;
        private string imageName;

        private void btn_GorselEkle_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Resim Dosyaları (*.jpg, *.png, *.gif)|*.jpg;*.png;*.gif";
            openFileDialog.Title = "Görsel Seç";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    imageName = Path.GetFileName(openFileDialog.FileName);
                    imageData = File.ReadAllBytes(openFileDialog.FileName);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Görsel seçerken bir sorun oluştu lütfen tekrar deneyin! ", "Görsel Seçme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            lbl_secilenGorsel.Text = imageName;
        }

        private byte[] sesData;
        private string sesName;

        private void btn_SesEkle_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Ses Dosyaları|*.wav;*.mp3";
            openFileDialog.Title = "Ses Dosyası Seç";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    sesName = Path.GetFileName(openFileDialog.FileName);
                    sesData = File.ReadAllBytes(openFileDialog.FileName);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ses seçerken bir sorun oluştu lütfen tekrar deneyin! ", "Ses Seçme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            lbl_secilenSes.Text = sesName;
        }

        private void btn_notKaydet_Click(object sender, EventArgs e)
        {

            using (SqlConnection baglanti = new SqlConnection(baglantiDizesi))
            {
                if (!string.IsNullOrWhiteSpace(txt_Baslik.Text) && !string.IsNullOrWhiteSpace(rtb_Notum.Text))
                {
                    if (!lst_Notlarim.Items.Cast<string>().Any(x => x.Equals(txt_Baslik.Text, StringComparison.OrdinalIgnoreCase))) // !lst_Notlarim.Items.Contains(txt_Baslik.Text))
                    {
                        baglanti.Open();

                        using (SqlCommand komut = new SqlCommand())
                        {
                            komut.Connection = baglanti;

                            if (imageData == null && sesData == null)
                            {
                                komut.CommandText = "INSERT INTO notlarim (kullanici_adi, not_tarih, not_baslik, not_metni) values (@p1, @p2, @p3, @p4)";

                                komut.Parameters.AddWithValue("@p1", sessionKAdi);
                                komut.Parameters.AddWithValue("@p2", bugun);
                                komut.Parameters.AddWithValue("@p3", txt_Baslik.Text);
                                komut.Parameters.AddWithValue("@p4", rtb_Notum.Text);

                                komut.ExecuteNonQuery();

                            }
                            else if (imageData == null && sesData != null)
                            {
                                komut.CommandText = "INSERT INTO notlarim " +
                                    "(kullanici_adi, not_tarih, not_baslik, not_metni, not_sesAdi, not_sesData) " +
                                    "values (@p1, @p2, @p3, @p4, @p5 ,@p6)";

                                komut.Parameters.AddWithValue("@p1", sessionKAdi);
                                komut.Parameters.AddWithValue("@p2", bugun);
                                komut.Parameters.AddWithValue("@p3", txt_Baslik.Text);
                                komut.Parameters.AddWithValue("@p4", rtb_Notum.Text);
                                komut.Parameters.AddWithValue("@p5", sesName);
                                komut.Parameters.AddWithValue("@p6", sesData);

                                komut.ExecuteNonQuery();

                            }
                            else if (sesData == null && imageData != null)
                            {
                                komut.CommandText = "INSERT INTO notlarim " +
                                    "(kullanici_adi, not_tarih, not_baslik, not_metni, not_gorselAdi, not_gorselData) " +
                                    "values (@p1, @p2, @p3, @p4, @p5 ,@p6)";

                                komut.Parameters.AddWithValue("@p1", sessionKAdi);
                                komut.Parameters.AddWithValue("@p2", bugun);
                                komut.Parameters.AddWithValue("@p3", txt_Baslik.Text);
                                komut.Parameters.AddWithValue("@p4", rtb_Notum.Text);
                                komut.Parameters.AddWithValue("@p5", imageName);
                                komut.Parameters.AddWithValue("@p6", imageData);

                                komut.ExecuteNonQuery();

                            }
                            else if (sesData != null && imageData != null)
                            {
                                komut.CommandText = @"INSERT INTO notlarim
                                    (kullanici_adi, not_tarih, not_baslik, not_metni, not_gorselAdi, not_gorselData, not_sesAdi, not_sesData)
                                    values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8)";

                                komut.Parameters.AddWithValue("@p1", sessionKAdi);
                                komut.Parameters.AddWithValue("@p2", bugun);
                                komut.Parameters.AddWithValue("@p3", txt_Baslik.Text);
                                komut.Parameters.AddWithValue("@p4", rtb_Notum.Text);
                                komut.Parameters.AddWithValue("@p5", imageName);
                                komut.Parameters.AddWithValue("@p6", imageData);
                                komut.Parameters.AddWithValue("@p7", sesName);
                                komut.Parameters.AddWithValue("@p8", sesData);

                                komut.ExecuteNonQuery();
                            }

                            lst_Notlarim.Items.Clear();

                            komut.CommandText = "SELECT * FROM notlarim where kullanici_adi = @k1";
                            komut.Parameters.AddWithValue("@k1", sessionKAdi);
                            okuyucu = komut.ExecuteReader();

                            while (okuyucu.Read())
                            {
                                lst_Notlarim.Items.Add(okuyucu[3]);
                            }
                        }

                        baglanti.Close();

                        imageName = null;
                        imageData = null;
                        sesName = null;
                        sesData = null;
                        txt_Baslik.Text = "";
                        rtb_Notum.Text = "";
                        lbl_secilenGorsel.Text = "";
                        lbl_secilenSes.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Bu başlıkta bir notunuz zaten var lütfen farklı bir başlık giriniz!", "Başlık Hatası");
                    }
                }
                else
                {
                    MessageBox.Show("Başlık ve not kısmı boş bırakılamaz. Lütfen birkaç metin giriniz!", "Metin Hatası");
                }
            }
        }

        private void btn_Temizle_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void Temizle()
        {
            txt_Baslik.Text = "";
            rtb_Notum.Text = "";

            imageName = null;
            imageData = null;

            sesData = null;
            sesName = null;

            lbl_oynatilanGorsel.Text = null;
            lbl_oynatilanSes.Text = null;
            lbl_notTarihi.Text = null;
            lbl_secilenGorsel.Text = null;
            lbl_secilenSes.Text = null;
            axWindowsMediaPlayer1.URL = null;
            pb_resim.Image = null;

            lbl_oynatilanGorselBaslik.Hide();
            lbl_oynatilanSesBaslik.Hide();
            lbl_notTarihiBaslik.Hide();
        }

        string notTarihi;

        private void lst_Notlarim_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (lst_Notlarim.SelectedIndex != -1)
            {
                lbl_oynatilanGorselBaslik.Show();
                lbl_oynatilanSesBaslik.Show();
                lbl_notTarihiBaslik.Show();

                string secilenNotBaslik = lst_Notlarim.SelectedItem.ToString();
                using (baglanti = new SqlConnection(baglantiDizesi))
                {
                    baglanti.Open();

                    SqlCommand komut = new SqlCommand("SELECT * FROM notlarim WHERE kullanici_adi = @p1 and not_baslik = @p2", baglanti);
                    komut.Parameters.AddWithValue("@p1", sessionKAdi);
                    komut.Parameters.AddWithValue("@p2", secilenNotBaslik);

                    using (SqlDataReader okuyucu = komut.ExecuteReader())
                    {
                        if (okuyucu.Read())
                        {
                            string notBaslik = okuyucu[3].ToString();
                            string notMetni = okuyucu[4].ToString();
                            string gorselAdi = okuyucu[5].ToString();
                            string sesAdi = okuyucu[7].ToString();

                            if (okuyucu.IsDBNull(okuyucu.GetOrdinal("not_gorselData")) && okuyucu.IsDBNull(okuyucu.GetOrdinal("not_sesData")))
                            {
                                pb_resim.Image = null;
                                axWindowsMediaPlayer1.URL = null;

                                lbl_oynatilanGorsel.Text = "Görsel Eklenmedi";
                                lbl_oynatilanSes.Text = "Ses Eklenmedi";
                            }
                            else if (!okuyucu.IsDBNull(okuyucu.GetOrdinal("not_gorselData")) && okuyucu.IsDBNull(okuyucu.GetOrdinal("not_sesData")))
                            {
                                byte[] gorselVerisi = (byte[])okuyucu["not_gorselData"];
                                using (MemoryStream ms = new MemoryStream(gorselVerisi))
                                {
                                    Image image = Image.FromStream(ms);
                                    pb_resim.Image = image;
                                }

                                lbl_oynatilanGorsel.Text = gorselAdi;

                                axWindowsMediaPlayer1.URL = null;
                                lbl_oynatilanSes.Text = "Ses Eklenmedi";

                            }
                            else if (okuyucu.IsDBNull(okuyucu.GetOrdinal("not_gorselData")) && !okuyucu.IsDBNull(okuyucu.GetOrdinal("not_sesData")))
                            {

                                byte[] audioBytes = sesDatasiGetir(notBaslik);

                                string tempFilePath = SaveAudioToFile(audioBytes);
                                axWindowsMediaPlayer1.URL = tempFilePath;
                                axWindowsMediaPlayer1.Ctlcontrols.play();

                                lbl_oynatilanSes.Text = sesAdi;

                                pb_resim.Image = null;
                                lbl_oynatilanGorsel.Text = "Görsel Eklenmedi";

                            }
                            else if (!okuyucu.IsDBNull(okuyucu.GetOrdinal("not_gorselData")) && !okuyucu.IsDBNull(okuyucu.GetOrdinal("not_sesData")))
                            {
                                byte[] gorselVerisi = (byte[])okuyucu["not_gorselData"];
                                using (MemoryStream ms = new MemoryStream(gorselVerisi))
                                {
                                    Image image = Image.FromStream(ms);
                                    pb_resim.Image = image;
                                }

                                byte[] audioBytes = sesDatasiGetir(notBaslik);

                                string tempFilePath = SaveAudioToFile(audioBytes);
                                axWindowsMediaPlayer1.URL = tempFilePath;
                                axWindowsMediaPlayer1.Ctlcontrols.play();

                                lbl_oynatilanGorsel.Text = gorselAdi;
                                lbl_oynatilanSes.Text = sesAdi;
                            }

                            okuyucu.Close();


                            using (SqlCommand command = new SqlCommand("SELECT not_tarih FROM notlarim WHERE kullanici_adi = @p1 and not_baslik = @p2", baglanti))
                            {
                                command.Parameters.AddWithValue("@p1", sessionKAdi);
                                command.Parameters.AddWithValue("@p2", notBaslik);
                                object sonuc = command.ExecuteScalar();

                                if (sonuc != null)
                                {
                                    DateTime tarih = (DateTime)sonuc;
                                    notTarihi = tarih.ToString("dd.MM.yyyy");
                                }
                            }

                            txt_Baslik.Text = notBaslik;
                            rtb_Notum.Text = notMetni;
                            lbl_notTarihi.Text = notTarihi;
                        }
                    }

                    baglanti.Close();
                }
            }
        }

        private byte[] sesDatasiGetir(string notBaslik)
        {
            byte[] audioBytes = null;

            using (SqlConnection baglanti = new SqlConnection(baglantiDizesi))
            {
                baglanti.Open();
                string sorgu = "SELECT not_sesData FROM notlarim WHERE not_baslik = @p1 and  kullanici_adi = @p2";
                using (SqlCommand komut = new SqlCommand(sorgu, baglanti))
                {
                    komut.Parameters.AddWithValue("@p1", notBaslik);
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

        private void btn_notuSil_Click(object sender, EventArgs e)
        {
            if (lst_Notlarim.SelectedIndex != -1)
            {

                string selectedBaslik = lst_Notlarim.SelectedItem.ToString();
                string sorgu = "DELETE FROM notlarim WHERE kullanici_adi=@kAdi and not_baslik = @baslik";

                using (SqlConnection baglanti = new SqlConnection(baglantiDizesi))
                {
                    baglanti.Open();

                    using (SqlCommand komut = new SqlCommand(sorgu, baglanti))
                    {
                        komut.Parameters.AddWithValue("@kAdi", sessionKAdi);
                        komut.Parameters.AddWithValue("@baslik", selectedBaslik);
                        komut.ExecuteNonQuery();


                        lst_Notlarim.Items.Clear();

                        komut.CommandText = "SELECT * FROM notlarim where kullanici_adi = @k1";
                        komut.Parameters.Clear();
                        komut.Parameters.AddWithValue("@k1", sessionKAdi);
                        SqlDataReader okuyucu = komut.ExecuteReader();

                        while (okuyucu.Read())
                        {
                            lst_Notlarim.Items.Add(okuyucu.GetString(3));
                        }

                        Temizle();
                    }
                }
            }
        }
    }
}
