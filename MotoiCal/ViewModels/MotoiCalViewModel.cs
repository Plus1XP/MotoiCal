using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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
        private bool isSearching;

        private bool canExecuteEasterEgg; // Disabled for next planned release/v1.0.0-LorenzosLand

        private readonly string easterEggDate = "04 May"; //DD MMM, YYYY
        private readonly string easterEggTitle = "Did you know?";
        private readonly string easterEggMessage = "On this day, Lorenzo made his championship debut.\n" +
                                                    "It was the second qualifying day for the 2002 125cc Spanish Grand Prix.\n" + 
                                                     "He missed Friday practice as he was not old enough to race!";

        public MotoiCalViewModel()
        {
            this.scraper = new Scraper();
            this.IsSearching = false;
            this.canExecuteEasterEgg = false; // this.scraper.IsEasterEggActive(this.easterEggDate);
            this.PullDatesCmd = new AsynchronousRelayCommand(async () => await this.PullDates(), () => this.CanExecuteCmd(this.motorSportSeries));
            this.GenerateIcsCmd = new SynchronousRelayCommand(this.GenerateIcs, () => this.CanExecuteCmd(this.motorSportSeries));
            this.ReadIcsCmd = new SynchronousRelayCommand(this.ReadIcs, () => this.CanExecuteCmd(this.motorSportSeries));
            this.DeleteIcsCmd = new SynchronousRelayCommand(this.DeleteIcs, () => this.CanExecuteCmd(this.motorSportSeries));
            this.EasterEggCmd = new SynchronousRelayCommand(this.EasterEgg, () => this.canExecuteEasterEgg);
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

        public Visibility IsEasterEggHidden => this.canExecuteEasterEgg ? Visibility.Visible : Visibility.Hidden;

        public Visibility ShowLoadingBar => this.isSearching ? Visibility.Visible : Visibility.Hidden;

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

        public bool IsButtonEnabled => !this.IsSearching;

        public ICommand PullDatesCmd { get; }

        public ICommand GenerateIcsCmd { get; }

        public ICommand ReadIcsCmd { get; }

        public ICommand DeleteIcsCmd { get; }

        public ICommand EasterEggCmd { get; }

        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void EasterEgg()
        {
            MessageBox.Show(this.easterEggMessage, this.easterEggTitle);
        }

        private async Task PullDates()
        {
            this.IsSearching = true;
            string unalteredMainHeader = this.MainHeader;
            this.ResultsOutput = await this.scraper.ScrapeEventsToiCalendar(this.MotorSportSeries);
            this.OnPropertyChanged("MainHeader");
            this.MainHeader += $" {this.scraper.RacesFound(this.MotorSportSeries)} Races";
            this.IsSearching = false;
            MessageBox.Show($"DONE! \nScraped {this.scraper.RacesFound(this.MotorSportSeries)} Races \nScraped {this.scraper.EventsFound()} Events", $"{unalteredMainHeader}");
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
                this.MotorSportSeries = new Formula1();
                this.mainHeader = "Formula 1 Calendar Results";
            }
        }

        private void SetMotoGPInstance()
        {
            if (this.isSearchingMotoGP)
            {
                this.MotorSportSeries = new MotoGP();
                this.mainHeader = "MotoGP Calendar Results";
            }
        }

        private void SetWorldSBKInstance()
        {
            if (this.isSearchingWorldSBK)
            {
                this.MotorSportSeries = new WorldSBK();
                this.mainHeader = "WorldSBK Calendar Results";
            }
        }

        private bool CanExecuteCmd(object parameter)
        {
            return parameter != null;
        }
    }
}