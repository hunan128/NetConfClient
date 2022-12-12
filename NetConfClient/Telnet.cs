using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetConfClientSoftware
{
    class Telnet
    {
        private System.Net.Sockets.Socket socket;
        private bool closed;
        private static string ipaddress = "";
        public Telnet()

        {

            socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            closed = true;
        }
        public bool Connect(string address, string port)
        {
            try
            {

                System.Net.IPAddress ipaddr = System.Net.IPAddress.Parse(address);
                ipaddress = address;
                System.Net.IPEndPoint ipep = new System.Net.IPEndPoint(ipaddr, int.Parse(port));
                socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                socket.Connect(ipep);
                int datalong = socket.Available;

                System.Threading.Thread.Sleep(100);
                while (datalong < socket.Available)
                {
                    datalong = socket.Available;
                    System.Threading.Thread.Sleep(100);
                }
                if (datalong > 0)
                {
                    byte[] recvdata = new byte[datalong];
                    byte[] senddat = new byte[datalong];
                    int p = 0;
                    socket.Receive(recvdata, 0, datalong, System.Net.Sockets.SocketFlags.None);
                    for (int i = 0; i < datalong; i++)
                    {
                        if (recvdata[i] == 255)
                        {

                            if (recvdata[i + 1] == 250)
                            {
                                senddat[p] = 255;
                                senddat[p + 1] = 240;
                                senddat[p + 2] = recvdata[i + 2];
                                i = i + 2;
                                p = p + 3;
                            }
                            if (recvdata[i + 1] == 251)
                            {
                                senddat[p] = 255;
                                senddat[p + 1] = 253;
                                senddat[p + 2] = recvdata[i + 2];
                                i = i + 2;
                                p = p + 3;
                            }
                            if (recvdata[i + 1] == 253)
                            {
                                senddat[p] = 255;
                                senddat[p + 1] = 251;
                                senddat[p + 2] = recvdata[i + 2];
                                i = i + 2;
                                p = p + 3;
                            }
                            if (recvdata[i + 1] == 252 || recvdata[i + 1] == 254)
                            {
                                senddat[p] = 255;
                                senddat[p + 1] = 254;
                                senddat[p + 2] = recvdata[i + 2];
                                i = i + 2;
                                p = p + 3;
                            }

                        }
                    }
                    socket.Send(senddat, 0, p, System.Net.Sockets.SocketFlags.None);
                }

            }
            catch
            {

                return false;
            }
            closed = false;
            return true;
        }


        /// <summary>
        /// 计算字符串中子串出现的次数
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="substring">子串</param>
        /// <returns>出现的次数</returns>
        static int SubstringCount(string str, string substring)
        {
            if (str.Contains(substring))
            {
                string strReplaced = str.Replace(substring, "");
                return (str.Length - strReplaced.Length) / substring.Length;
            }
            return 0;
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="ts">不同数据块到达的最大间隔（毫秒）</param>
        /// <returns></returns>
        public string ReceiveData(int ts)
        {
            string str = "";
            int datalong;
            byte[] recvdata;
            try
            {
                datalong = socket.Available;
                System.Threading.Thread.Sleep(ts);
                while (datalong < socket.Available)
                {
                    datalong = socket.Available;
                    System.Threading.Thread.Sleep(ts);
                }
                recvdata = new byte[datalong];
                if (recvdata.Length > 0)
                {
                    socket.Receive(recvdata, 0, datalong, System.Net.Sockets.SocketFlags.None);
                    string ss = Encoding.ASCII.GetString(recvdata).Trim();
                    string luanma = "[7m --Press any key to continue Ctrl+c to stop-- [m";
                    string newSS = ss.Replace(luanma, "Press any key to continue Ctrl+c to stop");
                    string luama2 = "                                              ";
                    string newSD = newSS.Replace(luama2, "");
                    string vcg = "[0m[0;0m";//[0;31m
                    string newvcg = newSD.Replace(vcg, "");
                    string vcg2 = "\n";
                    string newvcg2 = newvcg.Replace(vcg2, "\r\n");
                    string kou = "";
                    string kou2 = "";
                    string newkou = newvcg2.Replace(kou, "");
                    string newkou2 = newkou.Replace(kou2, "");
                    string msapeth = "[0;32m";
                    string msapeth2 = newkou2.Replace(msapeth, "");
                    string msapeth1 = "[0m";
                    string msapeth3 = msapeth2.Replace(msapeth1, "");
                    string msapeth4 = "[0;0m";
                    string msapeth5 = msapeth3.Replace(msapeth4, "");
                    string msapeth6 = "[0;31m";
                    str = msapeth5.Replace(msapeth6, "");

                }
            }
            catch (Exception eee)
            {
                str = eee.ToString();
            }
            if (str != "")
            {
                WriteLogs("Logs", "应答：", str);
            }

            return str;

        }


        /// <summary>
        /// 日志部分
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        /// <param name="content"></param>
        public static void WriteLogs(string fileName, string type, string content)
        {
            try
            {
                string path = @"C:\netconf\";
                if (!string.IsNullOrEmpty(path))
                {
                    path = @"C:\netconf\" + fileName;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = path + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = path + "\\" + ipaddress + "-" + DateTime.Now.ToString("yyyyMMdd") +"telnet"+ ".txt";
                    if (!File.Exists(path))
                    {
                        FileStream fs = File.Create(path);
                        fs.Close();
                    }
                    if (File.Exists(path))
                    {
                        //StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default);
                        //sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + type + "-->" + content);
                        ////  sw.WriteLine("----------------------------------------");
                        //sw.Close();
                        using (StreamWriter sw = new StreamWriter(path, true, Encoding.Default))
                        {
                            string s = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + type + "-->" + content;

                            sw.WriteLine(s);
                            sw.WriteLine("-------------------------------------------------------------------------------------------------------");
                            sw.Close();
                        }

                    }
                }
            }
            catch
            {
            }

        }
        /// <summary>
        /// 发送带命令结束符\r\n的数据
        /// </summary>
        /// <param name="dataStr"></param>
        /// <returns></returns>
        public bool SendData(string dataStr)
        {
            bool r = true;
            if (dataStr == null || dataStr.Length < 0)
                return false;
            byte[] cmd = Encoding.ASCII.GetBytes(dataStr + "\r\n");
            WriteLogs("Logs", "请求：", dataStr);
            try
            {
                int n = socket.Send(cmd, 0, cmd.Length, System.Net.Sockets.SocketFlags.None);
                if (n < 1)
                    r = false;
            }
            catch
            {

                r = false;
            }
            return r;
        }
        public bool SendDate(string dataStr)
        {
            bool r = true;
            if (dataStr == null || dataStr.Length < 0)
                return false;
            byte[] cmd = Encoding.ASCII.GetBytes(dataStr);
            //WriteLogs("Logs", "请求：", dataStr);
            try
            {
                int n = socket.Send(cmd, 0, cmd.Length, System.Net.Sockets.SocketFlags.None);
                if (n < 1)
                    r = false;
            }
            catch
            {

                r = false;
            }
            return r;
        }
        public bool SendData(byte[] dataByte)
        {
            bool r = true;
            if (dataByte == null || dataByte.Length < 0)
                return false;
            try
            {
                int n = socket.Send(dataByte, 0, dataByte.Length, System.Net.Sockets.SocketFlags.None);
                if (n < 1)
                    r = false;
            }
            catch
            {
                r = false;
            }
            return r;
        }
        public void Close()
        {
            try
            {
                socket.Close();
            }
            catch { }
            closed = true;
        }
        public bool IsClosed()
        {
            return closed;
        }

    }
}
