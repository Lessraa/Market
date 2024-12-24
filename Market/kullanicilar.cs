using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Market
{
    public partial class kullanicilar : Form
    {


        private readonly string connectionString;
       


        public kullanicilar()
        {
            InitializeComponent();

             

            connectionString = "Host=93.113.57.54;" +
                         "Port=5432;" +
                         "Database=odev;" +
                         "Username=lessra;" +
                         "Password=476634.Ss";

            // DataGridView ayarları
            kullaniciList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

          



            // Event handlers
            kullaniciList.CellClick += kullaniciList_CellClick;
            kullaniciList.CellContentClick += kullaniciList_CellContentClick;

          


            

        }


        private void KullanicilariListele()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM kullanicilar ORDER BY id";

                    using (var adapter = new NpgsqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        kullaniciList.DataSource = null;
                        kullaniciList.Columns.Clear();

                       


                        // Resim kolonu ekle
                        DataGridViewImageColumn resimKolonu = new DataGridViewImageColumn();
                        resimKolonu.HeaderText = "Resim";
                        resimKolonu.Name = "Resim";
                        resimKolonu.ImageLayout = DataGridViewImageCellLayout.Zoom;
                        resimKolonu.Width = 200;
                        kullaniciList.Columns.Add(resimKolonu);

                      



                        // Diğer kolonları ekle
                        foreach (DataColumn col in dataTable.Columns)
                        {
                            if (col.ColumnName != "resim") // Resim kolonunu tekrar ekleme
                            {
                                kullaniciList.Columns.Add(col.ColumnName, col.ColumnName);
                            }
                        }

                        // Yönetici checkbox kolonunu ekle
                        DataGridViewCheckBoxColumn yoneticiKolonu = new DataGridViewCheckBoxColumn();
                        yoneticiKolonu.HeaderText = "Yönetici";
                        yoneticiKolonu.Name = "YoneticiCheckbox";
                        kullaniciList.Columns.Add(yoneticiKolonu);

                        // Sil butonu kolonunu ekle
                        DataGridViewButtonColumn silButonu = new DataGridViewButtonColumn();
                        silButonu.HeaderText = "İşlem";
                        silButonu.Name = "SilButonu";
                        silButonu.Text = "Sil";
                        silButonu.UseColumnTextForButtonValue = true;
                        kullaniciList.Columns.Add(silButonu);

                        // Verileri doldur
                        foreach (DataRow row in dataTable.Rows)
                        {
                            int rowIndex = kullaniciList.Rows.Add();
                            var gridRow = kullaniciList.Rows[rowIndex];

                            // Resmi dönüştür ve göster
                            if (row["resim"] != DBNull.Value)
                            {
                                byte[] resimBytes = (byte[])row["resim"];
                                using (MemoryStream ms = new MemoryStream(resimBytes))
                                {
                                    gridRow.Cells["Resim"].Value = Image.FromStream(ms);
                                }
                            }

                            // Diğer kolonları doldur
                            foreach (DataColumn col in dataTable.Columns)
                            {
                                if (col.ColumnName != "resim")
                                {
                                    gridRow.Cells[col.ColumnName].Value = row[col.ColumnName];
                                }
                            }

                            // Checkbox durumunu ayarla
                            if (row["kullanici_tipi"] != null)
                            {
                                gridRow.Cells["YoneticiCheckbox"].Value =
                                    row["kullanici_tipi"].ToString().ToLower() == "yonetici";
                            }
                        }

                        // Görünüm ayarları
                     /*   kullaniciList.RowTemplate.Height = 200;
                        kullaniciList.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                        kullaniciList.DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue;
                        kullaniciList.DefaultCellStyle.SelectionForeColor = Color.White;
                        kullaniciList.EnableHeadersVisualStyles = false;
                        kullaniciList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                        kullaniciList.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                        kullaniciList.ColumnHeadersHeight = 50;
                       */
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veri çekme hatası: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void kullaniciList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == kullaniciList.Columns["SilButonu"].Index && e.RowIndex >= 0)
            {
                try
                {
                    int kullaniciId = Convert.ToInt32(kullaniciList.Rows[e.RowIndex].Cells["id"].Value);
                    string kullaniciAdi = kullaniciList.Rows[e.RowIndex].Cells["kullanici_adi"].Value.ToString();

                    var onay = MessageBox.Show($"{kullaniciAdi} kullanıcısını silmek istediğinize emin misiniz?",
                        "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (onay == DialogResult.Yes)
                    {
                        KullaniciSil(kullaniciId);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata oluştu: {ex.Message}",
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void kullaniciList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == kullaniciList.Columns["YoneticiCheckbox"].Index && e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewCheckBoxCell checkbox =
                        (DataGridViewCheckBoxCell)kullaniciList.Rows[e.RowIndex].Cells["YoneticiCheckbox"];

                    bool yeniDeger = !(bool)checkbox.EditedFormattedValue;
                    int kullaniciId = Convert.ToInt32(kullaniciList.Rows[e.RowIndex].Cells["id"].Value);
                    string kullaniciAdi = kullaniciList.Rows[e.RowIndex].Cells["kullanici_adi"].Value.ToString();

                    string yeniTip = !yeniDeger ? "yonetici" : "standart";
                    var onay = MessageBox.Show(
                        $"{kullaniciAdi} kullanıcısını '{yeniTip}' olarak değiştirmek istediğinize emin misiniz?",
                        "Tip Değiştirme Onayı",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (onay == DialogResult.Yes)
                    {
                        KullaniciTipiGuncelle(kullaniciId, yeniTip);
                    }
                    else
                    {
                        KullanicilariListele();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata oluştu: {ex.Message}",
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }


        private void KullaniciTipiGuncelle(int kullaniciId, string yeniTip)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"UPDATE kullanicilar SET kullanici_tipi = '{yeniTip}'::kullanici_tipi_enum WHERE id = @id";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", kullaniciId);
                        int etkilenenSatir = command.ExecuteNonQuery();

                        if (etkilenenSatir > 0)
                        {
                            MessageBox.Show("Kullanıcı tipi başarıyla güncellendi.",
                                "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            KullanicilariListele();
                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı tipi güncellenirken bir hata oluştu.",
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Güncelleme işlemi sırasında hata oluştu: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void KullaniciSil(int kullaniciId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM kullanicilar WHERE id = @id";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", kullaniciId);
                        int etkilenenSatir = command.ExecuteNonQuery();

                        if (etkilenenSatir > 0)
                        {
                            MessageBox.Show("Kullanıcı başarıyla silindi.",
                                "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            KullanicilariListele();
                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı silinirken bir hata oluştu.",
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Silme işlemi sırasında hata oluştu: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void kullanicilar_Load(object sender, EventArgs e)
        {
            KullanicilariListele();

            kullaniciList.RowTemplate.Height = 60;
            kullaniciList.Width = this.ClientSize.Width;



            // Giriş formundan kullanıcı ID'sini alıyoruz
            int userIdFromLogin = Form1.userId;

            // Kullanıcı bilgilerini çekiyoruz
            KullaniciBilgileriniGetir(userIdFromLogin);

        }

        private void KullaniciBilgileriniGetir(int userId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT kullanici_adi, sifre, resim FROM kullanicilar WHERE id = @id";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                kullaniciAdiText.Text = reader["kullanici_adi"].ToString();
                                sifreText.Text = reader["sifre"].ToString();

                                if (reader["resim"] != DBNull.Value)
                                {
                                    byte[] imageBytes = (byte[])reader["resim"];
                                    using (var ms = new MemoryStream(imageBytes))
                                    {
                                        pictureBoxProfil.Image = Image.FromStream(ms);
                                    }
                                }
                                else
                                {
                                    pictureBoxProfil.Image = null; // Resim yoksa boş bırak
                                }
                            }
                            else
                            {
                                MessageBox.Show("Kullanıcı bilgileri bulunamadı.", "Hata",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kullanıcı bilgileri alınırken hata oluştu: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void sifreText_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void kullaniciAdiText_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void kullaniciEkle_Click(object sender, EventArgs e)
        {
            Form3 kullaniciEklemeFormu = new Form3();
            kullaniciEklemeFormu.Show();
        }

        private void ppGuncelle_Click(object sender, EventArgs e)
        {
            // Dosya seçme penceresi açıyoruz
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"; // Görsel dosyalarını filtrele
            openFileDialog.Title = "Profil Fotoğrafı Seçin";

            // Kullanıcı bir dosya seçtiyse
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Seçilen dosyanın yolunu alıyoruz
                string filePath = openFileDialog.FileName;

                try
                {
                    // Seçilen resmi byte[] formatına çeviriyoruz
                    byte[] imageBytes = File.ReadAllBytes(filePath);

                    // Resmi PictureBox'a yüklüyoruz
                    pictureBoxProfil.Image = Image.FromFile(filePath);

                    // Resmi veritabanına kaydediyoruz
                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "UPDATE kullanicilar SET resim = @resim WHERE id = @id";

                        using (var command = new NpgsqlCommand(query, connection))
                        {
                            // Parametreleri ekliyoruz
                            command.Parameters.AddWithValue("@resim", imageBytes);
                            command.Parameters.AddWithValue("@id", Form1.userId); // Giriş yapan kullanıcının ID'sini kullanıyoruz

                            // Sorguyu çalıştırıyoruz
                            int rowsAffected = command.ExecuteNonQuery();

                            // Başarılı güncelleme mesajı
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Profil fotoğrafınız başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Profil fotoğrafı güncellenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Resim güncellenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            // Kullanıcıdan güncellenmiş bilgileri alıyoruz
            string yeniKullaniciAdi = kullaniciAdiText.Text.Trim();
            string yeniSifre = sifreText.Text.Trim();

            // Kullanıcı bilgilerini boş bırakmışsa uyarı mesajı gösteriyoruz
            if (string.IsNullOrEmpty(yeniKullaniciAdi) || string.IsNullOrEmpty(yeniSifre))
            {
                MessageBox.Show("Kullanıcı adı ve şifre boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kullanıcıya bilgileri güncellemek isteyip istemediğini soran bir MessageBox gösteriyoruz
            DialogResult result = MessageBox.Show("Kullanıcı adı ve şifreyi güncellemek istiyor musunuz?",
                "Bilgi Güncelleme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "UPDATE kullanicilar SET kullanici_adi = @kullaniciAdi, sifre = @sifre WHERE id = @id";

                        using (var command = new NpgsqlCommand(query, connection))
                        {
                            // Parametreleri ekliyoruz
                            command.Parameters.AddWithValue("@kullaniciAdi", yeniKullaniciAdi);
                            command.Parameters.AddWithValue("@sifre", yeniSifre);
                            command.Parameters.AddWithValue("@id", Form1.userId); // Giriş yapan kullanıcının ID'sini kullanıyoruz

                            // Sorguyu çalıştırıyoruz
                            int rowsAffected = command.ExecuteNonQuery();

                            // Başarılı güncelleme mesajı
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Kullanıcı bilgileri başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Kullanıcı bilgileri güncellenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }

                    // Kaydet butonunu gizliyoruz
                    btnKaydet.Visible = false;
                    // TextBox'ları tekrar devre dışı bırakıyoruz
                    kullaniciAdiText.Enabled = false;
                    sifreText.Enabled = false;
                   
                    kullaniciAdiText.ReadOnly = true;
                    sifreText.ReadOnly = true;


                    // Bilgileri Güncelle butonunu tekrar etkin hale getiriyoruz
                    BilgileriGuncelle.Enabled = true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Güncelleme sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Kullanıcı bilgileri güncellenmedi.", "İptal", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BilgileriGuncelle_Click(object sender, EventArgs e)
        {
            // TextBox'ları düzenlemeye açıyoruz
            kullaniciAdiText.Enabled = true;
            sifreText.Enabled = true;

            kullaniciAdiText.ReadOnly = false;
            sifreText.ReadOnly = false;

            // Kaydet butonunu görünür hale getiriyoruz
            btnKaydet.Visible = true;

            // Bilgileri Güncelle butonunu devre dışı bırakıyoruz
            BilgileriGuncelle.Enabled = false;
        }



     

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        /*  protected override void OnFormClosing(FormClosingEventArgs e)
          {
              base.OnFormClosing(e);
              if (e.CloseReason == CloseReason.UserClosing)
              {
                  yonetici yoneticiForm = new yonetici();
                  yoneticiForm.Show();
              }
          }

          */

    }
}
