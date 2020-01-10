using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoiCal.Models
{
    public class WSBK : IMotorSport
    {
        public string FilePath => "WSBK.ics";
        public string SportIdentifier => "WSBK";
        public string EventTablePath => "//div[@class='timeIso']";
        public string ClassNamePath => "//title";
        public string SessionNamePath => ".//div[contains (@class, 'cat-session')]";
        public string RaceNamePath => "//div[@class='title-event-circuit']/span";
        public string CircuitNamePath => "//div[@class='title-event-circuit']/h2";

        // Location value is filler as the website doesnt have much scrape.
        public string LocationNamePath => "//*[text()[contains(., 'WorldSBK')]]";
        public string StartDatePath => ".//div[@data_ini]";
        public string StartDateAttribute => "data_ini";
        public string EndDatePath => ".//div[@data_end]";
        public string EndDateAttribute => "data_end";
        public string GMTOffset => string.Empty;

        public string[] EventURLs => new string[]
        {
            "http://www.worldsbk.com/en/event/AUS/2019",
            "http://www.worldsbk.com/en/event/THA/2019",
            "http://www.worldsbk.com/en/event/ESP1/2019",
            "http://www.worldsbk.com/en/event/NED/2019",
            "http://www.worldsbk.com/en/event/ITA1/2019",
            "http://www.worldsbk.com/en/event/ESP2/2019",
            "http://www.worldsbk.com/en/event/ITA2/2019",
            "http://www.worldsbk.com/en/event/GBR/2019",
            "http://www.worldsbk.com/en/event/USA/2019",
            "http://www.worldsbk.com/en/event/POR/2019",
            "http://www.worldsbk.com/en/event/FRA/2019",
            "http://www.worldsbk.com/en/event/ARG/2019",
            "http://www.worldsbk.com/en/event/QAT/2019"
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
