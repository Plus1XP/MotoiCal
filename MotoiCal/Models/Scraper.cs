using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using HtmlAgilityPack;

namespace MotoiCal.Models
{
    public class Scraper
    {
        private HtmlDocument doc;
        private readonly HtmlWeb webGet;
        private readonly CalendarManager iCalendar;
        private ObservableCollection<IRaceData> raceData;


        public Scraper()
        {
            this.doc = new HtmlDocument();
            this.webGet = new HtmlWeb();
            this.iCalendar = new CalendarManager();
        }

         public string GenerateiCalendar(IMotorSport motorSport)
        {
            // Checks if resultsOutput has a value, if not then it is assumed the dates have not been pulled.
            return this.resultsOutput != null
                ? this.iCalendar.CreateICSFile(motorSport.FilePath)
                : "Can not generate ICS file without first showing dates";
        }
       
        public string ReadiCalendar(IMotorSport motorSport)
        {
            return this.iCalendar.ReadICSFile(motorSport.FilePath);
        }

        public string DeleteiCalendar(IMotorSport motorSport)
        {
            return this.iCalendar.DeleteICSFile(motorSport.FilePath);
        }

        public void ResetResultsOutput()
        {
            this.resultsOutput = null;
        }

        public string GetResultsString()
        {
            return this.resultsOutput != null ? this.resultsOutput.ToString() : "Please Pull Results First";
        }

        public string ScrapeEventsToiCalendar(IMotorSport motorSport)
        {
            this.resultsOutput = new StringBuilder();
            this.iCalendar.CreateCalendarEntry();
            /*
            this.iCal.CreateTimeZone();
            */
            this.GetEventURL(motorSport);
            this.iCalendar.CloseCalendarEntry();
            return this.resultsOutput.ToString();
        }

        private void GetEventURL(IMotorSport motorSport)
        {
            foreach (string url in motorSport.EventURLs)
            {
                this.ScrapeEventURL(motorSport, url);
            }
        }

        private void ScrapeEventURL(IMotorSport motorSport, string url)
        {
            this.doc = this.webGet.Load(url);

            string RaceName = this.doc.DocumentNode.SelectSingleNode(motorSport.RaceNamePath).InnerText;
            string LocationName = this.doc.DocumentNode.SelectSingleNode(motorSport.LocationNamePath).InnerText;
            string CircuitName = motorSport.CheckForExcludedWords(this.doc.DocumentNode.SelectSingleNode(motorSport.CircuitNamePath).InnerText);

            this.resultsOutput.AppendLine($"\n{motorSport.SportIdentifier} {RaceName}\n{CircuitName}\n{LocationName}");

            foreach (HtmlNode node in this.doc.DocumentNode.SelectNodes(motorSport.EventTablePath))
            {
                string ClassName = node.SelectSingleNode(motorSport.ClassNamePath).InnerText.Trim();

                // This checks if the node contains any of the substrings in the excluded array.
                if (motorSport.ExcludedClasses.Any(ClassName.Contains))
                {
                    continue;
                }

                string SessionName = motorSport.CheckForExcludedWords(node.SelectSingleNode(motorSport.SessionNamePath).InnerText.Trim());

                // This checks if the node contains any of the substrings in the excluded array.
                if (motorSport.ExcludedEvents.Any(SessionName.Contains))
                {
                    continue;
                }

                string dtStart = node.SelectSingleNode(motorSport.StartDatePath)?.Attributes[motorSport.StartDateAttribute].Value;

                // This checks if the node is null or empty, if it is the session is missing on the website and skips over it.
                if (string.IsNullOrEmpty(dtStart))
                {
                    this.resultsOutput.AppendLine($"{ClassName} {SessionName}");
                    continue;
                }

                // To handle empty rows ? checks whether or not the returned HtmlNodeCollection is null.
                string dtEndNullCheck = node.SelectSingleNode(motorSport.EndDatePath)?.Attributes[motorSport.EndDateAttribute].Value;

                // Handles if the end time result is null (default to start time + 1 HR) or not (parse end time) using a ternary operator.
                string dtEnd = string.IsNullOrEmpty(dtEndNullCheck)
                    ? DateTime.Parse(dtStart).AddHours(1).ToString()
                    : dtEndNullCheck;

                // Formula1 handles the GMT offset differently (+0000) compared to MotoGP & WSBK (0000-00-00T00:00:00+0000").
                if (motorSport.SportIdentifier.Equals("Formula1"))
                {
                    string dtOffset = node.SelectSingleNode(motorSport.StartDatePath).Attributes[motorSport.GMTOffset].Value;
                    dtStart += dtOffset;
                    dtEnd += dtOffset;
                }

                // Use UTC time for iCal event.
                DateTime StartTimeUTC = this.iCalendar.ParseDateTimeToUTC(dtStart);
                DateTime EndTimeUTC = this.iCalendar.ParseDateTimeToUTC(dtEnd);

                // Use local time for console results.
                DateTime StartTimeLocal = this.iCalendar.ParseDateTimeToLocal(dtStart);
                DateTime EndTimeLocal = this.iCalendar.ParseDateTimeToLocal(dtEnd);

                this.resultsOutput.AppendLine($"{ClassName} {SessionName} : {StartTimeLocal} - {EndTimeLocal}");

                this.iCalendar.CreateCalendarEventEntry(
                    StartTimeUTC, EndTimeUTC, $"{ClassName} {RaceName} {SessionName}", LocationName, CircuitName);
            }
        }
    }
}