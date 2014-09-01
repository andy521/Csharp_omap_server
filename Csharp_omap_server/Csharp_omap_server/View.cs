using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace Csharp_omap_server
{
    /// <summary>
    /// 视图控制类
    /// </summary>
    class View
    {
        public View(Fmain f1) 
        {
            f = f1;
        }
        /// <summary>
        /// 界面初始化
        /// </summary>
        public void Start() 
        {
            f.PortX.SelectedIndex = 0;
            f.BaudX.SelectedIndex = 5;
            f.DataX.SelectedIndex = 3;
            f.StopX.SelectedIndex = 0;
            f.CheckX.SelectedIndex = 0;
            f.PortID.Focus();
            f.Refresh();
        }
        /// <summary>
        /// 退出程序
        /// </summary>
        public void Exit() { }
        /// <summary>
        /// 刷新串口列表
        /// </summary>
        /// <param name="m1"></param>
        public void FlushListBox(ref Model m1 ) 
        {
            f.PortBox.Items.Clear();
            foreach (Serial i in m1.ListOfPorts)
            {
                f.PortBox.Items.Add(i.PortID);
                f.toolStripStatusLabel2.Text = i.PortID + " " + i.MyPort.PortName + " " + (i.MyPort.IsOpen ? "On" : "Off");
            }                
        }
        /// <summary>
        /// 刷新串口选项集合
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="PortID"></param>
        public void FlushCombolBoX(Model m1,string PortID) 
        {
            foreach(Serial i in  m1.ListOfPorts)
                if (i.PortID == PortID)
                {
                    #region 标准刷新过程
                    f.PortID.Text=PortID;
                    f.PortX.SelectedItem = (object) i.MyPort.PortName.ToString();
                    f.BaudX.SelectedItem = i.MyPort.BaudRate;
                    f.DataX.SelectedItem = i.MyPort.DataBits;
                    f.StopX.SelectedItem = 
                        i.MyPort.StopBits.ToString()==StopBits.One.ToString()?1:i.MyPort.StopBits.ToString()==StopBits.Two.ToString()?2:1;
                    f.CheckX.SelectedItem = i.MyPort.Parity;
                    f.toolStripStatusLabel2.Text = i.PortID + " " + i.MyPort.PortName + " "+(i.MyPort.IsOpen?"On":"Off");
                    #endregion
                }
        }
        /// <summary>
        /// 切换选项卡页面
        /// </summary>
        /// <param name="f"></param>
        /// <param name="m1"></param>
        public void SwitchTab(Fmain f,Model m1,string t1)
        {
            
            if (t1.Equals("tabPage3"))
            {
                f.DBSwitch .Items.Clear();
                foreach (Serial i in m1.ListOfPorts)
                    f.DBSwitch.Items.Add(i.PortID);
            }
            else
            {
                f.Channel.Items.Clear();
                foreach (Serial i in m1.ListOfPorts)
                    f.Channel.Items.Add(i.PortID + " " + i.MyPort.PortName);
            }
        }
        Fmain f;
    }
}
