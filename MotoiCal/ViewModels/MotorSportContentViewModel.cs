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

        EmailService emailService;

        private MotorSport motorSportSeries;

        private string resultsText;

        private bool isSearching;

        public MotorSportContentViewModel(MotorSport motorSportSeries)
        {
            this.motorSportSeries = motorSportSeries;

            this.IsSearching = false;

            this.scraperService = new ScraperService();
            this.calendarService = new CalendarService();
            this.emailService = new EmailService();

            this.buttonManagerModel = new ButtonManagerModel();

            this.FindRacesCommand = new AsyncCommand(async () => await this.FindRaces());
            this.EmailIcalCommand = new AsyncCommand(async () => await this.EmailIcal());
            this.GenerateIcalCommand = new SyncCommand(this.GenerateIcal);
            this.ReadIcalCommand = new SyncCommand(this.ReadIcal);
            this.DeleteIcalCommand = new SyncCommand(this.DeleteIcal);

            this.buttonManagerModel.AddButton(this.FindRacesButtonStatus = new ButtonStatusModel("Find Races", "Find Available Races")); // Search races
            this.buttonManagerModel.AddButton(this.EmailIcalButtonStatus = new ButtonStatusModel("Email Ical", "email the ICS file"));
            this.buttonManagerModel.AddButton(this.GenerateIcalButtonStatus = new ButtonStatusModel("Generate Ical", "Generate a ICS File")); // Genertae Local Ical
            this.buttonManagerModel.AddButton(this.ReadIcalButtonStatus = new ButtonStatusModel("Read Ical", "Read a ICS File"));
            this.buttonManagerModel.AddButton(this.DeleteIcalButtonStatus = new ButtonStatusModel("Delete Ical", "Delete a ICS File"));

            this.FindRacesButtonStatus.ButtonStatusChanged = new EventHandler(this.FindRacesButtonActive);
            this.GenerateIcalButtonStatus.ButtonStatusChanged = new EventHandler(this.GenerateIcalButtonActive);
            this.ReadIcalButtonStatus.ButtonStatusChanged = new EventHandler(this.ReadIcalButtonActive);
            this.DeleteIcalButtonStatus.ButtonStatusChanged = new EventHandler(this.DeleteButtonActive);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public AsyncCommand FindRacesCommand { get; }
        public SyncCommand EmailIcalCommand { get; }
        public SyncCommand GenerateIcalCommand { get; }
        public SyncCommand ReadIcalCommand { get; }
        public SyncCommand DeleteIcalCommand { get; }

        public ButtonStatusModel FindRacesButtonStatus { get; set; }
        public ButtonStatusModel EmailIcalButtonStatus { get; set; }
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

        private void EmailIcalButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("EmailIcalButtonStatus");
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
            if (this.timeTable == null)
            {
                this.timeTable = new ObservableCollection<IRaceTimeTable>();
                await Task.Run(() => this.timeTable = this.scraperService.GetSeriesCollection(this.motorSportSeries));

                // Generate after finding to avoid confusion.
                this.calendarService.GenerateiCalendar(this.motorSportSeries, this.timeTable);
            }

            this.ResultsText = this.ViewRaceTimeTable(this.timeTable);
            this.IsSearching = false;
        }

        private async Task EmailIcal()
        {
            this.buttonManagerModel.SetActiveButton(this.EmailIcalButtonStatus);
            this.ResultsText = await this.emailService.SendCalendar(this.motorSportSeries);
        }

        private void GenerateIcal()
        {
            this.buttonManagerModel.SetActiveButton(this.GenerateIcalButtonStatus);
            this.ResultsText = this.calendarService.GenerateiCalendar(this.motorSportSeries, this.timeTable);
        }

        private void ReadIcal()
        {
            this.buttonManagerModel.SetActiveButton(this.ReadIcalButtonStatus);
            this.ResultsText = this.calendarService.ReadiCalendar(this.motorSportSeries);
        }

        private void DeleteIcal()
        {
            this.buttonManagerModel.SetActiveButton(this.DeleteIcalButtonStatus);
            this.ResultsText = this.calendarService.DeleteiCalendar(this.motorSportSeries);
        }

        // currentSponser is initially set to null, then each loop is given the current Sponser value.
        // This allows the stringbuilder to check if it needs to update header.
        // * This was changed from checking again the currentGrandPrix in the COVID update due to multiple races at the same GrandPrix.
        public string ViewRaceTimeTable(ObservableCollection<IRaceTimeTable> timeTable)
        {
            StringBuilder results = new StringBuilder();

            string currentSponser = null;

            foreach (IRaceTimeTable motorSport in timeTable)
            {
                string header = motorSport.Sponser == currentSponser ? string.Empty : this.FilterMotoGPHeaders(motorSport.DisplayHeader);
                string body = motorSport.DisplayBody;

                results.Append(header);
                results.AppendLine(body);

                currentSponser = motorSport.Sponser;
            }
            return results.ToString();
        }

        // Sorry about readability. >_< due to ViewRaceTimeTable using first item in list (usually moto2),
        // this removes moto2 or moto3 from header and replaces with motoGP.
        private string FilterMotoGPHeaders(string header)
        {
            return header.Contains("Moto2") ? header.Replace("Moto2", "MotoGP") : header.Contains("Moto3") ? header.Replace("Moto3", "MotoGP") : header;
        }
    }
}
