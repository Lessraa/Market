using System;
using System.IO;
using System.Windows.Forms;
using Npgsql;

namespace Market
{
    public partial class Form1 : Form
    {
        private readonly string connectionString;

        public Form1()
        {
            InitializeComponent();

            connectionString = "Host=93.113.57.54;" +
                             "Port=5432;" +
                             "Database=odev;" +
                             "Username=lessra;" +
                             "Password=476634.Ss";
        }

        public static int userId;
        public static string userName;
        public static string userPassword;
        public static string userType;

        public static int userId;
        public static string userName;
        public static string userPassword;
        public static string userType;

        int TogMove;
        int MValX;
        int MValY;
        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Kullanıcı adı ve şifre boş bırakılamaz!",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT id, kullanici_adi,sifre, kullanici_tipi 
                                   FROM kullanicilar 
                                   WHERE kullanici_adi = @username 
                                   AND sifre = @password";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                 userId = reader.GetInt32(0);
                                userName = reader.GetString(1);
                                userPassword = reader.GetString(2);
                                 userType = reader.GetString(3);

                                MessageBox.Show("Giriş başarılı!", "Başarılı",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                if (userType.ToLower() == "yonetici")
                                {
                                    string secilenTip = GirisTipiForm.Show(this);

                                    if (!string.IsNullOrEmpty(secilenTip))
                                    {
                                        if (secilenTip == "yonetici")
                                        {
                                            var yoneticiForm = new yonetici();
                                            yoneticiForm.Show();
                                        }
                                        else
                                        {
                                            var standartForm = new standart(userId, userName);
                                            standartForm.Show();
                                        }
                                        this.Hide();
                                    }
                                }
                                else
                                {
                                    var standartForm = new standart(userId, userName);
                                    standartForm.Show();
                                    this.Hide();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Kullanıcı adı veya şifre hatalı!",
                                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı hatası: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
                Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var yoneticiForm = new yonetici();
            yoneticiForm.Show();
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            var kullaniciekle = new Form3();
            kullaniciekle.Show();
            
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            TogMove = 1;
            MValX = e.X;
            MValY = e.Y;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            TogMove = 1;
            MValX = e.X;
            MValY = e.Y;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            TogMove = 0;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if(TogMove == 1)
            {
                this.SetDesktopLocation(MousePosition.X - MValX, MousePosition.Y - MValY);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            TogMove = 0;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (TogMove == 1)
            {
                this.SetDesktopLocation(MousePosition.X - MValX, MousePosition.Y - MValY);
            }
        }
    }
}