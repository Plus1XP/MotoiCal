using MotoiCal.Enums;
using MotoiCal.Interfaces;
using MotoiCal.Utilities.Helpers;

using System.Collections.Generic;

namespace MotoiCal.Models
{
    public class WorldSBK : MotorSport
    {
        public WorldSBK()
        {

        }

        public WorldSBK(IRaceTimeTable motorSport) : base(motorSport)
        {

        }

        public override MotorSportID SportIdentifier => MotorSportID.WorldSBK;
        public override string DisplayHeader => $"\n{this.Series} {this.Sponser.CheckForExcludedWords(this.ExcludedWords)} \n{this.Sponser.Before("Round")}{this.GrandPrix} \n{this.Location} \n";
        public override string DisplayBody => $"{this.Series} {this.Sponser.CheckForExcludedWords(this.ExcludedWords).Before("Round")} {this.Session}: {this.Start} - {this.End}";
        public override string IcalendarSubject => $"{this.Series} {this.Sponser.CheckForExcludedWords(this.ExcludedWords).Before("Round")} {this.Session}";
        public override string IcalendarLocation => $"{this.Location}";
        public override string IcalendarDescription => $"{this.Sponser.Before("Round")}{this.GrandPrix}";

        public override string Url => "http://www.worldsbk.com/en/calendar";
        public override string UrlPartial => "http://www.worldsbk.com";
        public override string UrlPath => "//li[@class='col-xs-12 col-sm-6 col-md-3 col-lg-3']//a";
        public override string UrlAttribute => "href";

        // Alternative Paths, not needed atm.
        //public override string UrlPath => "//a[@class='track-link']";
        //public override string UrlPartial => "";
        //public override string UrlPath => "//li[@class='col-lg-3 col-md-3 col-sm-4'][3]//a";
        //public override string UrlPartial => "";
        //public override string UrlPath => "//a[contains(text(),'Round')]";
        //public override string UrlPartial => "";

        public override string EventTablePath => "//div[@class='timeIso']";
        public override string SeriesNamePath => "//title";
        public override string SessionNamePath => ".//div[contains (@class, 'cat-session')]";
        public override string GrandPrixNamePath => "//div[@class='title-event-circuit']/span";
        public override string SponserNamePath => "//div[@class='title-event-circuit']/h2";
        // Not all results are in english, ie. Imola is initalian and doesnt contain the word "Circuit".
        //public override string LocationNamePath => "//div[@class = 'info-circuit'][contains(., 'Circuit')]/br[1]/preceding-sibling::text()[1]";
        public override string LocationNamePath => "//div[@class='info-circuit'][2]/br[1]/preceding-sibling::text()[1]";
        public override string StartDatePath => ".//div[@data_ini]";
        public override string StartDateAttribute => "data_ini";
        public override string EndDatePath => ".//div[@data_end]";
        public override string EndDateAttribute => "data_end";
        public override string GMTOffset => string.Empty;

        public override List<string> ExcludedEvents { get; set; } = new List<string>()
        {
            "WorldSSP",
            "WorldSSP200"
        };

        public override string[] ExcludedWords => new string[]
        {
            "Live Video",
            "WorldSBK - ",
            "Yamaha", // Remove from Round 01 Sponser
            "Finance" // Remove from Round 01 Sponser
        };
    }
}