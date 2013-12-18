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
    public partial class 订水 : Form
    {
        public const int BufferSize = 1024;    //缓存大小

        //Users user;
        NetworkStream ns = null;
        //string account;

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

        string orderNumber; 
        string uAccount; 
        string place; 
        int quantity; 
        float uPrice;
        string timePeriod;
        //string bottle;
        string balance;
        string defaultNumber;

        public 订水(Users user, NetworkStream ns, string account, string balance,string defaultNumber)
        {
            uAccount = account;
            this.ns = ns;
            this.balance = balance;
            this.defaultNumber = defaultNumber;

            InitializeComponent();
            textBox1.Text = defaultNumber;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" )
            {
                MessageBox.Show("请输入送水地点！");
            }
            else if (comboBox1.Text.Trim() == "")
            {
                MessageBox.Show("请输入送水量！");
            }
            else
            {
                place = textBox1.Text;
                if (Convert.ToInt32(balance) < uPrice)
                    MessageBox.Show("余额不足！");
                else
                {
                    Orders order = new Orders();
                    orderNumber = DateTime.Now.ToString() + " " + uAccount;
                    place = textBox1.Text;
                    if (comboBox1.Text == "1桶")
                        quantity = 1;
                    else if (comboBox1.Text == "2桶")
                        quantity = 2;
                    else if (comboBox1.Text == "3桶")
                        quantity = 3;
                    else
                        quantity = 0;
                    timePeriod = textBox3.Text + "-" + textBox4.Text;

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
                    AsciiGetBytesSend(ns, "13");
                    uPrice = Convert.ToInt32(AsciiGetstringRead(ns));

                    if (quantity == 0)
                    {
                        MessageBox.Show("请选择定水量！");
                    }

                    else if (order.Initialization(ns, orderNumber, uAccount, place, quantity, uPrice, timePeriod) == 1)
                    {
                        MessageBox.Show("订水成功！");
                        this.Close();
                    }
                    else
                        MessageBox.Show("订水失败，请检查余额是否充足！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
