using System;
using System.Xml;

namespace MotoiCal.Models
{
    class XMLSettingsDataModel
    {
        private const string settings_Data_Location = ".\\Settings.xml";
        private const string formula1_Location = "/Settings/Formula1/Event";
        private const string motoGP_Location = "/Settings/MotoGP/Event";
        private const string worldSBK_Location = "/Settings/WorldSBK/Event";

        private string motorSportChildElementPath;

        private XMLManager xmlManager;

        public XMLSettingsDataModel(IMotorSport motorSportSeries)
        {
            this.xmlManager = new XMLManager();

            this.motorSportChildElementPath = this.GetMotorSportSeriesChildElementPath(motorSportSeries);

            if (!this.xmlManager.IsFileCreated(settings_Data_Location))
            {
                this.xmlManager.CreateFile(settings_Data_Location);
                this.CreateXMLSettingsDocument(settings_Data_Location);
            }
        }

        public bool GetToggleSwitchValue(string eventName)
        {
            return this.xmlManager.GetNodeAttributeValueAsBool(settings_Data_Location, this.motorSportChildElementPath, "Name", eventName, "Saved");
        }

        public void SetToggleSwitchValue(string eventName, bool eventValue)
        {
            this.xmlManager.SetNodeAttributeValueFromBool(settings_Data_Location, this.motorSportChildElementPath, "Name", eventName, "Saved", eventValue);
        }

        public int GetToggleSwitchValueAsInt(string eventName)
        {
            return this.xmlManager.GetNodeAttributeValueAsInt(settings_Data_Location, this.motorSportChildElementPath, "Name", eventName, "Tirgger");
        }

        public void SetToggleSwitchValueAsInt(string eventName, int eventValue)
        {
            this.xmlManager.SetNodeAttributeValueFromInt(settings_Data_Location, this.motorSportChildElementPath, "Name", eventName, "Saved", eventValue);
        }

        private string GetMotorSportSeriesChildElementPath(IMotorSport motorSportSeries)
        {
            switch (motorSportSeries.SportIdentifier)
            {
                case MotorSportID.Formula1:
                    return formula1_Location;
                case MotorSportID.MotoGP:
                    return motoGP_Location;
                case MotorSportID.WorldSBK:
                    return worldSBK_Location;
                default:
                    throw new ArgumentException($"{motorSportSeries} is invalid.");
            }
        }

        private void CreateXMLSettingsDocument(string dataLocation)
        {
            XmlDocument settingsDoc = new XmlDocument();
            XmlNode SettingsHeader = settingsDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            settingsDoc.AppendChild(SettingsHeader);

            //Settings
            XmlNode settingsRootElement = settingsDoc.CreateElement("Settings");
            settingsDoc.AppendChild(settingsRootElement);

            //Formula1
            XmlNode formula1ChildElement = settingsDoc.CreateElement("Formula1");
            settingsRootElement.AppendChild(formula1ChildElement);

            XmlNode formula1PracticeEventSubChildElement = settingsDoc.CreateElement("Event");
            formula1PracticeEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Practice";
            formula1PracticeEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "True";
            formula1ChildElement.AppendChild(formula1PracticeEventSubChildElement);

            XmlNode formula1QualifyingEventSubChildElement = settingsDoc.CreateElement("Event");
            formula1QualifyingEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Qualifying";
            formula1QualifyingEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "True";
            formula1ChildElement.AppendChild(formula1QualifyingEventSubChildElement);

            XmlNode formula1RaceEventSubChildElement = settingsDoc.CreateElement("Event");
            formula1RaceEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Race";
            formula1RaceEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "True";
            formula1ChildElement.AppendChild(formula1RaceEventSubChildElement);

            XmlNode formula1ReminderEventSubChildElement = settingsDoc.CreateElement("Event");
            formula1ReminderEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Reminder";
            formula1ReminderEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "True";
            formula1ChildElement.AppendChild(formula1ReminderEventSubChildElement);

            XmlNode formula1TriggerEventSubChildElement = settingsDoc.CreateElement("Event");
            formula1TriggerEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Trigger";
            formula1TriggerEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "15";
            formula1ChildElement.AppendChild(formula1TriggerEventSubChildElement);

            // MotoGP
            XmlNode motoGPChildElement = settingsDoc.CreateElement("MotoGP");
            settingsRootElement.AppendChild(motoGPChildElement);

            XmlNode motoGPPracticeEventSubChildElement = settingsDoc.CreateElement("Event");
            motoGPPracticeEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Practice";
            motoGPPracticeEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "True";
            motoGPChildElement.AppendChild(motoGPPracticeEventSubChildElement);

            XmlNode motoGPQualifyingEventSubChildElement = settingsDoc.CreateElement("Event");
            motoGPQualifyingEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Qualifying";
            motoGPQualifyingEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "True";
            motoGPChildElement.AppendChild(motoGPQualifyingEventSubChildElement);

            XmlNode motoGPWarmupEventSubChildElement = settingsDoc.CreateElement("Event");
            motoGPWarmupEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Warmup";
            motoGPWarmupEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "False";
            motoGPChildElement.AppendChild(motoGPWarmupEventSubChildElement);

            XmlNode motoGPRaceEventSubChildElement = settingsDoc.CreateElement("Event");
            motoGPRaceEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Race";
            motoGPRaceEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "True";
            motoGPChildElement.AppendChild(motoGPRaceEventSubChildElement);

            XmlNode motoGPBehindTheScenesEventSubChildElement = settingsDoc.CreateElement("Event");
            motoGPBehindTheScenesEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "BehindTheScenes";
            motoGPBehindTheScenesEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "False";
            motoGPChildElement.AppendChild(motoGPBehindTheScenesEventSubChildElement);

            XmlNode motoGPAfterTheFlagEventSubChildElement = settingsDoc.CreateElement("Event");
            motoGPAfterTheFlagEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "AfterTheFlag";
            motoGPAfterTheFlagEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "False";
            motoGPChildElement.AppendChild(motoGPAfterTheFlagEventSubChildElement);

            XmlNode motoGPReminderEventSubChildElement = settingsDoc.CreateElement("Event");
            motoGPReminderEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Reminder";
            motoGPReminderEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "True";
            motoGPChildElement.AppendChild(motoGPReminderEventSubChildElement);

            XmlNode motoGPTriggerEventSubChildElement = settingsDoc.CreateElement("Event");
            motoGPTriggerEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Trigger";
            motoGPTriggerEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "15";
            motoGPChildElement.AppendChild(motoGPTriggerEventSubChildElement);

            //WorldSBK
            XmlNode worldSBKChildElement = settingsDoc.CreateElement("WorldSBK");
            settingsRootElement.AppendChild(worldSBKChildElement);

            XmlNode worldSBKPracticeEventSubChildElement = settingsDoc.CreateElement("Event");
            worldSBKPracticeEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Practice";
            worldSBKPracticeEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "True";
            worldSBKChildElement.AppendChild(worldSBKPracticeEventSubChildElement);

            XmlNode worldSBKQualifyingEventSubChildElement = settingsDoc.CreateElement("Event");
            worldSBKQualifyingEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Superpole";
            worldSBKQualifyingEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "True";
            worldSBKChildElement.AppendChild(worldSBKQualifyingEventSubChildElement);

            XmlNode worldSBKWarmupEventSubChildElement = settingsDoc.CreateElement("Event");
            worldSBKWarmupEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Warmup";
            worldSBKWarmupEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "False";
            worldSBKChildElement.AppendChild(worldSBKWarmupEventSubChildElement);

            XmlNode worldSBKRaceEventSubChildElement = settingsDoc.CreateElement("Event");
            worldSBKRaceEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Race";
            worldSBKRaceEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "True";
            worldSBKChildElement.AppendChild(worldSBKRaceEventSubChildElement);

            XmlNode worldSBKReminderEventSubChildElement = settingsDoc.CreateElement("Event");
            worldSBKReminderEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Reminder";
            worldSBKReminderEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "True";
            worldSBKChildElement.AppendChild(worldSBKReminderEventSubChildElement);

            XmlNode worldSBKTriggerEventSubChildElement = settingsDoc.CreateElement("Event");
            worldSBKTriggerEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Name")).Value = "Trigger";
            worldSBKTriggerEventSubChildElement.Attributes.Append(settingsDoc.CreateAttribute("Saved")).Value = "15";
            worldSBKChildElement.AppendChild(worldSBKTriggerEventSubChildElement);

            settingsDoc.Save(dataLocation);
        }
    }
}
