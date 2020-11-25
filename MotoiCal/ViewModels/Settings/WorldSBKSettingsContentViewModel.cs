using MotoiCal.Models;

namespace MotoiCal.ViewModels.Settings
{
    class WorldSBKSettingsContentViewModel : SettingsContentViewModel
    {
        private bool isPracticeSaved;
        private bool isSuperpoleSaved;
        private bool isWarmUpSaved;

        public WorldSBKSettingsContentViewModel(IMotorSport motorSport) : base(motorSport)
        {
            this.IsQualifyingVisible = false;
            this.IsSuperpoleVisible = true;
            this.IsWarmupVisible = true;
            this.IsBehindTheScenesVisible = false;
            this.IsAfterTheFlagVisible = false;

            this.IsSuperpoleSaved = this.SettingsData.GetToggleSwitchValue("Superpole");
            this.IsWarmUpSaved = this.SettingsData.GetToggleSwitchValue("Warmup");
        }

        public override bool IsQualifyingVisible { get; }
        public override bool IsSuperpoleVisible { get; }
        public override bool IsWarmupVisible { get; }
        public override bool IsBehindTheScenesVisible { get; }
        public override bool IsAfterTheFlagVisible { get; }

        public override bool IsPracticeSaved
        {
            get
            {
                return this.isPracticeSaved;
            }
            set
            {
                this.isPracticeSaved = value;
                this.UpdateIMotorSportEvenList(value, "FP");
                this.SettingsData.SetToggleSwitchValue("Practice", value);
                this.OnPropertyChanged("IsPracticeSaved");
            }
        }

        public bool IsSuperpoleSaved
        {
            get
            {
                return this.isSuperpoleSaved;
            }
            set
            {
                this.isSuperpoleSaved = value;
                this.UpdateIMotorSportEvenList(value, "Superpole");
                this.SettingsData.SetToggleSwitchValue("Superpole", value);
                this.OnPropertyChanged("IsSuperpoleSaved");
            }
        }

        public bool IsWarmUpSaved
        {
            get
            {
                return this.isWarmUpSaved;
            }
            set
            {
                this.isWarmUpSaved = value;
                this.UpdateIMotorSportEvenList(value, "WUP");
                this.SettingsData.SetToggleSwitchValue("Warmup", value);
                this.OnPropertyChanged("IsWarmUpSaved");
            }
        }
    }
}
