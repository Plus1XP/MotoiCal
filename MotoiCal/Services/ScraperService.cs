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
        private ScraperModel scraperModel;

        public ScraperService()
        {
            this.scraperModel = new ScraperModel();
        }

        public ObservableCollection<IRaceTimeTable> GetSeriesCollection(MotorSport motorSport)
        {
            Stopwatch stopWatch1 = Stopwatch.StartNew();
            ObservableCollection<IRaceTimeTable> timeTable = new ObservableCollection<IRaceTimeTable>();
            this.PopulateEventURLList(motorSport);
            foreach (string url in motorSport.EventUrlList)
            {
                Stopwatch stopWatch2 = Stopwatch.StartNew();
                HtmlDocument doc = this.GetHTMLDoc(url);
                this.PopulateTimeTable(motorSport, doc, timeTable);
                stopWatch2.Stop();
                Debug.WriteLine($"Page scrape search time: {stopWatch2.Elapsed.Seconds}.{stopWatch2.Elapsed.Milliseconds / 10}");
            }
            stopWatch1.Stop();
            Debug.WriteLine($"Total Operation Time: {stopWatch1.Elapsed.Seconds}.{stopWatch1.Elapsed.Milliseconds / 10}");

            return timeTable;
        }

        private HtmlDocument GetHTMLDoc(string url)
        {
            // The HTMLWeb parameters set the encoding to the URL, otherwise special characters wont display correctly.
            HtmlWeb webGet = new HtmlWeb();
            webGet.AutoDetectEncoding = false;
            webGet.OverrideEncoding = Encoding.UTF8;
            //this.doc.OptionEmptyCollection = true;
            return webGet.Load(url);
        }

        private void PopulateEventURLList<T>(T motorSport) where T : IDocNodePath, IDocExclusionList
        {
            // Checks list, Same as if list == null or motorSport.Count == 0
            if (motorSport.EventUrlList?.Any() != true)
            {
                motorSport.EventUrlList = new List<string>();
                HtmlDocument doc = this.GetHTMLDoc(motorSport.Url);
                this.AddURLToEventList(motorSport, doc);
            }
        }

        private void AddURLToEventList<T>(T motorSport) where T : IDocNodePath, IDocExclusionList
        {
            Stopwatch stopWatch3 = Stopwatch.StartNew();
            foreach (HtmlNode node in this.doc.DocumentNode.SelectNodes(motorSport.UrlPath)) // Add a check here!
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

        // Some Nodes return null if there is a problem with the paths or the data is missing.
        // "?" checks and allows the returned HtmlNodeCollection to be null, "??" returns a string if the node is null.
        private void PopulateTimeTable<T>(T motorSport, HtmlDocument doc, ObservableCollection<IRaceTimeTable> timeTable) where T : IRaceTimeTable, IDocNodePath, IDocExclusionList
        {
            motorSport.GrandPrix = this.scraperModel.GetGrandPrix(motorSport, doc);
            motorSport.Sponser = this.scraperModel.GetSponser(motorSport, doc);
            motorSport.Location = this.scraperModel.GetLocation(motorSport, doc, motorSport.Sponser);

            //Debug.Assert(this.doc.DocumentNode.SelectNodes(motorSport.EventTablePath) != null);
            //Debug.Assert(!GrandPrix.Contains("Spain"));
            //Debug.WriteIf(motorSport.Url.Contains("https://www.worldsbk.com/en/event/ESP3/2020"), $"{GrandPrix}");

            // MotoGP are updated the schedule, eventsTable loads 404.
            if (doc.DocumentNode.SelectNodes(motorSport.EventTablePath) != null)
            {
                foreach (HtmlNode node in doc.DocumentNode.SelectNodes(motorSport.EventTablePath))
                {
                    bool isEventSkipped = false;
                    motorSport.Series = this.scraperModel.GetSeries(motorSport, node, ref isEventSkipped);
                    motorSport.Session = this.scraperModel.GetSession(motorSport, node, ref isEventSkipped);
                    Tuple<string, string> dateTime = this.scraperModel.GetDateTime(motorSport, node, ref isEventSkipped);
                    motorSport.Start = this.scraperModel.ParseDateTimeLocal(dateTime).Item1;
                    motorSport.End = this.scraperModel.ParseDateTimeLocal(dateTime).Item2;
                    motorSport.StartUTC = this.scraperModel.ParseDateTimeUTC(dateTime).Item1;
                    motorSport.EndUTC = this.scraperModel.ParseDateTimeUTC(dateTime).Item2;

                    if (!isEventSkipped)
                    {
                        this.AddTimeTableToCollection(motorSport, timeTable);
                    }
                }
            }
            else
            {
                Debug.WriteLine($"Event Missing: {motorSport.GrandPrix}");
            }
        }

        private void AddTimeTableToCollection(IRaceTimeTable motorSport, ObservableCollection<IRaceTimeTable> timeTable)
        {
            switch (motorSport.SportIdentifier)
            {
                case MotorSportID.None:
                    break;
                case MotorSportID.Formula1:
                    timeTable.Add(new Formula1(motorSport));
                    break;
                case MotorSportID.MotoGP:
                    timeTable.Add(new MotoGP(motorSport));
                    break;
                case MotorSportID.WorldSBK:
                    timeTable.Add(new WorldSBK(motorSport));
                    break;
                default:
                    break;
            }
        }
    }
}
