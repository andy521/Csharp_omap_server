using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Windows.Forms;


namespace Csharp_omap_server
{
    class Conf
    {
        public Conf() { }
        /// <summary>
        /// 保存可用串口配置信息
        /// </summary>
        /// <param name="ListOfPorts">串口集合</param>
        /// <returns></returns>
        public bool SaveSerialConf(ref List<Serial> ListOfPorts)
        {
            if (ListOfPorts.Count > 0)
            {

                using (StreamWriter sw = new StreamWriter(Application.StartupPath + @"\SerConfig", false, Encoding.UTF8))
                {
                    foreach (Serial i in ListOfPorts)
                        if (i.MyPort.IsOpen)
                        {
                            MessageBox.Show(i.PortID.ToString());
                            sw.WriteLine(i.PortID.ToString());
                            sw.WriteLine(i.MyPort.PortName.ToString());
                            sw.WriteLine(i.MyPort.BaudRate.ToString());
                            sw.WriteLine(i.MyPort.DataBits.ToString());
                            sw.WriteLine(i.MyPort.StopBits.ToString());
                            sw.WriteLine(i.MyPort.Parity.ToString());
                            sw.Flush();
                        }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 加载可用串口配置信息
        /// </summary>
        /// <param name="f1">窗体引用</param>
        /// <param name="ListOfPorts">串口集合</param>
        /// <returns></returns>
        public bool LoadSeriaoConf(Fmain f1,ref List<Serial> ListOfPorts)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "configfile|*.*";
            openFile.ShowDialog();

            if (openFile.FileName.Length > 0)
            {
                using (StreamReader sr = new StreamReader(openFile.FileName, Encoding.UTF8))
                {
                    bool find=false;
                    //Serial m1 = new Serial();
                    string[] s=new string[6];
                    #region 读取记录
                    for (; !sr.EndOfStream;find = false)
                    {
                        for(int i=0;i<6 ;i++)
                        {
                            s[i]=sr.ReadLine().ToString();
                        }
                        foreach (Serial i in ListOfPorts)
                            if ( i.PortID.Equals(s[0])) 
                            {                           
                                find=true;
                                break;
                            }
                        if(!find)
                        {                            
                            ListOfPorts.Add(new Serial(s));
                            
                        }
                        else
                        {
                            sr.ReadLine();
                            sr.ReadLine();
                            sr.ReadLine();
                            sr.ReadLine();
                            sr.ReadLine();

                        }
                        
                    }
                    #endregion
                }
            }   
            if (!(ListOfPorts.Count > 0))
                return false;
            else
                return true;
        }
    }
}
