using MotoiCal.Enums;
using MotoiCal.Interfaces;
using MotoiCal.Utilities.Helpers;

using System;

namespace MotoiCal.Models
{
    public class Formula1 : MotorSport
    {
        public Formula1()
        {

        }

        public Formula1(IRaceTimeTable motorSport) : base(motorSport)
        {

        }

        public override MotorSportID SportIdentifier => MotorSportID.Formula1;
        public override string DisplayHeader => $"\n{this.Series} {this.GrandPrix.Before($"{DateTime.Now.Year}")} \n{this.Sponser.Between("1 ", $" {DateTime.Now.Year}")} \n{this.Location} \n";
        public override string DisplayBody => $"{this.Series} {this.GrandPrix.Before("Grand")}{this.Session} : {this.Start} - {this.End}";
        public override string IcalendarSubject => $"{this.Series} {this.GrandPrix.Before("Grand")}{this.Session}";
        public override string IcalendarLocation => $"{this.Location}";
        public override string IcalendarDescription => $"{this.Sponser.Between("1 ", $" {DateTime.Now.Year}")}";

        public override string Url => $"https://www.formula1.com/en/racing/{DateTime.Now.Year}.html";
        public override string UrlPartial => "https://www.formula1.com";
        public override string UrlPath => "//a[@class='event-item-wrapper event-item-link']";
        public override string UrlAttribute => "href";
        public override string EventTablePath => "//div[contains(@class, 'f1-race-hub--timetable-listings')]//div[contains(@class, 'row js')]";
        public override string SeriesNamePath => "//a/span[text()='F1']//text()[not(ancestor::sup)]";
        public override string SessionNamePath => ".//p[@class='f1-timetable--title']";
        public override string GrandPrixNamePath => "//title";
        public override string SponserNamePath => "//h2[@class='f1--s']";
        public override string LocationNamePath => "//p[@class='f1-uppercase misc--tag no-margin']";
        public override string StartDatePath => ".";
        public override string StartDateAttribute => "data-start-time";
        public override string EndDatePath => ".";
        public override string EndDateAttribute => "data-end-time";
        public override string GMTOffset => "data-gmt-offset";

        public override string[] ExcludedUrls => new string[]
        {
            "Test"
        };

        public override string[] ExcludedWords => new string[]
        {
            "FORMULA 1",
            "2019"
        };
    }
}