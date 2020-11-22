using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoiCal.Models
{
    public enum CalendarEventTrigger
    {
        [Description("At Time of Event")]
        AtTimeOfEvent = 0,
        [Description("5 Minutes Before Event")]
        Minutes5 = 5,
        [Description("15 Minutes Before Event")]
        Minutes15 = 15,
        [Description("30 Minutes Before Event")]
        Minutes30 = 30,
        [Description("45 Minutes Before Event")]
        Minutes45 = 45,
        [Description("1 Hour Before Event")]
        Minutes60 = 60,
        [Description("2 Hours Before Event")]
        Minutes120 = 120
    }
}
