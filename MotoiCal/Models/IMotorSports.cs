using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoiCal.Models
{
    public interface IMotorSports
    {
        string FilePath { get; set; }
        string SportIdentifier { get; set; }
        string EventTablePath { get; set; }
        string ClassNamePath { get; set; }
        string SessionNamePath { get; set; }
        string RaceNamePath { get; set; }
        string CircuitNamePath { get; set; }
        string LocationNamePath { get; set; }
        string StartDatePath { get; set; }
        string StartDateAttribute { get; set; }
        string EndDatePath { get; set; }
        string EndDateAttribute { get; set; }
        string GMTOffset { get; set; }

        string[] EventURLs { get; set; }
        string[] ExcludedClasses { get; set; }
        string[] ExcludedEvents { get; set; }
        string[] ExcludedWords { get; set; }
    }
}
