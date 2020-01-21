using System;
using System.Text;

namespace MotoiCal.Models
{
    public class CalendarManager
    {
        private StringBuilder calendarEntry;
        private readonly FileManager fileManager;

        public CalendarManager()
        {
            this.fileManager = new FileManager();
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

        private void ValidateFileIsUsable(string filePath)
        {
            // If file has been created => clear file, otherwise create new file.
            if (this.fileManager.IsFileCreated(filePath))
            {
                this.fileManager.ClearFile(filePath);
            }
            else
            {
                this.fileManager.CreateFile(filePath);
            }
        }

        public string CreateICSFile(string filePath)
        {
            // After validating the ICS file the iCalendar entries will be writtern and confirmation will be returned.
            this.ValidateFileIsUsable(filePath);
            this.fileManager.SaveToFile(filePath, this.RetrieveCalendarEntries());
            return "ICS file created";
        }

        public string ReadICSFile(string filePath)
        {
            // If file exists return all lines from file, otherwise return string "No Data".
            return this.fileManager.IsFileCreated(filePath) ? this.fileManager.ReadFromFile(filePath) : "No Data";
        }

        public string DeleteICSFile(string filePath)
        {
            // If file exists delete file and return confirmation, otherwise return string "No File".
            if (this.fileManager.IsFileCreated(filePath))
            {
                this.fileManager.DeleteFile(filePath);
                return $"{filePath} Deleted";
            }
            else
            {
                return "No File";
            }
        }
    }
}