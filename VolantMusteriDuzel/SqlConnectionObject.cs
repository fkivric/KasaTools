using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolantMusteriDuzel
{
    class SqlConnectionObject
    {   
        public string QueryEntegref(string q, SqlConnection conn)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(q, conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                if (dataTable != null)
                {
                    return dataTable.Rows[0][0].ToString();
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }
        public string BulkInsertRetorn(DataTable dt, string KaydedilecekTAbloAdı, SqlConnection sqlConnection)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection))
            {                
                bulkCopy.DestinationTableName = KaydedilecekTAbloAdı;
                try
                {
                    bulkCopy.WriteToServer(dt);
                    return ("Aktarım Tamamlandı");
                }
                catch (Exception ex)
                {
                    return (ex.Message);
                }
            }
        }
        public String GetValue(string query)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connectionstring))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }
            if (dt.Rows != null)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }
        public DataTable GetData(string spName, SqlConnection sql)
        {
            DataTable returnType = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(spName, sql);
            da.Fill(returnType);
            if (returnType.Rows.Count > 0)
            {
                return returnType;
            }
            else
            {
                return null;
            }
        }
        public DataTable Query(string spName, Dictionary<string, string> param)
        {
            DataTable returnType = new DataTable();
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connectionstring))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandTimeout = 0;
                    if (param != null)
                    {
                        foreach (var item in param)
                        {
                            cmd.Parameters.AddWithValue(item.Key, item.Value);
                        }
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adap = new SqlDataAdapter(cmd);
                    adap.Fill(returnType);
                    conn.Close();
                }
            }

            return returnType;
        }
        public DataTable QuerySpOnly(string spName)
        {
            DataTable returnType = new DataTable();
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connectionstring))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandTimeout = 0;
                    SqlDataAdapter adap = new SqlDataAdapter(cmd);
                    adap.Fill(returnType);
                }
                conn.Close();
            }

            return returnType;

        }



        public void Insert(string spName, Dictionary<string, string> param)
        {
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connectionstring))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandTimeout = 0;
                    if (param != null)
                    {
                        foreach (var item in param)
                        {
                            cmd.Parameters.AddWithValue(item.Key, item.Value);
                        }
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

        }

        public string InsertBack(string spName, Dictionary<string, string> param)
        {
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connectionstring))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandTimeout = 0;
                    if (param != null)
                    {
                        foreach (var item in param)
                        {
                            if (item.Key == "@ReturnDesc")
                            {
                                cmd.Parameters.Add("@ReturnDesc", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue(item.Key, item.Value);
                            }
                        }
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return (string)cmd.Parameters["@ReturnDesc"].Value;
                }
            }            
        }
        public int InsertValue(string query,SqlConnection sql)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, sql);
                if (sql.State == ConnectionState.Closed)
                {
                    sql.Open();
                }
                var id = cmd.ExecuteNonQuery();
                return id;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
