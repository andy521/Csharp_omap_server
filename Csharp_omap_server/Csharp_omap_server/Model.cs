using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using A1;

using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
namespace Csharp_omap_server
{
    /// <summary>
    /// 事务模型类
    /// </summary>
    class Model
    {
        public Model(Fmain f1) 
        {
            MySqlHelper = new SqlHelper();
            MyConf = new Conf();
            ListOfPorts = new List<Serial>();
            ListOfThread = new List<Thread>();
            ListOfPorts.Clear();
            ListOfThread.Clear();
            f = f1;
            MyClass = new CLass();
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        public void SaveConf() 
        {
            if (MyConf.SaveSerialConf(ref ListOfPorts))
            {
                MessageBox.Show("Save config Successfully", "Inform");
            }
            else
            {
                MessageBox.Show("Make sure any port opened", "Error");
            }
        }
        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="f1"></param>
        public void LoadConf(Fmain f1) 
        {
            if (MyConf.LoadSeriaoConf(f1,ref ListOfPorts))
            {
                MessageBox.Show("Load config Successfully", "Inform");
            }
            else
            {
                MessageBox.Show("Make sure any config exist", "Error");
            }
        }
        /// <summary>
        /// 打开并侦听
        /// </summary>
        public bool OpenPort(String PortID,Fmain f1) 
        {
            bool find = false;
            
            if (f1.ForAllCheck.Checked == false)
            {
                foreach (Serial i in ListOfPorts)
                {
                    if (i.PortID == PortID)
                    {
                        i.OpenPort();
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    Serial s1 = new Serial(f1);
                    if (f1.PortX.SelectedItem == null || f1.BaudX.SelectedItem == null ||
                        f1.DataX.SelectedItem == null || f1.StopX.SelectedItem == null || f1.CheckX.SelectedItem == null)
                    {
                        MessageBox.Show("Config first", "Inform");
                        return false;
                    }
                    else
                    {
                        s1.OpenPort(f1);
                        ListOfPorts.Add(s1);
                        return true;
                    }                   
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (ListOfPorts.Count > 0)
                {
                    foreach (Serial i in ListOfPorts)
                    {
                        i.OpenPort();
                    }
                    return true;
                }
                else
                {
                    MessageBox.Show("No port to open", "Inform");
                    return false;
                }
            }
        }
        /// <summary>
        /// 关闭名为PortID的串口
        /// </summary>
        /// <param name="PortID"></param>
        public bool ClosePort(String PortID,Fmain f1)
        {
            //ListOfPorts.Find(x => x.PortID == PortID).ClosePort();
            bool find = false;
            if (f1.ForAllCheck.Checked == false)
            {
                foreach (Serial i in ListOfPorts)
                {
                    if (i.PortID == PortID)
                    {
                        i.ClosePort();
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                        MessageBox.Show("No such port found", "Inform");
                        return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (ListOfPorts.Count > 0)
                {
                    foreach (Serial i in ListOfPorts)
                    {
                        i.ClosePort();
                    }
                    return true;
                }
                else
                {
                    MessageBox.Show("No port to close", "Inform");
                    return false;
                }
            }
        }
        /// <summary>
        /// 关闭所有串口
        /// </summary>
        public void Exit()
        {
            foreach (Serial i in ListOfPorts)
            {
                i.ClosePort();
            }
        }

        /// <summary>
        /// 数据成员
        /// </summary>
        public List<Serial> ListOfPorts;
        List<Thread> ListOfThread;
        Conf MyConf;
        SqlHelper MySqlHelper;
        Fmain f;
        CLass MyClass;

        /// <summary>
        /// 删除串口
        /// </summary>
        /// <param name="f1"></param>
        public void DelPort(Fmain f1)
        {
            foreach (string j in f1.PortBox.SelectedItems)
                for (int i = ListOfPorts.Count - 1; i > -1;i-- )
                    if (j.Equals(ListOfPorts.ElementAt(i).PortID))
                    {
                        ListOfPorts.ElementAt(i).ClosePort();
                        ListOfPorts.RemoveAt(i);
                    }
        }
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="MyModel"></param>
        public void RecvData(Fmain f1,Model MyModel)
        {
            bool find = false;
            if (f1.ForAllCheck.Checked == false)
            {
                foreach (Serial i in ListOfPorts)
                    if (i.PortID == f1.PortID.Text)
                    {
                        i.OpenPort();
                        find = true;
                        break;
                    }
                if (!find)
                {
                    Serial s1 = new Serial(f1);
                    if (f1.PortX.SelectedItem == null || f1.BaudX.SelectedItem == null ||
                        f1.DataX.SelectedItem == null || f1.StopX.SelectedItem == null || f1.CheckX.SelectedItem == null)
                    {
                        MessageBox.Show("Config first", "Inform");
                    }
                    else
                    {
                        s1.OpenPort(f1);
                        ListOfPorts.Add(s1);
                    }
                }          
            }
            else
            {
                if (ListOfPorts.Count > 0)
                foreach (Serial i in ListOfPorts)
                {
                    i.OpenPort();

                    }
                 
            }

        }



        public void QueryData(string PortID)
        {
            foreach (Serial i in ListOfPorts)
            {
                if ( i.PortID.Equals(PortID))
                    MySqlHelper.QueryData(i.MyPort.PortName,f);
            }
        }

        public void Anaylase()
        {
            Random ranobj = new Random();
            int row = 26, col = 256,k=0;
            double[,] CSoutput = new double[1, col];
            double[,] CSinput = new double[row, col];

            for (int i = 0; i < row; i++)
                for (int j = 0; j < col; j++)
                {
                    //CSinput[i, j] = ranobj.Next(0, 255);
                    CSinput[i, j] =  Convert.ToDouble(MySqlHelper.myds.Tables[0].Rows[k][1]);
                    k++;
                }
            MWNumericArray mwin = CSinput;

            try
            {
                CSoutput = (double[,])MyClass.xhcl(mwin).ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR");
            }
           
        }
    }
}
