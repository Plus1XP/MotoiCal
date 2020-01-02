using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoiCal.Models
{
    public class CalendarManager
    {
        public StringBuilder calendarEntry;

        public CalendarManager()
        {

        }

        public DateTime ParseDateTimeToUTC(string dateTime)
        {
            DateTime.TryParse(dateTime, out DateTime parsedDateTime);
            return parsedDateTime.ToUniversalTime();
        }

        public DateTime ParseDateTimeToLocal(string dateTime)
        {
            DateTime.TryParse(dateTime, out DateTime parsedDateTime);
            return parsedDateTime.ToLocalTime();
        }

        public void CreateCalendarEntry()
        {
            // Creates the iCalendar entry.
            this.calendarEntry = new StringBuilder();
            this.calendarEntry.AppendLine("BEGIN:VCALENDAR");
            this.calendarEntry.AppendLine("VERSION:2.0");
            this.calendarEntry.AppendLine("PRODID:-//github.com/aleuts//MotoiCal//EN");
        }
    }
}
