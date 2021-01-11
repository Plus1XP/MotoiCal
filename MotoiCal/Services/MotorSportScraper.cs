using HtmlAgilityPack;

using MotoiCal.Enums;
using MotoiCal.Interfaces;
using MotoiCal.Models;
using MotoiCal.Models.FileManagement;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace MotoiCal.Services
{
    public class MotorSportScraper
    {
        private HtmlDocument doc;
        private readonly HtmlWeb webGet;
        private readonly CalendarManager iCalendar;
        private ObservableCollection<IRaceTimeTable> timeTable;  //REFACTOR THIS

        public MotorSportScraper()
        {
            this.doc = new HtmlDocument();
            this.webGet = new HtmlWeb();
            this.iCalendar = new CalendarManager();
        }

        public void ScrapeSeries(MotorSport motorSport)
        {
            this.timeTable = new ObservableCollection<IRaceTimeTable>();
            PopulateRaceURL(motorSport);
            foreach (string url in motorSport.EventUrlList)
            {
                bool isEventSkipped = false;
                SetHTMLParams(url);
                this.GetRaceHeaders(motorSport);
                this.GetEventTimeTable(motorSport, isEventSkipped);
                AddTimeTableToList(motorSport, timeTable, isEventSkipped);
            }
        }

        private void PopulateRaceURL(MotorSport motorSport)
        {
            if (motorSport.EventUrlList?.Any() != true)
            {
                this.doc = this.webGet.Load(motorSport.Url);
                motorSport.EventUrlList = new List<string>();

                foreach (HtmlNode node in this.doc.DocumentNode.SelectNodes(motorSport.UrlPath))
                {
                    string scrapedUrl = node.Attributes[motorSport.UrlAttribute]?.Value;
                    string url = string.IsNullOrEmpty(scrapedUrl) ? url = "URL not found" : url = $"{motorSport.UrlPartial}{scrapedUrl}";
                    if (motorSport.ExcludedUrls.Any(url.Contains))
                    {
                        continue;
                    }
                    motorSport.EventUrlList.Add(url);
                    Debug.WriteLine(url);
                }
            }
        }

        private void SetHTMLParams(string url)
        {
            // The HTMLWeb parameters set the encoding to the URL, otherwise special characters wont display correctly.
            this.webGet.AutoDetectEncoding = false;
            this.webGet.OverrideEncoding = Encoding.UTF8;
            //this.doc.OptionEmptyCollection = true;
            this.doc = this.webGet.Load(url);
        }

        private void GetRaceHeaders(MotorSport motorSport)
        {
            motorSport.GrandPrix = GetGrandPrix(motorSport);
            motorSport.Sponser = GetSponser(motorSport);
            motorSport.Location = GetLocation(motorSport, motorSport.Sponser);
        }

        private void GetEventTimeTable(MotorSport motorSport, bool isEventSkipped)
        {
            if (this.doc.DocumentNode.SelectNodes(motorSport.EventTablePath) != null)
            {
                foreach (HtmlNode node in this.doc.DocumentNode.SelectNodes(motorSport.EventTablePath))
                {
                    motorSport.Series = GetSeries(motorSport, node, isEventSkipped);
                    motorSport.Session = GetSession(motorSport, node, isEventSkipped);
                    Tuple<string, string> dateTime = GetDateTime(motorSport, node, isEventSkipped);
                    Tuple<DateTime, DateTime> dateTimeUTC = ParseDateTimeUTC(dateTime);
                    motorSport.StartUTC = dateTimeUTC.Item1;
                    motorSport.EndUTC = dateTimeUTC.Item2;
                    Tuple<DateTime, DateTime> dateTimeLocal = ParseDateTimeLocal(dateTime);
                    motorSport.Start = dateTimeLocal.Item1;
                    motorSport.End = dateTimeLocal.Item2;
                }
            }
            else
            {
                Debug.WriteLine($"Event Missing: url??");
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

        private string GetSeries(MotorSport motorSport, HtmlNode node, bool isEventSkipped)
        {
            string Series = node.SelectSingleNode(motorSport.SeriesNamePath).InnerText.Trim();

            // This checks if the node contains any of the substrings in the excluded array.
            if (motorSport.ExcludedClasses.Any(Series.Contains))
            {
                isEventSkipped = true;
            }

            return Series;
        }

        private string GetSession(MotorSport motorSport, HtmlNode node, bool isEventSkipped)
        {
            string Session = motorSport.CheckForExcludedWords(node.SelectSingleNode(motorSport.SessionNamePath).InnerText.Trim());

            // This checks if the node contains any of the substrings in the excluded array.
            if (motorSport.ExcludedEvents.Any(Session.Contains))
            {
                isEventSkipped = true;
            }

            return Session;
        }

        private Tuple<string, string> GetDateTime(MotorSport motorSport, HtmlNode node, bool isEventSkipped)
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

        private Tuple<DateTime, DateTime> ParseDateTimeUTC(Tuple<string, string> dateTime)
        {
            // Use UTC time for iCal event.
            DateTime StartUTC = this.iCalendar.ParseDateTimeToUTC(dateTime.Item1);
            DateTime EndUTC = this.iCalendar.ParseDateTimeToUTC(dateTime.Item2);

            return Tuple.Create(StartUTC, EndUTC);
        }

        private Tuple<DateTime, DateTime> ParseDateTimeLocal(Tuple<string, string> dateTime)
        {
            // Use local time for console results.
            DateTime Start = this.iCalendar.ParseDateTimeToLocal(dateTime.Item1);
            DateTime End = this.iCalendar.ParseDateTimeToLocal(dateTime.Item2);

            return Tuple.Create(Start, End);
        }

        private void AddTimeTableToList(IRaceTimeTable motorSport, ObservableCollection<IRaceTimeTable> timeTable, bool isEventSkipped)
        {
            if (!isEventSkipped)
            {
                timeTable.Add(motorSport);
            }
            //switch (motorSport.SportIdentifier)
            //{
            //    case MotorSportID.None:
            //        break;
            //    case MotorSportID.Formula1:
            //        timeTable.Add(motorSport);
            //        break;
            //    case MotorSportID.MotoGP:
            //        timeTable.Add(motorSport);
            //        break;
            //    case MotorSportID.WorldSBK:
            //        timeTable.Add(motorSport);
            //        break;
            //    default:
            //        break;
            //}
        }

        // Code below not is use ??

        /*
                public int EventsFound()
                {
                    return this.raceData.Count;
                }

                public int RacesFound(IDocNodePath motorSport) //REFACTOR THIS
                {
                    return motorSport.EventUrlList.Count;
                }

                public bool IsEasterEggActive(string easterEggDate)
                {
                    return DateTime.Parse(easterEggDate) == DateTime.Now.Date ? true : false;
                }
        */

        // currentSponser is initially set to null, then each loop is given the current Sponser value.
        // This allows the stringbuilder to check if it needs to update header.
        // * This was changed from checking again the currentGrandPrix in the COVID update due to multiple races at the same GrandPrix.
      



        public string ScrapeEventsToiCalendar(MotorSport motorSport) //REFACTOR THIS
        {
            Stopwatch stopWatch = Stopwatch.StartNew();
            this.timeTable = new ObservableCollection<IRaceTimeTable>();
            // Checks list, Same as if list == null or motorSport.Count == 0
            if (motorSport.EventUrlList?.Any() != true)
            {
                this.AddMotoSportEventsToList(motorSport);
            }
            this.ProcessMotorSportEvents(motorSport);
            stopWatch.Stop();
            Debug.WriteLine($"Total Operation Time: {stopWatch.Elapsed.Seconds}.{stopWatch.Elapsed.Milliseconds / 10}");
            return "Removed method";
        }

        private void AddMotoSportEventsToList(MotorSport motorSport) //REFACTOR THIS
        {
            Stopwatch stopWatch = Stopwatch.StartNew();
            
            stopWatch.Stop();
            Debug.WriteLine($"URL Collection search time: {stopWatch.Elapsed.Seconds}.{stopWatch.Elapsed.Milliseconds / 10}");
        }

        private void ProcessMotorSportEvents(MotorSport motorSport) //REFACTOR THIS
        {
            Stopwatch stopWatch = Stopwatch.StartNew();
            //Parallel.ForEach(motorSport.EventUrlList, async url =>
            //{
            //    await this.FindMotorSportSessions(motorSport, url);
            //    Debug.WriteLine($"Thread No. {Thread.CurrentThread.ManagedThreadId}");
            //    //Thread.Sleep(50);
            //});
            
            stopWatch.Stop();
            Debug.WriteLine($"Total search time: {stopWatch.Elapsed.Seconds}.{stopWatch.Elapsed.Milliseconds / 10}");
        }

        // Some Nodes return null if there is a problem with the paths or the data is missing.
        // "?" checks and allows the returned HtmlNodeCollection to be null, "??" returns a string if the node is null.
        private void FindMotorSportSessions(MotorSport motorSport, string url) //REFACTOR THIS
        {
            Stopwatch stopWatch = Stopwatch.StartNew();


            stopWatch.Stop();
            Debug.WriteLine($"Page scrape search time: {stopWatch.Elapsed.Seconds}.{stopWatch.Elapsed.Milliseconds / 10}");



            //Debug.Assert(this.doc.DocumentNode.SelectNodes(motorSport.EventTablePath) != null);
            //Debug.Assert(!GrandPrix.Contains("Spain"));
            //Debug.WriteIf(motorSport.Url.Contains("https://www.worldsbk.com/en/event/ESP3/2020"), $"{GrandPrix}");

            // MotoGP are updated the schedule, eventsTable loads 404.

        }

        private void RaceTimeTableModel(
            IRaceTimeTable motorSport, //REFACTOR THIS
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
                    this.timeTable.Add(new Formula1
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
                    this.timeTable.Add(new MotoGP
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
                    this.timeTable.Add(new WorldSBK
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
