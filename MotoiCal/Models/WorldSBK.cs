using MotoiCal.Enums;
using MotoiCal.Interfaces;
using MotoiCal.Utilites.Helpers;

using System;
using System.Collections.Generic;

namespace MotoiCal.Models
{
    public class WorldSBK : IRaceData, IMotorSport
    {
        public string Series { get; set; } // WorldSBK
        public string GrandPrix { get; set; } // Round ##
        public string Session { get; set; } // Race #
        public string Sponser { get; set; } // Yamaha Finance Australian Round
        public string Location { get; set; } // Phillip Island
        public DateTime Season { get; set; } // YYYY
        public DateTime Start { get; set; } // Local DD/MM/YYYY HH:MM:SS
        public DateTime End { get; set; } // Local DD/MM/YYYY HH:MM:SS
        public DateTime StartUTC { get; set; } // iCal DD/MM/YYYY HH:MM:SS
        public DateTime EndUTC { get; set; } // iCal DD/MM/YYYY HH:MM:SS

        public string DisplayHeader => $"\n{this.Series} {this.CheckForExcludedWords(this.Sponser)} \n{this.Sponser.Before("Round")}{this.GrandPrix} \n{this.Location} \n";
        public string DisplayBody => $"{this.Series} {this.CheckForExcludedWords(this.Sponser.Before("Round"))} {this.Session}: {this.Start} - {this.End}";
        public string IcalendarSubject => $"{this.Series} {this.CheckForExcludedWords(this.Sponser.Before("Round"))} {this.Session}";
        public string IcalendarLocation => $"{this.Location}";
        public string IcalendarDescription => $"{this.Sponser.Before("Round")}{this.GrandPrix}";
        public bool IsIcalendarEventTriggerActive { get; }
        public int IcalendarEventTriggerMins { get; }

        public MotorSportID SportIdentifier => MotorSportID.WorldSBK;
        public string FilePath => "WorldSBK.ics";
        public string Url => "http://www.worldsbk.com/en/calendar";
        public string UrlPartial => "http://www.worldsbk.com";
        public string UrlPath => "//li[@class='col-xs-12 col-sm-6 col-md-3 col-lg-3']//a";
        public string UrlAttribute => "href";

        // Alternative Paths, not needed atm.
        //public string UrlPath => "//a[@class='track-link']";
        //public string UrlPartial => "";
        //public string UrlPath => "//li[@class='col-lg-3 col-md-3 col-sm-4'][3]//a";
        //public string UrlPartial => "";
        //public string UrlPath => "//a[contains(text(),'Round')]";
        //public string UrlPartial => "";

        public string EventTablePath => "//div[@class='timeIso']";
        public string SeriesNamePath => "//title";
        public string SessionNamePath => ".//div[contains (@class, 'cat-session')]";
        public string GrandPrixNamePath => "//div[@class='title-event-circuit']/span";
        public string SponserNamePath => "//div[@class='title-event-circuit']/h2";
        // Not all results are in english, ie. Imola is initalian and doesnt contain the word "Circuit".
        //public string LocationNamePath => "//div[@class = 'info-circuit'][contains(., 'Circuit')]/br[1]/preceding-sibling::text()[1]";
        public string LocationNamePath => "//div[@class='info-circuit'][2]/br[1]/preceding-sibling::text()[1]";
        public string StartDatePath => ".//div[@data_ini]";
        public string StartDateAttribute => "data_ini";
        public string EndDatePath => ".//div[@data_end]";
        public string EndDateAttribute => "data_end";
        public string GMTOffset => string.Empty;

        public bool IsEventReminderActive { get; set; }
        public int EventReminderMins { get; set; }

        List<string> IMotorSport.EventUrlList { get; set; }

        public string[] ExcludedUrls => new string[]
        {
        };

        public string[] ExcludedClasses => new string[]
        {
        };

        public List<string> ExcludedEvents { get; set; } = new List<string>()
        {
            "WorldSSP",
            "WorldSSP200"
        };

        public string[] ExcludedWords => new string[]
        {
            "Live Video",
            "WorldSBK - ",
            "Yamaha", // Remove from Round 01 Sponser
            "Finance" // Remove from Round 01 Sponser
        };

        // Removes unwanted strings from Sponser.
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