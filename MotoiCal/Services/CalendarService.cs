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
    }
}
