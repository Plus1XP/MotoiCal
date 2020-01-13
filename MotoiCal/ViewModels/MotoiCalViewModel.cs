using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows;

using MotoiCal.Models;

namespace MotoiCal.ViewModels
{
    public class MotoiCalViewModel : INotifyPropertyChanged
    {
        private Scraper scraper;
        private IMotorSport motorSportSeries;
        private string resultsOutput;
        private string iCalendarResults;
        private string mainHeader;
        private string subHeader;
        private bool isSearchingF1;
        private bool isSearchingMotoGP;
        private bool isSearchingWSBK;

        public MotoiCalViewModel()
        {
            
        }

        public string AppVersion => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

        public string AppTitle => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName;

        public string WindowTitle => $"{this.AppTitle} v{this.AppVersion}";

        public string SearchingFormula1Content => "Formula1";

        public string SearchingMotoGPContent => "MotoGP";

        public string SearchingWSBKContent => "WSBK";

        public IMotorSport MotorSportSeries
        {
            get => this.motorSportSeries;
            set
            {
                this.motorSportSeries = value;
                this.OnPropertyChanged("MotorSportSeries");
            }
        }

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

        public bool IsSearchingWSBK
        {
            get => this.isSearchingWSBK;
            set
            {
                this.isSearchingWSBK = value;
                this.SetWSBKInstance();
                this.OnPropertyChanged("IsSearchingWSBK");
            }

        }

        public RelayCommand PullDatesCMD { get; }

        public RelayCommand GenerateICSCMD { get; }

        public RelayCommand ReadICSCMD { get; }

        public RelayCommand DeleteICSCMD { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        private void PullDates()
        {
            this.OnPropertyChanged("MainHeader");
            this.ResultsOutput = this.scraper.ScrapeEventsToiCalendar(this.MotorSportSeries);
            MessageBox.Show("DONE!", $"{this.MainHeader}");
        }

        private void GenerateICS()
        {
            this.SubHeader = $"{this.motorSportSeries.FilePath}";
            this.ICalendarResults = this.scraper.GenerateiCalendar(this.motorSportSeries);
        }

        private void ReadICS()
        {
            this.SubHeader = $"{this.motorSportSeries.FilePath}";
            this.ICalendarResults = this.scraper.ReadiCalendar(this.motorSportSeries);
        }

        private void DeleteICS()
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

        private void SetWSBKInstance()
        {
            if (this.isSearchingWSBK)
            {
                this.motorSportSeries = new WSBK();
                this.mainHeader = "WSBK Calendar Results";
            }
        }
    }
}
