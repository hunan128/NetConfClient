﻿using System;
using System.Collections;
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
        public static bool Element_Value(string  xml,string _element,string _vaule,string ISP)
        {

            bool _Element_Value = false;
            _element_bool = false;
            _value_bool = false;
            try
            {
                XmlDocument dom = new XmlDocument();
                dom.LoadXml(xml);
                foreach (XmlNode node in dom.ChildNodes)
                {
                    if (node.Name == "namespace" && node.ChildNodes.Count == 0 && string.IsNullOrEmpty(GetAttributeText(node, "name")))
                    {
                        continue;
                    }
                    AddNode(node,_element,_vaule,ISP);
                    if (!string.IsNullOrEmpty(_vaule)) {
                        if (_element_bool == true && _value_bool == true) { _Element_Value = true; }
                    }
                    if (string.IsNullOrEmpty(_vaule))
                    {
                        if (_element_bool == true && _value_bool == true) { _Element_Value = true; }
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

        public static void AddNode(XmlNode inXmlNode,string _element,string _value,string ISP)
        {

            if (inXmlNode.HasChildNodes)
            {
                string text = GetAttributeText(inXmlNode, "name");
                if (string.IsNullOrEmpty(text))
                    text = inXmlNode.Name;
                string nc = "";
                for (int i = 0; i < inXmlNode.Attributes.Count; i++)
                {
                     nc =  inXmlNode.Attributes[i].OuterXml;

                }
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
                            if (text == _element) { _element_bool = true; _value_bool = true; }
                            if (nc == _element) { _element_bool = true; _value_bool = true; }

                        }
                    }
                    else
                    {
                        string value = GetAttributeText(xNode, "name");
                        if (string.IsNullOrEmpty(value))
                        {
                            value = xNode.Value;
                            if (string.IsNullOrEmpty(value))
                            {
                                value = xNode.Name;
                                if (text == _element) { _element_bool = true; _value_bool = true; }
                                if (nc == _element) { _element_bool = true; _value_bool = true; }
                                if (value == _element) { _element_bool = true; _value_bool = true; }
                            }
                            else
                            {
                                if (nc == _element) { _element_bool = true; _value_bool = true; }
                                if (inXmlNode.Name == _element)
                                {

                                    _element_bool = true;
                                    _value_bool = true;
                                    //if (value == _value) { _value_bool = true; }
                                    if (ISP.Contains("联通"))
                                    {
                                        if (CUCC_Array.Count != 0)
                                        {
                                            for (int g = 0; g < CUCC_Array.Count; g++)
                                            {
                                                if (CUCC_Array[g][0] == _element)
                                                {
                                                    if (CUCC_Array[g][2].Contains(value))
                                                    {
                                                        _value_bool = false;
                                                        string[] value_ = CUCC_Array[g][2].Split(',');
                                                        for (int l = 0; l < value_.Length; l++)
                                                        {
                                                            if (value_[l] == value) {
                                                                _value_bool = true;
                                                                break;
                                                            }
                                                        }
                                                        if (_value_bool) {
                                                            break;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        if (_element == "status")
                                                        {
                                                            for (int h = 0; h < CUCC_Array.Count; h++)
                                                            {
                                                                if (CUCC_Array[h][0] == "me-status")
                                                                {
                                                                    if (CUCC_Array[h][2].Contains(value))
                                                                    {
                                                                        _value_bool = false;
                                                                        string[] value_ = CUCC_Array[h][2].Split(',');
                                                                        for (int l = 0; l < value_.Length; l++)
                                                                        {
                                                                            if (value_[l] == value)
                                                                            {
                                                                                _value_bool = true;
                                                                                break;
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        _value_bool = false;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                        }

                                                        _value_bool = false;
                                                        break;
                                                    }
                                                }
                                              //  _value_bool = true;
                                            }


                                        }
                                        else
                                        {
                                            _value_bool = true;
                                        }
                                    }
                                    if (ISP.Contains("电信"))
                                    {
                                        if (CTCC_Array.Count != 0)
                                        {
                                            for (int g = 0; g < CTCC_Array.Count; g++)
                                            {
                                                if (CTCC_Array[g][0] == _element)
                                                {
                                                    if (CTCC_Array[g][2].Contains(value))
                                                    {
                                                        _value_bool = false;
                                                        string[] value_ = CTCC_Array[g][2].Split(',');
                                                        for (int l = 0; l < value_.Length; l++)
                                                        {
                                                            if (value_[l] == value)
                                                            {
                                                                _value_bool = true;
                                                                break;
                                                            }
                                                        }
                                                        if (_value_bool)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        _value_bool = false;
                                                        break;
                                                    }
                                                }
                                             //   _value_bool = true;
                                            }


                                        }
                                        else
                                        {
                                            _value_bool = true;
                                        }
                                    }
                                    if (ISP.Contains("移动"))
                                    {
                                        if (CMCC_Array.Count != 0)
                                        {
                                            for (int g = 0; g < CMCC_Array.Count; g++)
                                            {
                                                if (CMCC_Array[g][0] == _element)
                                                {
                                                    if (CMCC_Array[g][2].Contains(value))
                                                    {
                                                        _value_bool = false;
                                                        string[] value_ = CMCC_Array[g][2].Split(',');
                                                        for (int l = 0; l < value_.Length; l++)
                                                        {
                                                            if (value_[l] == value)
                                                            {
                                                                _value_bool = true;
                                                                break;
                                                            }
                                                        }
                                                        if (_value_bool)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        _value_bool = false;
                                                        break;
                                                    }
                                                }
                                             //   _value_bool = true;
                                            }


                                        }
                                        else
                                        {
                                            _value_bool = true;
                                        }
                                    }

                                }
                            }
                        }




                        //string value = GetAttributeText(xNode, "name");
                        //if (string.IsNullOrEmpty(value))
                        //{
                        //    value = (xNode.OuterXml).Trim();
                        //}
                        //if (nc == _element) { _element_bool = true; _value_bool = true; }
                        //if (inXmlNode.Name == _element) {
                           
                        //    _element_bool = true;
                        //    //if (value == _value) { _value_bool = true; }
                        //    if (ISP.Contains("联通")) {
                        //        if (CUCC_Array.Count != 0)
                        //        {
                        //            for (int g = 0; g < CUCC_Array.Count; g++)
                        //            {
                        //                if (CUCC_Array[g][0] == _element)
                        //                {
                        //                    if (CUCC_Array[g][2].Contains(value))
                        //                    {
                        //                        _value_bool = true;
                        //                        break;
                        //                    }
                        //                    else
                        //                    {
                        //                        if (_element == "status") {
                        //                            for (int h = 0; h < CUCC_Array.Count; h++)
                        //                            {
                        //                                if (CUCC_Array[h][0] == "me-status")
                        //                                {
                        //                                    if (CUCC_Array[h][2].Contains(value))
                        //                                    {
                        //                                        _value_bool = true;
                        //                                        break;
                        //                                    }
                        //                                    else {
                        //                                        _value_bool = false;
                        //                                        break;
                        //                                    }
                        //                                }
                        //                            }
                        //                            break;
                        //                        }

                        //                        _value_bool = false;
                        //                        break;
                        //                    }
                        //                }
                        //                _value_bool = true;
                        //            }


                        //        }
                        //        else
                        //        {
                        //            _value_bool = true;
                        //        }
                        //    }
                        //    if (ISP.Contains("电信"))
                        //    {
                        //        if (CTCC_Array.Count != 0)
                        //        {
                        //            for (int g = 0; g < CTCC_Array.Count; g++)
                        //            {
                        //                if (CTCC_Array[g][0] == _element)
                        //                {
                        //                    if (CTCC_Array[g][2].Contains(value))
                        //                    {
                        //                        _value_bool = true;
                        //                        break;
                        //                    }
                        //                    else
                        //                    {
                        //                        _value_bool = false;
                        //                        break;
                        //                    }
                        //                }
                        //                _value_bool = true;
                        //            }


                        //        }
                        //        else
                        //        {
                        //            _value_bool = true;
                        //        }
                        //    }
                        //    if (ISP.Contains("移动"))
                        //    {
                        //        if (CMCC_Array.Count != 0)
                        //        {
                        //            for (int g = 0; g < CMCC_Array.Count; g++)
                        //            {
                        //                if (CMCC_Array[g][0] == _element)
                        //                {
                        //                    if (CMCC_Array[g][2].Contains(value))
                        //                    {
                        //                        _value_bool = true;
                        //                        break;
                        //                    }
                        //                    else
                        //                    {
                        //                        _value_bool = false;
                        //                        break;
                        //                    }
                        //                }
                        //                _value_bool = true;
                        //            }


                        //        }
                        //        else
                        //        {
                        //            _value_bool = true;
                        //        }
                        //    }

                        //}


                    }

                    if (newNode != null)
                    {
                        AddNode(xNode, _element, _value,ISP);

                    }
                }
            }

        }


        static string GetAttributeText(XmlNode inXmlNode, string name)
        {
            XmlAttribute attr = (inXmlNode.Attributes?[name]);
            return attr?.Value;
        }
        public static List<string[]> CUCC_Array = new List<string[]>();
        public static List<string[]> CTCC_Array = new List<string[]>();
        public static List<string[]> CMCC_Array = new List<string[]>();
        public void Element_Value_Find(XmlDocument xmlDoc,string ISP)
        {
           
            try
            {
                string typedefname = "";
                string typename = "";
                string enumname = "";
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:yang:yin:1");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//rpc:module//rpc:typedef", root);
                for (int i = 0; i < itemNodes.Count; i++)
                {
                    ArrayList list = new ArrayList();
                    XmlNode itemNode = itemNodes[i];
                    if (itemNode != null)
                    {
                        if (itemNode.Attributes["name"] != null)
                            typedefname = itemNode.Attributes["name"].Value;
                        if (itemNode.ChildNodes[0].Attributes["name"] != null)
                            typename = itemNode.ChildNodes[0].Attributes["name"].Value;
                        XmlNodeList enum0 = itemNode.ChildNodes[0].ChildNodes;
                        for (int j = 0; j < enum0.Count; j++)
                        {
                            XmlNode itemNode1 = enum0[j];
                            if (itemNode1 != null)
                            {
                                if (itemNode1.Attributes["name"] != null)
                                    list.Add(itemNode1.Attributes["name"].Value);
                            }
                        }
                        enumname = string.Join(",", (string[])list.ToArray(typeof(string)));
                        string[] en = { typedefname, typename, enumname };
                        if (ISP.Contains("联通")) {
                           // CUCC_Array.Clear();
                            CUCC_Array.Add(en);
                        }
                        if (ISP.Contains("移动"))
                        {
                           // CMCC_Array.Clear();
                            CMCC_Array.Add(en);
                        }
                        if (ISP.Contains("电信"))
                        {
                           // CTCC_Array.Clear();
                            CTCC_Array.Add(en);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
               ex.ToString();   //读取该节点的相关信息

            }
         
        }

    }
}
