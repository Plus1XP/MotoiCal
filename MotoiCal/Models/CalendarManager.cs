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

        public void CreateCalendarTimeZoneEntry()
        {
            // Add the time zone to the calendar item if needed.
            this.calendarEntry.AppendLine("BEGIN:VTIMEZONE");
            this.calendarEntry.AppendLine("TZID:GMT Standard Time");
            this.calendarEntry.AppendLine("BEGIN:STANDARD");
            this.calendarEntry.AppendLine("DTSTART:16011028T020000");
            this.calendarEntry.AppendLine("RRULE:FREQ=YEARLY;BYDAY=-1SU;BYMONTH=10");
            this.calendarEntry.AppendLine("TZOFFSETTO:+0100");
            this.calendarEntry.AppendLine("TZOFFSETFROM:-0000");
            this.calendarEntry.AppendLine("END:STANDARD");
            this.calendarEntry.AppendLine("BEGIN:DAYLIGHT");
            this.calendarEntry.AppendLine("DTSTART:16010325T010000");
            this.calendarEntry.AppendLine("RRULE:FREQ=YEARLY;BYDAY=-1SU;BYMONTH=3");
            this.calendarEntry.AppendLine("TZOFFSETTO:-0000");
            this.calendarEntry.AppendLine("TZOFFSETFROM:+0100");
            this.calendarEntry.AppendLine("END:DAYLIGHT");
            this.calendarEntry.AppendLine("END:VTIMEZONE");
        }

        public void CreateCalendarEventEntry(DateTime startTime, DateTime endTime, string subject, string location, string description)
        {
            // Add the event.
            this.calendarEntry.AppendLine("BEGIN:VEVENT");

            // Specify the date time with the time zone stamp. 
            /*
            this.calendarEntry.AppendLine("DTSTART;TZID=GMT Standard Time:" + startTime.ToString("yyyyMMddTHHmm00"));
            this.calendarEntry.AppendLine("DTEND;TZID=GMT Standard Time:" + endTime.ToString("yyyyMMddTHHmm00"));           
            */

            // Specify the date time in UTC (Z).
            this.calendarEntry.AppendLine("DTSTART:" + startTime.ToString("yyyyMMddTHHmm00Z"));
            this.calendarEntry.AppendLine("DTEND:" + endTime.ToString("yyyyMMddTHHmm00Z"));

            this.calendarEntry.AppendLine("SUMMARY:" + subject);
            this.calendarEntry.AppendLine("LOCATION:" + location);
            this.calendarEntry.AppendLine("DESCRIPTION:" + description);
            this.calendarEntry.AppendLine("END:VEVENT");
        }

        public void CloseCalendarEntry()
        {
            // Ends the icalendar entry.
            this.calendarEntry.AppendLine("END:VCALENDAR");
        }

        private string RetrieveCalendarEntries()
        {
            // Returns the string initially created with the iCalendar entry.
            return this.calendarEntry.ToString();
        }
    }
}
