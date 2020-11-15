using MotoiCal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            this.IsWarmUpSaved = false;
            this.IsSuperpoleSaved = true;
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
                this.OnPropertyChanged("IsWarmUpSaved");
            }
        }
    }
}
