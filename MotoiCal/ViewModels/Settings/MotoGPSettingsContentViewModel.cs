using MotoiCal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            this.IsBehindTheScenesSaved = false;
            this.IsWarmUpSaved = false; 
            this.IsAfterTheFlagSaved = false;
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
                this.SetEvent(value, "Warm Up");
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
                this.SetEvent(value, "behind the scenes");
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
                this.SetEvent(value, "After The Flag");
                this.OnPropertyChanged("IsAfterTheFlagSaved");
            }
        }

    }
}
