using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetConfClientSoftware
{
    class AutoClosingMessageBox
    {
        public System.Threading.Timer _timeoutTimer;
        public string _caption;
        public AutoClosingMessageBox()
        {
        }
        //弹窗显示时长timeout默认值设置的是3000ms(也就是3秒)
        public AutoClosingMessageBox(string text, string caption, int timeout = 3000)
        {
            _caption = caption;
            _timeoutTimer = new System.Threading.Timer(OnTimerElapsed, null, timeout, System.Threading.Timeout.Infinite);
            MessageBox.Show(text, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }
        /// <summary>
        /// 将指定内容弹窗显示一定时长后自动关闭窗口
        /// </summary>
        /// <param name="text">显示内容</param>
        /// <param name="caption">说明内容</param>
        /// <param name="timeout">显示时长（毫秒）</param>
        public void Show(string text, string caption, int timeout)
        {
            new AutoClosingMessageBox(text, caption, timeout);
        }
        public void Show(string text, string caption)
        {
            new AutoClosingMessageBox(text, caption);
        }
        public void OnTimerElapsed(object state)
        {
            IntPtr mbWnd = FindWindow(null, _caption);
            if (mbWnd != IntPtr.Zero)
                SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            _timeoutTimer.Dispose();
        }
        const int WM_CLOSE = 0x0010;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }
}
