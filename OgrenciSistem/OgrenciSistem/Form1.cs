using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OgrenciSistem
{
    public partial class Form1 : Form
    {
        List<Ogrenci> ogrenciler = new List<Ogrenci>();
        int gnclleIdx = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadDers()
        {
            cbxDers.Items.Add("Matematik");
            cbxDers.Items.Add("Kimya");
            cbxDers.Items.Add("Biyoloji");
            cbxDers.Items.Add("Edebiyat");
            cbxDers.Items.Add("Tarih");
            cbxDers.Items.Add("Fizik");
        }

        private void LoadSinif()
        {
            cbxSinif.Items.Add("215");
            cbxSinif.Items.Add("216");
            cbxSinif.Items.Add("315");
            cbxSinif.Items.Add("316");
            cbxSinif.Items.Add("415");
            cbxSinif.Items.Add("416");
        }

        private void ListViewDizayn()
        {
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.Columns.Add(label4.Text, 100);
            listView1.Columns.Add(label5.Text, 100);
            listView1.Columns.Add(label7.Text, 100);
            listView1.Columns.Add("Not Ortalaması", 100);
            listView1.Columns.Add("Durum", 100);
        }

        private void OgrenciEkle()
        {
            Random random = new Random();
            var ogrenci = new Ogrenci();

            if (ogrenciler.Count == 0 || OkulNoKontrol(int.Parse(tbOgrNo.Text)))
            {
                ogrenci.OgrenciEkle(int.Parse(tbOgrNo.Text), tbOgrAd.Text, cbxDers.Text, cbxSinif.Text, random.Next(1, 100));
                ogrenciler.Add(ogrenci);
                ListViewEkle(ogrenci);
                Temizle();
            }
            else
            {
                foreach (var ogr in ogrenciler)
                {
                    if (ogr.OgrenciNo == int.Parse(tbOgrNo.Text))
                    {
                        if (ogr.OgrenciAdSoyad == tbOgrAd.Text)
                        {
                            if (ogr.OgrenciDers.Count < 5)
                            {
                                if (ogr.DersKontrol(cbxDers.Text, ogr))
                                {
                                    ogr.OgrenciDers.Add(cbxDers.Text);
                                    ogr.Notlar.Add(random.Next(1, 100));
                                    ListViewGuncelle(ogr);
                                    break;
                                }
                                else
                                {
                                    MessageBox.Show("Bir Öğrenci Aynı Dersi 2 kere Alamaz!!\nLütfen Başka Ders Seçiniz!!");
                                    break;
                                }
                            }
                            else
                            {
                                MessageBox.Show(ogr.OgrenciAdSoyad + " isimli öğrenci maksimum ders sayısına ulaşmıştır");
                                break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Bu numaraya sahip başka öğrenci bulunmaktadır!!\nLütfen farklı bir numara girin..");
                            break;
                        }
                    }
                }
            }
        }

        private void Temizle()
        {
            tbOgrNo.Clear();
            tbOgrAd.Clear();
            cbxDers.Text = "";
            cbxSinif.Text = "";
        }

        private void ListViewEkle(Ogrenci ogrenci)
        {
            string[] row = { ogrenci.OgrenciNo.ToString(), ogrenci.OgrenciAdSoyad, ogrenci.Sinif, OrtalamaHesapla(ogrenci).ToString(), (OrtalamaHesapla(ogrenci) > 50 ? "GEÇTİ" : "KALDI") };
            ListViewItem lvi = new ListViewItem(row);
            listView1.Items.Add(lvi);

        }

        private void ListViewGuncelle(Ogrenci ogr)
        {

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                ListViewItem item = listView1.Items[i];
                if (item.SubItems[0].Text == ogr.OgrenciNo.ToString())
                {
                    string[] row = { ogr.OgrenciNo.ToString(), ogr.OgrenciAdSoyad, ogr.Sinif, OrtalamaHesapla(ogr).ToString(), (OrtalamaHesapla(ogr) > 50 ? "GEÇTİ" : "KALDI") };
                    ListViewItem lvi = new ListViewItem(row);
                    listView1.Items[i] = lvi;
                    break;
                }
            }

        }

        private static double OrtalamaHesapla(Ogrenci ogrenci)
        {
            double toplam = 0;
            foreach (var item in ogrenci.Notlar)
            {
                toplam += item;
            }
            return toplam / ogrenci.OgrenciDers.Count;
        }

        public bool OkulNoKontrol(int okulNo)
        {
            foreach (var item in ogrenciler)
            {
                if (item.OgrenciNo == okulNo)
                {
                    return false;
                }
            }
            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ListViewDizayn();
            LoadSinif();
            LoadDers();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            OgrenciEkle();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            for (int i = listView1.Items.Count - 1; i >= 0; i--)
            {
                if (listView1.Items[i].Selected)
                {
                    listView1.Items[i].Remove();
                    ogrenciler.RemoveAt(i);
                }
            }
            Temizle();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            listView1.Items[gnclleIdx].SubItems[0].Text = tbOgrNo.Text;
            listView1.Items[gnclleIdx].SubItems[1].Text = tbOgrAd.Text;
            listView1.Items[gnclleIdx].SubItems[2].Text = cbxSinif.Text;

            foreach (var item in ogrenciler)
            {
                if (item.OgrenciNo == int.Parse(listView1.Items[gnclleIdx].SubItems[0].Text))
                {
                    item.OgrenciNo = int.Parse(tbOgrNo.Text);
                    item.OgrenciAdSoyad = tbOgrAd.Text;
                    item.Sinif = cbxSinif.Text;
                    break;
                }
            }
            gnclleIdx = 0;
            Temizle();
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            for (int i = listView1.Items.Count - 1; i >= 0; i--)
            {
                if (listView1.Items[i].Selected)
                {
                    gnclleIdx = i;
                    tbOgrNo.Text = listView1.Items[i].SubItems[0].Text;
                    tbOgrAd.Text = listView1.Items[i].SubItems[1].Text;
                    cbxSinif.Text = listView1.Items[i].SubItems[2].Text;
                    break;
                }
            }
        }
    }
}