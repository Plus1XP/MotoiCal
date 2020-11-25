using System;
using System.ComponentModel;
using System.Windows;

using MotoiCal.Models;
using MotoiCal.ViewModels.Settings;
using MotoiCal.Views;
using MotoiCal.Views.Settings;

namespace MotoiCal.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged //Rename this class
    {
        private ButtonManagerModel buttonManagerModel;

        private FrameworkElement controlContentView;

        private MotorSportContentViewModel formulaOneMotorSportContent;
        private MotorSportContentViewModel motoGPMotorSportContent;
        private MotorSportContentViewModel worldSBKMotorSportContent;
        private SettingsViewModel settingsMotorSportContent;

        private Formula1 formula1;
        private MotoGP motoGP;
        private WorldSBK worldSBK;

        //private IMotorSport motorSportSeries;

        public MainViewModel()
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

            this.buttonManagerModel = new ButtonManagerModel();

            this.buttonManagerModel.AddButton(this.FormulaOneButtonStatus = new ButtonStatusModel("Formula 1", "Pull Formula 1 Calendar"));
            this.buttonManagerModel.AddButton(this.MotoGPButtonStatus = new ButtonStatusModel("MotoGP", "Pull MotoGP Calendar"));
            this.buttonManagerModel.AddButton(this.WorldSBKButtonStatus = new ButtonStatusModel("WorldSBK", "Pull WorldSBK Calendar"));
            this.buttonManagerModel.AddButton(this.SettingsButtonStatus = new ButtonStatusModel("Settings", "Configure Parameters"));

            this.FormulaOneButtonStatus.ButtonStatusChanged = new EventHandler(this.FormulaOneButtonActive);
            this.MotoGPButtonStatus.ButtonStatusChanged = new EventHandler(this.MotoGPButtonActive);
            this.WorldSBKButtonStatus.ButtonStatusChanged = new EventHandler(this.WorldSBKButtonActive);
            this.SettingsButtonStatus.ButtonStatusChanged = new EventHandler(this.SettingsButtonActive);

            this.formula1 = new Formula1();
            this.motoGP = new MotoGP();
            this.worldSBK = new WorldSBK();

            this.formulaOneMotorSportContent = new MotorSportContentViewModel(this.formula1);
            this.motoGPMotorSportContent = new MotorSportContentViewModel(this.motoGP);
            this.worldSBKMotorSportContent = new MotorSportContentViewModel(this.worldSBK);
            this.settingsMotorSportContent = new SettingsViewModel(this.formula1, this.motoGP, this.worldSBK);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool canResizeWindow { get; set; }
        private bool canMinimizeWindow { get; set; }

        public SynchronousRelayCommand<Window> CloseWindowCommand { get; set; }
        public SynchronousRelayCommand<Window> MaximizeWindowCommand { get; set; }
        public SynchronousRelayCommand<Window> MinimizeWindowCommand { get; set; }
        public SynchronousRelayCommand<Window> RestoreWindowCommand { get; set; }

        public SynchronousRelayCommand FormulaOneViewCommand { get; }
        public SynchronousRelayCommand MotoGPViewCommand { get; }
        public SynchronousRelayCommand WorldSBKViewCommand { get; }
        public SynchronousRelayCommand SettingsViewCommand { get; }

        public ButtonStatusModel FormulaOneButtonStatus { get; set; }
        public ButtonStatusModel MotoGPButtonStatus { get; set; }
        public ButtonStatusModel WorldSBKButtonStatus { get; set; }
        public ButtonStatusModel SettingsButtonStatus { get; set; }

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

        private void FormulaOneButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("FormulaOneButtonStatus");
        }

        private void MotoGPButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("MotoGPButtonStatus");
        }

        private void WorldSBKButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("WorldSBKButtonStatus");
        }

        private void SettingsButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("SettingsButtonStatus");
        }

        private void Formula1Tab()
        {
            this.buttonManagerModel.SetActiveButton(this.FormulaOneButtonStatus);
            this.ContentControlView = new MotorSportContentView();
            this.ContentControlView.DataContext = this.formulaOneMotorSportContent;
        }

        private void MotoGPTab()
        {
            this.buttonManagerModel.SetActiveButton(this.MotoGPButtonStatus);
            this.ContentControlView = new MotorSportContentView();
            this.ContentControlView.DataContext = this.motoGPMotorSportContent;
        }

        private void WorldSBKTab()
        {
            this.buttonManagerModel.SetActiveButton(this.WorldSBKButtonStatus);
            this.ContentControlView = new MotorSportContentView();
            this.ContentControlView.DataContext = this.worldSBKMotorSportContent;
        }

        private void SettingsTab() // Maybe load all instances for IMotorSports in here,,
        {
            this.buttonManagerModel.SetActiveButton(this.SettingsButtonStatus);
            this.ContentControlView = new SettingsView();
            this.ContentControlView.DataContext = this.settingsMotorSportContent; // and parameter for IMotorSport here..
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
}