using System.Drawing;
using System.Windows.Forms;

public class MiktarGirisForm : Form
{
    private NumericUpDown numericUpDown1;
    private Button btnOK;
    private Button btnCancel;
    private Label label1;
    public int Miktar { get; private set; }

    public MiktarGirisForm(string urunAdi, int maxMiktar)
    {
        InitializeComponents();
        this.Text = "Miktar Girişi";
        label1.Text = $"Kaç adet {urunAdi} almak istiyorsunuz?\nMevcut Stok: {maxMiktar}";
        numericUpDown1.Maximum = maxMiktar;
        numericUpDown1.Minimum = 1;
        numericUpDown1.Value = 1;
    }

    private void InitializeComponents()
    {
        this.Size = new Size(300, 150);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        label1 = new Label
        {
            Location = new Point(10, 10),
            Size = new Size(260, 40),
            AutoSize = false
        };

        numericUpDown1 = new NumericUpDown
        {
            Location = new Point(10, 50),
            Size = new Size(260, 25),
            TextAlign = HorizontalAlignment.Center
        };

        btnOK = new Button
        {
            Text = "Tamam",
            DialogResult = DialogResult.OK,
            Location = new Point(110, 80),
            Size = new Size(80, 25)
        };

        btnCancel = new Button
        {
            Text = "İptal",
            DialogResult = DialogResult.Cancel,
            Location = new Point(190, 80),
            Size = new Size(80, 25)
        };

        this.Controls.AddRange(new Control[] { label1, numericUpDown1, btnOK, btnCancel });
        this.AcceptButton = btnOK;
        this.CancelButton = btnCancel;
    }

    public static int? ShowDialog(string urunAdi, int maxMiktar)
    {
        using (var form = new MiktarGirisForm(urunAdi, maxMiktar))
        {
            if (form.ShowDialog() == DialogResult.OK)
            {
                return (int)form.numericUpDown1.Value;
            }
            return null;
        }
    }

    private void InitializeComponent()
    {
            this.SuspendLayout();
            // 
            // MiktarGirisForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "MiktarGirisForm";
            this.Load += new System.EventHandler(this.MiktarGirisForm_Load);
            this.ResumeLayout(false);

    }

    private void MiktarGirisForm_Load(object sender, System.EventArgs e)
    {

    }
}