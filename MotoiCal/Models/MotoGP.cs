using MotoiCal.Enums;
using MotoiCal.Interfaces;

using System.Collections.Generic;

namespace MotoiCal.Models
{
    public class MotoGP : MotorSport
    {
        public MotoGP()
        {

        }

        public MotoGP(IRaceTimeTable motorSport) : base(motorSport)
        {

        }

        public override MotorSportID SportIdentifier => MotorSportID.MotoGP;
        public override string DisplayHeader => $"\n{this.Series} {this.GrandPrix} \n{this.Sponser} \n{this.Location} \n";
        public override string DisplayBody => $"{this.Series} {this.GrandPrix} {this.Session} : {this.Start} - {this.End}";
        public override string IcalendarSubject => $"{this.Series} {this.GrandPrix} {this.Session}";
        public override string IcalendarLocation => $"{this.Location}";
        public override string IcalendarDescription => $"{this.Sponser}";

        public override string Url => "https://www.motogp.com/en/calendar";
        public override string UrlPartial => "";
        public override string UrlPath => "//a[@class='event_name']";
        public override string UrlAttribute => "href";
        public override string EventTablePath => "//div[contains(@class, 'c-schedule__table-container')]/div/div[contains(@class, 'c-schedule__table-row')]";
        public override string SeriesNamePath => ".//div[@class='c-schedule__table-cell'][1]";
        public override string SessionNamePath => ".//span[@class='hidden-xs']";
        public override string GrandPrixNamePath => "//div[@class='circuit_subtitle'][2]";
        public override string SponserNamePath => "//h1[@id='circuit_title']";
        public override string LocationNamePath => "//div[@class='circuit_subtitle']";
        public override string StartDatePath => ".//span[@data-ini-time]";
        public override string StartDateAttribute => "data-ini-time";
        public override string EndDatePath => ".//span[@data-end]";
        public override string EndDateAttribute => "data-end";
        public override string GMTOffset => string.Empty;

        public override string[] ExcludedUrls => new string[]
        {
            "Test",
        };

        public override string[] ExcludedClasses => new string[]
        {
            "Moto2",
            "Moto3",
            "MotoE"
        };

        public override List<string> ExcludedEvents { get; set; } = new List<string>()
        {
            "group photo",
            "Press Conference",
            "Ceremony",
            "Marc Marquez"
            //"After The Flag",
            //"behind the scenes"
        };

        public override string[] ExcludedWords => new string[]
        {
            "Nr. "
        };
    }
}