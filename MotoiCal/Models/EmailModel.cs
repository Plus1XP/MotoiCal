namespace MotoiCal.Models
{
    class EmailModel
    {
        XMLEmailConfigDataModel emailConfig;

        public EmailModel()
        {
            this.emailConfig = new XMLEmailConfigDataModel();
        }

        public string Host
        {
            get => this.emailConfig.GetTextBoxValue("Host");
            set => this.emailConfig.SetTextBoxValue("Host", value);
        }

        public int Port
        {
            get => this.emailConfig.GetTextBoxValueAsInt("Port");
            set => this.emailConfig.SetTextBoxValueAsInt("Port", value);
        }

        public bool IsSSL
        {
            get => this.emailConfig.GetToggleValueAsBool("SSL");
            set => this.emailConfig.SetToggleValueAsBool("SSL", value);
        }

        public string UserName
        {
            get => this.emailConfig.GetTextBoxValue("UserName");
            set => this.emailConfig.SetTextBoxValue("UserName", value);
        }

        public string Password
        {
            get => this.emailConfig.GetEncryptedTextBoxValue("Password");
            set => this.emailConfig.SetEncryptedTextBoxValue("Password", value);
        }

        public string From
        {
            get => this.emailConfig.GetTextBoxValue("Sender");
            set => this.emailConfig.SetTextBoxValue("Sender", value);
        }

        public string To
        {
            get => this.emailConfig.GetTextBoxValue("EmailAddress");
            set => this.emailConfig.SetTextBoxValue("EmailAddress", value);
        }
    }
}
