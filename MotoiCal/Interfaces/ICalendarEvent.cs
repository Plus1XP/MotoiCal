namespace MotoiCal.Interfaces
{
    public interface ICalendarEvent
    {
        string DisplayHeader { get; }
        string DisplayBody { get; }
        string IcalendarSubject { get; }
        string IcalendarLocation { get; }
        string IcalendarDescription { get; }
        bool IsEventReminderActive { get; set; }
        int EventReminderMins { get; set; }
        string FilePath { get; }
    }
}
