using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;
using System.Drawing;
using System.IO;
using System.Linq.Expressions;

namespace Market
{
    public partial class yonetici : Form
    {
        private readonly string connectionString;
        private PictureBox pictureBox1;
        private Button btnResimSec;

        public yonetici()
        {
            InitializeComponent();

            connectionString = "Host=93.113.57.54;" +
                             "Port=5432;" +
                             "Database=odev;" +
                             "Username=lessra;" +
                             "Password=476634.Ss";

            // DataGridView ayarları
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // PictureBox ayarları
            pictureBox1 = new PictureBox();
            pictureBox1.Size = new Size(100, 100);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(textBox2.Left, textBox2.Bottom + 10);

            // Resim seçme butonu
            btnResimSec = new Button();
            btnResimSec.Text = "Resim Seç";
            btnResimSec.Location = new Point(pictureBox1.Right + 10, pictureBox1.Top);
            btnResimSec.Click += BtnResimSec_Click;

            // Kontrolleri forma ekle
            Controls.Add(pictureBox1);
            Controls.Add(btnResimSec);

            // Event handlers
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
        }

        private ImageList imageList1;
        private ImageList imageList2;


        private void BtnResimSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.Title = "Bir Resim Dosyası Seçin";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
                }
            }
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

                        dataGridView1.DataSource = null;
                        dataGridView1.Columns.Clear();

                        // Resim kolonu ekle
                        DataGridViewImageColumn resimKolonu = new DataGridViewImageColumn();
                        resimKolonu.HeaderText = "Resim";
                        resimKolonu.Name = "Resim";
                        resimKolonu.ImageLayout = DataGridViewImageCellLayout.Zoom;
                        resimKolonu.Width = 100;
                        dataGridView1.Columns.Add(resimKolonu);

                        // Diğer kolonları ekle
                        foreach (DataColumn col in dataTable.Columns)
                        {
                            if (col.ColumnName != "resim") // Resim kolonunu tekrar ekleme
                            {
                                dataGridView1.Columns.Add(col.ColumnName, col.ColumnName);
                            }
                        }

                        // Yönetici checkbox kolonunu ekle
                        DataGridViewCheckBoxColumn yoneticiKolonu = new DataGridViewCheckBoxColumn();
                        yoneticiKolonu.HeaderText = "Yönetici";
                        yoneticiKolonu.Name = "YoneticiCheckbox";
                        dataGridView1.Columns.Add(yoneticiKolonu);

                        // Sil butonu kolonunu ekle
                        DataGridViewButtonColumn silButonu = new DataGridViewButtonColumn();
                        silButonu.HeaderText = "İşlem";
                        silButonu.Name = "SilButonu";
                        silButonu.Text = "Sil";
                        silButonu.UseColumnTextForButtonValue = true;
                        dataGridView1.Columns.Add(silButonu);

                        // Verileri doldur
                        foreach (DataRow row in dataTable.Rows)
                        {
                            int rowIndex = dataGridView1.Rows.Add();
                            var gridRow = dataGridView1.Rows[rowIndex];

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
                        dataGridView1.RowTemplate.Height = 100;
                        dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                        dataGridView1.DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue;
                        dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
                        dataGridView1.EnableHeadersVisualStyles = false;
                        dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                        dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                        dataGridView1.ColumnHeadersHeight = 35;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veri çekme hatası: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["YoneticiCheckbox"].Index && e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewCheckBoxCell checkbox =
                        (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells["YoneticiCheckbox"];

                    bool yeniDeger = !(bool)checkbox.EditedFormattedValue;
                    int kullaniciId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                    string kullaniciAdi = dataGridView1.Rows[e.RowIndex].Cells["kullanici_adi"].Value.ToString();

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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string kullaniciAdi = textBox1.Text.Trim();
                string sifre = textBox2.Text.Trim();
                string kullaniciTipi = checkBox1.Checked ? "'yonetici'" : "'standart'";

                if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre))
                {
                    MessageBox.Show("Kullanıcı adı ve şifre alanları boş bırakılamaz!",
                        "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Resmi byte dizisine çevir
                byte[] resimBytes = null;
                if (pictureBox1.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        resimBytes = ms.ToArray();
                    }
                }

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string kontrolQuery = "SELECT COUNT(*) FROM kullanicilar WHERE kullanici_adi = @kullaniciadi";
                    using (var kontrolCommand = new NpgsqlCommand(kontrolQuery, connection))
                    {
                        kontrolCommand.Parameters.AddWithValue("@kullaniciadi", kullaniciAdi);
                        int kullaniciSayisi = Convert.ToInt32(kontrolCommand.ExecuteScalar());

                        if (kullaniciSayisi > 0)
                        {
                            MessageBox.Show("Bu kullanıcı adı zaten kullanılıyor!",
                                "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string insertQuery = $@"INSERT INTO kullanicilar (kullanici_adi, sifre, kullanici_tipi, resim) 
                                         VALUES (@kullaniciadi, @sifre, {kullaniciTipi}::kullanici_tipi_enum, @resim)";

                    using (var command = new NpgsqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@kullaniciadi", kullaniciAdi);
                        command.Parameters.AddWithValue("@sifre", sifre);

                        if (resimBytes == null)
                        {
                            command.Parameters.AddWithValue("@resim", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@resim", resimBytes);
                        }


                        int etkilenenSatir = command.ExecuteNonQuery();

                        if (etkilenenSatir > 0)
                        {
                            MessageBox.Show("Kullanıcı başarıyla eklendi.",
                                "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            textBox1.Clear();
                            textBox2.Clear();
                            checkBox1.Checked = false;
                            pictureBox1.Image = null;

                            KullanicilariListele();
                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı eklenirken bir hata oluştu.",
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kullanıcı ekleme sırasında hata oluştu: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["SilButonu"].Index && e.RowIndex >= 0)
            {
                try
                {
                    int kullaniciId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                    string kullaniciAdi = dataGridView1.Rows[e.RowIndex].Cells["kullanici_adi"].Value.ToString();

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

        private void SetupListViews()
        {
            // Onaylı ürünler için ImageList
            imageList1 = new ImageList();
            imageList1.ImageSize = new Size(80, 80);
            imageList1.ColorDepth = ColorDepth.Depth32Bit;

            // Onaylanmamış ürünler için ImageList
            imageList2 = new ImageList();
            imageList2.ImageSize = new Size(80, 80);
            imageList2.ColorDepth = ColorDepth.Depth32Bit;

            // ListView ayarları
            listView1.View = View.LargeIcon;
            listView1.LargeImageList = imageList1;
            listView1.Groups.Add(new ListViewGroup("Onaylı Ürünler", HorizontalAlignment.Left));

            // Onaylanmamış ürünler için ikinci ListView
            listView2.View = View.LargeIcon;
            listView2.LargeImageList = imageList2;
            listView2.Groups.Add(new ListViewGroup("Onaylanmamış Ürünler", HorizontalAlignment.Left));

            // ListView'lar için click olayları
            listView1.Click += ListView1_Click;
            listView2.Click += ListView2_Click;

            
        }

        private void LoadProducts()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Onaylı ve onaysız ürünleri ayrı ayrı yükle
                    LoadApprovedProducts(connection);
                    LoadUnapprovedProducts(connection);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ürünler yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadApprovedProducts(NpgsqlConnection connection)
        {
            string query = @"SELECT id, urun_adi, urun_resmi FROM urunler WHERE satista_mi = true ORDER BY id";

            using (var command = new NpgsqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                imageList1.Images.Clear();
                listView1.Items.Clear();

                int imageIndex = 0;
                while (reader.Read())
                {
                    string urunAdi = reader["urun_adi"].ToString();

                    // Resim işleme
                    if (!reader.IsDBNull(reader.GetOrdinal("urun_resmi")))
                    {
                        byte[] imageData = (byte[])reader["urun_resmi"];
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            Image img = Image.FromStream(ms);
                            imageList1.Images.Add(new Bitmap(img, new Size(80, 80)));
                        }
                    }
                    else
                    {
                        imageList1.Images.Add(new Bitmap(80, 80)); // Varsayılan boş resim
                    }

                    ListViewItem item = new ListViewItem();
                    item.Text = urunAdi;
                    item.ImageIndex = imageIndex;
                    item.Tag = reader["id"];
                    item.Group = listView1.Groups[0];
                    listView1.Items.Add(item);

                    imageIndex++;
                }
            }
        }

        private void LoadUnapprovedProducts(NpgsqlConnection connection)
        {
            string query = @"SELECT id, urun_adi, urun_resmi FROM urunler WHERE satista_mi = false ORDER BY id";

            using (var command = new NpgsqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                imageList2.Images.Clear();
                listView2.Items.Clear();

                int imageIndex = 0;
                while (reader.Read())
                {
                    string urunAdi = reader["urun_adi"].ToString();

                    if (!reader.IsDBNull(reader.GetOrdinal("urun_resmi")))
                    {
                        byte[] imageData = (byte[])reader["urun_resmi"];
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            Image img = Image.FromStream(ms);
                            imageList2.Images.Add(new Bitmap(img, new Size(80, 80)));
                        }
                    }
                    else
                    {
                        imageList2.Images.Add(new Bitmap(80, 80));
                    }

                    ListViewItem item = new ListViewItem();
                    item.Text = urunAdi;
                    item.ImageIndex = imageIndex;
                    item.Tag = reader["id"];
                    item.Group = listView2.Groups[0];
                    listView2.Items.Add(item);

                    imageIndex++;
                }
            }
        }

        private void ListView1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int urunId = (int)listView1.SelectedItems[0].Tag;
                ShowProductStats(urunId);
            }
        }

        private void ShowProductStats(int urunId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT 
                    urun_adi,
                    urun_aciklamasi,
                    satilan_adet,
                    stok_adeti,
                    alis_fiyati,
                    tuketici_fiyati,
                    toplam_kar,
                    (tuketici_fiyati - alis_fiyati) * stok_adeti as beklenen_kar
                FROM urunler 
                WHERE id = @id";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", urunId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                listBox1.Items.Clear();
                                listBox1.Items.Add($"Ürün: {reader["urun_adi"]}");
                                listBox1.Items.Add($"Açıklama: {reader["urun_aciklamasi"]}");
                                listBox1.Items.Add($"Satılan Adet: {reader["satilan_adet"]}");
                                listBox1.Items.Add($"Kalan Stok: {reader["stok_adeti"]}");
                                listBox1.Items.Add($"Alış Fiyatı: {reader.GetDecimal(4):C2}");
                                listBox1.Items.Add($"Satış Fiyatı: {reader.GetDecimal(5):C2}");
                                listBox1.Items.Add($"Toplam Kar: {reader.GetDecimal(6):C2}");
                                listBox1.Items.Add($"Beklenen Kar: {reader.GetDecimal(7):C2}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ürün bilgileri yüklenirken hata: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowTotalProfitAndSoldItems()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT 
                    SUM((tuketici_fiyati - alis_fiyati) * satilan_adet) AS toplam_kar,
                    SUM(satilan_adet) AS toplam_satilan_adet
                FROM urunler";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                decimal totalProfit = reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
                                int totalSoldItems = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);

                                listBox2.Items.Clear(); // Clear previous items
                                listBox2.Items.Add($"Toplam Kâr: {totalProfit:C2}");
                                listBox2.Items.Add($"Toplam Satılan Ürün Adedi: {totalSoldItems}");
                            }
                            else
                            {
                                listBox2.Items.Clear();
                                listBox2.Items.Add("Kâr ve satılan ürün bilgisi bulunamadı.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Toplam kâr ve satılan ürün bilgisi yüklenirken hata: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }






        private void ListView2_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                int urunId = (int)listView2.SelectedItems[0].Tag;
                string urunAdi = listView2.SelectedItems[0].Text;

                var result = MessageBox.Show(
                    $"{urunAdi} ürününü onaylayıp satışa çıkarmak istiyor musunuz?",
                    "Ürün Onaylama",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    ApproveProduct(urunId);
                }
            }
        }

        private void ApproveProduct(int urunId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE urunler SET satista_mi = true WHERE id = @id";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", urunId);
                        int affected = command.ExecuteNonQuery();

                        if (affected > 0)
                        {
                            MessageBox.Show("Ürün başarıyla onaylandı ve satışa çıkarıldı.",
                                "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadProducts(); // Listeleri güncelle
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ürün onaylanırken hata: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void yonetici_Load(object sender, EventArgs e)
        {
            KullanicilariListele();
            SetupListViews();
            LoadProducts();
            ShowTotalProfitAndSoldItems(); 
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Form1 loginForm = new Form1();
                loginForm.Show();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}