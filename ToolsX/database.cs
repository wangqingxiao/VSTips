
using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace ToolsX
{
    public class database
    {
        public string ConStr = "port=5432;database=postgres;host=127.0.0.1;password=admin_123;userid=postgres;";
        // public string ConStr = "port=5432;database=imos;host=206.206.128.228;password=admin_123;userid=postgres;";

        /* 执行查询并返回结果 */
        public DataSet ExecuteQuery(string sqrstr)
        {
            /*
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
            */

            DataSet ds = new DataSet();
            using (var connection = new SQLiteConnection("URI=file:D:\\toolsx.db"))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sqrstr, connection))
                {
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    adapter.Fill(ds);
                }
            }
            return ds;
        }

        /* 只执行，不返回结果 */
        public int ExecuteNonQuery(string sqrstr)
        {
            try
            {

                /*
                NpgsqlConnection sqlConn = new NpgsqlConnection(ConStr);
                sqlConn.Open();
                using (NpgsqlCommand SqlCommand = new NpgsqlCommand(sqrstr, sqlConn))
                {
                    int r = SqlCommand.ExecuteNonQuery();
                    sqlConn.Close();
                    return r;
                }

                */
                using (var connection = new SQLiteConnection("URI=file:D:\\toolsx.db"))
                {
                    connection.Open(); 
                    using (var command = new SQLiteCommand(sqrstr, connection))
                    {
                        int r = 0;
                        command.ExecuteNonQuery(); // 执行无返回值的 SQL
                        return r;
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
                return 0;
            }
        }

        public void initdatabase()
        {
            try
            {
                //URI=file:D:\\toolsx.db
                using (var connection = new SQLiteConnection("Data Source=toolsx.db"))
                {
                    connection.Open();           // 打开连接

                    // 查找表，不存在则新建表
                    string sql = @"CREATE TABLE IF NOT EXISTS tbl_toolx_tips( 
                    SHEETID TEXT NOT NULL,
                    SOLUTION TEXT NOT NULL,
                    FILEPATH TEXT NOT NULL,
                    DESCRIPTION TEXT NOT NULL,
                    USER TEXT NOT NULL,
                    STATUS TEXT NOT NULL,
                    KEYSTRING TEXT NOT NULL)";

                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.ExecuteNonQuery(); // 执行无返回值的 SQL
                    }

                    string sqrstr = "select * from tbl_toolx_tips";
                    using (var command = new SQLiteCommand(sqrstr, connection))
                    {
                        int r = 0;
                        command.ExecuteNonQuery(); // 执行无返回值的 SQL
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: {ex.Message}");
            }
        }

    }
}























