using System;
using System.Windows.Forms;
using System.Drawing;
using Npgsql;
using System.IO;
using System.Collections.Generic;

namespace Market
{
    public partial class standart : Form
    {
        private readonly int userId;
        private readonly string userName;
        private readonly string connectionString;

        private standart()
        {
            InitializeComponent();
        }


        public standart(int userId, string userName)
        {
            InitializeComponent();
            this.userId = userId;
            this.userName = userName;

            connectionString = "Host=93.113.57.54;" +
                             "Port=5432;" +
                             "Database=odev;" +
                             "Username=lessra;" +
                             "Password=476634.Ss";
        }

        private void standart_Load(object sender, EventArgs e)
        {
            this.Text = $"Hoşgeldin {userName}";
            label1.Text = $"Hoşgeldiniz {userName}";
            LoadUserData();
            SetupListView();
            LoadProducts();
        }


        private Dictionary<int, (string name, int quantity, decimal price)> sepet = new Dictionary<int, (string, int, decimal)>();


      
        private void UpdateSepetListbox()
        {
            listBox1.Items.Clear();
            decimal toplamTutar = 0;

            foreach (var item in sepet)
            {
                decimal tutar = item.Value.quantity * item.Value.price;
                listBox1.Items.Add($"{item.Value.name} x {item.Value.quantity} = {tutar:C2}");
                toplamTutar += tutar;
            }

            // Toplam tutar
            if (listBox1.Items.Count > 0)
            {
                listBox1.Items.Add("------------------------");
                listBox1.Items.Add($"Toplam: {toplamTutar:C2}");
            }
        }

        // Satış butonu
        private void button4_Click(object sender, EventArgs e)
        {
            if (sepet.Count == 0)
            {
                MessageBox.Show("Sepet boş!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in sepet)
                            {
                                string updateQuery = @"
                            UPDATE urunler 
                            SET 
                                stok_adeti = stok_adeti - @miktar,
                                satilan_adet = satilan_adet + @miktar
                            WHERE id = @urunId AND stok_adeti >= @miktar";

                                using (var command = new NpgsqlCommand(updateQuery, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@urunId", item.Key);
                                    command.Parameters.AddWithValue("@miktar", item.Value.quantity);

                                    int etkilenenSatir = command.ExecuteNonQuery();
                                    if (etkilenenSatir == 0)
                                    {
                                        throw new Exception($"'{item.Value.name}' için yeterli stok kalmamış!");
                                    }
                                }
                            }

                            transaction.Commit();
                            MessageBox.Show("Satış başarıyla tamamlandı!", "Başarılı",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Sepeti temizle ve görünümü güncelle
                            sepet.Clear();
                            listBox1.Items.Clear();
                            LoadProducts(); // Ürün listesini güncelle
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Satış işlemi iptal edildi: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Satış hatası: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUserData()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT resim FROM kullanicilar WHERE id = @userId";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read() && !reader.IsDBNull(0))
                            {
                                byte[] imageData = (byte[])reader["resim"];
                                using (MemoryStream ms = new MemoryStream(imageData))
                                {
                                    pictureBox1.Image = Image.FromStream(ms);
                                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                }
                            }
                            else
                            {
                                pictureBox1.Image = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veri yükleme hatası: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private ImageList imageList1;


        private void SetupListView()
        {
            // ImageList ayarları
            imageList1 = new ImageList();
            imageList1.ImageSize = new Size(80, 80);
            imageList1.ColorDepth = ColorDepth.Depth32Bit;

            // ListView ayarları
            listView1.View = View.LargeIcon;
            listView1.LargeImageList = imageList1;
            listView1.Groups.Add(new ListViewGroup("Satıştaki Ürünler", HorizontalAlignment.Left));

            // Tooltip için
            ToolTip toolTip1 = new ToolTip();
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;

            // Mouse hover 
            listView1.MouseMove += (s, e) =>
            {
                ListViewItem item = listView1.GetItemAt(e.X, e.Y);
                if (item != null)
                {
                    toolTip1.SetToolTip(listView1, item.ToolTipText);
                }
            };
        }



        private void ClearForm()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            pictureBox2.Image = null;
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

        private void LoadProducts()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT 
                    id, 
                    urun_adi, 
                    urun_resmi, 
                    tuketici_fiyati, 
                    stok_adeti
                FROM urunler 
                WHERE satista_mi = true 
                ORDER BY id";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            imageList1.Images.Clear();
                            listView1.Items.Clear();

                            int imageIndex = 0;
                            while (reader.Read())
                            {
                                string urunAdi = reader["urun_adi"].ToString();
                                decimal fiyat = reader.GetDecimal(reader.GetOrdinal("tuketici_fiyati"));
                                int stok = reader.GetInt32(reader.GetOrdinal("stok_adeti"));

                                // Ürün bilgilerini hazırla
                                string itemText = $"{urunAdi}\n{fiyat:C2}\nStok: {stok}";
                                string tooltipText = $"Ürün: {urunAdi}\nFiyat: {fiyat:C2}\nStok: {stok}";

                                Image urunResmi;
                                if (!reader.IsDBNull(reader.GetOrdinal("urun_resmi")))
                                {
                                    byte[] imageData = (byte[])reader["urun_resmi"];
                                    using (MemoryStream ms = new MemoryStream(imageData))
                                    {
                                        urunResmi = Image.FromStream(ms);

                                        // Resmi 80x80e
                                        urunResmi = new Bitmap(urunResmi, new Size(80, 80));

                                        // Fiyat ve stok bilgisini resmin üzerine ekle
                                        using (Graphics g = Graphics.FromImage(urunResmi))
                                        {
                                            // Yarı saydam arka plan
                                            using (SolidBrush brush = new SolidBrush(Color.FromArgb(180, 0, 0, 0)))
                                            {
                                                g.FillRectangle(brush, 0, urunResmi.Height - 40, urunResmi.Width, 40);
                                            }

                                            // Fiyat ve stok bilgisini yaz
                                            using (Font font = new Font("Arial", 8, FontStyle.Bold))
                                            {
                                                g.DrawString($"{fiyat:C2}", font, Brushes.White, new PointF(5, urunResmi.Height - 35));
                                                g.DrawString($"Stok: {stok}", font, Brushes.White, new PointF(5, urunResmi.Height - 20));
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    // Varsayılan resim
                                    urunResmi = new Bitmap(80, 80);
                                    using (Graphics g = Graphics.FromImage(urunResmi))
                                    {
                                        g.Clear(Color.LightGray);
                                        g.DrawString(urunAdi, new Font("Arial", 8), Brushes.Black, new PointF(5, 20));
                                        g.DrawString($"{fiyat:C2}", new Font("Arial", 8), Brushes.Black, new PointF(5, 35));
                                        g.DrawString($"Stok: {stok}", new Font("Arial", 8), Brushes.Black, new PointF(5, 50));
                                    }
                                }

                              
                                imageList1.Images.Add(urunResmi);

                                ListViewItem item = new ListViewItem();
                                item.Text = urunAdi;
                                item.ImageIndex = imageIndex;
                                item.Group = listView1.Groups[0];
                                item.ToolTipText = tooltipText;
                                listView1.Items.Add(item);

                                imageIndex++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ürünler yüklenirken hata oluştu: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /*private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var url = "https://www.rekoroyun.com/embed/recep-ivedik/";
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Tarayıcı açılırken hata oluştu: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/ 

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                    openFileDialog.Title = "Ürün Resmi Seçin";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        pictureBox2.Image = Image.FromFile(openFileDialog.FileName);
                        pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Resim seçme hatası: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Veri kontrolü
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Ürün adı boş olamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(textBox4.Text, out int stokAdeti) || stokAdeti < 0)
                {
                    MessageBox.Show("Geçerli bir stok adeti giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(textBox5.Text, out decimal alisFiyati) || alisFiyati < 0)
                {
                    MessageBox.Show("Geçerli bir alış fiyatı giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(textBox6.Text, out decimal tuketiciFiyati) || tuketiciFiyati < 0)
                {
                    MessageBox.Show("Geçerli bir tüketici fiyatı giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Resmi byte dizisine çevir
                byte[] urunResmi = null;
                if (pictureBox2.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pictureBox2.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        urunResmi = ms.ToArray();
                    }
                }

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = @"
                INSERT INTO urunler (
                    urun_adi, 
                    urun_aciklamasi, 
                    stok_adeti, 
                    alis_fiyati, 
                    tuketici_fiyati,
                    urun_resmi,
                    satista_mi
                ) VALUES (
                    @urunAdi, 
                    @aciklama, 
                    @stokAdeti, 
                    @alisFiyati, 
                    @tuketiciFiyati,
                    @urunResmi,
                    false
                )";

                    using (var command = new NpgsqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@urunAdi", textBox1.Text.Trim());

                        if (string.IsNullOrWhiteSpace(textBox2.Text))
                        {
                            command.Parameters.AddWithValue("@aciklama", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@aciklama", textBox2.Text.Trim());
                        }

                        command.Parameters.AddWithValue("@stokAdeti", stokAdeti);
                        command.Parameters.AddWithValue("@alisFiyati", alisFiyati);
                        command.Parameters.AddWithValue("@tuketiciFiyati", tuketiciFiyati);

                        if (urunResmi == null)
                        {
                            command.Parameters.AddWithValue("@urunResmi", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@urunResmi", urunResmi);
                        }

                        command.ExecuteNonQuery();
                        MessageBox.Show("Ürün başarıyla eklendi! Not: Ürün şu anda satışta değil.",
                            "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProducts();

                        ClearForm();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ürün ekleme hatası: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                try
                {
                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT id, urun_adi, stok_adeti, tuketici_fiyati FROM urunler WHERE urun_adi = @urunAdi";

                        using (var command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@urunAdi", listView1.SelectedItems[0].Text);

                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    int urunId = reader.GetInt32(0);
                                    string urunAdi = reader.GetString(1);
                                    int stokAdeti = reader.GetInt32(2);
                                    decimal fiyat = reader.GetDecimal(3);

                                    // Yeni miktar giriş formunu kullan
                                    int? secilenMiktar = MiktarGirisForm.ShowDialog(urunAdi, stokAdeti);

                                    if (secilenMiktar.HasValue)
                                    {
                                        // Sepete ekle veya miktarı güncelle
                                        if (sepet.ContainsKey(urunId))
                                        {
                                            var eskiDeger = sepet[urunId];
                                            sepet[urunId] = (eskiDeger.name, eskiDeger.quantity + secilenMiktar.Value, eskiDeger.price);
                                        }
                                        else
                                        {
                                            sepet.Add(urunId, (urunAdi, secilenMiktar.Value, fiyat));
                                        }

                                        UpdateSepetListbox();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                Chat chatForm = new Chat(userName);
                chatForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chat başlatılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}