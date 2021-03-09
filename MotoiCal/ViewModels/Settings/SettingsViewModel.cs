using MotoiCal.Models;
using MotoiCal.Models.ButtonManagement;
using MotoiCal.Utilities.Commands;
using MotoiCal.Views.Settings;

using System;
using System.ComponentModel;
using System.Windows;

namespace MotoiCal.ViewModels.Settings
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        private ButtonManagerModel buttonManagerModel;

        private FrameworkElement settingsContentView;

        private Formula1SettingsContentViewModel formula1SettingsContent;
        private MotoGPSettingsContentViewModel motoGPSettingsContent;
        private WorldSBKSettingsContentViewModel worldSBKSettingsContent;
        private EmailSettingsContentViewModel emailSettingsContent;

        private AboutContentViewModel aboutContent;

        private MotorSport formula1;
        private MotorSport motoGP;
        private MotorSport worldSBK;

        public SettingsViewModel(MotorSport formula1, MotorSport motoGP, MotorSport worldSBK)
        {
            this.formula1 = formula1;
            this.motoGP = motoGP;
            this.worldSBK = worldSBK;

            this.buttonManagerModel = new ButtonManagerModel();

            this.FormulaOneParametersCommand = new SyncCommand(this.FormulaOneParameters);
            this.MotoGPParametersCommand = new SyncCommand(this.MotoGPParameters);
            this.WorldSBKParametersCommand = new SyncCommand(this.WorldSBKParameters);
            this.EmailParametersCommand = new SyncCommand(this.EmailParameters);
            this.AboutCommand = new SyncCommand(this.About);

            this.buttonManagerModel.AddButton(this.FormulaOneParametersButtonStatus = new ButtonStatusModel("Formula One", "Configure Formula One Search Settings"));
            this.buttonManagerModel.AddButton(this.MotoGPParametersButtonStatus = new ButtonStatusModel("MotoGP", "Configure MotoGP Search Settings"));
            this.buttonManagerModel.AddButton(this.WorldSBKParametersButtonStatus = new ButtonStatusModel("World SBK", "Configure World SBK Search Settings"));
            this.buttonManagerModel.AddButton(this.EmailParametersButtonStatus = new ButtonStatusModel("Email", "Configure Email Settings"));
            this.buttonManagerModel.AddButton(this.AboutButtonStatus = new ButtonStatusModel("About", "Display information about this program"));

            this.FormulaOneParametersButtonStatus.ButtonStatusChanged = new EventHandler(this.FormulaOneButtonActive);
            this.MotoGPParametersButtonStatus.ButtonStatusChanged = new EventHandler(this.MotoGPButtonActive);
            this.WorldSBKParametersButtonStatus.ButtonStatusChanged = new EventHandler(this.WorldSBKButtonActive);
            this.EmailParametersButtonStatus.ButtonStatusChanged = new EventHandler(this.EmailButtonActive);
            this.AboutButtonStatus.ButtonStatusChanged = new EventHandler(this.AboutButtonActive);

            this.formula1SettingsContent = new Formula1SettingsContentViewModel(this.formula1);
            this.motoGPSettingsContent = new MotoGPSettingsContentViewModel(this.motoGP);
            this.worldSBKSettingsContent = new WorldSBKSettingsContentViewModel(this.worldSBK);
            this.emailSettingsContent = new EmailSettingsContentViewModel();
            this.aboutContent = new AboutContentViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public SyncCommand FormulaOneParametersCommand { get; }
        public SyncCommand MotoGPParametersCommand { get; }
        public SyncCommand WorldSBKParametersCommand { get; }
        public SyncCommand EmailParametersCommand { get; }
        public SyncCommand AboutCommand { get; }

        public ButtonStatusModel FormulaOneParametersButtonStatus { get; set; }
        public ButtonStatusModel MotoGPParametersButtonStatus { get; set; }
        public ButtonStatusModel WorldSBKParametersButtonStatus { get; set; }
        public ButtonStatusModel EmailParametersButtonStatus { get; set; }
        public ButtonStatusModel AboutButtonStatus { get; set; }

        public FrameworkElement SettingsContentView
        {
            get { return this.settingsContentView; }
            set
            {
                this.settingsContentView = value;
                this.OnPropertyChanged("SettingsContentView");
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
            this.OnPropertyChanged("FormulaOneParametersButtonStatus");
        }

        private void MotoGPButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("MotoGPParametersButtonStatus");
        }

        private void WorldSBKButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("WorldSBKParametersButtonStatus");
        }

        private void EmailButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("EmailParametersButtonStatus");
        }

        private void AboutButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("AboutButtonStatus");
        }

        private void FormulaOneParameters()
        {
            this.buttonManagerModel.SetActiveButton(this.FormulaOneParametersButtonStatus);
            this.SettingsContentView = new SettingsContentView();
            this.SettingsContentView.DataContext = this.formula1SettingsContent;
        }

        private void MotoGPParameters()
        {
            this.buttonManagerModel.SetActiveButton(this.MotoGPParametersButtonStatus);
            this.SettingsContentView = new SettingsContentView();
            this.SettingsContentView.DataContext = this.motoGPSettingsContent;
        }

        private void WorldSBKParameters()
        {
            this.buttonManagerModel.SetActiveButton(this.WorldSBKParametersButtonStatus);
            this.SettingsContentView = new SettingsContentView();
            this.SettingsContentView.DataContext = this.worldSBKSettingsContent;
        }

        private void EmailParameters()
        {
            this.buttonManagerModel.SetActiveButton(this.EmailParametersButtonStatus);
            this.SettingsContentView = new EmailSettingsContentView();
            this.SettingsContentView.DataContext = new EmailSettingsContentViewModel();
        }

        private void About()
        {
            this.buttonManagerModel.SetActiveButton(this.AboutButtonStatus);
            this.SettingsContentView = new AboutContentView();
            this.SettingsContentView.DataContext = new AboutContentViewModel(); //this.aboutContent;
        }
    }
}
