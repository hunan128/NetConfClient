using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NetConfClientSoftware
{
    public partial class Form_Forget_Password : Form
    {
        public Form_Forget_Password()
        {
            InitializeComponent();
        }
        #region 声明ini变量
        public static string user = "";
        public static string password = "";
        public static string email = "";
        public static string licence = "";
        public static string sn = "";
        private string strFilePath = @"C:\netconf\Config.ini";
        private string strSec = ""; //INI文件名
        String connetStr = Form_Login.connetStr; //连接MYSQL
        private Point mouseOff;//鼠标移动位置变量
        private bool leftFlag;//标签是否为左键
        public static string remember = "是";
        public static string ip = ""; 
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
        // private string strFilePath = Application.StartupPath + "\\Config.ini";//获取INI文件路径

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
                    remember = ContentValue(strSec, "remember");


                    
                }
            }
            catch 
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
                WritePrivateProfileString(strSec, "remember", remember, strFilePath);




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
        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                String update_time;
                user = textBoxUser.Text;
                password = textBoxPass.Text;
                update_time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                if (password != textBoxPass2.Text) {
                    MessageBox.Show("俩次输入密码不一致，请重新输入");
                    return;

                }
                MySqlConnection conn = new MySqlConnection(connetStr);
                conn.Open();
                licence = "1";
                sn = MachineCode.GetMachineCodeString();
                String sql = "UPDATE users set "+ "pass = '" + password + "',update_time = '" + update_time + "',ipaddress = '" + ip + "'Where user = '"+user+"'";
                //String sql = "select user,pass,licence,sn from users where user='" + username + "'and pass='" + password + "'and licence='" + licence + "'and sn='" + sn + "'";//SQL语句实现表数据的读取
                MySqlCommand cmd = new MySqlCommand(sql, conn);
               // MySqlDataReader sqlDataReader = cmd.ExecuteReader();
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("密码修改成功");
                    Setini();
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("修改失败：用户未注册，请先注册");
                    return;
                }

                conn.Close();
            }
            catch (Exception  ex){
                MessageBox.Show(ex.ToString());

            }

            

        }
        public static string GetEmail(string email)
        {
            if (email == null) {
                return email;
            }
            string[] emailParts = email.Split('@');
            if (emailParts.Length != 2)
            {
                return email;
            }
            int len = emailParts[0].Length;
            String char1 = emailParts[0].Substring(0, 2);
            String char2 = emailParts[0].Substring(len - 1);
            // String char3 = STARS.substring(0, len - 2);  // 截取中间字符串位数   
            String char3 = "";
            for (int i = 0; i < len-3; i++)
            {
                char3 = char3 + "*";
            }
            return char1 + char3 + char2 + "@" + emailParts[1];
        }
        private void PicClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

        }
        private void E_mail() {
            MySqlConnection conn = new MySqlConnection(connetStr);
            try
            {
                conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                //MessageBox.Show("已经建立连接");
                //在这里使用代码对数据库进行增删查改
                //设置查询命令
                string SQLoid = "SELECT users.user, users.pass,users.mail  from users WHERE user = '" + user + "'";
                MySqlCommand oid = new MySqlCommand(SQLoid, conn);
                //查询结果读取器
                MySqlDataReader readeroid = oid.ExecuteReader();
                // MessageBox.Show(reader[0].ToString());
                while (readeroid.Read())
                {
                    email = readeroid[2].ToString();
                    password = readeroid[1].ToString();
                }

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 从窗体加载ini文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Login_Load(object sender, EventArgs e)
        {
            var t0_ip = Myip.GetIPFromHtml(Myip.HttpGetPageHtml("http://myip.ipip.net", "utf-8"));// 111.198.29.123

            // var t2_ip = Myip.GetIPFromHtml(Myip.HttpGetPageHtml("https://www.whatismyip.com/my-ip-information/", "utf-8"));// 111.198.29.123
            //var t3_ip = Myip.GetIPFromHtml(Myip.HttpGetPageHtml("https://www.cman.jp/network/support/go_access.cgi", "utf-8"));// 111.198.29.123
            if (!string.IsNullOrEmpty(t0_ip))
            {
                ip = t0_ip;
            }
            else
            {
                var t1_ip = Myip.GetIPFromHtml(Myip.HttpGetPageHtml("http://www.net.cn/static/customercare/yourip.asp", "gbk"));// 111.198.29.123
                if (!string.IsNullOrEmpty(t1_ip))
                {
                    ip = t1_ip;
                }
            }
            if (string.IsNullOrEmpty(ip))
            {
                MessageBox.Show("设备未接入互联网，请接入互联网后再此尝试！" + MachineCode.GetMachineCodeString());
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            user = Form_Login.user;
            textBoxUser.Text = user;
            email = "";
            E_mail();
            textBoxViewMail.Text = GetEmail(email);
            if (email == "") {
                MessageBox.Show("用户不存在，请重新填写");
                this.DialogResult = DialogResult.Cancel;
            }

        }

        private void buttoncheckemail_Click(object sender, EventArgs e)
        {
            if (textBoxEmail.Text == email)
            {
                textBoxPass.Visible = true;
                textBoxPass2.Visible = true;
                ButtonForget.Visible = true;
                labelp.Visible = true;
                labelp2.Visible = true;
                panel5.Visible = true;
                panel6.Visible = true;
                MessageBox.Show("邮箱正确，请设置密码");
            }
            else {
                MessageBox.Show("邮箱验证失败，请重试");
            }
        }
    }
}
