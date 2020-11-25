using MotoiCal.Models;

namespace MotoiCal.ViewModels.Settings
{
    class MotoGPSettingsContentViewModel : SettingsContentViewModel
    {
        private bool isWarmUpSaved;
        private bool isBehindTheScenesSaved;
        private bool isAfterTheFlagSaved;

        public MotoGPSettingsContentViewModel(IMotorSport motorSport) : base(motorSport)
        {
            this.IsQualifyingVisible = true;
            this.IsSuperpoleVisible = false;
            this.IsWarmupVisible = true;
            this.IsBehindTheScenesVisible = true;
            this.IsAfterTheFlagVisible = true;

            this.IsWarmUpSaved = this.SettingsData.GetToggleSwitchValue("Warmup");
            this.IsBehindTheScenesSaved = this.SettingsData.GetToggleSwitchValue("BehindTheScenes");
            this.IsAfterTheFlagSaved = this.SettingsData.GetToggleSwitchValue("AfterTheFlag");
        }

        public override bool IsQualifyingVisible { get; }
        public override bool IsSuperpoleVisible { get; }
        public override bool IsWarmupVisible { get; }
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
