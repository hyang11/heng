using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;

namespace 宿舍饮用水登记系统
{
    public partial class 确认已送水 : Form
    {
        public const int BufferSize = 1024;    //缓存大小

        NetworkStream ns = null;
        string uAccount;

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

        static DataSet DataSetDeserialize(byte[] bytes)
        {
            System.IO.MemoryStream memStream = new MemoryStream(bytes);

            memStream.Position = 0;

            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter deserializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            DataSet ds;
            try
            {
                ds = (DataSet)deserializer.Deserialize(memStream); //反序列化 
                memStream.Close();
                return ds;
            }
            catch (Exception)
            {
                memStream.Close();
                MessageBox.Show("反序列化数据时出错！");
                return null;
            }
        }

        public DataSet OrderNumber(NetworkStream ns, string uAccount)   //查询当前正在送水的订单号
        {
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
            AsciiGetBytesSend(ns, "24");
            AsciiGetBytesSend(ns, uAccount);
            DataSet myst;
            myst = new DataSet();

            byte[] bytes = new byte[BufferSize * 64];
            int bytesRead = ns.Read(bytes, 0, bytes.Length);
            myst = DataSetDeserialize(bytes);
            return myst;
        }

        public int HaveBottled(NetworkStream ns, string orderNumber)
        {
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
            AsciiGetBytesSend(ns, "25");
            AsciiGetBytesSend(ns, orderNumber);
            if (AsciiGetstringRead(ns) == "1")
                return 1;
            else
                return 0;
        }

        class MyItem
        {
            private string did;

            public string Did
            {
                get { return did; }
                set { did = value; }
            }
        }

        public 确认已送水(NetworkStream ns, string uAccount)
        {
            this.ns = ns;
            this.uAccount = uAccount;

            InitializeComponent();

            List<MyItem> vtData = new List<MyItem>();
            int count = OrderNumber(ns, uAccount).Tables["Orders"].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                MyItem rItem = new MyItem();
                rItem.Did = OrderNumber(ns, uAccount).Tables["Orders"].Rows[i][0].ToString().TrimEnd();
                vtData.Add(rItem);
            }

            comboBox1.DataSource = vtData;
            comboBox1.ValueMember = "did";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Trim() == "")
                MessageBox.Show("请选择订单号！");
            else if (HaveBottled(ns,comboBox1.Text.Trim()) == 1)
            {
                MessageBox.Show("确认送水成功，订单已存入送水记录！");
                this.Close();
            }
            else
                MessageBox.Show("确认送水失败！"); 
        }
    }
}
