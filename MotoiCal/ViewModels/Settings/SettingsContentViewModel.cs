using MotoiCal.Enums;
using MotoiCal.Models;
using MotoiCal.Models.ButtonManagement;
using MotoiCal.Utilities.Commands;

using System;
using System.ComponentModel;

namespace MotoiCal.ViewModels.Settings
{
    abstract class SettingsContentViewModel : INotifyPropertyChanged
    {
        private ButtonManagerModel buttonManager;

        public XMLSettingsDataModel SettingsData;

        private MotorSport motorSportSeries;

        private bool isPracticeSaved;
        private bool isQualifyingSaved;
        private bool isRaceSaved;

        public SettingsContentViewModel(MotorSport motorSportSeries)
        {
            this.motorSportSeries = motorSportSeries;

            this.SettingsData = new XMLSettingsDataModel(motorSportSeries);

            this.buttonManager = new ButtonManagerModel();

            this.AtEventCommand = new SyncCommand(this.AtEvent);
            this.Minutes5EventCommand = new SyncCommand(this.Minutes5Event);
            this.Minutes15EventCommand = new SyncCommand(this.Minutes15Event);
            this.Minutes30EventCommand = new SyncCommand(this.Minutes30Event);
            this.Minutes45EventCommand = new SyncCommand(this.Minutes45Event);
            this.Minutes60EventCommand = new SyncCommand(this.Minutes60Event);
            this.Minutes120EventCommand = new SyncCommand(this.Minutes120Event);

            this.buttonManager.AddButton(this.AtEventButtonStatus = new ButtonStatusModel("At Event", "Set event to remind you at the time of event"));
            this.buttonManager.AddButton(this.Minutes5EventButtonStatus = new ButtonStatusModel("5 Mins", "Set event to remind you 5 Minutes before the time of event"));
            this.buttonManager.AddButton(this.Minutes15EventButtonStatus = new ButtonStatusModel("15 Mins", "Set event to remind you 15 Minutes before the time of event"));
            this.buttonManager.AddButton(this.Minutes30EventButtonStatus = new ButtonStatusModel("30 Mins", "Set event to remind you 30 Minutes before the time of event"));
            this.buttonManager.AddButton(this.Minutes45EventButtonStatus = new ButtonStatusModel("45 Mins", "Set event to remind you 45 Minutes before the time of event"));
            this.buttonManager.AddButton(this.Minutes60EventButtonStatus = new ButtonStatusModel("60 Mins", "Set event to remind you 60 Minutes before the time of event"));
            this.buttonManager.AddButton(this.Minutes120EventButtonStatus = new ButtonStatusModel("120 Mins", "Set event to remind you 120 Minutes before the time of event"));

            this.AtEventButtonStatus.ButtonStatusChanged = new EventHandler(this.AtEventButtonActive);
            this.Minutes5EventButtonStatus.ButtonStatusChanged = new EventHandler(this.Minutes5EventButtonActive);
            this.Minutes15EventButtonStatus.ButtonStatusChanged = new EventHandler(this.Minutes15EventButtonActive);
            this.Minutes30EventButtonStatus.ButtonStatusChanged = new EventHandler(this.Minutes30EventButtonActive);
            this.Minutes45EventButtonStatus.ButtonStatusChanged = new EventHandler(this.Minutes45EventButtonActive);
            this.Minutes60EventButtonStatus.ButtonStatusChanged = new EventHandler(this.Minutes60EventButtonActive);
            this.Minutes120EventButtonStatus.ButtonStatusChanged = new EventHandler(this.Minutes120EventButtonActive);

            this.IsPracticeSaved = this.SettingsData.GetToggleSwitchValue("Practice");
            this.IsQualifyingSaved = this.SettingsData.GetToggleSwitchValue("Qualifying");
            this.IsRaceSaved = this.SettingsData.GetToggleSwitchValue("Race");
            this.IsEventReminderActive = this.SettingsData.GetToggleSwitchValue("Reminder");
            this.SetEventTriggerInterval(this.SettingsData.GetToggleSwitchValueAsInt("Trigger"));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public abstract bool IsQualifyingVisible { get; }
        public abstract bool IsSuperpoleVisible { get; }
        public abstract bool IsWarmupVisible { get; }
        public abstract bool IsMoto2Visible { get; }
        public abstract bool IsMoto3Visible { get; }
        public abstract bool IsBehindTheScenesVisible { get; }
        public abstract bool IsAfterTheFlagVisible { get; }

        public virtual bool IsPracticeSaved
        {
            get
            {
                return this.isPracticeSaved;
            }
            set
            {
                this.isPracticeSaved = value;
                this.UpdateIMotorSportEvenList(value, "Practice");
                this.SettingsData.SetToggleSwitchValue("Practice", value);
                this.OnPropertyChanged("IsPracticeSaved");
            }
        }

        public bool IsQualifyingSaved
        {
            get
            {
                return this.isQualifyingSaved;
            }
            set
            {
                this.isQualifyingSaved = value;
                this.UpdateIMotorSportEvenList(value, "Qualifying");
                this.SettingsData.SetToggleSwitchValue("Qualifying", value);
                this.OnPropertyChanged("IsQualifyingSaved");
            }
        }

        public bool IsRaceSaved
        {
            get
            {
                return this.isRaceSaved;
            }
            set
            {
                this.isRaceSaved = value;
                this.UpdateIMotorSportEvenList(value, "Race");
                this.SettingsData.SetToggleSwitchValue("Race", value);
                this.OnPropertyChanged("IsRaceSaved");
            }
        }

        public bool IsEventReminderActive
        {
            get
            {
                return this.GetIMotorSportEventTriggerStatus();
            }
            set
            {
                this.SetIMotorSportEventTriggerStatus(value);
                this.SettingsData.SetToggleSwitchValue("Reminder", value);
                this.OnPropertyChanged("IsEventReminderActive");
                this.OnPropertyChanged("IsEventIntervalButtonEnabled");
            }
        }
        public int EventTriggerInterval
        {
            get
            {
                return this.GetIMotorSportEventTriggerMins();
            }
            set
            {
                this.SetIMotorSportEventTriggerMins(value);
                this.SettingsData.SetToggleSwitchValueAsInt("Trigger", value);
                this.OnPropertyChanged("EventTriggerInterval");
            }
        }

        public bool IsEventIntervalButtonEnabled => this.GetIMotorSportEventTriggerStatus();

        public SyncCommand AtEventCommand { get; }
        public SyncCommand Minutes5EventCommand { get; }
        public SyncCommand Minutes15EventCommand { get; }
        public SyncCommand Minutes30EventCommand { get; }
        public SyncCommand Minutes45EventCommand { get; }
        public SyncCommand Minutes60EventCommand { get; }
        public SyncCommand Minutes120EventCommand { get; }

        public ButtonStatusModel AtEventButtonStatus { get; set; }
        public ButtonStatusModel Minutes5EventButtonStatus { get; set; }
        public ButtonStatusModel Minutes15EventButtonStatus { get; set; }
        public ButtonStatusModel Minutes30EventButtonStatus { get; set; }
        public ButtonStatusModel Minutes45EventButtonStatus { get; set; }
        public ButtonStatusModel Minutes60EventButtonStatus { get; set; }
        public ButtonStatusModel Minutes120EventButtonStatus { get; set; }

        private void AtEventButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("AtEventButtonStatus");
        }

        private void Minutes5EventButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("Minutes5EventButtonStatus");
        }

        private void Minutes15EventButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("Minutes15EventButtonStatus");
        }

        private void Minutes30EventButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("Minutes30EventButtonStatus");
        }

        private void Minutes45EventButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("Minutes45EventButtonStatus");
        }

        private void Minutes60EventButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("Minutes60EventButtonStatus");
        }

        private void Minutes120EventButtonActive(object sender, EventArgs e)
        {
            this.OnPropertyChanged("Minutes120EventButtonStatus");
        }

        private void AtEvent()
        {
            this.buttonManager.SetActiveButton(this.AtEventButtonStatus);
            this.EventTriggerInterval = (int)CalendarEventTrigger.AtTimeOfEvent;
        }

        private void Minutes5Event()
        {
            this.buttonManager.SetActiveButton(this.Minutes5EventButtonStatus);
            this.EventTriggerInterval = (int)CalendarEventTrigger.Minutes5;
        }

        private void Minutes15Event()
        {
            this.buttonManager.SetActiveButton(this.Minutes15EventButtonStatus);
            this.EventTriggerInterval = (int)CalendarEventTrigger.Minutes15;
        }

        private void Minutes30Event()
        {
            this.buttonManager.SetActiveButton(this.Minutes30EventButtonStatus);
            this.EventTriggerInterval = (int)CalendarEventTrigger.Minutes30;
        }

        private void Minutes45Event()
        {
            this.buttonManager.SetActiveButton(this.Minutes45EventButtonStatus);
            this.EventTriggerInterval = (int)CalendarEventTrigger.Minutes45;
        }

        private void Minutes60Event()
        {
            this.buttonManager.SetActiveButton(this.Minutes60EventButtonStatus);
            this.EventTriggerInterval = (int)CalendarEventTrigger.Minutes60;
        }

        private void Minutes120Event()
        {
            this.buttonManager.SetActiveButton(this.Minutes120EventButtonStatus);
            this.EventTriggerInterval = (int)CalendarEventTrigger.Minutes120;
        }

        private bool GetIMotorSportEventTriggerStatus()
        {
            return this.motorSportSeries.IsEventReminderActive;
        }

        private void SetIMotorSportEventTriggerStatus(bool isEventTriggerActive)
        {
            this.motorSportSeries.IsEventReminderActive = isEventTriggerActive;
        }

        private int GetIMotorSportEventTriggerMins()
        {
            return this.motorSportSeries.EventReminderMins;
        }

        private void SetIMotorSportEventTriggerMins(int minsToTrigger)
        {
            this.motorSportSeries.EventReminderMins = minsToTrigger;
        }

        // Sets if the event is scraped or not depending on bool value.
        public void UpdateIMotorSportEvenList(bool isEventEnabled, string eventName)
        {
            if (!isEventEnabled)
            {
                this.motorSportSeries.ExcludedEvents.Add(eventName);
            }
            else
            {
                this.motorSportSeries.ExcludedEvents.Remove(eventName);
            }
        }

        // Sets if the Series is scraped or not depending on bool value.
        public void UpdateIMotorSportSeriesList(bool isEventEnabled, string eventName)
        {
            if (!isEventEnabled)
            {
                this.motorSportSeries.ExcludedClasses.Add(eventName);
            }
            else
            {
                this.motorSportSeries.ExcludedClasses.Remove(eventName);
            }
        }

        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void SetEventTriggerInterval(int interval)
        {
            switch (interval)
            {
                case 0:
                    this.AtEvent();
                    break;
                case 5:
                    this.Minutes5Event();
                    break;
                case 15:
                    this.Minutes15Event();
                    break;
                case 30:
                    this.Minutes30Event();
                    break;
                case 45:
                    this.Minutes45Event();
                    break;
                case 60:
                    this.Minutes60Event();
                    break;
                case 120:
                    this.Minutes120Event();
                    break;
                default:
                    this.Minutes15Event();
                    break;
            }
        }
    }
}
