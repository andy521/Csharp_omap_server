using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using A1;
using System.Data.SqlClient;
using System.Data;

namespace Csharp_omap_server
{
    /// <summary>
    /// 封装的串口类
    /// </summary>
    public class Serial 
    {
        /// <summary>
        /// 串口初始化
        /// </summary>
        public Serial(Fmain f) 
        {
            MyPort = new SerialPort();
            PortID = "";
            MyPort.DataReceived += delegate(object sender ,SerialDataReceivedEventArgs e) { this.RecvData(f); };
        }
        /// <summary>
        /// （重载）串口初始化方法
        /// </summary>
        /// <param name="s">控件参数</param>
        public Serial(string[] s)
        {
            MyPort = new SerialPort();
            PortID = s[0];
            MyPort.PortName = s[1];
            MyPort.BaudRate = Convert.ToInt32(s[2]);
            MyPort.DataBits = Convert.ToInt32(s[3]);
            switch (s[4])
            {
                case "One":
                    MyPort.StopBits = StopBits.One;
                    break;
                case "Two":
                    MyPort.StopBits = StopBits.Two;
                    break;
                case "OnePointFive":
                    MyPort.StopBits = StopBits.OnePointFive;
                    break;
            }
            switch (s[5])
            {
                case "None":
                    MyPort.Parity = Parity.None;
                    break;
                case "Odd":
                    MyPort.Parity = Parity.Odd;
                    break;
                case "Even":
                    MyPort.Parity = Parity.Even;
                    break;
                case "Mark":
                    MyPort.Parity = Parity.Mark;
                    break;
                case "Space":
                    MyPort.Parity = Parity.Space;
                    break;
            }

            
        }
        /// <summary>
        /// 打开串口方法(直接)
        /// </summary>
        /// <param name="f1">获得主窗体引用</param>
        public void OpenPort()
        {
            try
            {
                if (!MyPort.IsOpen)
                    MyPort.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString() + "\n" + "Open faild" + "\n" + "1.Check config" + "\n"
                    + "2.Check device available" + "\n" + "3.Check device driver", "Error");
            }
        }
        /// <summary>
        /// （重载）打开控件所示串口
        /// </summary>
        /// <param name="f1"></param>
        public void OpenPort(Fmain f1)
        {
            try
            {
                if(!MyPort.IsOpen)
                {
                    #region 事务.串口打开
                    PortID = f1.PortID.Text.ToString();
                    MyPort.PortName = f1.PortX.SelectedItem.ToString();
                    MyPort.BaudRate = Convert.ToInt32(f1.BaudX.SelectedItem.ToString());
                    switch (f1.CheckX.SelectedIndex)
                    {
                        case 0:
                            MyPort.Parity = Parity.None;
                            break;
                        case 1:
                            MyPort.Parity = Parity.Odd;
                            break;
                        case 2:
                            MyPort.Parity = Parity.Even;
                            break;
                        case 3:
                            MyPort.Parity = Parity.Mark;
                            break;
                        case 4:
                            MyPort.Parity = Parity.Space;
                            break;
                    }

                    MyPort.DataBits = Convert.ToInt32(f1.DataX.SelectedItem.ToString());

                    switch (f1.StopX.SelectedIndex)
                    {
                        case 0:
                            MyPort.StopBits = StopBits.One;
                            break;
                        case 1:
                            MyPort.StopBits = StopBits.Two;
                            break;
                        case 2:
                            MyPort.StopBits = StopBits.OnePointFive;
                            break;
                    }


                    #endregion
                    MyPort.Open();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString()+"Open faild" + "\n" + "1.Check config" + "\n"
                    + "2.Check device available" + "\n" + "3.Check device driver", "Error");
            }


        }
        /// <summary>
        /// 关闭串口方法
        /// </summary>
        /// <param name="f1"></param>
        public void ClosePort()
        {
            //MyPort.IsOpen?MyPort.():;
            if (MyPort.IsOpen)
                MyPort.Close();
            else
                return;
        }
        /// <summary>
        /// 接收数据(线程方法)
        /// </summary>
        public void RecvData(Fmain f1)
        {
            if (!MyPort.IsOpen)
                MyPort.Open();
            try
            {             
                string text = string.Empty;
                byte[] result = new byte[MyPort.BytesToRead];
                MyPort.Read(result, 0, MyPort.BytesToRead);                
                f1.DataBox.AppendText(System.Text.Encoding.ASCII.GetString(result)+"\n");
                double a = Convert.ToDouble(System.Text.Encoding.ASCII.GetString(result));
                InsertData(a);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
        }

        public void InsertData(double CSinput)
        {

            #region 插入数值
            using (SqlConnection con = new SqlConnection(@"Data Source=5E9SKILDFCYGLBJ\SQL_L;Initial Catalog=Data;Integrated Security=True"))
            {
                SqlCommand cmd = new SqlCommand();
                //设置命令对象执行sql语句时需要的连接对象
                cmd.Connection = con;
                //sql语句
                cmd.CommandText = string.Format("insert {0} values(@record)",MyPort.PortName);
                //为sql语句参数赋值
                cmd.Parameters.Add("@record", SqlDbType.Float).Value =CSinput;
                try
                {
                    //3.打开连接
                    con.Open();
                    //4.
                    //执行sql语句
                    int count = cmd.ExecuteNonQuery();
                    ////5.关闭连接
                    con.Close();
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            #endregion
        }
        
        /// <summary>
        /// 数据成员
        /// </summary>
        public SerialPort MyPort ;
        public string PortID;
        public Thread MyThread;

    }
}
