using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;

namespace NetConfClientSoftware
{
    class Connection_Arrange
    {
        public static Telnet Telnet = new Telnet();
        static int ts = 50;
        static int XHTime = 1000;
        static int count = 10;

        public static string Get_DXC()
        {
            string dxc = "";
            //dxc += Connect(ip, username, password);
            Telnet.SendData("config otn");
            string str = Telnet.ReceiveData(10 * ts);
            Telnet.SendData("show dxc");
            for (int i = 0; i < count; i++)
            {
                Thread.Sleep(XHTime / 10);
                str = Telnet.ReceiveData(10 * ts);
                if (str != "")
                {
                    dxc += str;

                }
                else {
                    break;
                }
            }
            Telnet.SendData("exit");
            return dxc;
        }

        public static string Get_UMS()
        {
            string dxc = "";
            Telnet.SendData("config otn");
            string str = Telnet.ReceiveData(10 * ts);
            Telnet.SendData("show running-config ums");
            for (int i = 0; i < count; i++)
            {
                Thread.Sleep(XHTime / 10);
                str = Telnet.ReceiveData(10 * ts);
                if (str != "")
                {
                    dxc += str;

                }
                else
                {
                    break;
                }
            }
            Telnet.SendData("exit");
            return dxc;
        }
        public static string Get_OTN()
        {
            string dxc = "";
            string str = "";
            Telnet.SendData("show running-config otn-mgmt");
            for (int i = 0; i < count; i++)
            {
                Thread.Sleep(XHTime / 10);
                str = Telnet.ReceiveData(10 * ts);
                if (str != "")
                {
                    dxc += str;

                }
                else
                {
                    break;
                }
            }
            return dxc;
        }
        public static string Get_PTN_CTP()
        {
            string dxc = "";
            string str = "";
            Telnet.SendData("show running-config ctp");
            for (int i = 0; i < count; i++)
            {
                Thread.Sleep(XHTime / 10);
                str = Telnet.ReceiveData(10 * ts);
                if (str != "")
                {
                    dxc += str;

                }
                else
                {
                    break;
                }
            }
            return dxc;
        }
        public static string Get_ETH_CTP()
        {
            string dxc = "";
            string str = Telnet.ReceiveData(10 * ts);
            Telnet.SendData("show running-config ctp");
            for (int i = 0; i < count; i++)
            {
                Thread.Sleep(XHTime / 10);
                str = Telnet.ReceiveData(10 * ts);
                if (str != "")
                {
                    dxc += str;

                }
                else
                {
                    break;
                }
            }
            return dxc;
        }
        public static string Delete_DXC(string ID)
        {
            string dxc = "";
            if (!string.IsNullOrEmpty(ID))
            {
                Telnet.SendData("config otn");
                Telnet.SendData("delete dxc " + ID);
                Thread.Sleep(XHTime / 5);
                Telnet.SendData("exit");
                dxc = "\r\n" + "交叉ID:\t" + ID + "已删除" + "\r\n";
            }

            return dxc;
        }
        public static string Delete_UMS_LPG(string PGID)
        {
            string dxc = "";
            if (!string.IsNullOrEmpty(PGID))
            {
                Telnet.SendData("config otn");
                Telnet.SendData("delete ums-lpg " + PGID);
                Thread.Sleep(XHTime / 5);
                Telnet.SendData("exit");
                dxc = "\r\n" + "保护组ID:\t" + PGID + "已删除" + "\r\n";
            }

            return dxc;
        }
        public static string Delete_UMS(string config)
        {
            string dxc = "";
            if (!string.IsNullOrEmpty(config))
            {
                Telnet.SendData("config otn");
                Telnet.SendData("delete " + config);
                Thread.Sleep(XHTime / 5);
                Telnet.SendData("exit");
                dxc = "\r\n" + "UMS配置:\t" + config + "已删除" + "\r\n";
            }

            return dxc;
        }
        public static string Delete_CTP(string dir, string rate, string port,string info)
        {
            string dxc = "";
            if (dir.Contains("client") && rate.Contains("ODU"))
            {
                if (!string.IsNullOrEmpty(port))
                {
                    string[] portts = port.Split(new char[] { '/' });
                    string Slot = portts[0];
                    string Port = portts[1];
                    string TS = portts[2];
                    Telnet.SendData("config otn");
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("int ptp " + Slot + "/" + Port);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("odu ctp del " + rate + " " + TS);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    dxc = "\r\n" + "PTP-CTP时隙:\t" + dir + "-" + rate + "-" + port + "已删除" + "\r\n";
                }
            }
            if (dir.Contains("logic") && rate.Contains("ODU"))
            {
                if (!string.IsNullOrEmpty(port))
                {
                    string[] portts = port.Split(new char[] { '/' });
                    string Slot = portts[0];
                    string Port = portts[1];
                    string TS = portts[2];
                    Telnet.SendData("config otn");
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("int ftp " + Slot + "/" + Port);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("odu ctp del " + rate + " " + TS);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    dxc = "\r\n" + "FTP-CTP时隙:\t" + dir + "-" + rate + "-" + port + "已删除" + "\r\n";
                }
            }
            if (dir.Contains("client") && rate.Contains("stream"))
            {
                if (!string.IsNullOrEmpty(port))
                {
                    string[] portts = port.Split(new char[] { '/' });
                    string Slot = portts[0];
                    string Port = portts[1];
                    string TS = portts[2];
                    Telnet.SendData("config otn");
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("int ptp " + Slot + "/" + Port);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("eth ctp  del " + rate + " " + TS);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    dxc = "\r\n" + "PTP-CTP流:\t" + dir + "-" + rate + "-" + port + "已删除" + "\r\n";
                }
            }
            if (dir.Contains("logic") && rate.Contains("stream"))
            {
                if (!string.IsNullOrEmpty(port))
                {
                    string[] portts = port.Split(new char[] { '/' });
                    string Slot = portts[0];
                    string Port = portts[1];
                    string TS = portts[2];
                    Telnet.SendData("config otn");
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("int ftp " + Slot + "/" + Port);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("eth ctp del " + rate + " " + TS);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    dxc = "\r\n" + "FTP-CTP流:\t" + dir + "-" + rate + "-" + port + "已删除" + "\r\n";
                }
            }
            if (dir.Contains("logic") && rate.Contains("OSU"))
            {
                if (!string.IsNullOrEmpty(port))
                {
                    string[] portts = port.Split(new char[] { '/' });
                    string Slot = portts[0];
                    string Port = portts[1];
                    string TS = portts[2];
                    Telnet.SendData("config otn");
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("int ftp " + Slot + "/" + Port);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("osu ctp del " + TS);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    dxc = "\r\n" + "FTP-CTP-OSU:\t" + dir + "-" + rate + "-" + port + "已删除" + "\r\n";
                }
            }
            if (dir.Contains("client") && rate.Contains("OSU"))
            {
                if (!string.IsNullOrEmpty(port))
                {
                    string[] portts = port.Split(new char[] { '/' });
                    string Slot = portts[0];
                    string Port = portts[1];
                    string TS = portts[2];
                    Telnet.SendData("config otn");
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("int ptp " + Slot + "/" + Port);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("osu ctp del " + TS);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    dxc = "\r\n" + "PTP-CTP-OSU:\t" + dir + "-" + rate + "-" + port + "已删除" + "\r\n";
                }
            }
            if (dir.Contains("client") && rate.Contains("VC4")&& info.Contains("stm"))
            {
                if (!string.IsNullOrEmpty(port))
                {
                    string[] portts = port.Split(new char[] { '/' });
                    string Slot = portts[0];
                    string Port = portts[1];
                    string TS = portts[2];
                    Telnet.SendData("config otn");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("int ptp " + Slot + "/" + Port);
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("sdh ctp del " + rate + " " + TS + " 1");
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    dxc = "\r\n" + "PTP-CTP-VC4:\t" + dir + "-" + rate + "-" + port + "已删除" + "\r\n";
                }
            }
            if (dir.Contains("logic") && rate.Contains("VC4") && info.Contains("stm"))
            {
                if (!string.IsNullOrEmpty(port))
                {
                    string[] portts = port.Split(new char[] { '/' });
                    string Slot = portts[0];
                    string Port = portts[1];
                    string TS = portts[2];
                    Telnet.SendData("config otn");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("int ftp " + Slot + "/" + Port);
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("sdh ctp del " + rate + " " + TS + " 1");
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    dxc = "\r\n" + "FTP-CTP-VC4:\t" + dir + "-" + rate + "-" + port + "已删除" + "\r\n";
                }
            }
            if (dir.Contains("client") && rate.Contains("VC12") && info.Contains("stm"))
            {
                if (!string.IsNullOrEmpty(port))
                {
                    string[] portts = port.Split(new char[] { '/' });
                    string Slot = portts[0];
                    string Port = portts[1];
                    string HP = portts[2];
                    string LP = portts[3];
                    Telnet.SendData("config otn");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("int ptp " + Slot + "/" + Port);
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("sdh ctp del " + rate + " " + HP + " " + LP);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    dxc = "\r\n" + "PTP-CTP-VC12:\t" + dir + "-" + rate + "-" + port + "已删除" + "\r\n";
                }
            }
            if (dir.Contains("logic") && rate.Contains("VC12") && info.Contains("stm"))
            {
                if (!string.IsNullOrEmpty(port))
                {
                    string[] portts = port.Split(new char[] { '/' });
                    string Slot = portts[0];
                    string Port = portts[1];
                    string HP = portts[2];
                    string LP = portts[3];
                    Telnet.SendData("config otn");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("int ftp " + Slot + "/" + Port);
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("sdh ctp del " + rate + " " + HP + " " + LP);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    dxc = "\r\n" + "FTP-CTP-VC12:\t" + dir + "-" + rate + "-" + port + "已删除" + "\r\n";
                }

               
            }
            if (dir.Contains("client") && rate.Contains("VC3") && info.Contains("stm"))
            {
                if (!string.IsNullOrEmpty(port))
                {
                    string[] portts = port.Split(new char[] { '/' });
                    string Slot = portts[0];
                    string Port = portts[1];
                    string HP = portts[2];
                    string LP = portts[3];
                    Telnet.SendData("config otn");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("int ptp " + Slot + "/" + Port);
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("sdh ctp del " + rate + " " + HP + " " + LP);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    dxc = "\r\n" + "PTP-CTP-VC3:\t" + dir + "-" + rate + "-" + port + "已删除" + "\r\n";
                }
            }
            if (dir.Contains("logic") && rate.Contains("VC3") && info.Contains("stm"))
            {
                if (!string.IsNullOrEmpty(port))
                {
                    string[] portts = port.Split(new char[] { '/' });
                    string Slot = portts[0];
                    string Port = portts[1];
                    string HP = portts[2];
                    string LP = portts[3];
                    Telnet.SendData("config otn");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("int ftp " + Slot + "/" + Port);
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("sdh ctp del " + rate + " " + HP + " " + LP);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    dxc = "\r\n" + "FTP-CTP-VC3:\t" + dir + "-" + rate + "-" + port + "已删除" + "\r\n";
                }
            }

            if (dir.Contains("client") && rate.Contains("VC4") && info.Contains("vcg"))
            {
                if (!string.IsNullOrEmpty(port))
                {
                    string[] portts = port.Split(new char[] { '/' });
                    string Slot = portts[0];
                    string Port = portts[1];
                    string TS = portts[2];
                    Telnet.SendData("config otn");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("int ptp " + Slot + "/" + Port);
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("undo grouping " + TS);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    dxc = "\r\n" + "PTP-CTP-VC4-VCG:\t" + dir + "-" + rate + "-" + port + "已删除" + "\r\n";
                }
            }
            if (dir.Contains("logic") && rate.Contains("VC4") && info.Contains("vcg"))
            {
                if (!string.IsNullOrEmpty(port))
                {
                    string[] portts = port.Split(new char[] { '/' });
                    string Slot = portts[0];
                    string Port = portts[1];
                    string TS = portts[2];
                    Telnet.SendData("config otn");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("int ftp " + Slot + "/" + Port);
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("undo grouping "+  TS );
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    dxc = "\r\n" + "FTP-CTP-VC4-VCG:\t" + dir + "-" + rate + "-" + port + "已删除" + "\r\n";
                }
            }

            if (dir.Contains("client") && rate.Contains("VC12") && info.Contains("vcg"))
            {
                if (!string.IsNullOrEmpty(port))
                {
                    string[] portts = port.Split(new char[] { '/' });
                    string Slot = portts[0];
                    string Port = portts[1];
                    string HP = portts[2];
                    string LP = portts[3];
                    Telnet.SendData("config otn");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("int ptp " + Slot + "/" + Port);
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("undo grouping " + LP);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    dxc = "\r\n" + "PTP-CTP-VC12-VCG:\t" + dir + "-" + rate + "-" + port + "已删除" + "\r\n";
                }
            }
            if (dir.Contains("logic") && rate.Contains("VC12") && info.Contains("vcg"))
            {
                if (!string.IsNullOrEmpty(port))
                {
                    string[] portts = port.Split(new char[] { '/' });
                    string Slot = portts[0];
                    string Port = portts[1];
                    string HP = portts[2];
                    string LP = portts[3];
                    Telnet.SendData("config otn");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("int ftp " + Slot + "/" + Port);
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("undo grouping " + LP);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    dxc = "\r\n" + "FTP-CTP-VC12-VCG:\t" + dir + "-" + rate + "-" + port + "已删除" + "\r\n";
                }


            }
            if (dir.Contains("client") && rate.Contains("VC3") && info.Contains("vcg"))
            {
                if (!string.IsNullOrEmpty(port))
                {
                    string[] portts = port.Split(new char[] { '/' });
                    string Slot = portts[0];
                    string Port = portts[1];
                    string HP = portts[2];
                    string LP = portts[3];
                    Telnet.SendData("config otn");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("int ptp " + Slot + "/" + Port);
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("undo grouping " + LP);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    dxc = "\r\n" + "PTP-CTP-VC3-VCG:\t" + dir + "-" + rate + "-" + port + "已删除" + "\r\n";
                }
            }
            if (dir.Contains("logic") && rate.Contains("VC3") && info.Contains("vcg"))
            {
                if (!string.IsNullOrEmpty(port))
                {
                    string[] portts = port.Split(new char[] { '/' });
                    string Slot = portts[0];
                    string Port = portts[1];
                    string HP = portts[2];
                    string LP = portts[3];
                    Telnet.SendData("config otn");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("int ftp " + Slot + "/" + Port);
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("undo grouping " + LP);
                    Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    //Thread.Sleep(XHTime / 3);
                    Telnet.SendData("exit");
                    dxc = "\r\n" + "FTP-CTP-VC3-VCG:\t" + dir + "-" + rate + "-" + port + "已删除" + "\r\n";
                }
            }
            return dxc;
        }
            public static string Get_CTP()
            {
                string dxc = "";
                Telnet.SendData("config otn");
                string str = Telnet.ReceiveData(10 * ts);
                Telnet.SendData("show ctp");
                for (int i = 0; i < count; i++)
                {
                    Thread.Sleep(XHTime / 10);
                    str = Telnet.ReceiveData(10 * ts);
                    if (str != "")
                    {
                        dxc += str;

                    }
                    else
                    {
                        break;
                    }
                }
                Telnet.SendData("exit");
                return dxc;
            }
            static bool IsIP(string ip)
            {
                //如果为空，认为验证合格
                if (string.IsNullOrEmpty(ip))
                {
                    return true;
                }
                //清除要验证字符串中的空格
                ip = ip.Trim();
                //模式字符串
                string pattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";
                //验证
                return Regex.IsMatch(ip, pattern);
            }
            public static string Connect(string ip, string username, string password)
            {
                string Connectionstr = "";
                try
                {
                    if (!IsIP(ip))
                    {
                        Connectionstr += "您输入了非法IP地址，请修改后再次尝试！";
                        return Connectionstr;
                    }
                    int XHCount = 720;
                    Ping ping = new Ping();
                    int timeout = 120;
                    PingReply pingReply = ping.Send(ip, timeout);
                    bool link = false;
                    //判断请求是否超时
                    for (int but = 0; but < 5; but++)
                    {
                        pingReply = ping.Send(ip, timeout);
                        if (pingReply.Status == IPStatus.Success)
                        {
                            link = true;
                            break;
                        }
                        Connectionstr += "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "设备无法ping通剩余：" + (5 - but).ToString() + "次，请检查IP地址：" + ip + "  设备是否正常！";
                        Thread.Sleep(XHTime);
                    }
                    if (link == false)
                    {
                        return Connectionstr;
                    }
                    Connectionstr += "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "设备可以ping通，正在尝试Telnet登录，请稍等...";
                    if (Telnet.Connect(ip, "23"))
                    {
                        Telnet.SendData(username);
                        for (int a = 0; a <= XHCount; a++)
                        {
                            string login = Telnet.ReceiveData(ts);
                            //Connectionstr +=login);
                            if (login.Contains("Password:"))
                            {
                                Telnet.SendData(password);
                                break;
                            }
                            if (login.Contains("Key"))
                            {
                                Connectionstr += "非我司设备，请更换IP重启登录！";
                                return Connectionstr;
                            }
                            if (login.Contains("Username or password is invalid"))
                            {
                                Connectionstr += "非我司设备，请更换IP重启登录！";
                                return Connectionstr;
                            }
                            Thread.Sleep(XHTime / 3);
                        }
                        for (int c = 0; c <= XHCount; c++)
                        {
                            string passd = Telnet.ReceiveData(ts);
                            //MessageBox.Show(passd);
                            if (passd.Contains("Error") || passd.Contains("failed") || passd.Contains("Bad passwords") || passd.Contains("Key"))
                            {
                                Connectionstr += "用户名或密码错误，请断开重新尝试！";
                                //Connectionstr +="\r\n" + "用户名或密码错误，请断开重新尝试！";
                                return Connectionstr;
                            }
                            if (passd.Contains("Password:"))
                            {
                                Telnet.SendData(password);
                            }
                            Thread.Sleep(XHTime / 3);
                            if (passd.Contains(">"))
                            {
                                Connectionstr += "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "用户名密码正确==========================================OK";
                                Telnet.SendData("enable");
                                for (int b = 0; b <= XHCount; b++)
                                {
                                    string pass = Telnet.ReceiveData(ts);
                                    if (pass.Contains("Pas"))
                                    {
                                        Telnet.SendData(password);
                                        //Thread.Sleep(XHTime);
                                        for (int d = 0; d <= 1000; d++)
                                        {
                                            string locked = Telnet.ReceiveData(ts);
                                            if (locked.Contains("configuration is locked by other user"))
                                            //configuration is locked by other user
                                            {
                                                Connectionstr += "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "已经有用户登录，正在重新登录============================OK";
                                                Telnet.SendData("grosadvdebug");
                                                Thread.Sleep(XHTime);
                                                Telnet.SendData("vty user limit no");
                                                Thread.Sleep(XHTime);
                                                Telnet.SendData("exit");
                                                Thread.Sleep(XHTime);
                                                Telnet.SendData("enable");
                                                Thread.Sleep(XHTime);
                                                if (Telnet.ReceiveData(ts).Contains("Pas"))
                                                {
                                                    Telnet.SendData(password);
                                                    Thread.Sleep(XHTime);
                                                    if (!Telnet.ReceiveData(ts).Contains("failed"))
                                                    {
                                                        Connectionstr += "用户名或密码错误，请断开重新尝试！";
                                                        return Connectionstr;
                                                    }
                                                    break;
                                                }
                                            }
                                            if (locked.Contains("#"))
                                            {
                                                break;
                                            }
                                            Thread.Sleep(XHTime / 3);
                                        }
                                        break;
                                    }
                                    if (pass.Contains("#"))
                                    {
                                        break;
                                    }
                                    if (pass.Contains("configuration is locked by other user"))
                                    //configuration is locked by other user
                                    {
                                        Connectionstr += "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "已经有用户登录，正在重新登录============================OK";
                                        Telnet.SendData("grosadvdebug");
                                        Thread.Sleep(XHTime);
                                        Telnet.SendData("vty user limit no");
                                        Thread.Sleep(XHTime);
                                        Telnet.SendData("exit");
                                        Thread.Sleep(XHTime);
                                        Telnet.SendData("enable");
                                        Thread.Sleep(XHTime);
                                        if (Telnet.ReceiveData(ts).Contains("Pas"))
                                        {
                                            Telnet.SendData(password);
                                            Thread.Sleep(XHTime);
                                            if (!Telnet.ReceiveData(ts).Contains("failed"))
                                            {
                                                Connectionstr += "用户名或密码错误，请断开重新尝试！";
                                                return Connectionstr;
                                            }
                                            break;
                                        }
                                        break;
                                    }
                                    Thread.Sleep(XHTime / 3);
                                }
                                break;
                            }
                            Thread.Sleep(XHTime / 3);
                        }
                        Telnet.SendData("screen idle-timeout 0");
                        Telnet.SendData("screen lines 0");
                        //  Connectionstr += "已连接";
                        // Telnet.SendData("service snmp source-ip auto";
                        Thread.Sleep(XHTime);
                        string slot = Telnet.ReceiveData(ts);


                        Connectionstr += "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "登录成功可以使用========================================OK" + "\r\n";
                    }
                    else
                    {
                        Connectionstr += "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "无法Telnet登录，请检查设备是否正常！";
                    }
                }
                catch (Exception ex)
                {
                    Connectionstr += ex.Message;
                    return Connectionstr;
                }
                return Connectionstr;

            }


            /// <summary>
            /// 将字符串转换成二维数组
            /// </summary>
            /// <param name="original"></param>
            /// <returns></returns>
            public static string[,] StringToArray(string original)
            {
                if (original.Length == 0)
                {

                    throw new IndexOutOfRangeException("二维数组导入为空");
                }
                //将字符串转换成数组（字符串拼接格式：***,***#***,***#***,***，例如apple,banana#cat,dog#red,black）
                string[] originalRow = original.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string[] originalColstart = null;
                int[] originalColstartcount = new int[originalRow.Length];

                for (int m = 0; m < originalRow.Length; m++)
                {
                    originalColstart = Regex.Split(originalRow[m], "\\s+", RegexOptions.IgnoreCase);
                    //  MessageBox.Show(originalColstart.Length.ToString());
                    if (originalColstart != null)
                    {
                        originalColstartcount[m] = originalColstart.Length;

                    }
                }
                ArrayList list = new ArrayList(originalColstartcount);
                list.Sort();
                int min = Convert.ToInt32(list[0]);
                int max = Convert.ToInt32(list[list.Count - 1]);
                //  MessageBox.Show(max.ToString());

                string[] originalCol = new string[max]; //string[,]是等长数组，列维度一样，只要取任意一行的列维度即可确定整个二维数组的列维度
                int x = originalRow.Length;
                int y = max;
                string[,] twoArray = new string[x, y];
                for (int i = 0; i < x; i++)
                {
                    originalCol = Regex.Split(originalRow[i], "\\s+", RegexOptions.IgnoreCase);
                    for (int j = 0; j < originalCol.Length; j++)
                    {
                        twoArray[i, j] = originalCol[j];
                    }
                }
                return twoArray;
            }
        }
    }
