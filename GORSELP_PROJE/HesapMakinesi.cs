using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GORSELP_PROJE
{
    public partial class HesapMakinesi : Form
    {
        public HesapMakinesi()
        {
            InitializeComponent();
        }

        int Move;
        int Mouse_X;
        int Mouse_Y;
        public string sessionKAdi;
        private void HesapMakinesi_Load(object sender, EventArgs e)
        {
            lbl_sessionKAdi.Text = sessionKAdi;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form1 giris = new Form1();
            giris.Show();
            this.Hide();
        }

        int secim = 0;
        double sayi1, sayi2, sonuc;

        private void ButonSayilar(object sender, EventArgs e)
        {
            if(lbl_sonuc.Text == "0")
            {
                lbl_sonuc.Text = "";
            }
            lbl_sonuc.Text = lbl_sonuc.Text + ((Button)sender).Text;
        }

        private void btn_Temizle_Click(object sender, EventArgs e)
        {
            lbl_sonuc.Text = "0";
        }

        private void btn_TekSil_Click(object sender, EventArgs e)
        {
            lbl_sonuc.Text = lbl_sonuc.Text.Substring(0, lbl_sonuc.Text.Length - 1);

            if (lbl_sonuc.Text == "")
            {
                lbl_sonuc.Text = "0";
            }
        }

        private void btn_Arti_Click(object sender, EventArgs e)
        {
            sayi1 = double.Parse(lbl_sonuc.Text);
            secim = 1;
            lbl_sonuc.Text = "0";
        }

        private void btn_Eksi_Click(object sender, EventArgs e)
        {
            sayi1 = double.Parse(lbl_sonuc.Text);
            secim = 2;
            lbl_sonuc.Text = "0";
        }

        private void btn_Carpi_Click(object sender, EventArgs e)
        {
            sayi1 = double.Parse(lbl_sonuc.Text);
            secim = 3;
            lbl_sonuc.Text = "0";
        }

        private void btn_Bol_Click(object sender, EventArgs e)
        {
            sayi1 = double.Parse(lbl_sonuc.Text);
            secim = 4;
            lbl_sonuc.Text = "0";
        }

        private void btn_Esittir_Click(object sender, EventArgs e)
        {
            sayi2 = double.Parse(lbl_sonuc.Text);
            
            if(secim == 1)
            {
                sonuc = sayi1 + sayi2;
                lbl_sonuc.Text = sonuc.ToString();
            }

            else if(secim == 2)
            {
                sonuc = sayi1 - sayi2;
                lbl_sonuc.Text = sonuc.ToString();
            }

            else if(secim == 3)
            {
                sonuc = sayi1 * sayi2;
                lbl_sonuc.Text = sonuc.ToString();
            }

            else if (secim == 4)
            {
                sonuc = sayi1 / sayi2;
                lbl_sonuc.Text = sonuc.ToString();
            }

        }

        private void btn_virgul_Click(object sender, EventArgs e)
        {
            if (lbl_sonuc.Text.IndexOf(",")<1)
            {
                lbl_sonuc.Text += ",";
            }
        }

        private void btn_Negatif_Click(object sender, EventArgs e)
        {
            if (lbl_sonuc.Text != "0")
            {
                if (lbl_sonuc.Text.StartsWith("-"))
                {
                    lbl_sonuc.Text = lbl_sonuc.Text.Replace("-", "");
                }
                else if (lbl_sonuc.Text.StartsWith(""))
                {
                    lbl_sonuc.Text = "-" + lbl_sonuc.Text;
                }
            }
        }

        private void HesapMakinesi_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
        }

        private void HesapMakinesi_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }

        private void HesapMakinesi_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
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

        private void btn_asagiIndir_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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
    }
}
