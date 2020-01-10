using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoiCal.Models
{
    public class MotoGP : IMotorSport
    {
        public string FilePath => "MotoGP.ics";
        public string SportIdentifier => "MotoGP";
        public string EventTablePath => "//div[contains(@class, 'c-schedule__table-container')]/div/div[contains(@class, 'c-schedule__table-row')]";
        public string ClassNamePath => ".//div[@class='c-schedule__table-cell'][1]";
        public string SessionNamePath => ".//span[@class='hidden-xs']";
        public string RaceNamePath => "//div[@class='circuit_subtitle'][2]";
        public string CircuitNamePath => "//h1[@id='circuit_title']";
        public string LocationNamePath => "//div[@class='circuit_subtitle']";
        public string StartDatePath => ".//span[@data-ini-time]";
        public string StartDateAttribute => "data-ini-time";
        public string EndDatePath => ".//span[@data-end]";
        public string EndDateAttribute => "data-end";
        public string GMTOffset => string.Empty;

        public string[] EventURLs => new string[]
        {
            "http://www.motogp.com/en/event/Qatar",
            "http://www.motogp.com/en/event/Argentina",
            "http://www.motogp.com/en/event/Americas",
            "http://www.motogp.com/en/event/Spain",
            "http://www.motogp.com/en/event/France",
            "http://www.motogp.com/en/event/Italy",
            "http://www.motogp.com/en/event/Catalunya",
            "http://www.motogp.com/en/event/Netherlands",
            "http://www.motogp.com/en/event/Germany",
            "http://www.motogp.com/en/event/Czech+Republic",
            "http://www.motogp.com/en/event/Austria",
            "http://www.motogp.com/en/event/Great+Britain",
            "http://www.motogp.com/en/event/San+Marino",
            "http://www.motogp.com/en/event/Aragon",
            "http://www.motogp.com/en/event/Thailand",
            "http://www.motogp.com/en/event/Japan",
            "http://www.motogp.com/en/event/Australia",
            "http://www.motogp.com/en/event/Malaysia",
            "http://www.motogp.com/en/event/Valencia"
        };

        public string[] ExcludedClasses => new string[]
        {
            "Moto2",
            "Moto3"
        };

        public string[] ExcludedEvents => new string[]
        {
            "Class photo",
            "Press Conference",
            "Ceremony"
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