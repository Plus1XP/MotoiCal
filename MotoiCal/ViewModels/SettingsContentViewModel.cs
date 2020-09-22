using MotoiCal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MotoiCal.ViewModels
{
    class SettingsContentViewModel : INotifyPropertyChanged
    {
        private ButtonManagerModel buttonManagerModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsContentViewModel()
        {
            this.buttonManagerModel = new ButtonManagerModel();

            this.FormulaOneParametersCommand = new SynchronousRelayCommand(this.FormulaOneParameters);
            this.MotoGPParametersCommand = new SynchronousRelayCommand(this.MotoGPParameters);
            this.WorldSBKParametersCommand = new SynchronousRelayCommand(this.WorldSBKParameters);

            this.buttonManagerModel.AddButton(FormulaOneParametersButtonStatus = new ButtonStatusModel("Formula One", "Configure Formula One Search Settings"));
            this.buttonManagerModel.AddButton(MotoGPParametersButtonStatus = new ButtonStatusModel("MotoGP", "Configure MotoGP Search Settings"));
            this.buttonManagerModel.AddButton(WorldSBKParametersButtonStatus = new ButtonStatusModel("World SBK", "Configure World SBK Search Settings"));

            this.FormulaOneParametersButtonStatus.ButtonStatusChanged = new EventHandler(this.FormulaOneButtonActive);
            this.MotoGPParametersButtonStatus.ButtonStatusChanged = new EventHandler(this.MotoGPButtonActive);
            this.WorldSBKParametersButtonStatus.ButtonStatusChanged = new EventHandler(this.WorldSBKButtonActive);
        }

        public SynchronousRelayCommand FormulaOneParametersCommand { get; }
        public SynchronousRelayCommand MotoGPParametersCommand { get; }
        public SynchronousRelayCommand WorldSBKParametersCommand { get; }

        public ButtonStatusModel FormulaOneParametersButtonStatus { get; set; }
        public ButtonStatusModel MotoGPParametersButtonStatus { get; set; }
        public ButtonStatusModel WorldSBKParametersButtonStatus { get; set; }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public void FormulaOneButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("FormulaOneParametersButtonStatus");
        }

        public void MotoGPButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("MotoGPParametersButtonStatus");
        }

        public void WorldSBKButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("WorldSBKParametersButtonStatus");
        }

        public void FormulaOneParameters()
        {
            this.buttonManagerModel.SetActiveButton(this.FormulaOneParametersButtonStatus);
        }

        public void MotoGPParameters()
        {
            this.buttonManagerModel.SetActiveButton(this.MotoGPParametersButtonStatus);
        }

        public void WorldSBKParameters()
        {
            this.buttonManagerModel.SetActiveButton(this.WorldSBKParametersButtonStatus);
        }
    }
}
