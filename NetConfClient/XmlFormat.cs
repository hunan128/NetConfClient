using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace NetConfClientSoftware
{
    class XmlFormat
    {
        public static string Xml(string xml)
        {
            var stringBuilder = new StringBuilder();

            try
            {

                var element = XElement.Parse(xml);

                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "   ",
                    NewLineChars = "\r\n",
                    NewLineHandling = System.Xml.NewLineHandling.Replace,
                    OmitXmlDeclaration = true,
                    ConformanceLevel = System.Xml.ConformanceLevel.Document
                };
                using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
                {
                    element.Save(xmlWriter);
                }
            }
            catch {

                stringBuilder.Append (xml);
            }

            return stringBuilder.ToString();

        }
    }
}
