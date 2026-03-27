using System;
using System.Windows.Forms;
using StockBaseApp.Modeloak;
using StockBaseApp.Kontrolagailuak;

namespace StockBaseApp
{
    public partial class Form1 : Form
    {
        private InbentarioKudeatzailea kudeatzailea;

        public Form1()
        {
            InitializeComponent();
            kudeatzailea = new InbentarioKudeatzailea();
        }

        // Selecciona tu botón 'btnGorde' en el diseñador visual, hazle doble clic 
        // y asegúrate de que se conecte con esta función:
        private void btnGorde_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Datuak jaso
                int id = Convert.ToInt32(txtId.Text);
                string marka = txtMarka.Text;
                string kokalekua = txtKokalekua.Text;
                DateTime data = dtpErosketa.Value;

                if (string.IsNullOrWhiteSpace(marka))
                {
                    MessageBox.Show("Marka ezin da hutsik egon.", "Abisua", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbMota.SelectedItem == null)
                {
                    MessageBox.Show("Mesedez, aukeratu gailu mota bat.", "Abisua", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string aukeratutakoMota = cmbMota.SelectedItem.ToString();

                // 2. Objektuak sortu eta gorde
                if (aukeratutakoMota == "Inprimagailua")
                {
                    bool koloretakoa = chkKolorea.Checked;
                    Inprimagailua inprimagailuBerria = new Inprimagailua(id, marka, kokalekua, "Aktiboa", data, koloretakoa);
                    kudeatzailea.GailuaGehitu(inprimagailuBerria);
                }
                else if (aukeratutakoMota == "Ordenagailua")
                {
                    // Para simplificar, le ponemos datos por defecto a la RAM y CPU si no hemos creado TextBox para ellos aún
                    Ordenagailua ordenagailuBerria = new Ordenagailua(id, marka, kokalekua, "Aktiboa", data, "Intel i5", 8);
                    kudeatzailea.GailuaGehitu(ordenagailuBerria);
                }

                MessageBox.Show("Gailua ondo erregistratu da!", "Arrakasta", MessageBoxButtons.OK, MessageBoxIcon.Information);

                EguneratuTaula();
                LimpiarFormulario();
            }
            catch (FormatException)
            {
                MessageBox.Show("Errorea: ID eremuan zenbaki bat sartu behar duzu.", "Datu okerra", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ezusteko errorea gertatu da: " + ex.Message, "Errorea", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EguneratuTaula()
        {
            dgvInbentarioa.DataSource = null;
            dgvInbentarioa.DataSource = kudeatzailea.LortuGailuak();

            
            if (dgvInbentarioa.Columns.Count > 0)
            {
                dgvInbentarioa.Columns["IdGailua"].HeaderText = "Gailuaren ID-a";
                dgvInbentarioa.Columns["Marka"].HeaderText = "Marka";
                dgvInbentarioa.Columns["Kokalekua"].HeaderText = "Kokalekua";
                dgvInbentarioa.Columns["Egoera"].HeaderText = "Egoera";
                dgvInbentarioa.Columns["ErosketaData"].HeaderText = "Erosketa Data";
            }
        }

        private void LimpiarFormulario()
        {
            txtId.Clear();
            txtMarka.Clear();
            txtKokalekua.Clear();
            chkKolorea.Checked = false;
            cmbMota.SelectedIndex = -1;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void dgvInbentarioa_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }
    }
}