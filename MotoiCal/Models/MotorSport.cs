using MotoiCal.Enums;
using MotoiCal.Interfaces;

using System;
using System.Collections.Generic;

namespace MotoiCal.Models
{
    public abstract class MotorSport : IRaceTimeTable, ICalendarEvent, IDocNodePath, IDocExclusionList
    {
        // IRaceTimeTable
        public abstract MotorSportID SportIdentifier { get; }
        public string Series { get; set; } // F1
        public string GrandPrix { get; set; } // Australian Grand Prix YYYY
        public string Session { get; set; } // Race
        public string Sponser { get; set; } // Rolex Australian Grand Prix
        public string Location { get; set; } // Melbourne Grand Prix Circuit
        public DateTime Season { get; set; } // YYYY
        public DateTime Start { get; set; } // Local DD/MM/YYYY HH:MM:SS
        public DateTime End { get; set; } // Local DD/MM/YYYY HH:MM:SS
        public DateTime StartUTC { get; set; } // iCal DD/MM/YYYY HH:MM:SS
        public DateTime EndUTC { get; set; } // iCal DD/MM/YYYY HH:MM:SS

        // ICalendarEvent
        public abstract string DisplayHeader { get; }
        public abstract string DisplayBody { get; }
        public abstract string IcalendarSubject { get; }
        public abstract string IcalendarLocation { get; }
        public abstract string IcalendarDescription { get; }
        public bool IsEventReminderActive { get; set; }
        public int EventReminderMins { get; set; }
        public abstract string FilePath { get; }

        // IScraperPath
        public virtual List<string> EventUrlList { get; set; }
        public abstract string Url { get; }
        public abstract string UrlPartial { get; }
        public abstract string UrlPath { get; }
        public abstract string UrlAttribute { get; }
        public abstract string EventTablePath { get; }
        public abstract string SeriesNamePath { get; }
        public abstract string SessionNamePath { get; }
        public abstract string GrandPrixNamePath { get; }
        public abstract string SponserNamePath { get; }
        public abstract string LocationNamePath { get; }
        public abstract string StartDatePath { get; }
        public abstract string StartDateAttribute { get; }
        public abstract string EndDatePath { get; }
        public abstract string EndDateAttribute { get; }
        public abstract string GMTOffset { get; }

        // IScraperList
        public virtual string[] ExcludedUrls => new string[]
        {
        };

        public virtual string[] ExcludedClasses => new string[]
        {
        };

        public virtual List<string> ExcludedEvents { get; set; } = new List<string>();

        public virtual string[] ExcludedWords => new string[]
        {
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
