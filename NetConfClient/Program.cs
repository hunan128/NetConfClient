using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NetConfClientSoftware
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form_Main());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //程序首先显示登陆界面
            Form_Login login = new Form_Login();
            login.ShowDialog();
            //登陆结果正确之后显示主界面
            if (login.DialogResult == DialogResult.OK)
            {
                login.Dispose();
                Application.Run(new Form_Main());
            }
            else if (login.DialogResult == DialogResult.Cancel)
            {
                login.Dispose();
                return;
            }
        }
    }
}
