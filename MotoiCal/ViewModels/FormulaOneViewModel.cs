using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoiCal.ViewModels
{
    public class FormulaOneViewModel : INotifyPropertyChanged
    {
        private string name = "Formula One Working!";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
