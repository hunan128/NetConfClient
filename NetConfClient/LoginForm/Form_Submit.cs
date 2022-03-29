using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NetConfClientSoftware
{
    public partial class Form_Submit : Form
    {
        public Form_Submit()
        {
            InitializeComponent();
        }
        #region 声明ini变量
        public static string user = "";
        public static string password = "";
        public static string email = "";
        public static string licence = "";
        public static string sn = "";
        public static string ip = "";
        // private string strFilePath = Application.StartupPath + "\\Config.ini";//获取INI文件路径
        private string strFilePath = @"C:\netconf\Config.ini";
        private string strSec = ""; //INI文件名
        String connetStr = Form_Login.connetStr;
        private Point mouseOff;//鼠标移动位置变量
        private bool leftFlag;//标签是否为左键
        public static string remember = "";
        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">节点名称[如[TypeName]]</param>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="def">值</param>
        /// <param name="retval">stringbulider对象</param>
        /// <param name="size">字节大小</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

        #endregion
        /// <summary>
        /// 读取config文件函数
        /// </summary>
        /// <param name="Section">config节点</param>
        /// <param name="key">节点内的子节点</param>
        /// <returns></returns>
        private string ContentValue(string Section, string key)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, strFilePath);
            return temp.ToString();
        }
        /// <summary>
        /// 加载ini的config文件
        /// </summary>
        private void Getini()
        {
            #region 读取ini文件
            try
            {
                if (!Directory.Exists(@"C:\netconf"))
                {
                    Directory.CreateDirectory(@"C:\netconf");
                }
                //   导入前俩列ToolStripMenuItem.PerformClick();

                if (File.Exists(strFilePath))//读取时先要判读INI文件是否存在
                {
                    strSec = "Login";
                    user = ContentValue(strSec, "user");
                    password = ContentValue(strSec, "password");
                    email = ContentValue(strSec, "email");
                    licence = ContentValue(strSec, "licence");
                    sn = ContentValue(strSec, "sn");
                }
            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
            #endregion
        }

        #region 设置ini文件内容
        /// <summary>
        /// 设置ini文件
        /// </summary>
        private void Setini()
        {

            try
            {
                //根据INI文件名设置要写入INI文件的节点名称
                //此处的节点名称完全可以根据实际需要进行配置
                strSec = "Login";
                WritePrivateProfileString(strSec, "user", user, strFilePath);
                WritePrivateProfileString(strSec, "password", password, strFilePath);
                //WritePrivateProfileString(strSec, "email", email, strFilePath);
                WritePrivateProfileString(strSec, "licence", licence.ToString(), strFilePath);
                WritePrivateProfileString(strSec, "sn", sn, strFilePath);
                WritePrivateProfileString(strSec, "remember", Form_Login.remember, strFilePath);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        private void FrmMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }
        private void FrmMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }
        private void FrmMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }
        /// <summary>
        /// 注册按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                String create_time, update_time;
                user = textUser.Text;
                password = textPass.Text;
                if (password != textPassS.Text)
                {
                    MessageBox.Show("密码输入与第一次不符，请重新输入", "提示");
                    return;
                }

                email = textMail.Text;
                if (!IsValidEmail(email))
                {
                    MessageBox.Show("请重新输入mail", "提示");
                    return;
                }
                sn = textSN.Text;
                if (string.IsNullOrEmpty(sn))
                {
                    MessageBox.Show("机器码获取失败，请联系作者：sxhunan@163.com", "提示");
                    return;
                }
                create_time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                update_time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                licence = "1";


                MySqlConnection conn = new MySqlConnection(connetStr);
                conn.Open();
               // sn = MachineCode.GetMachineCodeString();

                String sql = "select user from users where user='" + user + "'";//SQL语句实现表数据的读取
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.HasRows)  //如果能查到，说明该用户密码存在
                {
                    MessageBox.Show("用户已存在，请更换用户名，再次尝试");
                    return;
                }
                else {
                    sqlDataReader.Close();
                    sql = "INSERT INTO users(user,pass,mail,licence,sn,create_time,update_time,ipaddress) VALUES('" + user + "','" + password + "','" + email + "','" + licence + "','" + sn + "','" + create_time + "','" + update_time + "','" + ip + "')"; // 没有判断重复插入
                    cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("注册成功");
                    Setini();
                    conn.Close();

                    this.DialogResult = DialogResult.OK;
                    this.Dispose();
                    this.Close();
                }
               
            }
            catch (Exception ex){
                MessageBox.Show(ex.ToString());
            }

        }
        /// <summary>
        /// 判断Email是否合法
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PicClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

        }
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Submit_Load(object sender, EventArgs e)
        {
            //textSN.Text = MachineCode.GetMachineCodeString();
            textSN.Text = MachineCode.GetHardDiskID();
            // 
            var t0_ip = Myip.GetIPFromHtml(Myip.HttpGetPageHtml("http://myip.ipip.net", "utf-8"));// 111.198.29.123

            // var t2_ip = Myip.GetIPFromHtml(Myip.HttpGetPageHtml("https://www.whatismyip.com/my-ip-information/", "utf-8"));// 111.198.29.123
            //var t3_ip = Myip.GetIPFromHtml(Myip.HttpGetPageHtml("https://www.cman.jp/network/support/go_access.cgi", "utf-8"));// 111.198.29.123
            if (!string.IsNullOrEmpty(t0_ip))
            {
                ip = t0_ip;
            }
            else {
                var t1_ip = Myip.GetIPFromHtml(Myip.HttpGetPageHtml("http://www.net.cn/static/customercare/yourip.asp", "gbk"));// 111.198.29.123
                if (!string.IsNullOrEmpty(t1_ip))
                {
                    ip = t1_ip;
                }
            }
            if (string.IsNullOrEmpty(ip))
            {
                MessageBox.Show("设备未接入互联网，请接入互联网后再此尝试！"+MachineCode.GetMachineCodeString());
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            //if (!string.IsNullOrEmpty(t2_ip))
            //{
            //    ip = t2_ip;
            //}
            //if (!string.IsNullOrEmpty(t3_ip))
            //{
            //    ip = t3_ip;
            //}

        }
       
    }
}
