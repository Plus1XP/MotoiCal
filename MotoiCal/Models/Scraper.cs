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

        // currentGrandPrix is initially set to null, then each loop is given the current GrandPRix value.
        // This allows the stringbuilder to check if it needs to update header.
        private string ProcessDisplayResults()
        {
            StringBuilder raceresults = new StringBuilder();

            string currentGrandPrix = null;

            foreach (var race in this.raceData)
            {
                string header = race.GrandPrix == currentGrandPrix ? string.Empty : race.DisplayHeader;
                string body = race.DisplayBody;

                raceresults.Append(header);
                raceresults.AppendLine(body);

                currentGrandPrix = race.GrandPrix;
            }
            return raceresults.ToString();
        }

        private void ProcessiCalendarResults()
        {
            this.iCalendar.CreateCalendarEntry();
            /*
            this.iCal.CreateTimeZone();
            */
            foreach (var item in this.raceData)
            {
                this.iCalendar.CreateCalendarEventEntry(item.StartUTC, item.EndUTC, item.IcalendarSubject, item.IcalendarLocation, item.IcalendarDescription);
            }
            this.iCalendar.CloseCalendarEntry();
        }

        public string ScrapeEventsToiCalendar(IMotorSport motorSport)
        {
            this.raceData = new ObservableCollection<IRaceData>();
            // Checks list, Same as if list == null or motorSport.Count == 0
            if (motorSport.EventUrlList?.Any() != true)
            {
                this.AddMotoSportEventsToList(motorSport);
            }
            this.ProcessMotorSportEvents(motorSport);
            return this.ProcessDisplayResults();
        }

        private void AddMotoSportEventsToList(IMotorSport motorSport)
        {
            this.doc = this.webGet.Load(motorSport.Url);
            motorSport.EventUrlList = new List<string>();

            foreach (var node in this.doc.DocumentNode.SelectNodes(motorSport.UrlPath))
            {              
                string scrapedUrl = node.Attributes[motorSport.UrlAttribute]?.Value;
                string url = string.IsNullOrEmpty(scrapedUrl) ? url = "URL not found" : url = $"{motorSport.UrlPartial}{scrapedUrl}";
                if (motorSport.ExcludedUrls.Any(url.Contains))
                {
                    continue;
                }
                motorSport.EventUrlList.Add(url);
            }
        }

        private void ProcessMotorSportEvents(IMotorSport motorSport)
        {
            foreach (string url in motorSport.EventUrlList)
            {
                this.FindMotorSportSessions(motorSport, url);
            }
        }

        // Some Nodes return null if there is a problem with the paths or the data is missing.
        // "?" checks and allows the returned HtmlNodeCollection to be null, "??" returns a string if the node is null.
        private void FindMotorSportSessions(IMotorSport motorSport, string url)
        {
            this.doc = this.webGet.Load(url);

            string GrandPrix = this.doc.DocumentNode.SelectSingleNode(motorSport.GrandPrixNamePath)?.InnerText ?? "No Data";
            // Location is empty in WSBK Catalunya
            string Location = this.doc.DocumentNode.SelectSingleNode(motorSport.LocationNamePath)?.InnerText ?? "No Data";
            string Sponser = this.doc.DocumentNode.SelectSingleNode(motorSport.SponserNamePath)?.InnerText ?? "No Data";

            foreach (HtmlNode node in this.doc.DocumentNode.SelectNodes(motorSport.EventTablePath))
            {
                string Series = node.SelectSingleNode(motorSport.SeriesNamePath).InnerText.Trim();

                // This checks if the node contains any of the substrings in the excluded array.
                if (motorSport.ExcludedClasses.Any(Series.Contains))
                {
                    continue;
                }

                string Session = motorSport.CheckForExcludedWords(node.SelectSingleNode(motorSport.SessionNamePath).InnerText.Trim());

                // This checks if the node contains any of the substrings in the excluded array.
                if (motorSport.ExcludedEvents.Any(Session.Contains))
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
                if (motorSport.SportIdentifier.Equals(MotorSportID.Formula1))
                {
                    string dtOffset = node.SelectSingleNode(motorSport.StartDatePath).Attributes[motorSport.GMTOffset].Value;
                    dtStart += dtOffset;
                    dtEnd += dtOffset;
                }

                // Use UTC time for iCal event.
                DateTime StartUTC = this.iCalendar.ParseDateTimeToUTC(dtStart);
                DateTime EndUTC = this.iCalendar.ParseDateTimeToUTC(dtEnd);

                // Use local time for console results.
                DateTime Start = this.iCalendar.ParseDateTimeToLocal(dtStart);
                DateTime End = this.iCalendar.ParseDateTimeToLocal(dtEnd);

                this.AddData(motorSport, GrandPrix, Location, Sponser, Series, Session, Start, End, StartUTC, EndUTC);
            }
        }

        private void AddData(
            IMotorSport motorSport, 
            string grandPrix, 
            string location, 
            string sponser, 
            string series, 
            string session, 
            DateTime start, 
            DateTime end, 
            DateTime startUTC, 
            DateTime endUTC
            )
        {
            switch (motorSport.SportIdentifier)
            {
                case MotorSportID.Formula1:
                    this.raceData.Add(new Formula1
                    {
                        GrandPrix = grandPrix,
                        Location = location,
                        Sponser = sponser,
                        Series = series,
                        Session = session,
                        Start = start,
                        End = end,
                        StartUTC = startUTC,
                        EndUTC = endUTC
                    });
                    break;
                case MotorSportID.MotoGP:
                    this.raceData.Add(new MotoGP
                    {
                        GrandPrix = grandPrix,
                        Location = location,
                        Sponser = sponser,
                        Series = series,
                        Session = session,
                        Start = start,
                        End = end,
                        StartUTC = startUTC,
                        EndUTC = endUTC
                    });
                    break;
                case MotorSportID.WorldSBK:
                    this.raceData.Add(new WorldSBK
                    {
                        GrandPrix = grandPrix,
                        Location = location,
                        Sponser = sponser,
                        Series = series,
                        Session = session,
                        Start = start,
                        End = end,
                        StartUTC = startUTC,
                        EndUTC = endUTC
                    });
                    break;
            }
        }
    }
}