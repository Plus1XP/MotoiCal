using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace MotoiCal.Models
{
    public class Scraper
    {
        private HtmlWeb webGet;
        private HtmlDocument doc;
        private CalendarManager iCalendar;
        private IMotorSport motorSport;
        private StringBuilder resultsOutput;

        public Scraper()
        {
            this.webGet = new HtmlWeb();
            this.doc = new HtmlDocument();
            this.iCalendar = new CalendarManager();
        }

        public string GenerateiCalendar()
        {
            return this.resultsOutput != null ? this.iCalendar.CreateICSFile(this.motorSport.FilePath) : "Can not generate ICS file without first showing dates";
        }

        public string ReadiCalendar()
        {
            return this.iCalendar.ReadICSFile(this.motorSport.FilePath);
        }

        public string DeleteiCalendar()
        {
            return this.iCalendar.DeleteICSFile(this.motorSport.FilePath);
        }

        public void ResetResultsOutput()
        {
            this.resultsOutput = null;
        }

        public void ScrapeEventsToiCalendar()
        {
            this.resultsOutput = new StringBuilder();
            this.iCalendar.CreateCalendarEntry();
            /*
            this.iCal.CreateTimeZone();
            */
            this.GetEventURL(this.motorSport.EventURLs);
            this.iCalendar.CloseCalendarEntry();
        }

        private void GetEventURL(string[] urls)
        {
            foreach (string url in urls)
            {
                this.ScrapeEventURL(url);
            }
        }

        private void ScrapeEventURL(string url)
        {
            this.doc = this.webGet.Load(url);

            string RaceName = this.doc.DocumentNode.SelectSingleNode(this.motorSport.RaceNamePath).InnerText;
            string LocationName = this.doc.DocumentNode.SelectSingleNode(this.motorSport.LocationNamePath).InnerText;
            string CircuitName = this.motorSport.CheckForExcludedWords(this.doc.DocumentNode.SelectSingleNode(this.motorSport.CircuitNamePath).InnerText);

            this.resultsOutput.AppendLine($"\n{this.motorSport.SportIdentifier} {RaceName}\n{CircuitName}\n{LocationName}");

            foreach (HtmlNode node in this.doc.DocumentNode.SelectNodes(this.motorSport.EventTablePath))
            {
                string ClassName = node.SelectSingleNode(this.motorSport.ClassNamePath).InnerText.Trim();

                // This checks if the node contains any of the substrings in the excluded array.
                if (this.motorSport.ExcludedClasses.Any(ClassName.Contains))
                {
                    continue;
                }

                string SessionName = this.motorSport.CheckForExcludedWords(node.SelectSingleNode(this.motorSport.SessionNamePath).InnerText.Trim());

                // This checks if the node contains any of the substrings in the excluded array.
                if (this.motorSport.ExcludedEvents.Any(SessionName.Contains))
                {
                    continue;
                }

                string dtStart = node.SelectSingleNode(this.motorSport.StartDatePath)?.Attributes[this.motorSport.StartDateAttribute].Value;

                // This checks if the node is null or empty, if it is the session is missing on the website and skips over it.
                if (string.IsNullOrEmpty(dtStart))
                {
                    this.resultsOutput.AppendLine($"{ClassName} {SessionName}");
                    continue;
                }

                // To handle empty rows ? checks whether or not the returned HtmlNodeCollection is null.
                string dtEndNullCheck = node.SelectSingleNode(this.motorSport.EndDatePath)?.Attributes[this.motorSport.EndDateAttribute].Value;

                // Handles if the end time result is null (default to start time + 1 HR) or not (parse end time) using a ternary operator.
                string dtEnd = string.IsNullOrEmpty(dtEndNullCheck) ? DateTime.Parse(dtStart).AddHours(1).ToString() : dtEndNullCheck;

                // Formula1 handles the GMT offset differently (+0000) compared to MotoGP & WSBK (0000-00-00T00:00:00+0000").
                if (this.motorSport.SportIdentifier.Equals("Formula1"))
                {
                    string dtOffset = node.SelectSingleNode(this.motorSport.StartDatePath).Attributes[this.motorSport.GMTOffset].Value;
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

                this.iCalendar.CreateCalendarEventEntry(StartTimeUTC, EndTimeUTC, $"{ClassName} {RaceName} {SessionName}", LocationName, CircuitName);
            }
        }
    }
}