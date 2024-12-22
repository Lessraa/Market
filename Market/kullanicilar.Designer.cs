namespace Market
{
    partial class kullanicilar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.kullaniciList = new System.Windows.Forms.DataGridView();
            this.pictureBoxProfil = new System.Windows.Forms.PictureBox();
            this.kullaniciAdiText = new System.Windows.Forms.TextBox();
            this.sifreText = new System.Windows.Forms.TextBox();
            this.ppGuncelle = new System.Windows.Forms.Button();
            this.BilgileriGuncelle = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.yoneticiText = new System.Windows.Forms.TextBox();
            this.btnKaydet = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.kullaniciList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfil)).BeginInit();
            this.SuspendLayout();
            // 
            // kullaniciList
            // 
            this.kullaniciList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(112)))), ((int)(((byte)(123)))));
            this.kullaniciList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.kullaniciList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.kullaniciList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.kullaniciList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kullaniciList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(238)))), ((int)(((byte)(243)))));
            this.kullaniciList.Location = new System.Drawing.Point(0, 397);
            this.kullaniciList.Name = "kullaniciList";
            this.kullaniciList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.kullaniciList.Size = new System.Drawing.Size(1164, 166);
            this.kullaniciList.TabIndex = 0;
            this.kullaniciList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.kullaniciList_CellContentClick);
            // 
            // pictureBoxProfil
            // 
            this.pictureBoxProfil.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxProfil.Location = new System.Drawing.Point(55, 68);
            this.pictureBoxProfil.Name = "pictureBoxProfil";
            this.pictureBoxProfil.Size = new System.Drawing.Size(175, 160);
            this.pictureBoxProfil.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxProfil.TabIndex = 1;
            this.pictureBoxProfil.TabStop = false;
            this.pictureBoxProfil.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // kullaniciAdiText
            // 
            this.kullaniciAdiText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(238)))), ((int)(((byte)(243)))));
            this.kullaniciAdiText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.kullaniciAdiText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kullaniciAdiText.Location = new System.Drawing.Point(55, 246);
            this.kullaniciAdiText.Name = "kullaniciAdiText";
            this.kullaniciAdiText.ReadOnly = true;
            this.kullaniciAdiText.Size = new System.Drawing.Size(175, 22);
            this.kullaniciAdiText.TabIndex = 4;
            this.kullaniciAdiText.Text = "Kullanıcı Adı";
            this.kullaniciAdiText.TextChanged += new System.EventHandler(this.kullaniciAdiText_TextChanged);
            // 
            // sifreText
            // 
            this.sifreText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(238)))), ((int)(((byte)(243)))));
            this.sifreText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sifreText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.sifreText.Location = new System.Drawing.Point(55, 276);
            this.sifreText.Name = "sifreText";
            this.sifreText.ReadOnly = true;
            this.sifreText.Size = new System.Drawing.Size(175, 22);
            this.sifreText.TabIndex = 5;
            this.sifreText.Text = "Şifre";
            // 
            // ppGuncelle
            // 
            this.ppGuncelle.Location = new System.Drawing.Point(477, 313);
            this.ppGuncelle.Name = "ppGuncelle";
            this.ppGuncelle.Size = new System.Drawing.Size(175, 47);
            this.ppGuncelle.TabIndex = 6;
            this.ppGuncelle.Text = "Profil Fotoğrafını Güncelle";
            this.ppGuncelle.UseVisualStyleBackColor = true;
            this.ppGuncelle.Click += new System.EventHandler(this.ppGuncelle_Click);
            // 
            // BilgileriGuncelle
            // 
            this.BilgileriGuncelle.Location = new System.Drawing.Point(707, 313);
            this.BilgileriGuncelle.Name = "BilgileriGuncelle";
            this.BilgileriGuncelle.Size = new System.Drawing.Size(175, 47);
            this.BilgileriGuncelle.TabIndex = 7;
            this.BilgileriGuncelle.Text = "Bilgileri Güncelle";
            this.BilgileriGuncelle.UseVisualStyleBackColor = true;
            this.BilgileriGuncelle.Click += new System.EventHandler(this.BilgileriGuncelle_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(926, 313);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(175, 47);
            this.button3.TabIndex = 8;
            this.button3.Text = "Kullanıcı Ekle";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // yoneticiText
            // 
            this.yoneticiText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(238)))), ((int)(((byte)(243)))));
            this.yoneticiText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.yoneticiText.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.yoneticiText.Location = new System.Drawing.Point(55, 26);
            this.yoneticiText.Name = "yoneticiText";
            this.yoneticiText.ReadOnly = true;
            this.yoneticiText.Size = new System.Drawing.Size(175, 24);
            this.yoneticiText.TabIndex = 9;
            this.yoneticiText.Text = "Yönetici";
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(55, 327);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(175, 33);
            this.btnKaydet.TabIndex = 10;
            this.btnKaydet.Text = "button1";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Visible = false;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // kullanicilar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(238)))), ((int)(((byte)(243)))));
            this.ClientSize = new System.Drawing.Size(1164, 563);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.yoneticiText);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.BilgileriGuncelle);
            this.Controls.Add(this.ppGuncelle);
            this.Controls.Add(this.sifreText);
            this.Controls.Add(this.kullaniciAdiText);
            this.Controls.Add(this.pictureBoxProfil);
            this.Controls.Add(this.kullaniciList);
            this.Name = "kullanicilar";
            this.Text = "kullanicilar";
            this.Load += new System.EventHandler(this.kullanicilar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kullaniciList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfil)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView kullaniciList;
        private System.Windows.Forms.PictureBox pictureBoxProfil;
        private System.Windows.Forms.TextBox kullaniciAdiText;
        private System.Windows.Forms.TextBox sifreText;
        private System.Windows.Forms.Button ppGuncelle;
        private System.Windows.Forms.Button BilgileriGuncelle;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox yoneticiText;
        private System.Windows.Forms.Button btnKaydet;
    }
}