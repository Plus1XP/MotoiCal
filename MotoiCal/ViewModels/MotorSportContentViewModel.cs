using MotoiCal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoiCal.ViewModels
{
    class MotorSportContentViewModel : INotifyPropertyChanged
    {
        private ButtonManagerModel buttonManagerModel;

        private IMotorSport motorSportSeries;

        public event PropertyChangedEventHandler PropertyChanged;

        public MotorSportContentViewModel(IMotorSport motorSportSeries)
        {
            this.motorSportSeries = motorSportSeries;

            this.buttonManagerModel = new ButtonManagerModel();

            this.FindRacesCommand = new SynchronousRelayCommand(this.FindRaces);
            this.GenerateIcalCommand = new SynchronousRelayCommand(this.GenerateIcal);
            this.ReadIcalCommand = new SynchronousRelayCommand(this.ReadIcal);
            this.DeleteIcalCommand = new SynchronousRelayCommand(this.DeleteIcal);

            this.buttonManagerModel.AddButton(this.FindRacesButtonStatus = new ButtonStatusModel("Find Races", "Find Available Races"));
            this.buttonManagerModel.AddButton(this.GenerateIcalButtonStatus = new ButtonStatusModel("Generate Ical", "Generate a ICS File"));
            this.buttonManagerModel.AddButton(this.ReadIcalButtonStatus = new ButtonStatusModel("Read Ical", "Read a ICS File"));
            this.buttonManagerModel.AddButton(this.DeleteIcalButtonStatus = new ButtonStatusModel("Delete Ical", "Delete a ICS File"));

            this.FindRacesButtonStatus.ButtonStatusChanged = new EventHandler(this.FindRacesButtonActive);
            this.GenerateIcalButtonStatus.ButtonStatusChanged = new EventHandler(this.GenerateIcalButtonActive);
            this.ReadIcalButtonStatus.ButtonStatusChanged = new EventHandler(this.ReadIcalButtonActive);
            this.DeleteIcalButtonStatus.ButtonStatusChanged = new EventHandler(this.DeleteButtonActive);
        }

        public SynchronousRelayCommand FindRacesCommand { get; }
        public SynchronousRelayCommand GenerateIcalCommand { get; }
        public SynchronousRelayCommand ReadIcalCommand { get; }
        public SynchronousRelayCommand DeleteIcalCommand { get; }

        public ButtonStatusModel FindRacesButtonStatus { get; set; }
        public ButtonStatusModel GenerateIcalButtonStatus { get; set; }
        public ButtonStatusModel ReadIcalButtonStatus { get; set; }
        public ButtonStatusModel DeleteIcalButtonStatus { get; set; }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public void FindRacesButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("FindRacesButtonStatus");
        }

        public void GenerateIcalButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("GenerateIcalButtonStatus");
        }

        public void ReadIcalButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("ReadIcalButtonStatus");
        }

        public void DeleteButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("DeleteIcalButtonStatus");
        }

        public void FindRaces()
        {
            this.buttonManagerModel.SetActiveButton(this.FindRacesButtonStatus);
        }

        public void GenerateIcal()
        {
            this.buttonManagerModel.SetActiveButton(this.GenerateIcalButtonStatus);
        }

        public void ReadIcal()
        {
            this.buttonManagerModel.SetActiveButton(this.ReadIcalButtonStatus);
        }

        public void DeleteIcal()
        {
            this.buttonManagerModel.SetActiveButton(this.DeleteIcalButtonStatus);
        }
    }
}
