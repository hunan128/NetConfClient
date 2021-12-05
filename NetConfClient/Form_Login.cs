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
    public partial class Form_Login : Form
    {
        public Form_Login()
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
        String connetStr = "server=hunan128.com;port=3306;user=admin;password=admin123; database=netconf;";
        private Point mouseOff;//鼠标移动位置变量
        private bool leftFlag;//标签是否为左键
        public static string remember = "是";
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
                    textBoxUser.Text = user;
                    if (remember == "是")
                    {
                        textBoxPass.Text = ContentValue(strSec, "password");
                        checkBoxRe.Checked = true;
                    }
                    else {
                        checkBoxRe.Checked = false;
                    }

                    
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
                WritePrivateProfileString(strSec, "email", email, strFilePath);
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
                //String username, password, licence, sn;
                user = textBoxUser.Text;
                password = textBoxPass.Text;

                MySqlConnection conn = new MySqlConnection(connetStr);
                conn.Open();
                licence = "1";
                sn = MachineCode.GetMachineCodeString();

                String sql = "select user,pass,licence,sn from users where user='" + user + "'and pass='" + password + "'and licence='" + licence + "'and sn='" + sn + "'";//SQL语句实现表数据的读取
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.HasRows)  //如果能查到，说明该用户密码存在
                {
                    MessageBox.Show("在线认证成功：用户名、密码、机器码、License认证成功");

                    this.DialogResult = DialogResult.OK;
                    this.Dispose();
                    this.Close();
                }
                else
                {
                    sqlDataReader.Close();
                    sql = "select user,pass,sn from users where user='" + user + "'and pass='" + password + "'and sn='" + sn + "'";//SQL语句实现表数据的读取
                    cmd = new MySqlCommand(sql, conn);
                    sqlDataReader = cmd.ExecuteReader();
                    if (sqlDataReader.HasRows)  //如果能查到，说明该用户密码存在
                    {
                        MessageBox.Show("在线认证成功：用户名、密码、机器码认证成功");

                        this.DialogResult = DialogResult.OK;
                        this.Dispose();
                        this.Close();
                    }
                    else
                    {
                        sqlDataReader.Close();
                        sql = "select user,pass,licence from users where user='" + user + "'and pass='" + password + "'and licence='" + licence + "'";//SQL语句实现表数据的读取
                        cmd = new MySqlCommand(sql, conn);
                        sqlDataReader = cmd.ExecuteReader();
                        if (sqlDataReader.HasRows)  //如果能查到，说明该用户密码存在
                        {
                            MessageBox.Show("在线认证成功：用户名、密码、Licence认证成功");

                            this.DialogResult = DialogResult.OK;
                            this.Dispose();
                            this.Close();
                        }
                        else
                        {
                            sqlDataReader.Close();
                            sql = "select user,pass from users where user='" + user + "'and pass='" + password + "'";//SQL语句实现表数据的读取
                            cmd = new MySqlCommand(sql, conn);
                            sqlDataReader = cmd.ExecuteReader();
                            if (sqlDataReader.HasRows)  //如果能查到，说明该用户密码存在
                            {
                                MessageBox.Show("在线认证成功：用户名密码认证成功，License认证失败。请联系作者：sxhunan@163.com,开通权限");
                            }
                            else
                            {
                                MessageBox.Show("账号或密码错误或未注册");
                            }

                        }

                    }
                    //MessageBox.Show("账号或密码错误或未注册");
                }
                conn.Close();
            }
            catch {
                if (user == textBoxUser.Text && password == textBoxPass.Text )
                {
                    if (sn == MachineCode.GetMachineCodeString())
                    {
                        MessageBox.Show("离线认证成功：机器码认证成功");

                        this.DialogResult = DialogResult.OK;
                        this.Dispose();
                        this.Close();

                    }
                    else {
                        MessageBox.Show("离线认证失败，调整网络正常，注册后即可免费使用");
                    }

                }
                else {
                    MessageBox.Show("离线认证失败：用户名密码错误");

                }

            }

            if (checkBoxRe.Checked == true) {
                remember = "是";
            }
            else {
                remember = "否";
            }
            Setini();
        }

        private void PicClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

        }
        /// <summary>
        /// 注册按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Submit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form_Submit submit = new Form_Submit();
            submit.ShowDialog();
            
            if (submit.DialogResult == DialogResult.OK)
            {
                submit.Dispose();
                Getini();
            }
        }
        /// <summary>
        /// 从窗体加载ini文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Login_Load(object sender, EventArgs e)
        {
            Getini();
        }

        private void Forget_Password_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (textBoxUser.Text == "") {
                MessageBox.Show("请输入用户名进行找回密码");
                return;
            }
            user = textBoxUser.Text;
            Form_Forget_Password Form_Forget_Password = new Form_Forget_Password();
            Form_Forget_Password.ShowDialog();

            if (Form_Forget_Password.DialogResult == DialogResult.OK)
            {
                Form_Forget_Password.Dispose();
                Getini();
            }
            if (Form_Forget_Password.DialogResult == DialogResult.Cancel)
            {
                Form_Forget_Password.Dispose();
                Form_Forget_Password.Close();
               // Getini();
            }
        }
    }
}
