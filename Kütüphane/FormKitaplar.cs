﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Kütüphane
{
    public partial class FormKitaplar : Form
    {
        SqlConnection baglanti = new SqlConnection(@"Data Source=ABDYLLA\SQLEXPRESS;Initial Catalog=DbYTAKutuphane;Integrated Security=True");

        public FormKitaplar()
        {
            InitializeComponent();
        }
        private void verileriGoster()
        {
            try
            {
                string q = "SELECT * FROM  TableKitaplar";
                SqlDataAdapter da = new SqlDataAdapter(q, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dataGridViewKitaplar.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(""+ex.Message);
            }
            
        }

        private void FormKitaplar_Load(object sender, EventArgs e)
        {
            verileriGoster();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void buttonKitapEkle_Click(object sender, EventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand("INSERT INTO TableKitaplar" +
         " (KitapAdi, YazarAdi , YazarSoyadi, ISBN, Durum,KitapTurKodu) VALUES (@P1, @P2, @P3, @P4, @P5, @P6)", baglanti);
            try
            {
                baglanti.Open();
                sqlCommand.Parameters.AddWithValue("P1", textBoxKitapAdi.Text);
                sqlCommand.Parameters.AddWithValue("P2", textBoxYazarAdi.Text);
                sqlCommand.Parameters.AddWithValue("P3", textBoxYazarSoyad.Text);
                sqlCommand.Parameters.AddWithValue("P4", textBoxISBN.Text);
                sqlCommand.Parameters.AddWithValue("P5", "True");
                sqlCommand.Parameters.AddWithValue("P6", textBoxKitapTurKodu.Text);

                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kitap eklenirken bir hata oluştu! " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
            verileriGoster();

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBoxKitapTurKodu_TextChanged(object sender, EventArgs e)
        {

        }

  

        private void labelID_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewKitaplar_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            labelGecikmeBedeli.Text = "0";
            int secilenSatir = dataGridViewKitaplar.SelectedCells[0].RowIndex;
            labelID.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[0].Value.ToString();
            textBoxKitapAdi.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[1].Value.ToString();
            textBoxYazarAdi.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[2].Value.ToString();
            textBoxYazarSoyad.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[3].Value.ToString();
            textBoxISBN.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[4].Value.ToString();
            textBoxKitapTurKodu.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[8].Value.ToString();

            textBoxOduncAlan.Text = dataGridViewKitaplar.Rows[secilenSatir].Cells[6].Value.ToString();
            if (dataGridViewKitaplar.Rows[secilenSatir].Cells[7].Value != DBNull.Value)
                dateTimePickerOduncAlmaTarihi.Value = (DateTime)dataGridViewKitaplar.Rows[secilenSatir].Cells[7].Value;

        }

        private void buttonKitapBilgiGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                SqlCommand sqlCommand = new SqlCommand("UPDATE  TableKitaplar SET KitapAdi= @P1, YazarAdi=@P2 , YazarSoyadi=@P3, ISBN=@P4, KitapTurKodu=@P5 WHERE ID=@P6", baglanti);
                sqlCommand.Parameters.AddWithValue("@P1", textBoxKitapAdi.Text);
                sqlCommand.Parameters.AddWithValue("@P2", textBoxYazarAdi.Text);
                sqlCommand.Parameters.AddWithValue("@P3", textBoxYazarSoyad.Text);
                sqlCommand.Parameters.AddWithValue("@P4", textBoxISBN.Text);
                sqlCommand.Parameters.AddWithValue("@P5", textBoxKitapTurKodu.Text);
                sqlCommand.Parameters.AddWithValue("@P6", labelID.Text);

                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncellenirken Hata Oluştu! " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
            verileriGoster();

        }

        private void buttonKitapOduncVer_Click(object sender, EventArgs e)
        {
            if (labelID.Text!="-")
            {
                try
                {
                    baglanti.Open();
                    SqlCommand sqlCommand = new SqlCommand("UPDATE  TableKitaplar SET OduncAlan= @P1, OduncAlmaTarihi=@P2 , Durum=@P3 WHERE ID=@P4", baglanti);


                    sqlCommand.Parameters.AddWithValue("@P1", textBoxOduncAlan.Text);
                    sqlCommand.Parameters.Add("@P2", SqlDbType.Date).Value=dateTimePickerOduncAlmaTarihi.Value.Date;
                    sqlCommand.Parameters.AddWithValue("@P3", "False");
                    sqlCommand.Parameters.AddWithValue("@P4", labelID.Text);
                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ödünç işlemi sırasında  Hata Oluştu! " + ex.Message);
                }
                finally
                {
                    baglanti.Close();
                }
                verileriGoster();
            }
            else
            {
                MessageBox.Show("Lütfen bir kitap seçiniz!");
            }
        }

        private void buttonGecikmeBedeliHesapla_Click(object sender, EventArgs e)
        {
            if (labelID.Text != "-")
            {
                DateTime bugununTarihi = DateTime.Now;
                double gunFarki = (int)(bugununTarihi - dateTimePickerOduncAlmaTarihi.Value.Date).TotalDays;
                if (gunFarki > 10)
                {
                    double gecikmeBedeli = (gunFarki - 10) * 0.5;
                    labelGecikmeBedeli.Text = gecikmeBedeli.ToString();

                }

            }
        }

        private void buttonKitabiladeEt_Click(object sender, EventArgs e)
        {
            if (labelID.Text != "-")
            {
                try
                {
                    baglanti.Open();
                    SqlCommand sqlCommand = new SqlCommand("UPDATE  TableKitaplar SET OduncAlan= @P1, OduncAlmaTarihi=@P2 , Durum=@P3 WHERE ID=@P4", baglanti);


                    sqlCommand.Parameters.AddWithValue("@P1", textBoxOduncAlan.Text);
                    sqlCommand.Parameters.Add("@P2", SqlDbType.Date).Value = DBNull.Value;
                    sqlCommand.Parameters.AddWithValue("@P3", "True");
                    sqlCommand.Parameters.AddWithValue("@P4", labelID.Text);
                    sqlCommand.ExecuteNonQuery();
                    textBoxOduncAlan.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ödünç iade sırasında  Hata Oluştu! " + ex.Message);
                }
                finally
                {
                    baglanti.Close();
                }
                verileriGoster();
            }

            
        }
        private void Temizle()
        {
            labelID.Text = "";
            textBoxKitapAdi.Text = "";
            textBoxYazarAdi.Text = "";
            textBoxYazarSoyad.Text = "";
            textBoxISBN.Text = "";
            textBoxOduncAlan.Text = "";
            textBoxKitapTurKodu.Text = "";
        }

        private void buttonTemizle_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void buttonAra_Click(object sender, EventArgs e)
        {
            aramaSonuclariniGoster();
        }


        private void aramaSonuclariniGoster()
        {
            try
            {
                string q = "SELECT* FROM TableKitaplar WHERE KitapAdi LIKE '" + textBoxKitapAdi.Text
                                                                             + "%' AND YazarAdi LIKE '" + textBoxYazarAdi.Text + "%' "
                                                                             + " AND YazarSoyadi LIKE '" + textBoxYazarSoyad.Text + "%' "
                                                                             + " AND ISBN LIKE '" + textBoxISBN.Text + "%' "
                                                                             + " AND KitapTurKodu LIKE '" + textBoxKitapTurKodu.Text + "%' "
                                                                             + " AND OduncAlan LIKE '" + textBoxOduncAlan.Text + "%' ";
                SqlDataAdapter da = new SqlDataAdapter(q, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dataGridViewKitaplar.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonTumKitaplariGoster_Click(object sender, EventArgs e)
        {
            verileriGoster();
        }

        private void buttonKitabiSil_Click(object sender, EventArgs e)
        {
            if (labelID.Text == "-" || labelID.Text == "")
            {
                MessageBox.Show("Lütfen silinecek kitabı seçiniz!");
            }
            else
            {
                try
                {
                    baglanti.Open();
                    SqlCommand sqlCommand = new SqlCommand("DELETE FROM TableKitaplar WHERE ID=@P1", baglanti);

                    sqlCommand.Parameters.AddWithValue("@P1", labelID.Text);

                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Silme işlemi sırasında Hata Oluştu! " + ex.Message);
                }
                finally
                {
                    baglanti.Close();
                }
                verileriGoster();
                Temizle();
            }
           
        }

        private void FormKitaplar_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridViewKitaplar_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
