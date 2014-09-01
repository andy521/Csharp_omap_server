using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace Csharp_omap_server
{
    class Configure
    {
        public Configure() { }
        /// <summary>
        /// 保存配置信息
        /// </summary>
        /// <param name="f1"></param>
        public bool SaveFile(Fmain f1) 
        {
            if (f1.ListPort.Count>0)
            {
                #region 事务.保存配置

                using (StreamWriter sw = new StreamWriter(Application.StartupPath + @"\SerConfig", false, Encoding.UTF8))
                {
                    foreach(Serial i in f1.ListPort)
                        if (i.state)
                        {                            
                            sw.WriteLine(i.MyPort.PortName.ToString());
                            sw.WriteLine(i.MyPort.BaudRate.ToString());
                            sw.WriteLine(i.MyPort.DataBits.ToString());
                            sw.WriteLine(i.MyPort.StopBits.ToString());
                            sw.WriteLine(i.MyPort.Parity.ToString());
                            sw.Flush();
                        }
                }

                #endregion                
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 加载配置信息
        /// </summary>
        /// <param name="f1"></param>
        public void LoadFile(Fmain f1) 
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "configfile|*.*";
            openFile.ShowDialog();

            if (openFile.FileName.Length > 0)
            {                
                using (StreamReader sr = new StreamReader(openFile.FileName, Encoding.UTF8))
                {
                    Serial m1 = new Serial();
                    for(;!sr.EndOfStream;)
                    { 
                        m1.MyPort.PortName = sr.ReadLine();
                        if (!f1.PortBox.Items.Contains(m1.MyPort.PortName))
                        {
                            #region 事务.加载配置信息
                            m1.MyPort.BaudRate = Convert.ToInt32(sr.ReadLine().ToString());
                            m1.MyPort.DataBits = Convert.ToInt32(sr.ReadLine().ToString());
                            switch (sr.ReadLine().ToString())
                            {
                                case "0":
                                    m1.MyPort.StopBits = StopBits.One;
                                    break;
                                case "1.5":
                                    m1.MyPort.StopBits = StopBits.Two;
                                    break;
                                case "2":
                                    m1.MyPort.StopBits = StopBits.OnePointFive;
                                    break;
                            }
                            switch (sr.ReadLine().ToString())
                            {
                                case "None":
                                    m1.MyPort.Parity = Parity.None;
                                    break;
                                case "Odd":
                                    m1.MyPort.Parity = Parity.Odd;
                                    break;
                                case "Even":
                                    m1.MyPort.Parity = Parity.Even;
                                    break;
                                case "Mark":
                                    m1.MyPort.Parity = Parity.Mark;
                                    break;
                                case "Space":
                                    m1.MyPort.Parity = Parity.Space;
                                    break;
                            }
                            f1.ListPort.Add(m1);
                            #endregion
                            #region 视图.添加监视串口

                            f1.PortBox.Items.Add(m1.MyPort.PortName);

                            #endregion
                        }
                        else
                        {
                            sr.ReadLine();
                            sr.ReadLine();
                            sr.ReadLine();
                            sr.ReadLine();
                            sr.DiscardBufferedData();
                        }
                    }
                }
            }
        }
    }
}
