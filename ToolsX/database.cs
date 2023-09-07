

using System;
using EnvDTE80;
using Npgsql;
using EnvDTE;
using System.Data;
using System.Windows.Forms;
using System.Xml;
namespace ToolsX
{
    public class database
    {
        public string ConStr = "port=5432;database=postgres;host=127.0.0.1;password=admin_123;userid=postgres;";
       // public string ConStr = "port=5432;database=imos;host=206.206.128.228;password=admin_123;userid=postgres;";

        /* 执行查询并返回结果 */
        public DataSet ExecuteQuery(string sqrstr)
        {
            NpgsqlConnection sqlConn = new NpgsqlConnection(ConStr);
            DataSet ds = new DataSet();
            try
            {
                using (NpgsqlDataAdapter sqldap = new NpgsqlDataAdapter(sqrstr, sqlConn))
                {
                    sqldap.Fill(ds);
                }
                sqlConn.Close();
                return ds;
            }
            catch (System.Exception ex)
            {
                sqlConn.Close();
                return ds;
            }
        }

        /* 只执行，不返回结果 */
        public int ExecuteNonQuery(string sqrstr)
        {
            try
            {
                NpgsqlConnection sqlConn = new NpgsqlConnection(ConStr);
                sqlConn.Open();
                using (NpgsqlCommand SqlCommand = new NpgsqlCommand(sqrstr, sqlConn))
                {
                    int r = SqlCommand.ExecuteNonQuery();
                    sqlConn.Close();
                    return r;
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
                return 0;
            }
        }
    }
}























