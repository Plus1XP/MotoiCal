using HtmlAgilityPack;

using MotoiCal.Enums;
using MotoiCal.Interfaces;
using MotoiCal.Models.FileManagement;
using MotoiCal.Utilities.Helpers;

using System;
using System.Linq;

namespace MotoiCal.Models
{
    class ScraperModel
    {
        private readonly CalendarManager iCalendar;

        public ScraperModel()
        {
            this.iCalendar = new CalendarManager();
        }

        public string GetGrandPrix(IDocNodePath motorSport, HtmlDocument doc)
        {
            return doc.DocumentNode.SelectSingleNode(motorSport.GrandPrixNamePath)?.InnerText ?? "No Data";
        }

        public string GetSponser(IDocNodePath motorSport, HtmlDocument doc)
        {
            return doc.DocumentNode.SelectSingleNode(motorSport.SponserNamePath)?.InnerText ?? "No Data";
        }

        public string GetLocation(IDocNodePath motorSport, HtmlDocument doc, string Sponser)
        {
            // Location is empty in WSBK Catalunya's URL.
            string Location = doc.DocumentNode.SelectSingleNode(motorSport.LocationNamePath)?.InnerText ?? "No Data";

            // Adds location to empty WSBK Catalunya.
            return this.WSBKCatalunyaLoctionFix(Sponser, Location);
        }

        // Tempary fix to add WSBK Catalunya location untill WSBK update this.
        private string WSBKCatalunyaLoctionFix(string sponser, string location)
        {
            if (sponser.Equals("Acerbis Catalunya Round") && location.Equals("No Data"))
            {
                return "Circuit de Barcelona-Catalunya";
            }
            else
            {
                return location;
            }
        }

        public string GetSeries<T>(T motorSport, HtmlNode node, ref bool isEventSkipped) where T : IDocNodePath, IDocExclusionList
        {
            string Series = node.SelectSingleNode(motorSport.SeriesNamePath).InnerText.Trim();

            if (!isEventSkipped)
            {
                // This checks if the node contains any of the substrings in the excluded array.
                isEventSkipped = this.CheckExcludedClasses(motorSport, Series);
            }

            return Series;
        }

        public string GetSession<T>(T motorSport, HtmlNode node, ref bool isEventSkipped) where T : IDocNodePath, IDocExclusionList
        {
            string Session = node.SelectSingleNode(motorSport.SessionNamePath).InnerText.Trim().CheckForExcludedWords(motorSport.ExcludedWords);

            if (!isEventSkipped)
            {
                // This checks if the node contains any of the substrings in the excluded array.
                isEventSkipped = this.CheckExcludedEvents(motorSport, Session);
            }

            return Session;
        }

        public Tuple<string, string> GetDateTime<T>(T motorSport, HtmlNode node, ref bool isEventSkipped) where T : IRaceTimeTable, IDocNodePath
        {
            string dtStart = node.SelectSingleNode(motorSport.StartDatePath)?.Attributes[motorSport.StartDateAttribute].Value;

            // This checks if the node is null or empty, if it is the session is missing on the website and skips over it.
            if (string.IsNullOrEmpty(dtStart))
            {
                isEventSkipped = true;
            }

            // To handle empty rows ? checks whether or not the returned HtmlNodeCollection is null.
            string dtEndNullCheck = node.SelectSingleNode(motorSport.EndDatePath)?.Attributes[motorSport.EndDateAttribute].Value;

            // Handles if the end time result is null (default to start time + 1 HR) or not (parse end time) using a ternary operator.
            string dtEnd = string.IsNullOrEmpty(dtEndNullCheck) ? DateTime.Parse(dtStart).AddHours(1).ToString() : dtEndNullCheck;

            // Formula1 handles the GMT offset differently (+0000) compared to MotoGP & WSBK (0000-00-00T00:00:00+0000").
            if (motorSport.SportIdentifier.Equals(MotorSportID.Formula1))
            {
                string dtOffset = node.SelectSingleNode(motorSport.StartDatePath).Attributes[motorSport.GMTOffset].Value;
                dtStart += dtOffset;
                dtEnd += dtOffset;
            }

            return Tuple.Create(dtStart, dtEnd);
        }

        // Use local time for console results.
        public Tuple<DateTime, DateTime> ParseDateTimeLocal(Tuple<string, string> dateTime)
        {
            DateTime Start = this.iCalendar.ParseDateTimeToLocal(dateTime.Item1);
            DateTime End = this.iCalendar.ParseDateTimeToLocal(dateTime.Item2);

            return Tuple.Create(Start, End);
        }

        // Use UTC time for iCal event.
        public Tuple<DateTime, DateTime> ParseDateTimeUTC(Tuple<string, string> dateTime)
        {
            DateTime StartUTC = this.iCalendar.ParseDateTimeToUTC(dateTime.Item1);
            DateTime EndUTC = this.iCalendar.ParseDateTimeToUTC(dateTime.Item2);

            return Tuple.Create(StartUTC, EndUTC);
        }

        public bool CheckExcludedURL(IDocExclusionList motorSport, string url)
        {
            return motorSport.ExcludedUrls.Any(url.Contains);
        }

        private bool CheckExcludedClasses(IDocExclusionList motorSport, string series)
        {
            return motorSport.ExcludedClasses.Any(series.Contains);
        }

        private bool CheckExcludedEvents(IDocExclusionList motorSport, string events)
        {
            return motorSport.ExcludedEvents.Any(events.Contains);
        }
    }
}
