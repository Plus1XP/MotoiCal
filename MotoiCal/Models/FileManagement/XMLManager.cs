using System;
using System.Xml;

namespace MotoiCal.Models.FileManagement
{
    public class XMLManager : FileManager
    {
        public XMLManager()
        {

        }

        public string GetNodeAttributeValueAsString(string xmlFileLoaction, string nodePath, string nodeName, string nodeValue, string attributeName)
        {
            XmlNodeList nodeList = this.LoadNodeList(this.LoadXmlDocument(xmlFileLoaction), nodePath);
            XmlAttribute elementAttribute = this.GetAttributeFromNodeList(nodeList, nodeName, nodeValue, attributeName);

            return this.GetAttributeValueAsString(elementAttribute);
        }

        public bool GetNodeAttributeValueAsBool(string xmlFileLoaction, string nodePath, string nodeName, string nodeValue, string attributeName)
        {
            XmlNodeList nodeList = this.LoadNodeList(this.LoadXmlDocument(xmlFileLoaction), nodePath);
            XmlAttribute elementAttribute = this.GetAttributeFromNodeList(nodeList, nodeName, nodeValue, attributeName);

            return this.GetAttributeValueAsBool(elementAttribute);
        }

        public int? GetNodeAttributeValueAsInt(string xmlFileLoaction, string nodePath, string nodeName, string nodeValue, string attributeName)
        {
            XmlNodeList nodeList = this.LoadNodeList(this.LoadXmlDocument(xmlFileLoaction), nodePath);
            XmlAttribute elementAttribute = this.GetAttributeFromNodeList(nodeList, nodeName, nodeValue, attributeName);

            return this.GetAttributeValueAsInt(elementAttribute);
        }

        public void SetNodeAttributeValueFromString(string xmlFileLocation, string nodePath, string nodeName, string nodeValue, string attributeName, string attributeValue)
        {
            XmlDocument xmlDoc = this.LoadXmlDocument(xmlFileLocation);
            XmlNodeList nodeList = this.LoadNodeList(xmlDoc, nodePath);
            XmlNode node = this.GetNodeFromNodeList(nodeList, nodeName, nodeValue);
            this.SetNodeAttributeFromString(node, attributeName, attributeValue);
            this.SaveXmlDocument(xmlDoc, xmlFileLocation);
        }

        public void SetNodeAttributeValueFromBool(string xmlFileLocation, string nodePath, string nodeName, string nodeValue, string attributeName, bool attributeValue)
        {
            XmlDocument xmlDoc = this.LoadXmlDocument(xmlFileLocation);
            XmlNodeList nodeList = this.LoadNodeList(xmlDoc, nodePath);
            XmlNode node = this.GetNodeFromNodeList(nodeList, nodeName, nodeValue);
            this.SetNodeAttributeFromBool(node, attributeName, attributeValue);
            this.SaveXmlDocument(xmlDoc, xmlFileLocation);
        }

        public void SetNodeAttributeValueFromInt(string xmlFileLocation, string nodePath, string nodeName, string nodeValue, string attributeName, int attributeValue)
        {
            XmlDocument xmlDoc = this.LoadXmlDocument(xmlFileLocation);
            XmlNodeList nodeList = this.LoadNodeList(xmlDoc, nodePath);
            XmlNode node = this.GetNodeFromNodeList(nodeList, nodeName, nodeValue);
            this.SetNodeAttributeFromInt(node, attributeName, attributeValue);
            this.SaveXmlDocument(xmlDoc, xmlFileLocation);
        }

        public XmlDocument LoadXmlDocument(string xmlFileLoaction)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(this.ReadFromFile(xmlFileLoaction));
                return xmlDoc;
            }
            catch (XmlException)
            {
                // If file contains unreadable XML then return a new empty XmlDoc
                return new XmlDocument();
            }
        }

        public XmlNodeList LoadNodeList(XmlDocument xmlDoc, string nodePath)
        {
            XmlNodeList nodeList;
            nodeList = xmlDoc.SelectNodes(nodePath);

            return nodeList;
        }

        public void SaveXmlDocument(XmlDocument xmlDoc, string xmlFileLocation)
        {
            xmlDoc.Save(xmlFileLocation);
        }

        public XmlNode GetNodeFromNodeList(XmlNodeList nodeList, string nodeName, string nodeValue)
        {
            XmlNode element = null;

            foreach (XmlNode node in nodeList)
            {
                if (this.GetNodeAttributeValueAsString(node, nodeName).Equals(nodeValue))
                {
                    element = node;
                }
            }

            return element;
        }

        public XmlAttribute GetAttributeFromNodeList(XmlNodeList nodeList, string nodeName, string nodeValue, string attributeName)
        {
            XmlAttribute elementAttribute = null;

            foreach (XmlNode node in nodeList)
            {
                if (this.GetNodeAttributeValueAsString(node, nodeName).Equals(nodeValue))
                {
                    elementAttribute = this.GetNodeAttribute(node, attributeName);
                }
            }

            return elementAttribute;
        }

        public string GetAttributeValueAsString(XmlAttribute attribute)
        {
            return attribute.Value;
        }

        // Add default value incase int can not be determined from XML.
        public int? GetAttributeValueAsInt(XmlAttribute attribute, int? defaultValue = null)
        {
            if (attribute == null)
            {
                return defaultValue;
            }

            return Convert.ToInt32(attribute.Value);
        }

        public bool GetAttributeValueAsBool(XmlAttribute attribute)
        {
            if (attribute == null)
            {
                return true;
            }

            return Convert.ToBoolean(attribute.Value);
        }

        private string GetNodeAttributeValueAsString(XmlNode node, string attributeName)
        {
            return this.GetNodeAttribute(node, attributeName).Value;
        }

        private int GetNodeAttributeValueAsInt(XmlNode node, string attributeName)
        {
            return Convert.ToInt32(this.GetNodeAttribute(node, attributeName).Value);
        }

        public bool GetNodeAttributeValueAsBool(XmlNode node, string attributeName)
        {
            return Convert.ToBoolean(this.GetNodeAttribute(node, attributeName).Value);
        }

        public XmlAttribute GetNodeAttribute(XmlNode node, string attributeName)
        {
            return node.Attributes?[attributeName];
        }

        private void SetNodeAttributeFromString(XmlNode node, string attributeName, string attributeValue)
        {
            this.SetNodeAttribute(node, attributeName, attributeValue);
        }

        private void SetNodeAttributeFromInt(XmlNode node, string attributeName, int attributeValue)
        {
            this.SetNodeAttribute(node, attributeName, Convert.ToString(attributeValue));
        }

        public void SetNodeAttributeFromBool(XmlNode node, string attributeName, bool attributeValue)
        {
            this.SetNodeAttribute(node, attributeName, Convert.ToString(attributeValue));
        }

        private void SetNodeAttribute(XmlNode node, string attributeName, string attributeValue)
        {
            if (node != null)
            {
                node.Attributes[attributeName].Value = attributeValue;
            }
        }
    }
}
