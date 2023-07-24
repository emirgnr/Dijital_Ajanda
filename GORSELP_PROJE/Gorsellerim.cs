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
    public partial class Gorsellerim : Form
    {
        public Gorsellerim()
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

        private void button1_Click(object sender, EventArgs e)
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

        private void Gorsellerim_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
        }

        private void Gorsellerim_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }

        private void Gorsellerim_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }

        private void Gorsellerim_Load(object sender, EventArgs e)
        {
            baglanti = new SqlConnection(baglantiDizesi);
            lbl_sessionKAdi.Text = sessionKAdi;
            lst_gorsellerim.Items.Clear();

            baglanti.Open();
            komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "SELECT * FROM gorsellerim where kullanici_adi = @kullanici_adi";
            komut.Parameters.AddWithValue("@kullanici_adi", sessionKAdi);
            okuyucu = komut.ExecuteReader();

            while (okuyucu.Read())
            {
                lst_gorsellerim.Items.Add(okuyucu[2]);
            }

            baglanti.Close();
        }

        private byte[] imageData;
        private string imageName;
        private void btn_gorselEkle_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Resim Dosyaları (*.jpg, *.jpeg, *.png, *.gif)|*.jpg;*.jpeg;*.png;*.gif";
            openFileDialog.Title = "Görsel Seç";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                imageName = Path.GetFileName(openFileDialog.FileName);
                imageData = File.ReadAllBytes(openFileDialog.FileName);

                baglanti.Open();
                komut = new SqlCommand();
                komut.Connection = baglanti;

                if (!lst_gorsellerim.Items.Cast<string>().Any(x => x.Equals(imageName, StringComparison.OrdinalIgnoreCase)))
                {
                    komut.CommandText = "INSERT INTO gorsellerim (kullanici_adi, gorsel_adi, gorsel_Data) values (@p1, @p2, @p3)";
                    komut.Parameters.AddWithValue("@p1", sessionKAdi);
                    komut.Parameters.AddWithValue("@p2", imageName);
                    komut.Parameters.AddWithValue("@p3", imageData);
                    komut.ExecuteNonQuery();

                    lst_gorsellerim.Items.Clear();

                    komut.CommandText = "SELECT * FROM gorsellerim where kullanici_adi = @kullanici_adi";
                    komut.Parameters.AddWithValue("@kullanici_adi", sessionKAdi);
                    okuyucu = komut.ExecuteReader();

                    while (okuyucu.Read())
                    {
                        lst_gorsellerim.Items.Add(okuyucu[2]);
                    }
                    
                }
                else
                {
                    MessageBox.Show("Seçtiğiniz görsel adına sahip bir görsel zaten bulunuyor lütfen görsel adını değiştirin! ", "Görsel Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                baglanti.Close();
            }
        }

        private void lst_gorsellerim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lst_gorsellerim.SelectedIndex != -1)
            {
                string secilenResimAdi = lst_gorsellerim.SelectedItem.ToString();
                ResmiGoster(secilenResimAdi);
            }
        }

        private void ResmiGoster(string resimAdi)
        {

            baglanti.Open();
            komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "SELECT * FROM gorsellerim WHERE kullanici_adi=@kAdi and gorsel_adi = @gorsel_adi";

            komut.Parameters.AddWithValue("@kAdi", sessionKAdi);
            komut.Parameters.AddWithValue("@gorsel_adi", resimAdi);
            okuyucu = komut.ExecuteReader();

            if (okuyucu.Read())
            {
                byte[] gorselVerisi = (byte[])okuyucu["gorsel_Data"];
                using (MemoryStream ms = new MemoryStream(gorselVerisi))
                {
                    Image image = Image.FromStream(ms);
                    pb_resim.Image = image;
                }
            }

            baglanti.Close();
        }

        private bool tamEkranModu = false;
        private Form tamEkranForm;
        private PictureBox tamEkranPictureBox;
        private Size eskiBoyut;
        private Form eskiForm;

        private void btn_tamEkran_Click(object sender, EventArgs e)
        {
            if (!tamEkranModu)
            {
                eskiForm = this;
                eskiBoyut = pb_resim.Size;

                tamEkranForm = new Form();
                tamEkranPictureBox = new PictureBox();

                tamEkranForm.FormBorderStyle = FormBorderStyle.None;
                tamEkranForm.WindowState = FormWindowState.Maximized;
                tamEkranForm.BackColor = Color.Black;

                tamEkranPictureBox.Image = pb_resim.Image;
                tamEkranPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                tamEkranPictureBox.Dock = DockStyle.Fill;

                tamEkranForm.Controls.Add(tamEkranPictureBox);
                tamEkranPictureBox.DoubleClick += TamEkranPictureBox_DoubleClick;

                tamEkranForm.ShowDialog();

                tamEkranModu = true;
            }
            else
            {
                tamEkranForm.Close();
                tamEkranModu = false;
                eskiForm.WindowState = FormWindowState.Normal;
                eskiForm.Show();
                pb_resim.Size = eskiBoyut;

                eskiForm = this;
                eskiBoyut = pb_resim.Size;
            }
        }

        private void TamEkranPictureBox_DoubleClick(object sender, EventArgs e)
        {
            TamEkranModunuKapat();
        }

        private void TamEkranModunuKapat()
        {
            tamEkranForm.Close();
            tamEkranModu = false;
            eskiForm.WindowState = FormWindowState.Normal;
            eskiForm.Show();
            pb_resim.Size = eskiBoyut;

            eskiForm = this;
            eskiBoyut = pb_resim.Size;
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            Form1 giris = new Form1();
            giris.Show();
            this.Hide();
        }

        private void btn_yapilacaklar_Click(object sender, EventArgs e)
        {
            AnaSayfa anaSayfa = new AnaSayfa();
            anaSayfa.sessionKAdi = sessionKAdi;
            anaSayfa.Show();
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

        private void btn_gorselSil_Click(object sender, EventArgs e)
        {
            if (lst_gorsellerim.SelectedIndex != -1)
            {

                string selectedGorsel = lst_gorsellerim.SelectedItem.ToString();
                string sorgu = "DELETE FROM gorsellerim WHERE kullanici_adi=@kAdi and gorsel_adi = @gorselAdi";

                using (SqlConnection baglanti = new SqlConnection(baglantiDizesi))
                {
                    baglanti.Open();

                    using (SqlCommand komut = new SqlCommand(sorgu, baglanti))
                    {
                        komut.Parameters.AddWithValue("@kAdi", sessionKAdi);
                        komut.Parameters.AddWithValue("@gorselAdi", selectedGorsel);
                        komut.ExecuteNonQuery();


                        lst_gorsellerim.Items.Clear();

                        komut.CommandText = "SELECT * FROM gorsellerim where kullanici_adi = @k1";
                        komut.Parameters.Clear();
                        komut.Parameters.AddWithValue("@k1", sessionKAdi);
                        SqlDataReader okuyucu = komut.ExecuteReader();

                        while (okuyucu.Read())
                        {
                            lst_gorsellerim.Items.Add(okuyucu.GetString(2));
                        }

                        pb_resim.Image = null;
                    }
                }
            }
        }
    }
}
