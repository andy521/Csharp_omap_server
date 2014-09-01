using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Csharp_omap_server;
using System.Windows.Forms;

namespace Csharp_omap_server
{
    /// <summary>
    /// 流程控制类
    /// </summary>
    class Control
    {
        /// <summary>
        /// 退出程序
        /// </summary>
        /// <param name="f1">住窗体引用</param>
        public Control(Fmain f1) 
        {
            MyView= new View(f1);
            MyModel= new Model(f1);
            f = f1;
            MyView.Start();
        }
        /// <summary>
        /// 推出程序
        /// </summary>
        public void Exit()
        {
            MyView.Exit();
            MyModel.Exit();
            MyView.FlushListBox(ref MyModel);
            Application.Exit();
        }
        /// <summary>
        /// 保存配置文件
        /// </summary>
        public void SaveConf()
        {
            MyModel.SaveConf();
            MyView.FlushListBox(ref MyModel);
        }
        /// <summary>
        /// 加载配置文件
        /// </summary>
        public void LoadConf() 
        {
            MyModel.LoadConf(f);
            MyView.FlushListBox(ref MyModel);
        }
        /// <summary>
        /// 打开串口
        /// </summary>
        public void OpenPort()
        {
            //String PortID = f.PortID.Text;
            if (!String.IsNullOrEmpty(f.PortID.Text))
            {
                if (MyModel.OpenPort(f.PortID.Text, f))
                {
                    MyView.FlushListBox(ref MyModel);
                }
                else 
                {
                    MessageBox.Show("Flush error", "Error");
                }
            }
            else 
            {
                MessageBox.Show("Add PortID to specific","Inform");
            }
        }
        /// <summary>
        /// 关闭串口
        /// </summary>
        public void ClosePort() 
        {
            if (!String.IsNullOrEmpty(f.PortID.Text))
            {
                if (MyModel.ClosePort(f.PortID.Text, f))
                {
                    MyView.FlushListBox(ref MyModel);
                }
                else
                {
                    MessageBox.Show("Can not close port", "Error");
                }
            }
            else
            {
                MessageBox.Show("Add PortID to specific", "Inform");
            }
        }
        /// <summary>
        /// 单击控件显示串口
        /// </summary>
        /// <param name="PortID">所选串口标识列</param>
        public void ActivePort(string PortID) 
        {
            MyView.FlushCombolBoX(MyModel,PortID);
        }
        /// <summary>
        /// 标签切换
        /// </summary>
        public void SwitchTab(string t1) 
        {
            MyView.SwitchTab(f,MyModel,t1);
        }
        /// <summary>
        /// 删除串口
        /// </summary>
        public void DelPort()
        {
            MyModel.DelPort(f);
            MyView.FlushListBox(ref MyModel);
        }
        /// <summary>
        /// 采集数据
        /// </summary>
        public void RecvData() 
        {
            MyModel.RecvData(f,MyModel);
        }

       
    
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="PortID"></param>
        public void QueryData(string PortID)
        {
            MyModel.QueryData(PortID);
        }
        /// <summary>
        /// 分析算法
        /// </summary>
        public void Anaylase()
        {
            MyModel.Anaylase();
        }   
        /// <summary>
        /// 数据成员（视图操作类,事务操作类）
        /// </summary> 
        View MyView;
        public Model MyModel;
        Fmain f;
    }
}
