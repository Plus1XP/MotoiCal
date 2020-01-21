using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

using MotoiCal.Models;

namespace MotoiCal.ViewModels
{
    public class MotoiCalViewModel : INotifyPropertyChanged
    {
        private readonly Scraper scraper;
        private IMotorSport motorSportSeries;
        private string iCalendarResults;    
        private string mainHeader;
        private string resultsOutput;
        private string subHeader;
        private bool isSearchingF1;
        private bool isSearchingMotoGP;
        private bool isSearchingWorldSBK;
        private bool canExecuteEasterEgg;

        private readonly string easterEggDate = "DD MMM, YYYY";
        private readonly string easterEggMessage = "Enter Easter Egg Text Here";

        public MotoiCalViewModel()
        {
            this.scraper = new Scraper();
            this.PullDatesCmd = new RelayCommand(o => this.PullDates(), o => this.CanExecuteCmd(this.motorSportSeries));
            this.GenerateIcsCmd = new RelayCommand(o => this.GenerateIcs(), o => this.CanExecuteCmd(this.motorSportSeries));
            this.ReadIcsCmd = new RelayCommand(o => this.ReadIcs(), o => this.CanExecuteCmd(this.motorSportSeries));
            this.DeleteIcsCmd = new RelayCommand(o => this.DeleteIcs(), o => this.CanExecuteCmd(this.motorSportSeries));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public IMotorSport MotorSportSeries
        {
            get => this.motorSportSeries;
            set
            {
                this.motorSportSeries = value;
                this.OnPropertyChanged("MotorSportSeries");
            }
        }

        public string AppVersion => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

        public string AppTitle => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName;

        public string WindowTitle => $"{this.AppTitle} v{this.AppVersion}";

        public string SearchingFormula1Content => "Formula 1";

        public string SearchingMotoGPContent => "MotoGP";

        public string SearchingWorldSBKContent => "WorldSBK";

        public string PullDatesContent => "Pull Dates";

        public string GenerateIcsContent => "Generate ICS";

        public string ReadIcsContent => "Read ICS";

        public string DeleteIcsContent => "Delete ICS";

        public string MainHeader
        {
            get => this.mainHeader;
            set
            {
                this.mainHeader = value;
                this.OnPropertyChanged("MainHeader");
            }
        }

        public string SubHeader
        {
            get => this.subHeader;
            set
            {
                this.subHeader = value;
                this.OnPropertyChanged("SubHeader");
            }
        }

        public string ResultsOutput
        {
            get => this.resultsOutput;
            set
            {
                this.resultsOutput = value;
                this.OnPropertyChanged("ResultsOutput");
            }
        }

        public string ICalendarResults
        {
            get => this.iCalendarResults;
            set
            {
                this.iCalendarResults = value;
                this.OnPropertyChanged("ICalendarResults");
            }
        }

        public bool IsSearchingF1
        {
            get => this.isSearchingF1;
            set
            {
                this.isSearchingF1 = value;
                this.SetFormula1Instance();
                this.OnPropertyChanged("IsSearchingF1");
            }
        }

        public bool IsSearchingMotoGP
        {
            get => this.isSearchingMotoGP;
            set
            {
                this.isSearchingMotoGP = value;
                this.SetMotoGPInstance();
                this.OnPropertyChanged("IsSearchingMotoGP");
            }
        }

        public bool IsSearchingWorldSBK
        {
            get => this.isSearchingWorldSBK;
            set
            {
                this.isSearchingWorldSBK = value;
                this.SetWorldSBKInstance();
                this.OnPropertyChanged("IsSearchingWorldSBK");
            }
        }

        public RelayCommand PullDatesCmd { get; }

        public RelayCommand GenerateIcsCmd { get; }

        public RelayCommand ReadIcsCmd { get; }

        public RelayCommand DeleteIcsCmd { get; }

        public RelayCommand EasterEggCmd { get; }

        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void PullDates()
        {
            string TempHeader = this.MainHeader;
            this.OnPropertyChanged("MainHeader");
            this.ResultsOutput = this.scraper.ScrapeEventsToiCalendar(this.MotorSportSeries);
            this.MainHeader += $" {this.scraper.RacesFound(this.MotorSportSeries)} Races";
            MessageBox.Show($"DONE! \nScraped {this.scraper.RacesFound(this.MotorSportSeries)} Races \nScraped {this.scraper.EventsFound()} Events", $"{TempHeader}");
        }

        private void GenerateIcs()
        {
            this.SubHeader = $"{this.motorSportSeries.FilePath}";
            this.ICalendarResults = this.scraper.GenerateiCalendar(this.motorSportSeries);
        }

        private void ReadIcs()
        {
            this.SubHeader = $"{this.motorSportSeries.FilePath}";
            this.ICalendarResults = this.scraper.ReadiCalendar(this.motorSportSeries);
        }

        private void DeleteIcs()
        {
            this.SubHeader = $"{this.motorSportSeries.FilePath}";
            this.ICalendarResults = this.scraper.DeleteiCalendar(this.motorSportSeries);
        }

        private void SetFormula1Instance()
        {
            if (this.isSearchingF1)
            {
                this.motorSportSeries = new Formula1();
                this.mainHeader = "Formula 1 Calendar Results";
            }
        }

        private void SetMotoGPInstance()
        {
            if (this.isSearchingMotoGP)
            {
                this.motorSportSeries = new MotoGP();
                this.mainHeader = "MotoGP Calendar Results";
            }
        }

        private void SetWorldSBKInstance()
        {
            if (this.isSearchingWorldSBK)
            {
                this.motorSportSeries = new WorldSBK();
                this.mainHeader = "WorldSBK Calendar Results";
            }
        }

        private bool CanExecuteCmd(object parameter)
        {
            if (this.motorSportSeries == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}