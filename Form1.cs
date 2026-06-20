using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using ExcelDataReader;

namespace CRUDMahasiswaADO
{
    public partial class Form1 : Form
    {
        DAL dbLogic = new DAL();
        private SqlConnection conn;
        private DataTable dtMahasiswa = new DataTable();
        private BindingSource bsMahasiswa = new BindingSource();

        private void SimpanLog(string pesan)
        {
            using (SqlConnection conn =
                new SqlConnection(DAL.GetConnectionString()))
            {
                string query =
                    "INSERT INTO LogError(PesanError) VALUES(@Pesan)";

                SqlCommand cmd =
                    new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Pesan", pesan);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void simpanLog(string message)
        {
            SimpanLog(message);
        }
        public Form1()
        {
            InitializeComponent();
            conn = new SqlConnection(DAL.GetConnectionString());
        }

        // 🔹 Method koneksi database
        private void ConnectDatabase()
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                MessageBox.Show("Koneksi berhasil");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi gagal: " + ex.Message);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn =
                    new SqlConnection(DAL.GetConnectionString()))
                {
                    conn.Open();

                    MessageBox.Show("Koneksi Berhasil");
                }
            }
            catch (SqlException ex)
            {
                SimpanLog(ex.Message);

                MessageBox.Show(
                    "SQL Error : " + ex.Message);
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);

                MessageBox.Show(
                    "General Error : " + ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] ConvertImageToBytes(PictureBox pb)
                {
                    if (pb.Image == null)
                        return null;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        pb.Image.Save(
                            ms,
                            System.Drawing.Imaging.ImageFormat.Jpeg);

                        return ms.ToArray();
                    }
                }

                byte[] imgBytes =
                    ConvertImageToBytes(fotoMhs);

                dbLogic.InsertMhs(
                    txtNIM.Text,
                    txtNama.Text,
                    txtAlamat.Text,
                    cmbJK.Text,
                    dtpTanggalLahir.Value.Date,
                    txtKodeProdi.Text,
                    imgBytes);

                MessageBox.Show(
                    "Data mahasiswa berhasil ditambahkan");

                ClearForm();

                LoadData();
            }
            catch (SqlException ex)
            {
                SimpanLog("Rollback Insert : " + ex.Message);

                MessageBox.Show(
                    "SQL Error : " + ex.Message);
            }
            catch (Exception ex)
            {
                SimpanLog("General Error : " + ex.Message);

                MessageBox.Show(
                    "General Error : " + ex.Message);
            }
        }

        private void ClearForm()
        {
            txtNIM.Enabled = true;

            txtNIM.Clear();
            txtNama.Clear();

            cmbJK.SelectedIndex = -1;

            txtAlamat.Clear();

            txtKodeProdi.Clear();

            dtpTanggalLahir.Value = DateTime.Now;

            if (fotoMhs != null)
            {
                fotoMhs.Image = null;
            }

            txtNIM.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] ConvertImageToBytes(PictureBox pb)
                {
                    if (pb.Image == null)
                        return null;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        pb.Image.Save(
                            ms,
                            System.Drawing.Imaging.ImageFormat.Jpeg);

                        return ms.ToArray();
                    }
                }

                byte[] imgBytes =
                    ConvertImageToBytes(fotoMhs);

                dbLogic.UpdateMhs(
                    txtNIM.Text,
                    txtNama.Text,
                    txtAlamat.Text,
                    cmbJK.Text,
                    dtpTanggalLahir.Value.Date,
                    txtKodeProdi.Text,
                    imgBytes);

                MessageBox.Show(
                    "Data mahasiswa berhasil diubah");

                ClearForm();

                LoadData();
            }
            catch (SqlException ex)
            {
                SimpanLog(ex.Message);

                MessageBox.Show(
                    "SQL Error : " + ex.Message);
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);

                MessageBox.Show(
                    "General Error : " + ex.Message);
            }
        }


        private void dataGridView1_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataRow row =
                    ((DataRowView)bsMahasiswa[e.RowIndex]).Row;

                txtNIM.Text = row[0].ToString();
                txtNama.Text = row[1].ToString();
                cmbJK.Text = row[2].ToString();

                dtpTanggalLahir.Value =
                    Convert.ToDateTime(row[3]);

                txtAlamat.Text = row[4].ToString();

                txtKodeProdi.Text =
                    row[6].ToString();

                if (row[5] != DBNull.Value)
                {
                    byte[] imgBytes = (byte[])row[5];

                    using (MemoryStream ms =
                        new MemoryStream(imgBytes))
                    {
                        Image originalImage = Image.FromStream(ms);
                        fotoMhs.Image = new Bitmap(originalImage);
                        originalImage.Dispose();

                        fotoMhs.SizeMode =
                            PictureBoxSizeMode.StretchImage;
                    }
                }
                else
                {
                    fotoMhs.Image = null;
                }

                txtNIM.Enabled = false;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // ❌ HAPUS ini (tidak dipakai)
            // this.mahasiswaTableAdapter.Fill(this.dBAkademikADODataSet.Mahasiswa);

            // ComboBox JK
            cmbJK.Items.Clear();
            cmbJK.Items.Add("L");
            cmbJK.Items.Add("P");

            // Setting DataGridView
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 🔥 INI INTI LANGKAH 2
            bsMahasiswa.DataSource = dtMahasiswa;
            dataGridView1.DataSource = bsMahasiswa;
            bindingNavigator1.BindingSource = bsMahasiswa;

            // 🔗 Binding ke TextBox (biar otomatis berubah)


            dataGridView1.CellClick += dataGridView1_CellClick;
        }

        private void HitungTotal()
        {
            try
            {
                int total = dbLogic.CountMhs();

                lblTotal.Text =
                    "Total Mahasiswa : " + total;
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);

                MessageBox.Show(
                    "Gagal load data : " + ex.Message);
            }
        }
        private void LoadData()
        {
            try
            {
                DataTable dt = dbLogic.GetMhs();

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Tidak ada data mahasiswa.");
                    dataGridView1.DataSource = null;
                    return;
                }

                bsMahasiswa.DataSource = dt;
                dataGridView1.DataSource = bsMahasiswa;

                HitungTotal();

                dataGridView1.Enabled = true;
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);

                MessageBox.Show(
                    "Gagal load data : " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dg = MessageBox.Show(
                    "Yakin ingin menghapus data?",
                    "Konfirmasi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dg == DialogResult.Yes)
                {
                    dbLogic.DeleteMhs(txtNIM.Text);

                    MessageBox.Show(
                        "Data mahasiswa berhasil dihapus");

                    ClearForm();

                    LoadData();
                }
            }
            catch (SqlException ex)
            {
                SimpanLog(ex.Message);

                MessageBox.Show(
                    "SQL Error : " + ex.Message);
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);

                MessageBox.Show(
                    "General Error : " + ex.Message);
            }
        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

        private void btnResetData_Click(object sender, EventArgs e)
        {
            try
            {
                dbLogic.resetData();

                MessageBox.Show(
                    "Data berhasil direset");

                LoadData();
            }
            catch (SqlException ex)
            {
                SimpanLog(ex.Message);

                MessageBox.Show(
                    "SQL Error : " + ex.ToString());
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);

                MessageBox.Show(
                    "General Error : " + ex.ToString());
            }
        }

        private void btnTestInjection_Click(object sender, EventArgs e)
        {
            try
            {
                dbLogic.testInject(txtNIM.Text);

                LoadData();
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("safe"))
                {
                    SimpanLog(ex.Message);

                    MessageBox.Show(
                        "SQL Error : Unsafe UPDATE operation not allowed");
                }
                else
                {
                    SimpanLog(ex.Message);

                    MessageBox.Show(
                        "SQL Error : " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);

                MessageBox.Show(
                    "General Error : " + ex.Message);
            }
        }

        private void btnRekapData_Click(object sender, EventArgs e)
        {
            FormRekapMahasiswa frm = new FormRekapMahasiswa();
            frm.Show();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter =
                "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fotoMhs.Image =
                    Image.FromFile(ofd.FileName);

                fotoMhs.SizeMode =
                    PictureBoxSizeMode.StretchImage;
            }
        }

        private void btnImpExcel_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog =
                new OpenFileDialog { Filter = "Excel Workbook|*.xlsx" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    using (var stream =
                        File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader =
                            ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet(
                                new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = (_) =>
                                        new ExcelDataTableConfiguration()
                                        {
                                            UseHeaderRow = true
                                        }
                                });

                            DataTable dt = result.Tables[0];

                            dataGridView1.DataSource = dt;

                            dataGridView1.Enabled = false;

                            btnImpDb.Enabled = true;
                            btnInsert.Enabled = false;
                            btnUpdate.Enabled = false;
                            btnDelete.Enabled = false;
                            btnCari.Enabled = false;
                            btnLoad.Enabled = false;
                            btnResetData.Enabled = false;
                            btnTestInjection.Enabled = false;
                        }
                    }
                }
            }
        }

        private void btnImportDB_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)dataGridView1.DataSource;

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Tidak ada data untuk diimport.");
                    return;
                }

                int sukses = 0;

                foreach (DataRow row in dt.Rows)
                {
                    string nim = row["NIM"].ToString().Trim();
                    string nama = row["Nama"].ToString().Trim();
                    string jk = row["JenisKelamin"].ToString().Trim();
                    string alamat = row["Alamat"].ToString().Trim();
                    string kodeProdi = row["NamaProdi"].ToString().Trim();

                    string fotoPath =
                        row.Table.Columns.Contains("FotoPath")
                        ? row["FotoPath"].ToString().Trim()
                        : string.Empty;

                    if (string.IsNullOrEmpty(nim) ||
                        string.IsNullOrEmpty(nama))
                        continue;

                    DateTime tglLahir;

                    if (!DateTime.TryParse(
                        row["TanggalLahir"].ToString(),
                        out tglLahir))
                        continue;

                    byte[] ConvertImageFromPath(string path)
                    {
                        if (string.IsNullOrWhiteSpace(path))
                            return null;

                        if (!File.Exists(path))
                            return null;

                        return File.ReadAllBytes(path);
                    }

                    byte[] fotoBytes =
                        ConvertImageFromPath(fotoPath);

                    dbLogic.InsertMhs(
                        nim,
                        nama,
                        alamat,
                        jk,
                        tglLahir,
                        kodeProdi,
                        fotoBytes);

                    sukses++;
                }

                MessageBox.Show(
                    "Data mahasiswa berhasil ditambahkan");

                ClearForm();
                LoadData();
            }
            catch (SqlException ex)
            {
                SimpanLog(
                    "Rollback Insert : " + ex.Message);

                MessageBox.Show(
                    "SQL Error : " + ex.Message);
            }
            catch (Exception ex)
            {
                SimpanLog(
                    "General Error : " + ex.Message);

                MessageBox.Show(
                    "General Error : " + ex.Message);
            }
        }
    }
}