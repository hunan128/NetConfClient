using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;

namespace NetConfClientSoftware
{
    class Element
    {
        public static bool _element_bool = false;
        public static bool _value_bool = false;
        public static bool Element_Value(XmlDocument dom,string _element,string _vaule)
        {
            bool _Element_Value = false;
            _element_bool = false;
            _value_bool = false;
            try
            {

                foreach (XmlNode node in dom.ChildNodes)
                {
                    if (node.Name == "namespace" && node.ChildNodes.Count == 0 && string.IsNullOrEmpty(GetAttributeText(node, "name")))
                    {
                        continue;
                    }
                    AddNode(node,_element,_vaule);
                    if (!string.IsNullOrEmpty(_vaule)) {
                        if (_element_bool == true && _value_bool == true) { _Element_Value = true; }
                    }
                    if (string.IsNullOrEmpty(_vaule))
                    {
                        if (_element_bool == true) { _Element_Value = true; }
                    }

                }

            }
            catch
            {
                _Element_Value = false;
                return _Element_Value;

            }
            return _Element_Value;
        }

        public static void AddNode(XmlNode inXmlNode,string _element,string _value)
        {


            if (inXmlNode.HasChildNodes)
            {
                string text = GetAttributeText(inXmlNode, "name");
                if (string.IsNullOrEmpty(text))
                    text = inXmlNode.Name;
                string newNode = null;
                XmlNodeList nodeList = inXmlNode.ChildNodes;
                for (int i = 0; i <= nodeList.Count - 1; i++)
                {
                    XmlNode xNode = inXmlNode.ChildNodes[i];
                    if (xNode.HasChildNodes)
                    {
                        if (newNode == null)
                        {
                            newNode = text;
                            if (text == _element) { _element_bool = true; }
                        }
                    }
                    else
                    {
                        string value = GetAttributeText(xNode, "name");
                        if (string.IsNullOrEmpty(value))
                            value = (xNode.OuterXml).Trim();
                        if (inXmlNode.Name == _element) { _element_bool = true; }
                        if (value == _value) { _value_bool = true; }

                    }

                    if (newNode != null)
                    {
                        AddNode(xNode, _element, _value);

                    }
                }
            }

        }

        static string GetAttributeText(XmlNode inXmlNode, string name)
        {
            XmlAttribute attr = (inXmlNode.Attributes?[name]);
            return attr?.Value;
        }

    }
}
