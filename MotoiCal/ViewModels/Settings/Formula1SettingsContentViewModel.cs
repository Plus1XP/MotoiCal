using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotoiCal.Models;

namespace MotoiCal.ViewModels.Settings
{
    class Formula1SettingsContentViewModel : SettingsContentViewModel
    {
        public Formula1SettingsContentViewModel(IMotorSport motorSport) : base(motorSport)
        {
            this.IsQualifyingVisible = true;
            this.IsSuperpoleVisible = false;
            this.IsWarmupVisible = false;
            this.IsBehindTheScenesVisible = false;
            this.IsAfterTheFlagVisible = false;
        }

        public override bool IsQualifyingVisible { get; }
        public override bool IsSuperpoleVisible { get; }
        public override bool IsWarmupVisible { get; }
        public override bool IsBehindTheScenesVisible { get; }
        public override bool IsAfterTheFlagVisible { get; }
    }
}
