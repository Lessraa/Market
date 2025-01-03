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
        }

        private ImageList imageList1;
        private ImageList imageList2;


        int TogMove;
        int MValX;
        int MValY;

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
                                decimal totalProfit = reader.IsDBNull(0) ? 0 : reader.GetDecimal(0) ;
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
           
            kullanicilar kullanicilarFormu = new kullanicilar();
            kullanicilarFormu.Show();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void yonetici_MouseDown(object sender, MouseEventArgs e)
        {
            TogMove = 1;
            MValX = e.X;
            MValY = e.Y;
        }

        private void yonetici_MouseUp(object sender, MouseEventArgs e)
        {
            TogMove = 0;
        }

        private void yonetici_MouseMove(object sender, MouseEventArgs e)
        {
            if (TogMove == 1)
            {
                this.SetDesktopLocation(MousePosition.X - MValX, MousePosition.Y - MValY);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}