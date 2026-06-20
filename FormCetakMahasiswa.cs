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
    public partial class FormCetakMahasiswa : Form
    {
        private SqlConnection conn;

        private string connectionString =
            "Data Source=.\\SQLEXPRESS;Initial Catalog=DBAkademikADO;User ID=sa;Password=123456789";

        SqlDataAdapter da;
        DataTable dtMahasiswa;

        string prodi;
        DateTime tglMasuk;
        public FormCetakMahasiswa(
            string Prodi,
            DateTime TglMasuk)
        {
            InitializeComponent();

            prodi = Prodi;
            tglMasuk = TglMasuk;

            conn = new SqlConnection(connectionString);

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd =
                    new SqlCommand("sp_Report", conn);

                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@inProdi",
                    prodi);

                cmd.Parameters.AddWithValue(
                    "@inTglMsuk",
                    tglMasuk.Year.ToString());

                da = new SqlDataAdapter(cmd);

                dtMahasiswa = new DataTable();

                da.Fill(dtMahasiswa);
                MessageBox.Show(
                    "Is64BitProcess = " +
                    Environment.Is64BitProcess.ToString());

                RptMahasiswa rpt =
                    new RptMahasiswa();

                rpt.SetDataSource(dtMahasiswa);

                crystalReportViewer2.ReportSource =
                    rpt;

                crystalReportViewer2.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal load data : " +
                    ex.Message);
            }
        }
    }
}
