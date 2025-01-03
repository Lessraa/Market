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
    public partial class Form3 : Form
    {

        private readonly string connectionString;
        //private PictureBox pictureBox1;
        private Button btnResimSec;
        public Form3()
        {
            InitializeComponent();

            connectionString = "Host=93.113.57.54;" +
                           "Port=5432;" +
                           "Database=odev;" +
                           "Username=lessra;" +
                           "Password=476634.Ss";


            // PictureBox ayarları
            PPsec = new PictureBox();
            PPsec.Size = new Size(100, 100);
            PPsec.SizeMode = PictureBoxSizeMode.Zoom;
            PPsec.BorderStyle = BorderStyle.FixedSingle;
            PPsec.Location = new Point(textBox2.Left, textBox2.Bottom + 10);

            // Resim seçme butonu
            btnResimSec = new Button();
            btnResimSec.Text = "Resim Seç";
            btnResimSec.Location = new Point(PPsec.Right + 10, PPsec.Top);
            btnResimSec.Click += button1_Click;

            // Kontrolleri forma ekle
            Controls.Add(PPsec);
            Controls.Add(btnResimSec);


        }



       /* private ImageList imageList1;
        private ImageList imageList2;*/


        private void BtnResimSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.Title = "Bir Resim Dosyası Seçin";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    PPsec.Image = Image.FromFile(openFileDialog.FileName);
                }
            }
        }



        int TogMove;
        int MValX;
        int MValY;
        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Kullanıcı Adı")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Kullanıcı Adı";
                textBox1.ForeColor = Color.Silver;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Şifre")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Şifre";
                textBox2.ForeColor = Color.Silver;
            }
        }

        private void Form3_MouseDown(object sender, MouseEventArgs e)
        {
            TogMove = 1;
            MValX = e.X;
            MValY = e.Y;
        }

        private void Form3_MouseUp(object sender, MouseEventArgs e)
        {
            TogMove = 0;
        }

        private void Form3_MouseMove(object sender, MouseEventArgs e)
        {
            if (TogMove == 1)
            {
                this.SetDesktopLocation(MousePosition.X - MValX, MousePosition.Y - MValY);
            }
        }

        private void KullaniciEkle_Click(object sender, EventArgs e)
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
                if (PPsec.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        PPsec.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
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
                            PPsec.Image = null;


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

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.Title = "Bir Resim Dosyası Seçin";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    PPsec.Image = Image.FromFile(openFileDialog.FileName);
                }
            }
        }
    }
}
