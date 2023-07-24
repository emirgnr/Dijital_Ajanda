using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;



namespace GORSELP_PROJE
{
    public partial class DosyaIslemleri : Form
    {
        public DosyaIslemleri()
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

        string folderPath = @"YENI_PDFLER\\";
        string folderPathB = @"BİRLEŞTİRİLMİŞLER\\";
        private void DosyaIslemleri_Load(object sender, EventArgs e)
        {
            lbl_sessionKAdi.Text = sessionKAdi;

            string pdflerDosyaYolu = Path.Combine(Application.StartupPath, "YENI_PDFLER");
            if (Directory.Exists(pdflerDosyaYolu))
            {
                var pdfFiles = Directory.GetFiles(folderPath, "*.pdf")
                                   .Select(Path.GetFileName)
                                   .ToArray();

                lst_pdfler.Items.AddRange(pdfFiles);
            }

            string birlestirilmisDosyaYolu = Path.Combine(Application.StartupPath, "BİRLEŞTİRİLMİŞLER");
            if (Directory.Exists(birlestirilmisDosyaYolu))
            {
                var pdfFilesB = Directory.GetFiles(folderPathB, "*.pdf")
                                                   .Select(Path.GetFileName)
                                                   .ToArray();

                lst_birlestirilenler.Items.AddRange(pdfFilesB);
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form1 giris = new Form1();
            giris.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_asagiIndir_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void DosyaIslemleri_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
        }

        private void DosyaIslemleri_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }

        private void DosyaIslemleri_MouseMove(object sender, MouseEventArgs e)
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

        private void btn_gorsellerim_Click(object sender, EventArgs e)
        {
            Gorsellerim gorselSayfa = new Gorsellerim();
            gorselSayfa.sessionKAdi = sessionKAdi;
            gorselSayfa.Show();
            this.Hide();
        }

        private void btn_gunluk_Click(object sender, EventArgs e)
        {
            Gunluk gunlukSayfa = new Gunluk();
            gunlukSayfa.sessionKAdi = sessionKAdi;
            gunlukSayfa.Show();
            this.Hide();
        }

        private void btn_pdfOlustur_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(rtb_Paragraf.Text) && !string.IsNullOrWhiteSpace(txt_DosyaAdi.Text))
            {
                string metin = rtb_Paragraf.Text;   // TextBox'tan dosya adını al
                string dosyaAdi = txt_DosyaAdi.Text + ".pdf"; // TextBox'tan dosya adını al

                // Yeni bir PDF belgesi oluştur
                Document belge = new Document();

                // Ana dosya yolunu al
                string anaDosyaYolu = AppDomain.CurrentDomain.BaseDirectory;

                // Yeni PDF'leri kaydetmek için klasör adını belirle
                string yeniPdfKlasoru = Path.Combine(anaDosyaYolu, "YENI_PDFLER");

                // Klasör varsa, içerisine kaydet; yoksa klasörü oluştur ve içerisine kaydet
                if (Directory.Exists(yeniPdfKlasoru))
                {
                    string yeniPdfYolu = Path.Combine(yeniPdfKlasoru, dosyaAdi);
                    PdfWriter.GetInstance(belge, new FileStream(yeniPdfYolu, FileMode.Create));
                }
                else
                {
                    Directory.CreateDirectory(yeniPdfKlasoru);
                    string yeniPdfYolu = Path.Combine(yeniPdfKlasoru, dosyaAdi);
                    PdfWriter.GetInstance(belge, new FileStream(yeniPdfYolu, FileMode.Create));
                }

                belge.Open();

                // Metni PDF'ye ekle
                belge.Add(new Paragraph(metin));

                belge.Close();

                lst_pdfler.Items.Clear();

                txt_DosyaAdi.Text = null;
                rtb_Paragraf.Text = null;


                var pdfFiles = Directory.GetFiles(folderPath, "*.pdf")
                                   .Select(Path.GetFileName)
                                   .ToArray();

                lst_pdfler.Items.AddRange(pdfFiles);

                MessageBox.Show("PDF oluşturuldu ve kaydedildi.");
            }
            else
            {
                MessageBox.Show("Lütfen dosya adı ve birkaç metin giriniz.");
            }


        }

        private List<string> secilenDosyalar = new List<string>();

        private void btn_pdfBirlestir_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "PDF Dosyaları (*.pdf)|*.pdf";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    secilenDosyalar.Clear();
                    secilenDosyalar.AddRange(openFileDialog.FileNames);
                    PDFBirlestirVeKaydet();
                }
            }
        }

        private void PDFBirlestirVeKaydet()
        {
            if (secilenDosyalar.Count < 2)
            {
                MessageBox.Show("Lütfen birleştirmek için en az iki adet PDF dosyası seçin.");
                return;
            }

            string birlestirilmisDosyaYolu = Path.Combine(Application.StartupPath, "BİRLEŞTİRİLMİŞLER");
            if (!Directory.Exists(birlestirilmisDosyaYolu))
            {
                Directory.CreateDirectory(birlestirilmisDosyaYolu);
            }

            string birlestirilmisDosyaAdi = BirlestirilmisDosyaAdiAl(secilenDosyalar.ToArray());
            string cikisDosyaYolu = Path.Combine(birlestirilmisDosyaYolu, birlestirilmisDosyaAdi);

            if (cikisDosyaYolu.Length >= 260)
            {
                string dosyaAdi1 = Path.GetFileNameWithoutExtension(secilenDosyalar[0]);
                string dosyaAdi2 = Path.GetFileNameWithoutExtension(secilenDosyalar[1]);

                // İlk dosya adının ilk yarısını ve ikinci dosya adının ikinci yarısını alarak yeni bir dosya adı oluştur
                birlestirilmisDosyaAdi = dosyaAdi1.Substring(0, dosyaAdi1.Length / 2) + dosyaAdi2.Substring(dosyaAdi2.Length / 2);
                birlestirilmisDosyaAdi += "BİRLEŞTİRİLDİ.pdf";
            }

            using (FileStream mergedFileStream = new FileStream(cikisDosyaYolu, FileMode.Create))
            {
                Document belge = new Document();
                PdfCopy kopya = new PdfCopy(belge, mergedFileStream);
                belge.Open();

                foreach (string dosya in secilenDosyalar)
                {
                    PdfReader okuyucu = new PdfReader(dosya);
                    kopya.AddDocument(okuyucu);
                    okuyucu.Close();
                }

                belge.Close();
            }

            var pdfFilesB = Directory.GetFiles(birlestirilmisDosyaYolu, "*.pdf")
                                    .Select(Path.GetFileName)
                                    .ToArray();

            lst_birlestirilenler.Items.Clear();
            lst_birlestirilenler.Items.AddRange(pdfFilesB);

            MessageBox.Show("PDF dosyaları başarıyla birleştirildi ve kaydedildi.");
            secilenDosyalar.Clear();
        }

        private string BirlestirilmisDosyaAdiAl(string[] dosyalar)
        {
            string dosyaAdi1 = Path.GetFileNameWithoutExtension(dosyalar[0]);
            string dosyaAdi2 = Path.GetFileNameWithoutExtension(dosyalar[1]);

            // İki dosyanın rastgele birleştirilmiş adını oluştur
            string birlestirilmisDosyaAdi = dosyaAdi1 + dosyaAdi2 + "BİRLEŞTİRİLDİ.pdf";

            return birlestirilmisDosyaAdi;
        }


        private void btn_Temizle_Click(object sender, EventArgs e)
        {
            txt_DosyaAdi.Text = null;
            rtb_Paragraf.Text = null;
        }

        private void lst_pdfler_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFile = lst_pdfler.SelectedItem.ToString();
            string filePath = Path.Combine(folderPath, selectedFile);

            if (File.Exists(filePath))
            {
                // Dosya mevcut olduğunda içeriği al ve richTextBox'e aktar
                string text = ExtractTextFromPDF(filePath);
                richTextBox1.Text = text;
            }
            else
            {
                richTextBox1.Text = "Seçilen dosya mevcut değil.";
            }
        }

        private string ExtractTextFromPDF(string filePath)
        {
            using (PdfReader reader = new PdfReader(filePath))
            {
                StringBuilder text = new StringBuilder();

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    byte[] pageContent = reader.GetPageContent(i);

                    string pageText = ExtractTextFromPageContent(pageContent);

                    text.Append(pageText);
                }

                return text.ToString();
            }
        }

        private static string ExtractTextFromPageContent(byte[] content)
        {
            StringBuilder text = new StringBuilder();

            int currentIndex = 0;
            int length = content.Length;

            while (currentIndex < length)
            {
                if (content[currentIndex] == '(') // Parantez içindeki metni al
                {
                    currentIndex++;

                    StringBuilder stringBuilder = new StringBuilder();

                    while (currentIndex < length && content[currentIndex] != ')')
                    {
                        stringBuilder.Append((char)content[currentIndex]);
                        currentIndex++;
                    }

                    text.Append(stringBuilder.ToString());
                }

                currentIndex++;
            }

            return text.ToString();
        }

        private void btn_gozatYeniPDF_Click(object sender, EventArgs e)
        {
            string pdflerDosyaYolu = Path.Combine(Application.StartupPath, "YENI_PDFLER");
            if (Directory.Exists(pdflerDosyaYolu))
            {
                try
                {
                    Process.Start("explorer.exe", pdflerDosyaYolu);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dosya gezginini açarken bir hata oluştu ");
                }
            }
            else
            {
                MessageBox.Show("Böyle bir klasör henüz yok lütfen önce bir PDF oluşturun. ");
            }
            
        }

        private void btn_gozatBirlestirilen_Click(object sender, EventArgs e)
        {
            string birlestirilenlerDosyaYolu = Path.Combine(Application.StartupPath, "BİRLEŞTİRİLMİŞLER");

            if (Directory.Exists(birlestirilenlerDosyaYolu))
            {
                try
                {
                    Process.Start("explorer.exe", birlestirilenlerDosyaYolu);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dosya gezginini açarken bir hata oluştu ");
                }
            }
            else
            {
                MessageBox.Show("Böyle bir klasör henüz yok lütfen önce PDF birleştirin. ");
            }
        }
    }
}
