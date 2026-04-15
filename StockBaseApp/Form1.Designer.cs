namespace StockBaseApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            txtId = new TextBox();
            txtMarka = new TextBox();
            txtKokalekua = new TextBox();
            dtpErosketa = new DateTimePicker();
            cmbMota = new ComboBox();
            chkKolorea = new CheckBox();
            btnGorde = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            dgvInbentarioa = new DataGridView();
            pictureBox1 = new PictureBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInbentarioa).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // txtId
            // 
            txtId.Location = new Point(35, 100);
            txtId.Name = "txtId";
            txtId.Size = new Size(165, 30);
            txtId.TabIndex = 1;
            // 
            // txtMarka
            // 
            txtMarka.Location = new Point(35, 169);
            txtMarka.Name = "txtMarka";
            txtMarka.Size = new Size(165, 30);
            txtMarka.TabIndex = 2;
            txtMarka.TextChanged += txtMarka_TextChanged;
            // 
            // txtKokalekua
            // 
            txtKokalekua.Location = new Point(35, 248);
            txtKokalekua.Name = "txtKokalekua";
            txtKokalekua.Size = new Size(165, 30);
            txtKokalekua.TabIndex = 3;
            // 
            // dtpErosketa
            // 
            dtpErosketa.Location = new Point(55, 91);
            dtpErosketa.Name = "dtpErosketa";
            dtpErosketa.Size = new Size(412, 30);
            dtpErosketa.TabIndex = 4;
            // 
            // cmbMota
            // 
            cmbMota.FormattingEnabled = true;
            cmbMota.Items.AddRange(new object[] { "Inprimagailua", "Ordenagailua" });
            cmbMota.Location = new Point(417, 100);
            cmbMota.Name = "cmbMota";
            cmbMota.Size = new Size(200, 31);
            cmbMota.TabIndex = 5;
            cmbMota.SelectedIndexChanged += cmbMota_SelectedIndexChanged_1;
            // 
            // chkKolorea
            // 
            chkKolorea.AutoSize = true;
            chkKolorea.Location = new Point(434, 183);
            chkKolorea.Name = "chkKolorea";
            chkKolorea.Size = new Size(155, 27);
            chkKolorea.TabIndex = 6;
            chkKolorea.Text = "Koloretakoa da?";
            chkKolorea.UseVisualStyleBackColor = true;
            chkKolorea.CheckedChanged += chkKolorea_CheckedChanged;
            // 
            // btnGorde
            // 
            btnGorde.BackColor = SystemColors.ActiveCaption;
            btnGorde.Location = new Point(494, 301);
            btnGorde.Name = "btnGorde";
            btnGorde.Size = new Size(123, 38);
            btnGorde.TabIndex = 8;
            btnGorde.Text = "Gorde";
            btnGorde.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(35, 63);
            label1.Name = "label1";
            label1.Size = new Size(31, 23);
            label1.TabIndex = 8;
            label1.Text = "ID:";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(35, 133);
            label2.Name = "label2";
            label2.Size = new Size(61, 23);
            label2.TabIndex = 9;
            label2.Text = "Marka:";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(35, 216);
            label3.Name = "label3";
            label3.Size = new Size(91, 23);
            label3.TabIndex = 10;
            label3.Text = "Kokalekua:";
            label3.Click += label3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(55, 34);
            label4.Name = "label4";
            label4.Size = new Size(119, 23);
            label4.TabIndex = 11;
            label4.Text = "Erosketa Data:";
            label4.Click += label4_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(417, 57);
            label5.Name = "label5";
            label5.Size = new Size(54, 23);
            label5.TabIndex = 12;
            label5.Text = "Mota:";
            label5.Click += label5_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(btnGorde);
            groupBox1.Controls.Add(txtId);
            groupBox1.Controls.Add(txtMarka);
            groupBox1.Controls.Add(chkKolorea);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtKokalekua);
            groupBox1.Controls.Add(cmbMota);
            groupBox1.Controls.Add(label2);
            groupBox1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(52, 187);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(803, 402);
            groupBox1.TabIndex = 13;
            groupBox1.TabStop = false;
            groupBox1.Text = "Gailu Berria";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dgvInbentarioa);
            groupBox2.Location = new Point(861, 187);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(815, 402);
            groupBox2.TabIndex = 14;
            groupBox2.TabStop = false;
            groupBox2.Text = "Inbentarioa";
            groupBox2.Enter += groupBox2_Enter;
            // 
            // dgvInbentarioa
            // 
            dataGridViewCellStyle1.BackColor = Color.AliceBlue;
            dgvInbentarioa.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvInbentarioa.BackgroundColor = SystemColors.Window;
            dgvInbentarioa.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvInbentarioa.Location = new Point(19, 57);
            dgvInbentarioa.Name = "dgvInbentarioa";
            dgvInbentarioa.ReadOnly = true;
            dgvInbentarioa.RowHeadersVisible = false;
            dgvInbentarioa.RowHeadersWidth = 62;
            dgvInbentarioa.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInbentarioa.Size = new Size(774, 312);
            dgvInbentarioa.TabIndex = 7;
            dgvInbentarioa.CellContentClick += dgvInbentarioa_CellContentClick;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.StockBase_logoa;
            pictureBox1.Location = new Point(1382, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(294, 150);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 15;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click_1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1768, 757);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(label4);
            Controls.Add(dtpErosketa);
            Controls.Add(pictureBox1);
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "StockBase - Inbentarioaren Kudeaktea";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvInbentarioa).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtId;
        private TextBox txtMarka;
        private TextBox txtKokalekua;
        private DateTimePicker dtpErosketa;
        private ComboBox cmbMota;
        private CheckBox chkKolorea;
        private Button btnGorde;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private DataGridView dgvInbentarioa;
        private PictureBox pictureBox1;
    }
}
