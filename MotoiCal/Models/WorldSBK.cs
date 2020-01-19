using System;
using System.Collections.Generic;

namespace MotoiCal.Models
{
    public class WorldSBK : IMotorSport, IRaceData
    {
        public MotorSportID SportIdentifier => MotorSportID.WorldSBK;
        public string FilePath => "WorldSBK.ics";
        public string Url => "http://www.worldsbk.com/en/calendar";
        // Alternative Paths, not needed atm.
        //public string UrlPartial => "http://www.worldsbk.com";
        //public string UrlPath => "//a[@class='track-link']";
        //public string UrlPartial => "";
        //public string UrlPath => "//li[@class='col-lg-3 col-md-3 col-sm-4'][3]//a";
        public string UrlPartial => "";
        public string UrlPath => "//a[contains(text(),'Round')]";
        public string UrlAttribute => "href";
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

        List<string> IMotorSport.EventUrlList { get; set; }

        public string[] ExcludedUrls => new string[]
        {
        };

        public string[] ExcludedClasses => new string[]
        {
        };

        public string[] ExcludedEvents => new string[]
        {
            "WorldSSP",
            "WorldSSP200"
        };

        public string[] ExcludedWords => new string[]
        {
            "Live Video",
            "WorldSBK - "
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