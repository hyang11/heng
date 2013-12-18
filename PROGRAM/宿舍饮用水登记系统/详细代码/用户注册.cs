using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;

namespace 宿舍饮用水登记系统
{
    public partial class 用户注册 : Form
    {
        public const int BufferSize = 1024;    //缓存大小
        宿舍饮用水登记系统.登录 land;
        NetworkStream ns;
        /*int ti;

        public int Ti
        {
            get { return ti; }
            set { ti = value; }
        }*/
        public 用户注册(NetworkStream ns, 宿舍饮用水登记系统.登录 land)
        {
            InitializeComponent();
            this.land = land;
            this.ns = ns;
            //ti = land.Ti;
        }

        //以ASCII编码的发送信息
        public void AsciiGetBytesSend(NetworkStream ns, string str)
        {
            byte[] byteTime = Encoding.ASCII.GetBytes(str);
            try
            {
                ns.Write(byteTime, 0, byteTime.Length);
            }
            catch (Exception)
            {
                MessageBox.Show("向服务器发送数据时出错！");
            }
        }

        //以ASCII编码的读取数据
        public string AsciiGetstringRead(NetworkStream ns)
        {
            byte[] bytes = new byte[BufferSize];
            int bytesRead = ns.Read(bytes, 0, bytes.Length);
            return Encoding.ASCII.GetString(bytes, 0, bytesRead);
        }

        public int UserRegistration(NetworkStream ns,string name, string no, string account, string password, string defaultNumber)
        {
            //向服务器发送操作号
            //0表示验证---用户---用户名和密码是否正确
            //1表示---用户---返回窗口初始化信息(用户名和密码)
            //2表示---送水职工---返回窗口初始化信息（用户名，姓名，ip以及端口）
            //3表示验证---送水职工---用户名和密码是否正确
            //4表示---用户---注册
            //5表示---送水职工---注册
            //6表示---用户---查询余额
            //7表示---送水职工---查询账户信息
            //8表示---用户---查询当前订单
            //9表示---用户---查询送水记录
            //10表示---送水职工---查询当前订单
            //11表示---送水职工---查询送水记录
            //12表示---用户---订水
            //13表示查询单价
            //14表示查询---用户---默认寝室号
            //15表示---用户---查询当前订单号
            //16表示---用户---取消订单
            //17表示---用户---更改送水时间段
            //18表示充值
            //19表示---用户---更改密码
            //20表示---送水职工---更改密码
            //21表示---送水职工---更改单价
            //22表示---送水职工---根据订单号查询订水寝室号和定水量，以及更改订单的送水账户、送水人及送水状态信息
            //23表示---送水职工---查询当前未开始送水的订单号
            //24表示---用户---查询当前正在送水的订单号
            //25表示---用户---确认已送水
            //26表示---送水职工---查询职工密钥
            //27表示---送水职工---更改职工密钥
            
            AsciiGetBytesSend(ns, "4");  //发送信息

            //向服务器发送用户名
            
            AsciiGetBytesSend(ns, account);  //发送信息

            //接收验证信息
            
            if (AsciiGetstringRead(ns) == "0")
            {
                //向服务器发送注册信息
                //姓名
                char[] bytes2 = name.ToCharArray();
                byte[] byteTime2 = Encoding.Unicode.GetBytes(bytes2, 0, name.Length);
                try
                {
                    ns.Write(byteTime2, 0, byteTime2.Length);
                }
                catch (Exception)
                {
                    MessageBox.Show("向服务器发送数据时出错！");
                }
                //学号\教职工号
                if (AsciiGetstringRead(ns) == "1")
                {
                    AsciiGetBytesSend(ns, no);  //发送信息
                    //密码
                    if (AsciiGetstringRead(ns) == "1")
                    {
                        AsciiGetBytesSend(ns, password);  //发送信息
                        //默认寝室号
                        if (AsciiGetstringRead(ns) == "1")
                        {
                            AsciiGetBytesSend(ns, defaultNumber);  //发送信息

                            //接收注册情况信息
                            if (AsciiGetstringRead(ns) == "1")
                            {
                                return 1;
                            }
                            else
                                return 0;
                        }
                        else
                            return 0;
                    }
                    else
                        return 0;
                }
                else
                    return 0;
            }
            else
                return 0;
        }

        /*private void CloseProcess(string name)
        {
            try
            {
                System.Diagnostics.Process[] myProcesses = System.Diagnostics.Process.GetProcesses();
                foreach (System.Diagnostics.Process myProcess in myProcesses)
                {
                    if (name == myProcess.ProcessName)
                        myProcess.Kill();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }*/

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult a = MessageBox.Show("您确定要取消注册吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (a == System.Windows.Forms.DialogResult.OK)
            {
                this.Close();
                land.Show();
            }
        }

        /*private void 用户注册_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult a = MessageBox.Show("您确定要取消注册吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (a == System.Windows.Forms.DialogResult.OK)
            {
                this.Hide();
                land.Show();
            }
            else
                e.Cancel = true;
        }*/

        public void UserR()
        {
            int t;
            t = Encoding.Default.GetBytes(textBox1.Text).Length;
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "" || textBox4.Text.Trim() == "" || textBox5.Text.Trim() == "" || textBox6.Text.Trim() == "")
            {
                MessageBox.Show("请把信息填写完整！");
                return;
            }
            else if (t > 8)
            {
                MessageBox.Show("姓名是汉字的不得超过4个字，是字母的不得超过8个字母！");
                return;
            }
            else if (textBox4.Text != textBox5.Text)
            {
                MessageBox.Show("密码和确认密码不相同，请重新输入！");
                return;
            }
            else
            {
                int a = UserRegistration(ns,textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox6.Text);
                if (a == 0)
                {
                    MessageBox.Show("用户名已使用，请重新输入用户名！");
                }
                else if (a == 1)
                {
                    MessageBox.Show("                      注册成功！\r请记住您的用户名（" + textBox3.Text + "）和密码！");
                    //ti = 1;
                    this.Close();
                    land.Show();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserR();
        }

    }
}
