using MotoiCal.Enums;
using MotoiCal.Interfaces;
using MotoiCal.Utilites.Helpers;

using System;
using System.Collections.Generic;

namespace MotoiCal.Models
{
    public class Formula1 : IRaceData, IMotorSport
    {
        public string Series { get; set; } // F1
        public string GrandPrix { get; set; } // Australian Grand Prix YYYY
        public string Session { get; set; } // Race
        public string Sponser { get; set; } // Rolex Australian Grand Prix
        public string Location { get; set; } // Melbourne Grand Prix Circuit
        public DateTime Season { get; set; } // YYYY
        public DateTime Start { get; set; } // Local DD/MM/YYYY HH:MM:SS
        public DateTime End { get; set; } // Local DD/MM/YYYY HH:MM:SS
        public DateTime StartUTC { get; set; } // iCal DD/MM/YYYY HH:MM:SS
        public DateTime EndUTC { get; set; } // iCal DD/MM/YYYY HH:MM:SS

        public string DisplayHeader => $"\n{this.Series} {this.GrandPrix.Before($"{DateTime.Now.Year}")} \n{this.Sponser.Between("1 ", $" {DateTime.Now.Year}")} \n{this.Location} \n";
        public string DisplayBody => $"{this.Series} {this.GrandPrix.Before("Grand")}{this.Session} : {this.Start} - {this.End}";
        public string IcalendarSubject => $"{this.Series} {this.GrandPrix.Before("Grand")}{this.Session}";
        public string IcalendarLocation => $"{this.Location}";
        public string IcalendarDescription => $"{this.Sponser.Between("1 ", $" {DateTime.Now.Year}")}";

        public MotorSportID SportIdentifier => MotorSportID.Formula1;
        public string FilePath => "Formula1.ics";
        public string Url => $"https://www.formula1.com/en/racing/{DateTime.Now.Year}.html";
        public string UrlPartial => "https://www.formula1.com";
        public string UrlPath => "//a[@class='event-item-wrapper event-item-link']";
        public string UrlAttribute => "href";
        public string EventTablePath => "//div[contains(@class, 'f1-race-hub--timetable-listings')]//div[contains(@class, 'row js')]";
        public string SeriesNamePath => "//a/span[text()='F1']//text()[not(ancestor::sup)]";
        public string SessionNamePath => ".//p[@class='f1-timetable--title']";
        public string GrandPrixNamePath => "//title";
        public string SponserNamePath => "//h2[@class='f1--s']";
        public string LocationNamePath => "//p[@class='f1-uppercase misc--tag no-margin']";
        public string StartDatePath => ".";
        public string StartDateAttribute => "data-start-time";
        public string EndDatePath => ".";
        public string EndDateAttribute => "data-end-time";
        public string GMTOffset => "data-gmt-offset";

        public bool IsEventReminderActive { get; set; }
        public int EventReminderMins { get; set; }

        public List<string> EventUrlList { get; set; }

        public string[] ExcludedUrls => new string[]
        {
            "Test"
        };

        public string[] ExcludedClasses => new string[]
        {
        };

        public List<string> ExcludedEvents { get; set; } = new List<string>();

        public string[] ExcludedWords => new string[]
        {
            "FORMULA 1",
            "2019"
        };

        // Removes unwanted strings from the Formula1 circuit names.
        public string CheckForExcludedWords(string stringToCheck)
        {
            foreach (string word in this.ExcludedWords)
            {
                stringToCheck = stringToCheck.Replace(word, string.Empty);
            }
            return stringToCheck.Trim();
        }
    }
}