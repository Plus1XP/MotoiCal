using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoiCal.Models
{
    class EmailModel
    {
        XMLEmailConfigDataModel emailConfig;

        public EmailModel()
        {
            emailConfig = new XMLEmailConfigDataModel();
        }

        public string Host
        {
            get => emailConfig.GetTextBoxValue("Host");
            set => emailConfig.SetTextBoxValue("Host", value);
        }

        public int Port
        {
            get => emailConfig.GetTextBoxValueAsInt("Port");
            set => emailConfig.SetTextBoxValueAsInt("Port", value);
        }

        public bool IsSSL
        {
            get => emailConfig.GetToggleValueAsBool("SSL");
            set => emailConfig.SetToggleValueAsBool("SSL", value);
        }

        public string UserName
        {
            get => emailConfig.GetTextBoxValue("UserName");
            set => emailConfig.SetTextBoxValue("UserName", value);
        }

        public string Password
        {
            get => emailConfig.GetEncryptedTextBoxValue("Password");
            set => emailConfig.SetEncryptedTextBoxValue("Password", value);
        }

        public string From
        {
            get => emailConfig.GetTextBoxValue("Sender");
            set => emailConfig.SetTextBoxValue("Sender", value);
        }

        public string To
        {
            get => emailConfig.GetTextBoxValue("EmailAddress");
            set => emailConfig.SetTextBoxValue("EmailAddress", value);
        }
    }
}
