
using System;
using System.Xml;
namespace ToolsX
{
    class XmlConfig
    {
        /// <summary>
        /// 创建XML文档
        /// </summary>
        /// <param name="name">根节点名称</param>
        /// <param name="type">根节点的一个属性值</param>
        /// <returns>XmlDocument对象</returns> 
        public static XmlDocument CreateXmlDocument(string name, string type)
        {
            XmlDocument doc;
            try
            {
                doc = new XmlDocument();
                doc.LoadXml("<" + name + "/>");
                var rootEle = doc.DocumentElement;
                rootEle.SetAttribute("type", type);
            }
            catch (Exception er)
            {
                throw new Exception(er.ToString());
            }
            return doc;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        /// <returns>string</returns>
        public static string Read(string path, string node, string attribute)
        {
            var value = "";
            try
            {
                var doc = new XmlDocument();
                doc.Load(path);
                var xn = doc.SelectSingleNode(node);
                if (xn != null && xn.Attributes != null)
                value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
            }
            catch (Exception er)
            {
                throw new Exception(er.ToString());
            }
            return value;
        }

	    /// <summary>
	    /// 插入数据
	    /// </summary>
	    /// <param name="path">路径</param>
	    /// <param name="node">节点</param>
	    /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
	    /// <param name="attribute">属性名，非空时插入该元素属性值，否则插入元素值</param>
	    /// <param name="value">值</param>
	    /// <returns></returns>
	    public static void Insert(string path, string node, string element, string attribute, string value)
	    {
		    try
		    {
			    var doc = new XmlDocument();
			    doc.Load(path);
			    var xn = doc.SelectSingleNode(node);
			    if (element.Equals(""))
			    {
				    if (!attribute.Equals(""))
				    {
					    var xe = (XmlElement)xn;
					    xe.SetAttribute(attribute, value);
				    }
			    }
			    else
			    {
				    var xe = doc.CreateElement(element);
				    if (attribute.Equals(""))
				    xe.InnerText = value;
				    else
				    xe.SetAttribute(attribute, value);
				    xn.AppendChild(xe);
			    }
			    doc.Save(path);
		    }
		    catch (Exception er)
		    {
			    throw new Exception(er.ToString());
		    }
	    }

	    /// <summary>
	    /// 修改数据
	    /// </summary>
	    /// <param name="path">路径</param>
	    /// <param name="node">节点</param>
	    /// <param name="attribute">属性名，非空时修改该节点属性值，否则修改节点值</param>
	    /// <param name="value">值</param>
	    /// <returns></returns>
	    public static void Update(string path, string node, string attribute, string value)
	    {
		    try
		    {
			    var doc = new XmlDocument();
			    doc.Load(path);
			    var xn = doc.SelectSingleNode(node);
			    var xe = (XmlElement)xn;
			    if (attribute.Equals(""))
			    {
				    if (xe != null) xe.InnerText = value;
			    }
			    else
			    {
				    xe.SetAttribute(attribute, value);
			    }
			    doc.Save(path);
		    }
		    catch (Exception er)
		    {
			    throw new Exception(er.ToString());
		    }
	    }

	    /// <summary>
	    /// 删除数据
	    /// </summary>
	    /// <param name="path">路径</param>
	    /// <param name="node">节点</param>
	    /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
	    /// <returns></returns>
	    public static void Delete(string path, string node, string attribute)
	    {
		    try
		    {
			    var doc = new XmlDocument();
			    doc.Load(path);
			    var xn = doc.SelectSingleNode(node);
			    var xe = (XmlElement)xn;
			    if (attribute.Equals(""))
			    {
				    xn.ParentNode.RemoveChild(xn);
			    }
			    else
			    {
				    xe.RemoveAttribute(attribute);
			    }
			    doc.Save(path);
		    }
		    catch (Exception er)
		    {
			    throw new Exception(er.ToString());
		    }
	    }

	    /// <summary>
	    /// 获得xml文件中指定节点的节点数据
	    /// </summary>
	    /// <param name="path"></param>
	    /// <param name="nodeName"></param>
	    /// <returns></returns>
	    public static string GetNodeInfoByNodeName(string path, string nodeName)
	    {
		    var xmlString = string.Empty;
		    try
		    {
			    var xml = new XmlDocument();
			    xml.Load(path);
			    var root = xml.DocumentElement;
			    if (root == null) return xmlString;
			    var node = root.SelectSingleNode("//" + nodeName);
			    if (node != null)
			    {
				    xmlString = node.InnerText;
			    }
		    }
		    catch (Exception er)
		    {
			    throw new Exception(er.ToString());
		    }
		    return xmlString;
	    }

	    /// <summary>
	    /// 功能:读取指定节点的指定属性值 
	    /// </summary>
	    /// <param name="path"></param>
	    /// <param name="strNode">节点名称</param>
	    /// <param name="strAttribute">此节点的属性</param>
	    /// <returns></returns>
	    public string GetXmlNodeAttributeValue(string path, string strNode, string strAttribute)
	    {
		    var strReturn = "";
		    try
		    {
			    var xml = new XmlDocument();
			    xml.Load(path);
			    //根据指定路径获取节点
			    var xmlNode = xml.SelectSingleNode(strNode);
			    if (xmlNode != null)
			    {
				    //获取节点的属性，并循环取出需要的属性值
				    var xmlAttr = xmlNode.Attributes;
				    if (xmlAttr == null) return strReturn;
				    for (var i = 0; i < xmlAttr.Count; i++)
				    {
					    if (xmlAttr.Item(i).Name != strAttribute) continue;
					    strReturn = xmlAttr.Item(i).Value;
					    break;
				    }
			    }
		    }
		    catch (XmlException xmle)
		    {
			    throw new Exception(xmle.Message);
		    }
		    return strReturn;
	    }

	    /// <summary>
	    /// 更新XML文件中的指定节点内容
	    /// </summary>
	    /// <param name="filePath">文件路径</param>
	    /// <param name="nodeName">节点名称</param>
	    /// <param name="nodeValue">更新内容</param>
	    /// <returns>更新是否成功</returns>
	    public static bool UpdateNode(string filePath, string nodeName, string nodeValue)
	    {  
		    try
		    {
			    bool flag;
			    var xd = new XmlDocument();
			    xd.Load(filePath);
			    var xe = xd.DocumentElement;
			    if (xe == null) return false;
			    var xn = xe.SelectSingleNode("//" + nodeName);
			    if (xn != null)
			    {
				    xn.InnerText = nodeValue;
				    flag = true;
			    }
			    else
			    {
				    flag = false;
			    }
			    return flag;
		    }
		    catch (Exception ex)
		    {
			    throw new Exception(ex.Message);
		    }
	    }
    }
}
