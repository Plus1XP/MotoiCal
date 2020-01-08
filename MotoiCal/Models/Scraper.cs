using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace MotoiCal.Models
{
    public class Scraper
    {
        private HtmlWeb webGet;
        private HtmlDocument doc;
        private CalendarManager iCalendar;
        private IMotorSport motorSport;
        private StringBuilder resultsOutput;

        public Scraper()
        {
            this.webGet = new HtmlWeb();
            this.doc = new HtmlDocument();
            this.iCalendar = new CalendarManager();
        }

        public string GenerateiCalendar()
        {
            return this.resultsOutput != null ? this.iCalendar.CreateICSFile(this.motorSport.FilePath) : "Can not generate ICS file without first showing dates";
        }

        public string ReadiCalendar()
        {
            return this.iCalendar.ReadICSFile(this.motorSport.FilePath);
        }

        public string DeleteiCalendar()
        {
            return this.iCalendar.DeleteICSFile(this.motorSport.FilePath);
        }
    }
}
