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
            this.kullaniciEkle = new System.Windows.Forms.Button();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
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
            this.kullaniciList.Location = new System.Drawing.Point(0, 433);
            this.kullaniciList.Name = "kullaniciList";
            this.kullaniciList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.kullaniciList.Size = new System.Drawing.Size(1164, 196);
            this.kullaniciList.TabIndex = 0;
            this.kullaniciList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.kullaniciList_CellContentClick);
            // 
            // pictureBoxProfil
            // 
            this.pictureBoxProfil.BackColor = System.Drawing.Color.Linen;
            this.pictureBoxProfil.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxProfil.Location = new System.Drawing.Point(129, 107);
            this.pictureBoxProfil.Name = "pictureBoxProfil";
            this.pictureBoxProfil.Size = new System.Drawing.Size(175, 160);
            this.pictureBoxProfil.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxProfil.TabIndex = 1;
            this.pictureBoxProfil.TabStop = false;
            this.pictureBoxProfil.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // kullaniciAdiText
            // 
            this.kullaniciAdiText.BackColor = System.Drawing.Color.Linen;
            this.kullaniciAdiText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.kullaniciAdiText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kullaniciAdiText.Location = new System.Drawing.Point(535, 109);
            this.kullaniciAdiText.Name = "kullaniciAdiText";
            this.kullaniciAdiText.ReadOnly = true;
            this.kullaniciAdiText.Size = new System.Drawing.Size(175, 22);
            this.kullaniciAdiText.TabIndex = 4;
            this.kullaniciAdiText.Text = "Kullanıcı Adı";
            this.kullaniciAdiText.TextChanged += new System.EventHandler(this.kullaniciAdiText_TextChanged);
            // 
            // sifreText
            // 
            this.sifreText.BackColor = System.Drawing.Color.Linen;
            this.sifreText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sifreText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.sifreText.Location = new System.Drawing.Point(536, 167);
            this.sifreText.Name = "sifreText";
            this.sifreText.ReadOnly = true;
            this.sifreText.Size = new System.Drawing.Size(175, 22);
            this.sifreText.TabIndex = 5;
            this.sifreText.Text = "Şifre";
            // 
            // ppGuncelle
            // 
            this.ppGuncelle.BackColor = System.Drawing.Color.Transparent;
            this.ppGuncelle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ppGuncelle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ppGuncelle.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ppGuncelle.Location = new System.Drawing.Point(91, 332);
            this.ppGuncelle.Name = "ppGuncelle";
            this.ppGuncelle.Size = new System.Drawing.Size(252, 68);
            this.ppGuncelle.TabIndex = 6;
            this.ppGuncelle.Text = "Profil Fotoğrafını Güncelle";
            this.ppGuncelle.UseVisualStyleBackColor = false;
            this.ppGuncelle.Click += new System.EventHandler(this.ppGuncelle_Click);
            // 
            // BilgileriGuncelle
            // 
            this.BilgileriGuncelle.BackColor = System.Drawing.Color.Transparent;
            this.BilgileriGuncelle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BilgileriGuncelle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.BilgileriGuncelle.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BilgileriGuncelle.Location = new System.Drawing.Point(475, 331);
            this.BilgileriGuncelle.Name = "BilgileriGuncelle";
            this.BilgileriGuncelle.Size = new System.Drawing.Size(260, 68);
            this.BilgileriGuncelle.TabIndex = 7;
            this.BilgileriGuncelle.Text = "Bilgileri Güncelle";
            this.BilgileriGuncelle.UseVisualStyleBackColor = false;
            this.BilgileriGuncelle.Click += new System.EventHandler(this.BilgileriGuncelle_Click);
            // 
            // kullaniciEkle
            // 
            this.kullaniciEkle.BackColor = System.Drawing.Color.Transparent;
            this.kullaniciEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.kullaniciEkle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kullaniciEkle.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.kullaniciEkle.Location = new System.Drawing.Point(871, 332);
            this.kullaniciEkle.Name = "kullaniciEkle";
            this.kullaniciEkle.Size = new System.Drawing.Size(230, 68);
            this.kullaniciEkle.TabIndex = 8;
            this.kullaniciEkle.Text = "Kullanıcı Ekle:";
            this.kullaniciEkle.UseVisualStyleBackColor = false;
            this.kullaniciEkle.Click += new System.EventHandler(this.kullaniciEkle_Click);
            // 
            // btnKaydet
            // 
            this.btnKaydet.BackColor = System.Drawing.Color.White;
            this.btnKaydet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKaydet.Location = new System.Drawing.Point(535, 222);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(175, 45);
            this.btnKaydet.TabIndex = 10;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = false;
            this.btnKaydet.Visible = false;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(454, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(257, 25);
            this.label1.TabIndex = 11;
            this.label1.Text = "Kullanıcı Profil Arayüzü";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.ForeColor = System.Drawing.Color.BlanchedAlmond;
            this.label2.Location = new System.Drawing.Point(397, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 24);
            this.label2.TabIndex = 12;
            this.label2.Text = "Kullanıcı Adı:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.ForeColor = System.Drawing.Color.BlanchedAlmond;
            this.label3.Location = new System.Drawing.Point(435, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 24);
            this.label3.TabIndex = 13;
            this.label3.Text = "Şifre      :";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(535, 134);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(175, 3);
            this.panel1.TabIndex = 14;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Location = new System.Drawing.Point(536, 193);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(175, 3);
            this.panel2.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.ForeColor = System.Drawing.SystemColors.Window;
            this.label4.Location = new System.Drawing.Point(170, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 24);
            this.label4.TabIndex = 16;
            this.label4.Text = "Yönetici";
            // 
            // kullanicilar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(238)))), ((int)(((byte)(243)))));
            this.BackgroundImage = global::Market.Properties.Resources._360_F_860318940_RPEYG8qhieE8pyk4Fnc1N4qGEU2avo0q;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1164, 629);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.kullaniciAdiText);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.kullaniciEkle);
            this.Controls.Add(this.BilgileriGuncelle);
            this.Controls.Add(this.ppGuncelle);
            this.Controls.Add(this.sifreText);
            this.Controls.Add(this.pictureBoxProfil);
            this.Controls.Add(this.kullaniciList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "kullanicilar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
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
        private System.Windows.Forms.Button kullaniciEkle;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
    }
}