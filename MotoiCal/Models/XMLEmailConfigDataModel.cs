using MotoiCal.Models.FileManagement;

using System.Xml;

namespace MotoiCal.Models
{
    class XMLEmailConfigDataModel
    {
        private const string emailConfig_Data_Location = ".\\EmailConfig.xml";
        private const string config_Location = "/Settings/Email/Config";

        private const int defaultSmtpPortValue = 587;

        private XMLManager xmlManager;
        private EncryptionManager encryptionManager;

        public XMLEmailConfigDataModel()
        {
            this.xmlManager = new XMLManager();
            this.encryptionManager = new EncryptionManager();

            if (!this.xmlManager.IsFileCreated(emailConfig_Data_Location))
            {
                this.xmlManager.CreateFile(emailConfig_Data_Location);
                this.CreateXMLSettingsDocument(emailConfig_Data_Location);
            }
        }

        public string GetTextBoxValue(string eventName)
        {
            return this.xmlManager.GetNodeAttributeValueAsString(emailConfig_Data_Location, config_Location, "Name", eventName, "Saved");
        }

        public void SetTextBoxValue(string eventName, string eventValue)
        {
            this.xmlManager.SetNodeAttributeValueFromString(emailConfig_Data_Location, config_Location, "Name", eventName, "Saved", eventValue);
        }

        public int GetTextBoxValueAsInt(string eventName)
        {
            return this.xmlManager.GetNodeAttributeValueAsInt(emailConfig_Data_Location, config_Location, "Name", eventName, "Saved") ?? defaultSmtpPortValue;
        }

        public void SetTextBoxValueAsInt(string eventName, int eventValue)
        {
            this.xmlManager.SetNodeAttributeValueFromInt(emailConfig_Data_Location, config_Location, "Name", eventName, "Saved", eventValue);
        }

        public bool GetToggleValueAsBool(string eventName)
        {
            return this.xmlManager.GetNodeAttributeValueAsBool(emailConfig_Data_Location, config_Location, "Name", eventName, "Saved");
        }

        public void SetToggleValueAsBool(string eventName, bool eventValue)
        {
            this.xmlManager.SetNodeAttributeValueFromBool(emailConfig_Data_Location, config_Location, "Name", eventName, "Saved", eventValue);
        }

        public string GetEncryptedTextBoxValue(string eventName)
        {
            return this.encryptionManager.DecryptString(this.encryptionManager.EncryptionKey,
                this.xmlManager.GetNodeAttributeValueAsString(emailConfig_Data_Location, config_Location, "Name", eventName, "Saved"));
        }

        public void SetEncryptedTextBoxValue(string eventName, string eventValue)
        {
            this.xmlManager.SetNodeAttributeValueFromString(emailConfig_Data_Location, config_Location, "Name", eventName, "Saved",
                this.encryptionManager.EncryptString(this.encryptionManager.EncryptionKey, eventValue));
        }

        private void CreateXMLSettingsDocument(string dataLocation)
        {
            XmlDocument settingsDoc = new XmlDocument();
            XmlNode SettingsHeader = settingsDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            settingsDoc.AppendChild(SettingsHeader);

            //Settings
            XmlNode settingsRootElement = settingsDoc.CreateElement("Settings");
            settingsDoc.AppendChild(settingsRootElement);

            //Email
            XmlNode emailConfigChildElement = settingsDoc.CreateElement("Email");
            settingsRootElement.AppendChild(emailConfigChildElement);

            XmlNode emailConfigEmailAdsressSubChildElement = settingsDoc.CreateElement("Config");
            emailConfigEmailAdsressSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "EmailAddress";
            emailConfigEmailAdsressSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "";
            emailConfigChildElement.AppendChild(emailConfigEmailAdsressSubChildElement);

            XmlNode emailConfigHostSubChildElement = settingsDoc.CreateElement("Config");
            emailConfigHostSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Host";
            emailConfigHostSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "smtp.sendgrid.net";
            emailConfigChildElement.AppendChild(emailConfigHostSubChildElement);

            XmlNode emailConfigPortSubChildElement = settingsDoc.CreateElement("Config");
            emailConfigPortSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Port";
            emailConfigPortSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "587";
            emailConfigChildElement.AppendChild(emailConfigPortSubChildElement);

            XmlNode emailConfigSslSubChildElement = settingsDoc.CreateElement("Config");
            emailConfigSslSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "SSL";
            emailConfigSslSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "False";
            emailConfigChildElement.AppendChild(emailConfigSslSubChildElement);

            XmlNode emailConfigSenderSubChildElement = settingsDoc.CreateElement("Config");
            emailConfigSenderSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Sender";
            emailConfigSenderSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "motoical@outlook.com";
            emailConfigChildElement.AppendChild(emailConfigSenderSubChildElement);

            XmlNode emailConfigUserNameSubChildElement = settingsDoc.CreateElement("Config");
            emailConfigUserNameSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "UserName";
            emailConfigUserNameSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "apikey";
            emailConfigChildElement.AppendChild(emailConfigUserNameSubChildElement);

            XmlNode emailConfigPasswordSubChildElement = settingsDoc.CreateElement("Config");
            emailConfigPasswordSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Password";
            emailConfigPasswordSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value ="";
            emailConfigChildElement.AppendChild(emailConfigPasswordSubChildElement);

            settingsDoc.Save(dataLocation);
        }
    }
}
