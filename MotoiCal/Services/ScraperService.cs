using HtmlAgilityPack;

using MotoiCal.Enums;
using MotoiCal.Interfaces;
using MotoiCal.Models;
using MotoiCal.Models.FileManagement;
using MotoiCal.Utilities.Helpers;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MotoiCal.Services
{
    public class ScraperService
    {
        private HtmlDocument doc;
        private readonly HtmlWeb webGet;
        private readonly CalendarManager iCalendar;

        private bool isEventSkipped;

        public ScraperService()
        {
            this.doc = new HtmlDocument();
            this.webGet = new HtmlWeb();
            this.iCalendar = new CalendarManager();
        }

        public ObservableCollection<IRaceTimeTable> GetSeriesCollection(MotorSport motorSport)
        {
            Stopwatch stopWatch1 = Stopwatch.StartNew();
            ObservableCollection<IRaceTimeTable> timeTable = new ObservableCollection<IRaceTimeTable>();
            this.PopulateEventURLList(motorSport);
            foreach (string url in motorSport.EventUrlList)
            {
                Stopwatch stopWatch2 = Stopwatch.StartNew();
                isEventSkipped = false;
                this.GetHTMLDoc(url);
                this.PopulateTimeTableHeader(motorSport);
                this.PopulateTimeTableBody(motorSport, timeTable);
                //this.AddTimeTableToCollection(motorSport, timeTable, isEventSkipped);
                stopWatch2.Stop();
                Debug.WriteLine($"Page scrape search time: {stopWatch2.Elapsed.Seconds}.{stopWatch2.Elapsed.Milliseconds / 10}");
            }
            stopWatch1.Stop();
            Debug.WriteLine($"Total Operation Time: {stopWatch1.Elapsed.Seconds}.{stopWatch1.Elapsed.Milliseconds / 10}");

            return timeTable;
        }

        private void GetHTMLDoc(string url)
        {
            // The HTMLWeb parameters set the encoding to the URL, otherwise special characters wont display correctly.
            this.webGet.AutoDetectEncoding = false;
            this.webGet.OverrideEncoding = Encoding.UTF8;
            //this.doc.OptionEmptyCollection = true;
            this.doc = this.webGet.Load(url);
        }

        private void PopulateEventURLList<T>(T motorSport) where T : IDocNodePath, IDocExclusionList
        {
            // Checks list, Same as if list == null or motorSport.Count == 0
            if (motorSport.EventUrlList?.Any() != true)
            {
                motorSport.EventUrlList = new List<string>();
                this.GetHTMLDoc(motorSport.Url);
                this.AddURLToEventList(motorSport);
            }           
        }

        private void AddURLToEventList<T>(T motorSport) where T : IDocNodePath, IDocExclusionList
        {
            Stopwatch stopWatch3 = Stopwatch.StartNew();
            foreach (HtmlNode node in this.doc.DocumentNode.SelectNodes(motorSport.UrlPath))
            {
                string scrapedUrl = node.Attributes[motorSport.UrlAttribute]?.Value;
                string url = string.IsNullOrEmpty(scrapedUrl) ? "URL not found" : $"{motorSport.UrlPartial}{scrapedUrl}";
                Debug.WriteLine($"{url} : {scrapedUrl}");
                if (this.CheckExcludedURL(motorSport, url) || url.Equals("URL not found"))
                {
                    Debug.WriteLine($"Skipped {url}");
                    continue;
                }
                motorSport.EventUrlList.Add(url);
                Debug.WriteLine($"Added: {url}");
            }
            stopWatch3.Stop();
            Debug.WriteLine($"URL Collection search time: {stopWatch3.Elapsed.Seconds}.{stopWatch3.Elapsed.Milliseconds / 10}");
        }

        private void PopulateTimeTableHeader<T>(T motorSport) where T : IRaceTimeTable, IDocNodePath
        {
            motorSport.GrandPrix = this.GetGrandPrix(motorSport);
            motorSport.Sponser = this.GetSponser(motorSport);
            motorSport.Location = this.GetLocation(motorSport, motorSport.Sponser);
        }

        // Some Nodes return null if there is a problem with the paths or the data is missing.
        // "?" checks and allows the returned HtmlNodeCollection to be null, "??" returns a string if the node is null.
        private void PopulateTimeTableBody<T>(T motorSport, ObservableCollection<IRaceTimeTable> timeTable) where T : IRaceTimeTable, IDocNodePath, IDocExclusionList
        {
            //Debug.Assert(this.doc.DocumentNode.SelectNodes(motorSport.EventTablePath) != null);
            //Debug.Assert(!GrandPrix.Contains("Spain"));
            //Debug.WriteIf(motorSport.Url.Contains("https://www.worldsbk.com/en/event/ESP3/2020"), $"{GrandPrix}");

            // MotoGP are updated the schedule, eventsTable loads 404.
            if (this.doc.DocumentNode.SelectNodes(motorSport.EventTablePath) != null)
            {
                foreach (HtmlNode node in this.doc.DocumentNode.SelectNodes(motorSport.EventTablePath))
                {
                    motorSport.Series = this.GetSeries(motorSport, node);
                    motorSport.Session = this.GetSession(motorSport, node);
                    Tuple<string, string> dateTime = this.GetDateTime(motorSport, node);
                    motorSport.Start = this.ParseDateTimeLocal(dateTime).Item1;
                    motorSport.End = this.ParseDateTimeLocal(dateTime).Item2;
                    motorSport.StartUTC = this.ParseDateTimeUTC(dateTime).Item1;
                    motorSport.EndUTC = this.ParseDateTimeUTC(dateTime).Item2;

                }
            }
            else
            {
                Debug.WriteLine($"Event Missing: {motorSport.GrandPrix}");
            }
        }

        private void AddTimeTableToCollection(IRaceTimeTable motorSport, ObservableCollection<IRaceTimeTable> timeTable, bool isEventSkipped)
        {
            if (!isEventSkipped)
            {
                timeTable.Add(motorSport);
            }
        }

        private string GetGrandPrix(IDocNodePath motorSport)
        {
            return this.doc.DocumentNode.SelectSingleNode(motorSport.GrandPrixNamePath)?.InnerText ?? "No Data";
        }

        private string GetSponser(IDocNodePath motorSport)
        {
            return this.doc.DocumentNode.SelectSingleNode(motorSport.SponserNamePath)?.InnerText ?? "No Data";
        }

        private string GetLocation(IDocNodePath motorSport, string Sponser)
        {
            // Location is empty in WSBK Catalunya's URL.
            string Location = this.doc.DocumentNode.SelectSingleNode(motorSport.LocationNamePath)?.InnerText ?? "No Data";

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

        private string GetSeries<T>(T motorSport, HtmlNode node) where T : IDocNodePath, IDocExclusionList
        {
            string Series = node.SelectSingleNode(motorSport.SeriesNamePath).InnerText.Trim();

            if (!isEventSkipped)
            {
                // This checks if the node contains any of the substrings in the excluded array.
                this.isEventSkipped = this.ChecExcludedClasses(motorSport, Series);
            }            

            return Series;
        }

        private string GetSession<T>(T motorSport, HtmlNode node) where T : IDocNodePath, IDocExclusionList
        {
            string Session = node.SelectSingleNode(motorSport.SessionNamePath).InnerText.Trim().CheckForExcludedWords(motorSport.ExcludedWords);

            if (!isEventSkipped)
            {
                // This checks if the node contains any of the substrings in the excluded array.
                this.isEventSkipped = this.CheckExcludedEvents(motorSport, Session);
            }

            return Session;
        }

        private Tuple<string, string> GetDateTime<T>(T motorSport, HtmlNode node) where T : IRaceTimeTable, IDocNodePath
        {
            string dtStart = node.SelectSingleNode(motorSport.StartDatePath)?.Attributes[motorSport.StartDateAttribute].Value;

            // This checks if the node is null or empty, if it is the session is missing on the website and skips over it.
            if (string.IsNullOrEmpty(dtStart))
            {
                this.isEventSkipped = true;
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
        private Tuple<DateTime, DateTime> ParseDateTimeLocal(Tuple<string, string> dateTime)
        {
            DateTime Start = this.iCalendar.ParseDateTimeToLocal(dateTime.Item1);
            DateTime End = this.iCalendar.ParseDateTimeToLocal(dateTime.Item2);

            return Tuple.Create(Start, End);
        }

        // Use UTC time for iCal event.
        private Tuple<DateTime, DateTime> ParseDateTimeUTC(Tuple<string, string> dateTime)
        {
            DateTime StartUTC = this.iCalendar.ParseDateTimeToUTC(dateTime.Item1);
            DateTime EndUTC = this.iCalendar.ParseDateTimeToUTC(dateTime.Item2);

            return Tuple.Create(StartUTC, EndUTC);
        }

        private bool CheckExcludedURL(IDocExclusionList motorSport, string url)
        {
            return motorSport.ExcludedUrls.Any(url.Contains);
        }

        private bool ChecExcludedClasses(IDocExclusionList motorSport, string series)
        {
            return motorSport.ExcludedClasses.Any(series.Contains);
        }

        private bool CheckExcludedEvents(IDocExclusionList motorSport, string events)
        {
            return motorSport.ExcludedEvents.Any(events.Contains);
        }
    }
}
