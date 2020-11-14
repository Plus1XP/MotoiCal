using System;
using System.Collections.Generic;

namespace MotoiCal.Models
{
    public class MotoGP : IRaceData, IMotorSport
    {
        public string Series { get; set; } // MotoGP
        public string GrandPrix { get; set; } // Qatar
        public string Session { get; set; } // Race
        public string Sponser { get; set; } // Grand Prix of Qatar
        public string Location { get; set; } // Losail International Circuit
        public DateTime Season { get; set; } // YYYY
        public DateTime Start { get; set; } // Local DD/MM/YYYY HH:MM:SS
        public DateTime End { get; set; } // Local DD/MM/YYYY HH:MM:SS
        public DateTime StartUTC { get; set; } // iCal DD/MM/YYYY HH:MM:SS
        public DateTime EndUTC { get; set; } // iCal DD/MM/YYYY HH:MM:SS

        public string DisplayHeader => $"\n{this.Series} {this.GrandPrix} \n{this.Sponser} \n{this.Location} \n";
        public string DisplayBody => $"{this.Series} {this.GrandPrix} {this.Session} : {this.Start} - {this.End}";
        public string IcalendarSubject => $"{this.Series} {this.GrandPrix} {this.Session}";
        public string IcalendarLocation => $"{Location}";
        public string IcalendarDescription => $"{Sponser}";

        public MotorSportID SportIdentifier => MotorSportID.MotoGP;
        public string FilePath => "MotoGP.ics";
        public string Url => "https://www.motogp.com/en/calendar";
        public string UrlPartial => "";
        public string UrlPath => "//a[@class='event_name']";
        public string UrlAttribute => "href";
        public string EventTablePath => "//div[contains(@class, 'c-schedule__table-container')]/div/div[contains(@class, 'c-schedule__table-row')]";
        public string SeriesNamePath => ".//div[@class='c-schedule__table-cell'][1]";
        public string SessionNamePath => ".//span[@class='hidden-xs']";
        public string GrandPrixNamePath => "//div[@class='circuit_subtitle'][2]";
        public string SponserNamePath => "//h1[@id='circuit_title']";
        public string LocationNamePath => "//div[@class='circuit_subtitle']";
        public string StartDatePath => ".//span[@data-ini-time]";
        public string StartDateAttribute => "data-ini-time";
        public string EndDatePath => ".//span[@data-end]";
        public string EndDateAttribute => "data-end";
        public string GMTOffset => string.Empty;

        public bool IsEventReminderActive { get; set; }
        public int EventReminderMins { get; set; }

        List<string> IMotorSport.EventUrlList { get; set; }

        public string[] ExcludedUrls => new string[]
        {
            "Test",
        };

        public string[] ExcludedClasses => new string[]
        {
            "Moto2",
            "Moto3",
            "MotoE"
        };

        public List<string> ExcludedEvents { get; set; } = new List<string>()
        {
            "group photo",
            "Press Conference",
            "Ceremony",
            "Marc Marquez"
            //"After The Flag",
            //"behind the scenes"
        };

        public string[] ExcludedWords => new string[]
        {
            "Nr. "
        };

        // Not in use.
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