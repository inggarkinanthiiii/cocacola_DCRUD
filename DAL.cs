using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public class DAL
    {
        public static string GetConnectionString()
        {
            string connectionString =
                @"Data Source=.\SQLEXPRESS;Initial Catalog=DBAkademikADO;User ID=sa;Password=123456789;";

            return connectionString;
        }
        public static string GetLocalIPAddress()
        {
            string localIP = string.Empty;

            try
            {
                var host =
                    System.Net.Dns.GetHostEntry(
                        System.Net.Dns.GetHostName());

                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily ==
                        System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error getting local IP address : "
                    + ex.Message);
            }

            return localIP;
        }

        SqlConnection conn;

        SqlDataAdapter da;
        DataTable dtMahasiswa;
        DataTable dtProdi;

        public int CountMhs()
        {
            conn = new SqlConnection(GetConnectionString());
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("sp_CountMahasiswa", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter outputParam =
                new SqlParameter("@Total", SqlDbType.Int);

            outputParam.Direction =
                ParameterDirection.Output;

            cmd.Parameters.Add(outputParam);

            cmd.ExecuteNonQuery();

            int result = Convert.ToInt32(outputParam.Value);
            conn.Close();
            return result;
        }

        public DataTable GetMhs()
        {
            conn = new SqlConnection(GetConnectionString());
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd =
                new SqlCommand("sp_GetMahasiswa", conn);

            cmd.CommandType =
                CommandType.StoredProcedure;

            da = new SqlDataAdapter(cmd);

            dtMahasiswa = new DataTable();

            da.Fill(dtMahasiswa);

            conn.Close();
            return dtMahasiswa;
        }
        public void InsertMhs(string nim, string nama,
        string alamat, string jenisKelamin,
        DateTime tanggalLahir,
        string kodeProdi,
        byte[] foto)
        {
            conn = new SqlConnection(GetConnectionString());
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlTransaction trans = conn.BeginTransaction();

            try
            {
                SqlCommand command =
                    new SqlCommand("sp_InsertMahasiswa",
                    conn, trans);

                command.CommandType =
                    CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@NIM", nim);
                command.Parameters.AddWithValue("@Nama", nama);
                command.Parameters.AddWithValue("@Alamat", alamat);
                command.Parameters.AddWithValue("@JenisKelamin", jenisKelamin);
                command.Parameters.AddWithValue("@TanggalLahir", tanggalLahir);
                command.Parameters.AddWithValue("@KodeProdi", kodeProdi);
                command.Parameters.AddWithValue("@TanggalDaftar", DateTime.Now);
                command.Parameters.AddWithValue("@Foto", foto);

                command.ExecuteNonQuery();

                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        public void UpdateMhs(string nim,
    string nama,
    string alamat,
    string jenisKelamin,
    DateTime tanggalLahir,
    string kodeProdi,
    byte[] foto)
        {
            conn = new SqlConnection(GetConnectionString());
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlTransaction trans = conn.BeginTransaction();

            try
            {
                SqlCommand command =
                    new SqlCommand("sp_UpdateMahasiswa", conn, trans);

                command.CommandType =
                    CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@NIM", nim);
                command.Parameters.AddWithValue("@Nama", nama);
                command.Parameters.AddWithValue("@Alamat", alamat);
                command.Parameters.AddWithValue("@JenisKelamin", jenisKelamin);
                command.Parameters.AddWithValue("@TanggalLahir", tanggalLahir);
                command.Parameters.AddWithValue("@KodeProdi", kodeProdi);
                command.Parameters.AddWithValue("@Foto", foto);

                command.ExecuteNonQuery();

                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        public void DeleteMhs(string nim)
        {
            conn = new SqlConnection(GetConnectionString());
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlTransaction trans = conn.BeginTransaction();

            try
            {
                SqlCommand cmd =
                    new SqlCommand("sp_DeleteMahasiswa", conn, trans);

                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NIM", nim);

                cmd.ExecuteNonQuery();

                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        public void resetData()
        {
            try
            {
                conn = new SqlConnection(GetConnectionString());
                conn.Open();

                SqlCommand cmd =
                    new SqlCommand("DELETE FROM Mahasiswa", conn);

                cmd.ExecuteNonQuery();

                MessageBox.Show("RESET BERHASIL");
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR DETAIL: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public void testInject(string nim)
        {
            conn = new SqlConnection(GetConnectionString());
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            try
            {
                // Vulnerable to SQL injection for testing purposes
                string query = "UPDATE Mahasiswa SET Nama = 'HACKED' WHERE NIM = '" + nim + "'";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public DataTable GetMhsByNIM(string nim)
        {
            conn = new SqlConnection(GetConnectionString());
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd =
                new SqlCommand("sp_GetMahasiswaByNIM", conn);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@NIM", nim);

            da = new SqlDataAdapter(cmd);

            dtMahasiswa = new DataTable();

            da.Fill(dtMahasiswa);

            conn.Close();
            return dtMahasiswa;
        }
        public DataTable getDataRekap(string prodi, DateTime tanggalMasuk)
        {
            conn = new SqlConnection(GetConnectionString());
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("sp_Report", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@inProdi", prodi);

            cmd.Parameters.AddWithValue("@inTglMsuk",
                tanggalMasuk.Year);

            da = new SqlDataAdapter(cmd);

            dtMahasiswa = new DataTable();

            da.Fill(dtMahasiswa);

            conn.Close();
            return dtMahasiswa;
        }
        public DataTable getAllDataChart()
        {
            conn = new SqlConnection(GetConnectionString());
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd =
                new SqlCommand("sp_DashBoard", conn);

            cmd.CommandType =
                CommandType.StoredProcedure;

            da = new SqlDataAdapter(cmd);

            dtMahasiswa = new DataTable();

            da.Fill(dtMahasiswa);

            conn.Close();
            return dtMahasiswa;
        }
        public DataTable getDataChartByTahun(DateTime thMasuk)
        {
            conn = new SqlConnection(GetConnectionString());
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd =
                new SqlCommand("sp_DashBoardByTahun", conn);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@inTglMsuk",
                thMasuk.Year);

            da = new SqlDataAdapter(cmd);

            dtMahasiswa = new DataTable();

            da.Fill(dtMahasiswa);

            conn.Close();
            return dtMahasiswa;
        }
    }

}