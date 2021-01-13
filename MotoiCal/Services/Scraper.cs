﻿using HtmlAgilityPack;

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
    public class Scraper
    {
        private HtmlDocument doc;
        private readonly HtmlWeb webGet;
        private readonly CalendarManager iCalendar;
        private ObservableCollection<IRaceTimeTable> raceData;  //REFACTOR THIS

        public Scraper()
        {
            this.doc = new HtmlDocument();
            this.webGet = new HtmlWeb();
            this.iCalendar = new CalendarManager();
        }

        public string GenerateiCalendar(ICalendarEvent motorSport)//, bool isReminderActive, int eventTriggerMinutes) //REFACTOR THIS
        {
            // Checks if url list has a value, if not then it is assumed the dates have not been pulled.
            //if (motorSport.EventUrlList?.Any() != true)
            if (string.IsNullOrEmpty(motorSport.IcalendarLocation))
            {
                return "Can not generate ICS file without first showing dates";
            }
            else
            {
                this.ProcessiCalendarResults(motorSport.IsEventReminderActive, motorSport.EventReminderMins);
                return this.iCalendar.CreateICSFile(motorSport.FilePath);
            }
        }

        public string ReadiCalendar(ICalendarEvent motorSport) //REFACTOR THIS
        {
            return this.iCalendar.ReadICSFile(motorSport.FilePath);
        }

        public string DeleteiCalendar(ICalendarEvent motorSport) //REFACTOR THIS
        {
            return this.iCalendar.DeleteICSFile(motorSport.FilePath);
        }

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

        // currentSponser is initially set to null, then each loop is given the current Sponser value.
        // This allows the stringbuilder to check if it needs to update header.
        // * This was changed from checking again the currentGrandPrix in the COVID update due to multiple races at the same GrandPrix.
        private string ProcessDisplayResults()
        {
            StringBuilder raceResults = new StringBuilder();

            string currentSponser = null;

            foreach (MotorSport race in this.raceData) //REFACTOR THIS
            {
                string header = race.Sponser == currentSponser ? string.Empty : race.DisplayHeader;
                string body = race.DisplayBody;

                raceResults.Append(header);
                raceResults.AppendLine(body);

                currentSponser = race.Sponser;
            }
            return raceResults.ToString();
        }

        private void ProcessiCalendarResults(bool isReminderActive, int eventTriggerMinutes)
        {
            this.iCalendar.CreateCalendarEntry();
            /*
            this.iCal.CreateTimeZone();
            */
            foreach (MotorSport item in this.raceData) //REFACTOR THIS
            {
                this.iCalendar.CreateCalendarEventEntry(item.StartUTC, item.EndUTC, item.IcalendarSubject, item.IcalendarLocation, item.IcalendarDescription);
                if (isReminderActive)
                {
                    this.iCalendar.CreateCalendarAlarmEntry(eventTriggerMinutes, item.IcalendarSubject); // Change from 15
                }
                this.iCalendar.CloseEventEntry();
            }
            this.iCalendar.CloseCalendarEntry();
        }

        public string ScrapeEventsToiCalendar(MotorSport motorSport) //REFACTOR THIS
        {
            Stopwatch stopWatch = Stopwatch.StartNew();
            this.raceData = new ObservableCollection<IRaceTimeTable>();
            // Checks list, Same as if list == null or motorSport.Count == 0
            if (motorSport.EventUrlList?.Any() != true)
            {
                this.AddMotoSportEventsToList(motorSport);
            }
            this.ProcessMotorSportEvents(motorSport);
            stopWatch.Stop();
            Debug.WriteLine($"Total Operation Time: {stopWatch.Elapsed.Seconds}.{stopWatch.Elapsed.Milliseconds / 10}");
            return this.ProcessDisplayResults();
        }

        private void AddMotoSportEventsToList(MotorSport motorSport) //REFACTOR THIS
        {
            Stopwatch stopWatch = Stopwatch.StartNew();
            this.doc = this.webGet.Load(motorSport.Url);
            motorSport.EventUrlList = new List<string>();

            foreach (HtmlNode node in this.doc.DocumentNode.SelectNodes(motorSport.UrlPath)) //Check URL
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
            foreach (string url in motorSport.EventUrlList)
            {
                this.FindMotorSportSessions(motorSport, url);
                Debug.WriteLine($"Thread No. {Thread.CurrentThread.ManagedThreadId} ^");
            }
            stopWatch.Stop();
            Debug.WriteLine($"Total search time: {stopWatch.Elapsed.Seconds}.{stopWatch.Elapsed.Milliseconds / 10}");
        }

        // Some Nodes return null if there is a problem with the paths or the data is missing.
        // "?" checks and allows the returned HtmlNodeCollection to be null, "??" returns a string if the node is null.
        private void FindMotorSportSessions(MotorSport motorSport, string url) //REFACTOR THIS
        {
            Stopwatch stopWatch = Stopwatch.StartNew();

            // The HTMLWeb parameters set the encoding to the URL, otherwise special characters wont display correctly.
            this.webGet.AutoDetectEncoding = false;
            this.webGet.OverrideEncoding = Encoding.UTF8;
            //this.doc.OptionEmptyCollection = true;
            this.doc = this.webGet.Load(url);

            stopWatch.Stop();
            Debug.WriteLine($"Page scrape search time: {stopWatch.Elapsed.Seconds}.{stopWatch.Elapsed.Milliseconds / 10}");

            string GrandPrix = this.doc.DocumentNode.SelectSingleNode(motorSport.GrandPrixNamePath)?.InnerText ?? "No Data";
            // Location is empty in WSBK Catalunya's URL.
            string Location = this.doc.DocumentNode.SelectSingleNode(motorSport.LocationNamePath)?.InnerText ?? "No Data";
            string Sponser = this.doc.DocumentNode.SelectSingleNode(motorSport.SponserNamePath)?.InnerText ?? "No Data";

            // Adds location to empty WSBK Catalunya.
            Location = this.WSBKCatalunyaLoctionFix(Sponser, Location);

            //Debug.Assert(this.doc.DocumentNode.SelectNodes(motorSport.EventTablePath) != null);
            //Debug.Assert(!GrandPrix.Contains("Spain"));
            //Debug.WriteIf(motorSport.Url.Contains("https://www.worldsbk.com/en/event/ESP3/2020"), $"{GrandPrix}");

            // MotoGP are updated the schedule, eventsTable loads 404.
            if (this.doc.DocumentNode.SelectNodes(motorSport.EventTablePath) != null)
            {
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
            else
            {
                Debug.WriteLine($"Event Missing: {url}");
            }
        }

        private void AddData(
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
    }
}