using System;
using System.Drawing;
using System.Windows.Forms;

namespace Market
{
    public class GirisTipiForm : Form
    {
        public string SecilenTip { get; private set; }

        public GirisTipiForm()
        {
            this.Text = "Giriş Tipi Seçin";
            this.Size = new Size(300, 150);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label label = new Label
            {
                Text = "Hangi yetkiyle giriş yapmak istiyorsunuz?",
                AutoSize = true,
                Location = new Point(30, 20)
            };

            Button btnYonetici = new Button
            {
                Text = "Yönetici Girişi",
                Size = new Size(100, 30),
                Location = new Point(30, 50)
            };
            btnYonetici.Click += (s, e) =>
            {
                SecilenTip = "yonetici";
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            Button btnStandart = new Button
            {
                Text = "Standart Giriş",
                Size = new Size(100, 30),
                Location = new Point(150, 50)
            };
            btnStandart.Click += (s, e) =>
            {
                SecilenTip = "standart";
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            this.Controls.AddRange(new Control[] { label, btnYonetici, btnStandart });
        }

        public static string Show(IWin32Window owner)
        {
            using (var form = new GirisTipiForm())
            {
                if (form.ShowDialog(owner) == DialogResult.OK)
                {
                    return form.SecilenTip;
                }
                return null;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // GirisTipiForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "GirisTipiForm";
            this.Load += new System.EventHandler(this.GirisTipiForm_Load);
            this.ResumeLayout(false);

        }

        private void GirisTipiForm_Load(object sender, EventArgs e)
        {

        }
    }
}