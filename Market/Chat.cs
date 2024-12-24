using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Market
{
    public partial class Chat : Form
    {
        private string _kullaniciAdi;
        private string connectionString = "Host=93.113.57.54;Port=5432;Database=odev;Username=lessra;Password=476634.Ss";

        public Chat(string kullaniciAdi)
        {
            InitializeComponent();
            _kullaniciAdi = kullaniciAdi;
        }

        private void Chat_Load(object sender, EventArgs e)
        {
            this.Text = $"Sohbet - {_kullaniciAdi}";
            MesajlariGetir();
        }

  
        private void MesajGonder(string mesaj)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO chat (user_id, message) VALUES ((SELECT id FROM kullanicilar WHERE kullanici_adi = @kullaniciAdi), @message)";
                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("kullaniciAdi", _kullaniciAdi);
                        cmd.Parameters.AddWithValue("message", mesaj);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Mesaj gönderilirken hata oluştu: {ex.Message}");
            }
        }

        private void MesajlariGetir()
        {
            listBox1.Items.Clear();
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT c.message, c.date, k.kullanici_adi, k.resim
                                     FROM chat c
                                     JOIN kullanicilar k ON c.user_id = k.id
                                     ORDER BY c.date ASC";
                    using (var cmd = new NpgsqlCommand(query, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string kullaniciAdi = reader["kullanici_adi"].ToString();
                            string mesaj = reader["message"].ToString();
                            DateTime tarih = Convert.ToDateTime(reader["date"]);
                            byte[] resimBytes = reader["resim"] as byte[];

                            string displayText = $"{kullaniciAdi}: {mesaj} ({tarih:G})";
                            listBox1.Items.Add(new MesajItem
                            {
                                KullaniciAdi = kullaniciAdi,
                                Mesaj = mesaj,
                                Tarih = tarih,
                                Resim = resimBytes
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Mesajlar getirilirken hata oluştu: {ex.Message}");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string mesaj = textBox1.Text.Trim();
            if (!string.IsNullOrEmpty(mesaj))
            {
                MesajGonder(mesaj);
                textBox1.Clear();
                MesajlariGetir();
            }
            else
            {
                MessageBox.Show("Lütfen bir mesaj yazın!");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Yaz..")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Yaz..";
                textBox1.ForeColor = Color.Black;
            }
        }
    }

    public class MesajItem
    {
        public string KullaniciAdi { get; set; }
        public string Mesaj { get; set; }
        public DateTime Tarih { get; set; }
        public byte[] Resim { get; set; }

        public override string ToString()
        {
            return $"{KullaniciAdi}: {Mesaj} ({Tarih:G})";
        }
    }
}
