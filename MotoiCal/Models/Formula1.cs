namespace MotoiCal.Models
{
    public class Formula1 : IMotorSport
    {
        public string FilePath => "Formula1.ics";
        public string SportIdentifier => "Formula1";
        public string EventTablePath => "//div[contains(@class, 'f1-race-hub--timetable-listings')]//div[contains(@class, 'row js')]";
        public string ClassNamePath => "//a/span[text()='F1']//text()[not(ancestor::sup)]";
        public string SessionNamePath => ".//p[@class='f1-timetable--title']";
        public string RaceNamePath => "//p[contains(@class, 'race-location')]";
        public string CircuitNamePath => "//h1[@class='f1--s']";
        public string LocationNamePath => "//p[@class='f1-uppercase misc--tag no-margin']";
        public string StartDatePath => ".";
        public string StartDateAttribute => "data-start-time";
        public string EndDatePath => ".";
        public string EndDateAttribute => "data-end-time";
        public string GMTOffset => "data-gmt-offset";

        public string[] EventURLs => new string[]
        {
            "https://www.formula1.com//en/racing/2019/Australia.html",
            "https://www.formula1.com//en/racing/2019/Bahrain.html",
            "https://www.formula1.com//en/racing/2019/China.html",
            "https://www.formula1.com//en/racing/2019/Azerbaijan.html",
            "https://www.formula1.com//en/racing/2019/Spain.html",
            "https://www.formula1.com//en/racing/2019/Monaco.html",
            "https://www.formula1.com//en/racing/2019/Canada.html",
            "https://www.formula1.com//en/racing/2019/france.html",
            "https://www.formula1.com//en/racing/2019/Austria.html",
            "https://www.formula1.com//en/racing/2019/Great_Britain.html",
            "https://www.formula1.com//en/racing/2019/Germany.html",
            "https://www.formula1.com//en/racing/2019/Hungary.html",
            "https://www.formula1.com//en/racing/2019/Belgium.html",
            "https://www.formula1.com//en/racing/2019/Italy.html",
            "https://www.formula1.com//en/racing/2019/Singapore.html",
            "https://www.formula1.com//en/racing/2019/Russia.html",
            "https://www.formula1.com//en/racing/2019/Japan.html",
            "https://www.formula1.com//en/racing/2019/Mexico.html",
            "https://www.formula1.com//en/racing/2019/United_States.html",
            "https://www.formula1.com//en/racing/2019/Brazil.html",
            "https://www.formula1.com//en/racing/2019/Abu_Dhabi.html"
        };

        public string[] ExcludedClasses => new string[]
        {
        };

        public string[] ExcludedEvents => new string[]
        {
        };

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