using MotoiCal.Models;
using MotoiCal.Models.FileManagement;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoiCal.ViewModels.Settings
{
    class EmailSettingsContentViewModel : INotifyPropertyChanged
    {
        EmailModel emailModel;

        private bool isAdvanced;

        public EmailSettingsContentViewModel()
        {
            emailModel = new EmailModel();

            IsAdvanced = false;
        }

        public bool IsAdvanced
        {
            get 
            { 
                return isAdvanced; 
            }
            set 
            { 
                isAdvanced = value;
                OnPropertyChanged("IsAdvanced");
            }
        }


        public string EmailAddress
        {
            get 
            { 
                return emailModel.To; 
            }
            set 
            { 
                emailModel.To = value;
                OnPropertyChanged("EmailAdress");
            }
        }


        public string Host
        {
            get 
            { 
                return emailModel.Host; 
            }
            set 
            { 
                emailModel.Host = value;
                OnPropertyChanged("Host");
            }
        }

        public int Port
        {
            get 
            { 
                return emailModel.Port; 
            }
            set 
            { 
                emailModel.Port = value;
                OnPropertyChanged("Port");
            }
        }

        public bool IsSSL
        {
            get
            {
                return emailModel.IsSSL;
            }
            set
            {
                emailModel.IsSSL = value;
                OnPropertyChanged("IsSSL");
            }
        }

        public string Sender
        {
            get 
            { 
                return emailModel.From; 
            }
            set 
            { 
                emailModel.From = value;
                OnPropertyChanged("Sender");
            }
        }

        public string UserName
        {
            get 
            { 
                return emailModel.UserName; 
            }
            set 
            { 
                emailModel.UserName = value;
                OnPropertyChanged("UserName");
            }
        }

        public string Password
        {
            get 
            {
                return emailModel.Password; 
            }
            set 
            { 
                emailModel.Password = value;
                OnPropertyChanged("Password");
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
