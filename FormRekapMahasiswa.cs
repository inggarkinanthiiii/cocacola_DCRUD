using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
    {
    public partial class FormRekapMahasiswa : Form
    {
        DAL dbLogic = new DAL();
        private SqlConnection conn;

        private string connectionString =
            "Data Source=.\\SQLEXPRESS;Initial Catalog=DBAkademikADO;User ID=sa;Password=123456789";

        SqlDataAdapter da;

        DataTable dtMahasiswa;
        DataTable dtProdi;

        public FormRekapMahasiswa()
        {
            InitializeComponent();

            conn = new SqlConnection(connectionString);
        }

        private void label2_Click(object sender, EventArgs e)
            {

            }

            private void label3_Click(object sender, EventArgs e)
            {

            }

        private void FormRekapMahasiswa_Load(
    object sender,
    EventArgs e)
        {
            dtpTanggalMasuk.Format =
                DateTimePickerFormat.Custom;

            dtpTanggalMasuk.CustomFormat = "yyyy";

            dtpTanggalMasuk.ShowUpDown = true;

            dtpTanggalMasuk.MinDate =
                new DateTime(2000, 1, 1);

            dtpTanggalMasuk.MaxDate =
                DateTime.Now;

            cmbProdi.DropDownStyle =
                ComboBoxStyle.DropDownList;

            btnCetak.Enabled = false;

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd =
                    new SqlCommand(
                        "SELECT namaprodi FROM programstudi",
                        conn);

                cmd.CommandType =
                    CommandType.Text;

                dtProdi = new DataTable();

                da = new SqlDataAdapter(cmd);

                da.Fill(dtProdi);

                cmbProdi.DataSource = dtProdi;

                cmbProdi.DisplayMember = "namaprodi";

                cmbProdi.ValueMember = "namaprodi";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal load data : " +
                    ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand("sp_Report", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@inProdi", SqlDbType.VarChar, 50).Value = cmbProdi.SelectedValue;
                cmd.Parameters.Add("@inTglMsuk", SqlDbType.VarChar, 4).Value = dtpTanggalMasuk.Value.Year.ToString();

                da = new SqlDataAdapter(cmd);

                dtMahasiswa = new DataTable();
                da.Fill(dtMahasiswa);

                dataGridView1.DataSource = dtMahasiswa;

                if (dtMahasiswa.Rows.Count > 0)
                {
                    btnCetak.Enabled = true;
                }
                else
                {
                    btnCetak.Enabled = false;
                    MessageBox.Show("Data tidak ditemukan");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message);
            }

        }

        private void FormRekapMahasiswa_Load_1(object sender, EventArgs e)
        {

            dtpTanggalMasuk.Format =
                DateTimePickerFormat.Custom;

            dtpTanggalMasuk.CustomFormat = "yyyy";

            dtpTanggalMasuk.ShowUpDown = true;

            dtpTanggalMasuk.MinDate =
                new DateTime(2000, 1, 1);

            dtpTanggalMasuk.MaxDate =
                DateTime.Now;

            cmbProdi.DropDownStyle =
                ComboBoxStyle.DropDownList;

            btnCetak.Enabled = false;

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd =
                    new SqlCommand(
                        "SELECT namaprodi FROM programstudi",
                        conn);

                cmd.CommandType =
                    CommandType.Text;

                dtProdi = new DataTable();

                da = new SqlDataAdapter(cmd);

                da.Fill(dtProdi);

                cmbProdi.DataSource = dtProdi;

                cmbProdi.DisplayMember = "namaprodi";

                cmbProdi.ValueMember = "namaprodi";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal load data : " +
                    ex.Message);
            }
        }

        private void btnCetak_Click(
            object sender,
            EventArgs e)
        {
            FormCetakMahasiswa frm =
                new FormCetakMahasiswa(
                    cmbProdi.SelectedValue.ToString(),
                    dtpTanggalMasuk.Value);

            frm.Show();
        }
    }
    }
