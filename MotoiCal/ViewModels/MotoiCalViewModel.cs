using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MotoiCal.Models;
using MotoiCal.Views;

namespace MotoiCal.ViewModels
{
    public class MotoiCalViewModel : INotifyPropertyChanged
    {
        private FrameworkElement controlContentView;

        private bool canResizeWindow { get; set; }
        private bool canMinimizeWindow { get; set; }

        public MotoiCalViewModel()
        {
            this.canResizeWindow = true;
            this.canMinimizeWindow = true;

            this.CloseWindowCommand = new SynchronousRelayCommand<Window>(this.CloseWindow);
            this.MaximizeWindowCommand = new SynchronousRelayCommand<Window>(this.MaximizeWindow, o => this.canResizeWindow);
            this.MinimizeWindowCommand = new SynchronousRelayCommand<Window>(this.MinimizeWindow, o => this.canMinimizeWindow);
            this.RestoreWindowCommand = new SynchronousRelayCommand<Window>(this.RestoreWindow, o => this.canResizeWindow);

            this.FormulaOneViewCommand = new SynchronousRelayCommand(this.Formula1Tab);
            this.MotoGPViewCommand = new SynchronousRelayCommand(this.MotoGPTab);
            this.WorldSBKViewCommand = new SynchronousRelayCommand(this.WorldSBKTab);
            this.SettingsViewCommand = new SynchronousRelayCommand(this.SettingsTab);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public SynchronousRelayCommand<Window> CloseWindowCommand { get; set; }
        public SynchronousRelayCommand<Window> MaximizeWindowCommand { get; set; }
        public SynchronousRelayCommand<Window> MinimizeWindowCommand { get; set; }
        public SynchronousRelayCommand<Window> RestoreWindowCommand { get; set; }

        public SynchronousRelayCommand FormulaOneViewCommand { get; }
        public SynchronousRelayCommand MotoGPViewCommand { get; }
        public SynchronousRelayCommand WorldSBKViewCommand { get; }
        public SynchronousRelayCommand SettingsViewCommand { get; }

        public FrameworkElement ContentControlView
        {
            get { return this.controlContentView; }
            set
            {
                this.controlContentView = value;
                this.OnPropertyChanged("ContentControlView");
            }
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void Formula1Tab()
        {
            this.ContentControlView = new FormulaOneView();
            this.ContentControlView.DataContext = new FormulaOneViewModel();
        }

        private void MotoGPTab()
        {
            this.ContentControlView = new MotoGPView();
            this.ContentControlView.DataContext = new MotoGPViewModel();
        }
        private void WorldSBKTab()
        {
            this.ContentControlView = new WorldSBKView();
            this.ContentControlView.DataContext = new WorldSBKViewModel();
        }
        private void SettingsTab()
        {
            this.ContentControlView = new SettingsView();
            this.ContentControlView.DataContext = new SettingsViewModel();
        }

        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        private void MaximizeWindow(Window window)
        {
            if (this.CanResizeWindow(window))
            {
                window.WindowState = WindowState.Maximized;
            }
        }

        private void MinimizeWindow(Window window)
        {
            if (this.CanMinimizeWindow(window))
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        private void RestoreWindow(Window window)
        {
            if (this.CanResizeWindow(window))
            {
                window.WindowState = WindowState.Normal;
            }
        }

        private bool CanResizeWindow(Window window)
        {
            return window.ResizeMode == ResizeMode.CanResize || window.ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private bool CanMinimizeWindow(Window window)
        {
            return window.ResizeMode != ResizeMode.NoResize;
        }
    }

    //public enum EventTrigger_Modern
    //{
    //    [Description("At Time of Event")]
    //    AtTimeOfEvent = 0,
    //    [Description("5 Minutes Before Event")]
    //    Minutes5 = 5,
    //    [Description("15 Minutes Before Event")]
    //    Minutes15 = 15,
    //    [Description("30 Minutes Before Event")]
    //    Minutes30 = 30,
    //    [Description("1 Hour Before Event")]
    //    Minutes60 = 60,
    //    [Description("2 Hours Before Event")]
    //    Minutes120 = 120
    //}

    //public class MotoiCalViewModel_Modern : INotifyPropertyChanged
    //{
    //    private readonly Scraper scraper;
    //    private IMotorSport motorSportSeries;
    //    private string iCalendarResults;    
    //    private string mainHeader;
    //    private string resultsOutput;
    //    private string subHeader;
    //    private int eventTriggerMinutes;
    //    private bool isSearchingF1;
    //    private bool isSearchingMotoGP;
    //    private bool isSearchingWorldSBK;
    //    private bool isReminderActive;
    //    private bool isSearching;
    //    private bool canExecuteEasterEgg;

    //    private readonly string easterEggDate = "04 May"; //DD MMM, YYYY
    //    private readonly string easterEggTitle = "Did you know?";
    //    private readonly string easterEggMessage = "On this day, Lorenzo made his championship debut.\n" +
    //                                                "It was the second qualifying day for the 2002 125cc Spanish Grand Prix.\n" + 
    //                                                 "He missed Friday practice as he was not old enough to race!";

    //    public MotoiCalViewModel_Modern()
    //    {
    //        this.scraper = new Scraper();
    //        this.IsSearching = false;
    //        this.IsReminderActive = true;
    //        this.EventTriggerMinutes = (int)EventTrigger.Minutes15;
    //        this.canExecuteEasterEgg = this.scraper.IsEasterEggActive(this.easterEggDate);
    //        this.PullDatesCmd = new AsynchronousRelayCommand(async () => await this.PullDates(), () => this.CanExecuteCmd(this.motorSportSeries));
    //        this.GenerateIcsCmd = new SynchronousRelayCommand(this.GenerateIcs, () => this.CanExecuteCmd(this.motorSportSeries));
    //        this.ReadIcsCmd = new SynchronousRelayCommand(this.ReadIcs, () => this.CanExecuteCmd(this.motorSportSeries));
    //        this.DeleteIcsCmd = new SynchronousRelayCommand(this.DeleteIcs, () => this.CanExecuteCmd(this.motorSportSeries));
    //        this.EasterEggCmd = new SynchronousRelayCommand(this.EasterEgg, () => this.canExecuteEasterEgg);
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    public IMotorSport MotorSportSeries
    //    {
    //        get => this.motorSportSeries;
    //        set
    //        {
    //            this.motorSportSeries = value;

    //            //motorSportSeries.ExcludedEvents.Remove("group photo"); // Remove this *Testing*
    //            //IsF1RaceEventEnabled = false;
    //            //IsF1QualifyingEventEnabled = false;
    //            //IsF1PracticeEventEnabled = false;

    //            this.OnPropertyChanged("MotorSportSeries");
    //        }
    //    }

    //    public Visibility IsEasterEggHidden => this.canExecuteEasterEgg ? Visibility.Visible : Visibility.Hidden;

    //    public Visibility ShowLoadingBar => this.isSearching ? Visibility.Visible : Visibility.Hidden;

    //    public string AppVersion => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

    //    public string AppTitle => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName;

    //    public string WindowTitle => $"{this.AppTitle} v{this.AppVersion}";

    //    public string SearchingFormula1Content => "Formula 1";

    //    public string SearchingMotoGPContent => "MotoGP";

    //    public string SearchingWorldSBKContent => "WorldSBK";

    //    public string PullDatesContent => "Pull Dates";

    //    public string GenerateIcsContent => "Generate ICS";

    //    public string ReadIcsContent => "Read ICS";

    //    public string DeleteIcsContent => "Delete ICS";

    //    public string MainHeader
    //    {
    //        get => this.mainHeader;
    //        set
    //        {
    //            this.mainHeader = value;
    //            this.OnPropertyChanged("MainHeader");
    //        }
    //    }

    //    public string SubHeader
    //    {
    //        get => this.subHeader;
    //        set
    //        {
    //            this.subHeader = value;
    //            this.OnPropertyChanged("SubHeader");
    //        }
    //    }

    //    public string ResultsOutput
    //    {
    //        get => this.resultsOutput;
    //        set
    //        {
    //            this.resultsOutput = value;
    //            this.OnPropertyChanged("ResultsOutput");
    //        }
    //    }

    //    public string ICalendarResults
    //    {
    //        get => this.iCalendarResults;
    //        set
    //        {
    //            this.iCalendarResults = value;
    //            this.OnPropertyChanged("ICalendarResults");
    //        }
    //    }

    //    public int EventTriggerMinutes
    //    {
    //        get => eventTriggerMinutes;
    //        set
    //        {
    //            this.eventTriggerMinutes = value;
    //            this.OnPropertyChanged("EventTriggerMinutes");
    //        }
    //    }

    //    public bool IsSearchingF1
    //    {
    //        get => this.isSearchingF1;
    //        set
    //        {
    //            this.isSearchingF1 = value;
    //            this.SetFormula1Instance();
    //            this.OnPropertyChanged("IsSearchingF1");
    //        }
    //    }

    //    public bool IsSearchingMotoGP
    //    {
    //        get => this.isSearchingMotoGP;
    //        set
    //        {
    //            this.isSearchingMotoGP = value;
    //            this.SetMotoGPInstance();
    //            this.OnPropertyChanged("IsSearchingMotoGP");
    //        }
    //    }

    //    public bool IsSearchingWorldSBK
    //    {
    //        get => this.isSearchingWorldSBK;
    //        set
    //        {
    //            this.isSearchingWorldSBK = value;
    //            this.SetWorldSBKInstance();
    //            this.OnPropertyChanged("IsSearchingWorldSBK");
    //        }
    //    }

    //    public bool IsReminderActive //For Ical event reminder, use enum
    //    {
    //        get => isReminderActive;
    //        set
    //        {
    //            this.isReminderActive = value;
    //            this.OnPropertyChanged("IsReminderActive");
    //        }
    //    }

    //    public bool IsSearching
    //    {
    //        get => this.isSearching;
    //        set
    //        {
    //            if (this.isSearching != value)
    //            {
    //                this.isSearching = value;
    //                this.OnPropertyChanged("IsSearching");
    //                this.OnPropertyChanged("IsButtonEnabled");
    //                this.OnPropertyChanged("ShowLoadingBar");
    //            }
    //        }
    //    }

    //    // Test events
    //    public bool IsF1PracticeEventEnabled
    //    {
    //        set
    //        {
    //            SetEvent(value, "Practice");
    //        }
    //    }

    //    public bool IsF1QualifyingEventEnabled
    //    {
    //        set
    //        {
    //            SetEvent(value, "Qualifying");
    //        }
    //    }

    //    public bool IsF1RaceEventEnabled
    //    {
    //        set
    //        {
    //            SetEvent(value, "Race");
    //        }
    //    }

    //    public bool IsMotoGPPracticeEventEnabled
    //    {
    //        set
    //        {
    //            SetEvent(value, "Practice");
    //        }
    //    }

    //    public bool IsMotoGPQualifyingEventEnabled
    //    {
    //        set
    //        {
    //            SetEvent(value, "Qualifying");
    //        }
    //    }

    //    public bool IsMotoGPWarmUpEventEnabled
    //    {
    //        set
    //        {
    //            SetEvent(value, "Warm Up");
    //        }
    //    }

    //    public bool IsMotoGPRaceEventEnabled
    //    {
    //        set
    //        {
    //            SetEvent(value, "Race");
    //        }
    //    }

    //    public bool IsMotoGPAfterTheFlagEventEnabled
    //    {
    //        set
    //        {
    //            SetEvent(value, "After The Flag");
    //        }
    //    }

    //    public bool IsMotoGPBehindTheScenesEventEnabled
    //    {
    //        set
    //        {
    //            SetEvent(value, "behind the scenes");
    //        }
    //    }

    //    public bool IsWSBKPracticeEventEnabled
    //    {
    //        set
    //        {
    //            SetEvent(value, "FP");
    //        }
    //    }

    //    public bool IsWSBKQualifyingEventEnabled
    //    {
    //        set
    //        {
    //            SetEvent(value, "Superpole");
    //        }
    //    }

    //    public bool IsWSBKWarmUpEventEnabled
    //    {
    //        set
    //        {
    //            SetEvent(value, "WUP");
    //        }
    //    }

    //    public bool IsWSBKRaceEventEnabled
    //    {
    //        set
    //        {
    //            SetEvent(value, "Race");
    //        }
    //    }

    //    public void SetEvent(bool isEventEnabled, string eventName)
    //    {
    //        if (!isEventEnabled)
    //        {
    //            motorSportSeries.ExcludedEvents.Add(eventName);
    //        }
    //        else
    //        {
    //            motorSportSeries.ExcludedEvents.Remove(eventName);
    //        }
    //    }

    //    public bool IsButtonEnabled => !this.IsSearching;

    //    public ICommand PullDatesCmd { get; }

    //    public ICommand GenerateIcsCmd { get; }

    //    public ICommand ReadIcsCmd { get; }

    //    public ICommand DeleteIcsCmd { get; }

    //    public ICommand EasterEggCmd { get; }

    //    public void OnPropertyChanged(string property)
    //    {
    //        if (PropertyChanged != null)
    //        {
    //            PropertyChanged(this, new PropertyChangedEventArgs(property));
    //        }
    //    }

    //    private void EasterEgg()
    //    {
    //        MessageBox.Show(this.easterEggMessage, this.easterEggTitle);
    //    }

    //    private async Task PullDates()
    //    {
    //        this.IsSearching = true;
    //        string unalteredMainHeader = this.MainHeader;
    //        await Task.Run(() => this.ResultsOutput = this.scraper.ScrapeEventsToiCalendar(this.MotorSportSeries));
    //        this.OnPropertyChanged("MainHeader");
    //        this.MainHeader += $" {this.scraper.RacesFound(this.MotorSportSeries)} Races";
    //        this.IsSearching = false;
    //        MessageBox.Show($"DONE! \nScraped {this.scraper.RacesFound(this.MotorSportSeries)} Races \nScraped {this.scraper.EventsFound()} Events", $"{unalteredMainHeader}");
    //    }

    //    private void GenerateIcs()
    //    {
    //        this.SubHeader = $"{this.motorSportSeries.FilePath}";
    //        this.ICalendarResults = this.scraper.GenerateiCalendar(this.motorSportSeries, this.IsReminderActive, this.EventTriggerMinutes);
    //    }

    //    private void ReadIcs()
    //    {
    //        this.SubHeader = $"{this.motorSportSeries.FilePath}";
    //        this.ICalendarResults = this.scraper.ReadiCalendar(this.motorSportSeries);
    //    }

    //    private void DeleteIcs()
    //    {
    //        this.SubHeader = $"{this.motorSportSeries.FilePath}";
    //        this.ICalendarResults = this.scraper.DeleteiCalendar(this.motorSportSeries);
    //    }

    //    private void SetFormula1Instance()
    //    {
    //        if (this.isSearchingF1)
    //        {
    //            this.MotorSportSeries = new Formula1();
    //            this.mainHeader = "Formula 1 Calendar Results";

    //            //IsF1RaceEventEnabled = false;
    //            //IsF1QualifyingEventEnabled = false;                
    //            //IsF1PracticeEventEnabled = false;
    //        }
    //    }

    //    private void SetMotoGPInstance()
    //    {
    //        if (this.isSearchingMotoGP)
    //        {
    //            this.MotorSportSeries = new MotoGP();
    //            this.mainHeader = "MotoGP Calendar Results";

    //            //IsMotoGPQualifyingEventEnabled = false;
    //            //IsMotoGPPracticeEventEnabled = false;
    //            //IsMotoGPWarmUpEventEnabled = false;
    //            //IsMotoGPRaceEventEnabled = false;
    //            //IsMotoGPBehindTheScenesEventEnabled = false;
    //            //IsMotoGPAfterTheFlagEventEnabled = false;
    //        }
    //    }

    //    private void SetWorldSBKInstance()
    //    {
    //        if (this.isSearchingWorldSBK)
    //        {
    //            this.MotorSportSeries = new WorldSBK();
    //            this.mainHeader = "WorldSBK Calendar Results";

    //            //IsWSBKPracticeEventEnabled = false;
    //            //IsWSBKQualifyingEventEnabled = false;
    //            //IsWSBKRaceEventEnabled = false;
    //            //IsWSBKWarmUpEventEnabled = false;
    //        }
    //    }

    //    private bool CanExecuteCmd(object parameter)
    //    {
    //        return parameter != null;
    //    }
    //}
}