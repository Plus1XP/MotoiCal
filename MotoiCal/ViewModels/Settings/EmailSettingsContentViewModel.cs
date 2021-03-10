using MotoiCal.Models;

using System.ComponentModel;

namespace MotoiCal.ViewModels.Settings
{
    class EmailSettingsContentViewModel : INotifyPropertyChanged
    {
        EmailModel emailModel;

        private bool isAdvanced;

        public EmailSettingsContentViewModel()
        {
            this.emailModel = new EmailModel();

            this.IsAdvanced = false;
        }

        public bool IsAdvanced
        {
            get 
            { 
                return this.isAdvanced; 
            }
            set 
            {
                this.isAdvanced = value;
                this.OnPropertyChanged("IsAdvanced");
            }
        }


        public string EmailAddress
        {
            get 
            { 
                return this.emailModel.To; 
            }
            set 
            {
                this.emailModel.To = value;
                this.OnPropertyChanged("EmailAdress");
            }
        }


        public string Host
        {
            get 
            { 
                return this.emailModel.Host; 
            }
            set 
            {
                this.emailModel.Host = value;
                this.OnPropertyChanged("Host");
            }
        }

        public int Port
        {
            get 
            { 
                return this.emailModel.Port; 
            }
            set 
            {
                this.emailModel.Port = value;
                this.OnPropertyChanged("Port");
            }
        }

        public bool IsSSL
        {
            get
            {
                return this.emailModel.IsSSL;
            }
            set
            {
                this.emailModel.IsSSL = value;
                this.OnPropertyChanged("IsSSL");
            }
        }

        public string Sender
        {
            get 
            { 
                return this.emailModel.From; 
            }
            set 
            {
                this.emailModel.From = value;
                this.OnPropertyChanged("Sender");
            }
        }

        public string UserName
        {
            get 
            { 
                return this.emailModel.UserName; 
            }
            set 
            {
                this.emailModel.UserName = value;
                this.OnPropertyChanged("UserName");
            }
        }

        public string Password
        {
            get 
            {
                return this.emailModel.Password; 
            }
            set 
            {
                this.emailModel.Password = value;
                this.OnPropertyChanged("Password");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
