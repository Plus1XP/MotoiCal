using MotoiCal.Models;

namespace MotoiCal.ViewModels.Settings
{
    class MotoGPSettingsContentViewModel : SettingsContentViewModel
    {
        private bool isWarmUpSaved;
        private bool isMoto2Saved;
        private bool isMoto3Saved;
        private bool isBehindTheScenesSaved;
        private bool isAfterTheFlagSaved;

        public MotoGPSettingsContentViewModel(MotorSport motorSport) : base(motorSport)
        {
            this.IsQualifyingVisible = true;
            this.IsSuperpoleVisible = false;
            this.IsWarmupVisible = true;
            this.IsMoto2Visible = true;
            this.IsMoto3Visible = true;
            this.IsBehindTheScenesVisible = true;
            this.IsAfterTheFlagVisible = true;

            this.IsWarmUpSaved = this.SettingsData.GetToggleSwitchValue("Warmup");
            this.IsMoto2Saved = this.SettingsData.GetToggleSwitchValue("Moto2");
            this.IsMoto3Saved = this.SettingsData.GetToggleSwitchValue("Moto3");
            this.IsBehindTheScenesSaved = this.SettingsData.GetToggleSwitchValue("BehindTheScenes");
            this.IsAfterTheFlagSaved = this.SettingsData.GetToggleSwitchValue("AfterTheFlag");
        }

        public override bool IsQualifyingVisible { get; }
        public override bool IsSuperpoleVisible { get; }
        public override bool IsWarmupVisible { get; }
        public override bool IsMoto2Visible { get; }
        public override bool IsMoto3Visible { get; }
        public override bool IsBehindTheScenesVisible { get; }
        public override bool IsAfterTheFlagVisible { get; }

        public bool IsWarmUpSaved
        {
            get
            {
                return this.isWarmUpSaved;
            }
            set
            {
                this.isWarmUpSaved = value;
                this.UpdateIMotorSportEvenList(value, "Warm Up");
                this.SettingsData.SetToggleSwitchValue("Warmup", value);
                this.OnPropertyChanged("IsWarmUpSaved");
            }
        }

        public bool IsMoto2Saved
        {
            get
            {
                return this.isMoto2Saved;
            }
            set
            {
                this.isMoto2Saved = value;
                this.UpdateIMotorSportSeriesList(value, "Moto2");
                this.SettingsData.SetToggleSwitchValue("Moto2", value);
                this.OnPropertyChanged("IsMoto2Saved");
            }
        }

        public bool IsMoto3Saved
        {
            get
            {
                return this.isMoto3Saved;
            }
            set
            {
                this.isMoto3Saved = value;
                this.UpdateIMotorSportSeriesList(value, "Moto3");
                this.SettingsData.SetToggleSwitchValue("Moto3", value);
                this.OnPropertyChanged("IsMoto2Saved");
            }
        }

        public bool IsBehindTheScenesSaved
        {
            get
            {
                return this.isBehindTheScenesSaved;
            }
            set
            {
                this.isBehindTheScenesSaved = value;
                this.UpdateIMotorSportEvenList(value, "behind the scenes");
                this.SettingsData.SetToggleSwitchValue("BehindTheScenes", value);
                this.OnPropertyChanged("IsBehindTheScenesSaved");
            }
        }

        public bool IsAfterTheFlagSaved
        {
            get
            {
                return this.isAfterTheFlagSaved;
            }
            set
            {
                this.isAfterTheFlagSaved = value;
                this.UpdateIMotorSportEvenList(value, "After The Flag");
                this.SettingsData.SetToggleSwitchValue("AfterTheFlag", value);
                this.OnPropertyChanged("IsAfterTheFlagSaved");
            }
        }

    }
}
