using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace NetConfClientSoftware
{
    class NetConfXml
    {
        // 获取指定网页的HTML代码 
        public static string GetPageSource(string URL)
        {
            try
            {
                Uri uri = new Uri(URL);

                HttpWebRequest hwReq = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse hwRes = (HttpWebResponse)hwReq.GetResponse();

                hwReq.Method = "Get";

                hwReq.KeepAlive = false;

                StreamReader reader = new StreamReader(hwRes.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8"));

                return reader.ReadToEnd();

            }
            catch (Exception)
            {
                string str = "";
                return str;
            }
        }
        // 提取HTML代码中的网址 
        public static ArrayList GetHyperLinks(string htmlCode)
        {
            ArrayList al = new ArrayList();

            string strRegex = @"(?<=>)([\-\d\.\(\)\（\）\ \w]+)(xml|yin)";

            Regex r = new Regex(strRegex, RegexOptions.IgnoreCase);
            MatchCollection m = r.Matches(htmlCode);

            for (int i = 0; i <= m.Count - 1; i++)
            {
                bool rep = false;
                string strNew = m[i].ToString();

                // 过滤重复的URL 
                foreach (string str in al)
                {
                    if (strNew == str)
                    {
                        rep = true;
                        break;
                    }
                }

                if (!rep) al.Add(strNew);
            }

            al.Sort();

            return al;
        }
        // 把网址写入xml文件 
        public static void WriteToXml(string strURL, ArrayList alHyperLinks,string ISP)
        {
            XmlTextWriter writer = new XmlTextWriter(@"C:\netconf\"+ISP+".xml", Encoding.UTF8);

            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument(false);
            //writer.WriteDocType("HyperLinks", null, "urls.dtd", null);
            writer.WriteComment("提取自" + strURL + "的超链接");
            writer.WriteStartElement("HyperLinks");
            writer.WriteStartElement("HyperLinks", null);
            writer.WriteAttributeString("DateTime", DateTime.Now.ToString());


            foreach (string str in alHyperLinks)
            {
                string title = GetDomain(str);
                string body = str;
                writer.WriteElementString(title, null, body);
            }

            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
        }

        // 获取网址的域名后缀 
        static string GetDomain(string strURL)
        {
            string retVal;

            string strRegex = @"(\.com/|\.net/|\.cn/|\.org/|\.gov/)";

            Regex r = new Regex(strRegex, RegexOptions.IgnoreCase);
            Match m = r.Match(strURL);
            retVal = m.ToString();

            strRegex = @"\.|/$";
            retVal = Regex.Replace(retVal, strRegex, "").ToString();

            if (retVal == "")
                retVal = "other";

            return retVal;
        }
    }
}
