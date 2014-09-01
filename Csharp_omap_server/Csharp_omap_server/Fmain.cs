﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
namespace Csharp_omap_server
{
    public partial class Fmain : Form
    {
        Control MyControl;

        /// <summary>
        /// 初始化窗体
        /// </summary>
        public Fmain() 
        {
            InitializeComponent();
            MyControl = new Control(this);
            CheckForIllegalCrossThreadCalls = false;
        }
        /// <summary>
        /// 关闭所有成功连接的端口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyControl.Exit();
        }
        /// <summary>
        /// 将内存中已打开的所有端口配置保存下来
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyControl.SaveConf();
        }
        /// <summary>
        /// 从文件加载端口配置信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyControl.LoadConf();
        }
        /// <summary>
        /// 关于开发者
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void meToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("IODP" + "\n" + "zhangli" + "\n" + "Tel:18333648737" + "\n" + "Mail:zhangli2946@gmail.com", "About me");
        }
        /// <summary>
        /// 关于产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void productToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("IOT508" + "\n" + "Ver 1.001" + "\n" + "Licence under LGPL" + "\n", "About me");
        }
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void open_Click(object sender, EventArgs e)
        {
            MyControl.OpenPort();
        }
        /// <summary>
        /// 激活串口数据侦听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void recieve_Click(object sender, EventArgs e)
        {
            MyControl.RecvData();
        }
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void close_Click(object sender, EventArgs e)
        {
            MyControl.ClosePort();
        }
        /// <summary>
        /// 激活一个串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PortBox_MouseClick(object sender, MouseEventArgs e)
        {            
            
        }
        /// <summary>
        /// 标签2切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabPage2_Enter(object sender, EventArgs e)
        {
            TabPage t1 = (TabPage)sender;
            MyControl.SwitchTab(t1.Name.ToString());
        }
        /// <summary>
        /// 删除串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyControl.DelPort();
        }
        /// <summary>
        /// 点选串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PortBox_MouseUp(object sender, MouseEventArgs e)
        {
            ListBox s1 = (ListBox)sender;
            if(s1.SelectedItem!=null)
            MyControl.ActivePort(s1.SelectedItem.ToString());
        }
        /// <summary>
        /// 标签3切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabPage3_Enter(object sender, EventArgs e)
        {
            TabPage t1 = (TabPage)sender;
            MyControl.SwitchTab(t1.Name.ToString());
        }
        /// <summary>
        /// 刷新下拉控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DBSwitch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox s1 = (ComboBox)sender;
            MyControl.QueryData(s1.SelectedItem.ToString());
        }
        /// <summary>
        /// 鼠标右键菜单（打开）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyControl.OpenPort();
        }
        /// <summary>
        /// 鼠标右键菜单（关闭）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyControl.ClosePort();
        }
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reflush_Click(object sender, EventArgs e)
        {
            string s1= this.DBSwitch.SelectedItem.ToString();
            MyControl.QueryData(s1);
        }

        private void anaylase_Click(object sender, EventArgs e)
        {
            MyControl.Anaylase();
        }
    }
}