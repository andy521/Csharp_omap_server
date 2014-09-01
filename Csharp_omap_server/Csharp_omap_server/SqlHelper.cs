using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;

namespace Csharp_omap_server
{
    class SqlHelper
    {
        public SqlHelper() { }
        public DataSet myds;
        public  void QueryData(string PortName,Fmain f1)
        {
            using (SqlConnection con = new SqlConnection(@"Data Source=5E9SKILDFCYGLBJ\SQL_L;Initial Catalog=Data;Integrated Security=True"))
            {
                string sqlstr = string.Format("select top 6656  * from {0} order by [RID] desc ", PortName);
                //string sqlstr = string.Format("select top 6656  * from Ran1 order by [RID] desc ");
                SqlDataAdapter myda = new SqlDataAdapter(sqlstr, con);
                 myds = new DataSet();
                
                try
                {
                    //3.打开连接
                    con.Open();
                    myda.Fill(myds, PortName);
                    f1.dataGridView1.DataSource = myds.Tables[0];
                    con.Close();
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
    }
}
