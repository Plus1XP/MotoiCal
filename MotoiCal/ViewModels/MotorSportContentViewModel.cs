using MotoiCal.Interfaces;
using MotoiCal.Models;
using MotoiCal.Models.ButtonManagement;
using MotoiCal.Services;
using MotoiCal.Utilities.Commands;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MotoiCal.ViewModels
{
    class MotorSportContentViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<IRaceTimeTable> timeTable;

        private ButtonManagerModel buttonManagerModel;

        private ScraperService scraperService;

        private CalendarService calendarService;

        private MotorSport motorSportSeries;

        private string resultsText;

        private bool isSearching;

        public MotorSportContentViewModel(MotorSport motorSportSeries)
        {
            this.motorSportSeries = motorSportSeries;

            this.IsSearching = false;

            this.scraperService = new ScraperService();
            this.calendarService = new CalendarService();

            this.buttonManagerModel = new ButtonManagerModel();

            this.FindRacesCommand = new AsyncCommand(async () => await this.FindRaces());
            this.GenerateIcalCommand = new SyncCommand(this.GenerateIcal);
            this.ReadIcalCommand = new SyncCommand(this.ReadIcal);
            this.DeleteIcalCommand = new SyncCommand(this.DeleteIcal);

            this.buttonManagerModel.AddButton(this.FindRacesButtonStatus = new ButtonStatusModel("Find Races", "Find Available Races"));
            this.buttonManagerModel.AddButton(this.GenerateIcalButtonStatus = new ButtonStatusModel("Generate Ical", "Generate a ICS File"));
            this.buttonManagerModel.AddButton(this.ReadIcalButtonStatus = new ButtonStatusModel("Read Ical", "Read a ICS File"));
            this.buttonManagerModel.AddButton(this.DeleteIcalButtonStatus = new ButtonStatusModel("Delete Ical", "Delete a ICS File"));

            this.FindRacesButtonStatus.ButtonStatusChanged = new EventHandler(this.FindRacesButtonActive);
            this.GenerateIcalButtonStatus.ButtonStatusChanged = new EventHandler(this.GenerateIcalButtonActive);
            this.ReadIcalButtonStatus.ButtonStatusChanged = new EventHandler(this.ReadIcalButtonActive);
            this.DeleteIcalButtonStatus.ButtonStatusChanged = new EventHandler(this.DeleteButtonActive);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public AsyncCommand FindRacesCommand { get; }
        public SyncCommand GenerateIcalCommand { get; }
        public SyncCommand ReadIcalCommand { get; }
        public SyncCommand DeleteIcalCommand { get; }

        public ButtonStatusModel FindRacesButtonStatus { get; set; }
        public ButtonStatusModel GenerateIcalButtonStatus { get; set; }
        public ButtonStatusModel ReadIcalButtonStatus { get; set; }
        public ButtonStatusModel DeleteIcalButtonStatus { get; set; }

        public Visibility ShowLoadingBar => this.IsSearching ? Visibility.Visible : Visibility.Hidden;

        public bool IsButtonEnabled => !this.IsSearching;

        public bool IsSearching
        {
            get => this.isSearching;
            set
            {
                if (this.isSearching != value)
                {
                    this.isSearching = value;
                    this.OnPropertyChanged("IsSearching");
                    this.OnPropertyChanged("IsButtonEnabled");
                    this.OnPropertyChanged("ShowLoadingBar");
                }
            }
        }

        public string ResultsText
        {
            get { return this.resultsText; }
            set
            {
                this.resultsText = value;
                this.OnPropertyChanged("ResultsText");
            }
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void FindRacesButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("FindRacesButtonStatus");
        }

        private void GenerateIcalButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("GenerateIcalButtonStatus");
        }

        private void ReadIcalButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("ReadIcalButtonStatus");
        }

        private void DeleteButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("DeleteIcalButtonStatus");
        }

        private async Task FindRaces()
        {
            this.buttonManagerModel.SetActiveButton(this.FindRacesButtonStatus);
            this.IsSearching = true;
            this.timeTable = new ObservableCollection<IRaceTimeTable>();
            await Task.Run(() => this.timeTable = this.scraperService.GetSeriesCollection(this.motorSportSeries));
            this.ResultsText = ViewRaceTimeTable(timeTable);
            this.IsSearching = false;
        }

        private void GenerateIcal()
        {
            this.buttonManagerModel.SetActiveButton(this.GenerateIcalButtonStatus);
            //this.SubHeader = $"{this.motorSportSeries.FilePath}";
            this.ResultsText = this.scraper.GenerateiCalendar(this.motorSportSeries, this.motorSportSeries.IsEventReminderActive, this.motorSportSeries.EventReminderMins); // Pass in event reminders
        }

        private void ReadIcal()
        {
            this.buttonManagerModel.SetActiveButton(this.ReadIcalButtonStatus);
            //this.SubHeader = $"{this.motorSportSeries.FilePath}";
            this.ResultsText = this.scraper.ReadiCalendar(this.motorSportSeries);
        }

        private void DeleteIcal()
        {
            this.buttonManagerModel.SetActiveButton(this.DeleteIcalButtonStatus);
            //this.SubHeader = $"{this.motorSportSeries.FilePath}";
            this.ResultsText = this.scraper.DeleteiCalendar(this.motorSportSeries);
        }
    }
}
