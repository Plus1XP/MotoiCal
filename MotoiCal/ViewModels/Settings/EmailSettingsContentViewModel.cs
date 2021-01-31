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
        private bool isAdvanced;
        private string emailAddress;
        private string host;
        private int port;
        private string sender;
        private string userName;
        private string password;

        public EmailSettingsContentViewModel()
        {
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
                return emailAddress; 
            }
            set 
            { 
                emailAddress = value;
                OnPropertyChanged("EmailAdress");
            }
        }


        public string Host
        {
            get 
            { 
                return host; 
            }
            set 
            { 
                host = value;
                OnPropertyChanged("Host");
            }
        }

        public int Port
        {
            get 
            { 
                return port; 
            }
            set 
            { 
                port = value;
                OnPropertyChanged("Port");
            }
        }

        public string Sender
        {
            get 
            { 
                return sender; 
            }
            set 
            { 
                sender = value;
                OnPropertyChanged("Sender");
            }
        }

        public string UserName
        {
            get 
            { 
                return userName; 
            }
            set 
            { 
                userName = value;
                OnPropertyChanged("UserName");
            }
        }

        public string Password
        {
            get 
            { 
                return password; 
            }
            set 
            { 
                password = value;
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
