using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;
using A1;
using MathWorks.MATLAB.NET.Arrays;

namespace Csharp_omap_server
{
    class SqlHelper
    {
        
        public SqlHelper() { myClass = new MyClass(); myds1 = new DataSet(); myds2 = new DataSet(); p = new Process(); }
        public DataSet myds1,myds2;
        MyClass myClass;
        Process p;
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="PortName"></param>
        /// <param name="f1"></param>
        public  void QueryData(string PortName,Fmain f1)
        {
            using (SqlConnection con = new SqlConnection(@"server=127.0.0.1,1433;database=DataRecover;Uid=sa;Pwd=wdzs"))
            {

                string sqlstr1 = string.Format("select top 15 * from {0} order by [RID] desc ", PortName + "Raw");
                string sqlstr2 = string.Format("select top 256 * from {0} order by [RID] desc ", PortName + "Ret");
                SqlDataAdapter myda1 = new SqlDataAdapter(sqlstr1, con);
                SqlDataAdapter myda2 = new SqlDataAdapter(sqlstr2, con);
                 myds1 = new DataSet();
                 myds2 = new DataSet();
                try
                {
                    //3.打开连接
                    con.Open();
                    myda1.Fill(myds1, PortName+"Raw");
                    myda2.Fill(myds2, PortName+"Ret");
                    f1.dataGridView1.DataSource = myds1.Tables[0];
                    f1.dataGridView2.DataSource = myds2.Tables[0];
                    con.Close();
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }/// <summary>
        /// 查询
        /// </summary>
        /// <param name="PortName"></param>
        /// <param name="f1"></param>
        public void QueryData(string PortName)
        {
            using (SqlConnection con = new SqlConnection(@"server=127.0.0.1,1433;database=DataRecover;Uid=sa;Pwd=wdzs"))
            {

                string sqlstr1 = string.Format("select top 64 * from {0} order by [RID] desc ", PortName + "Raw");
                SqlDataAdapter myda1 = new SqlDataAdapter(sqlstr1, con);
                myds1 = new DataSet();
                try
                {
                    //3.打开连接
                    con.Open();
                    myda1.Fill(myds1, PortName + "Raw");
                    con.Close();
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="PortName"></param>
        /// <param name="f1"></param>
        public double[] QueryData( int j,string notifer)
        {
            using (SqlConnection con = new SqlConnection(@"server=127.0.0.1,1433;database=DataRecover;Uid=sa;Pwd=wdzs"))
            {

                string sqlstr1 = string.Format("select top 64 * from {0} order by [RID] desc ",notifer);
                SqlDataAdapter myda1 = new SqlDataAdapter(sqlstr1, con);
                myds1 = new DataSet();
                try
                {
                    //3.打开连接
                    con.Open();
                    myda1.Fill(myds1,notifer);
                    con.Close();
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            double[] output= new double[myds1.Tables[0].Rows.Count];
            for (int i = 0; i<myds1.Tables[0].Rows.Count;i++)
                output[i]= Convert.ToDouble(myds1.Tables[0].Rows[i][1]);
            return output;
        }
        /// <summary>
        /// 插入数据（分析结果）
        /// </summary>
        /// <param name="CSinput"></param>
        /// <param name="PortName"></param>
        public void InsertData(double[,] CSIO, string PortName,string notifer)
        {
            using (SqlConnection con = new SqlConnection(@"server=127.0.0.1,1433;database=DataRecover;Uid=sa;Pwd=wdzs"))
            {
                SqlCommand cmd = new SqlCommand();
                //设置命令对象执行sql语句时需要的连接对象
                cmd.Connection = con;
                //sql语句
                try
                {
                    //3.打开连接
                    con.Open();
                    for (int i = 0; i < CSIO.Length; i++)
                    {
                        cmd.CommandText = string.Format("insert {0} values(@record)", PortName + notifer);
                        cmd.Parameters.Add("@record", SqlDbType.Float).Value = CSIO[i,0];
                        int count = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();//插入完毕后一定清除参数 
                    }
                    con.Close();
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
        /// <summary>
        /// 插入数据(采集数据)
        /// </summary>
        /// <param name="CSoutput"></param>
        /// <param name="PortName"></param>
        public void InsertData(double[] CSIO, string PortName,string notifer)
        {                           
            using (SqlConnection con = new SqlConnection(@"server=127.0.0.1,1433;database=DataRecover;Uid=sa;Pwd=wdzs"))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                try
                {
                    con.Open();
                    for (int i = 0; i < CSIO.Length; i++)
                    {
                        cmd.CommandText = string.Format("insert {0} values(@record)", PortName + notifer);
                        cmd.Parameters.Add("@record", SqlDbType.Float).Value = CSIO[i];
                        int count = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();//插入完毕后一定清除参数 
                    }
                    con.Close();
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.ToString());
                }
        
            }
        }
        /// <summary>
        /// 调用matlab算法
        /// </summary>
        /// <param name="CSinput"></param>
        /// <param name="PortName"></param>
        public void Anaylase(double[] CSinput,string PortName)
        {

            //int retSize = 256; 256, i, retSize, mwin
            double[,] CSoutput = new double[2,256];
            MWArray i = 2;
            MWNumericArray mwin = new MWNumericArray(CSinput);

            try
            {
                CSoutput = (double[,])((MWNumericArray)myClass.xhcl(mwin)).ToArray(MWArrayComponent.Real);  

                InsertData(CSoutput, PortName, "ret");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR");
            }

        }





















        #region 备用方案

        ///// <summary>
        ///// 备用方案 调用python程序
        ///// </summary>
        ///// <param name="path"></param>
        ///// <param name="args"></param>
        //void RunPythonScript(string path, string args = "")
        //{
            
        //    path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "script\\" + path;

            
        //    p.StartInfo.FileName = "python.exe";
        //    string sArguments = "\"" + path + "\"";
        //    if (args.Length > 0)
        //    {
        //        sArguments += " " + args;
        //    }
        //    //p.StartInfo.WorkingDirectory = "D:\\";
        //    p.StartInfo.Arguments = sArguments;
        //    p.StartInfo.UseShellExecute = false;
        //    p.StartInfo.RedirectStandardOutput = true;
        //    p.StartInfo.RedirectStandardInput = true;
        //    p.StartInfo.RedirectStandardError = true;
        //    p.StartInfo.CreateNoWindow = true;
        //    p.Start();
        //    //p.CloseMainWindow();
        //    p.WaitForExit();
        //}
        #endregion
    }
}
