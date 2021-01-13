using MotoiCal.Interfaces;
using MotoiCal.Models;
using MotoiCal.Models.FileManagement;

using System.Collections.ObjectModel;

namespace MotoiCal.Services
{
    class CalendarService
    {
        private readonly CalendarManager iCalendar;

        public CalendarService()
        {
            this.iCalendar = new CalendarManager();
        }

        public string GenerateiCalendar(ICalendarEvent motorSport, ObservableCollection<IRaceTimeTable> timeTable)
        {
            // Checks if url list has a value, if not then it is assumed the dates have not been pulled.
            //if (motorSport.EventUrlList?.Any() != true)
            if (string.IsNullOrEmpty(motorSport.IcalendarLocation))
            {
                return "Can not generate ICS file without first showing dates";
            }
            else
            {
                this.ProcessiCalendarResults(timeTable, motorSport.IsEventReminderActive, motorSport.EventReminderMins);
                return this.iCalendar.CreateICSFile(motorSport.FilePath);
            }
        }

        private void ProcessiCalendarResults(ObservableCollection<IRaceTimeTable> timeTable, bool isReminderActive, int eventTriggerMinutes)
        {
            this.iCalendar.CreateCalendarEntry();
            /*
            this.iCalendar.CreateTimeZone();
            */
            foreach (MotorSport item in timeTable)
            {
                this.iCalendar.CreateCalendarEventEntry(item.StartUTC, item.EndUTC, item.IcalendarSubject, item.IcalendarLocation, item.IcalendarDescription);
                if (isReminderActive)
                {
                    this.iCalendar.CreateCalendarAlarmEntry(eventTriggerMinutes, item.IcalendarSubject);
                }
                this.iCalendar.CloseEventEntry();
            }
            this.iCalendar.CloseCalendarEntry();
        }

    }
}
