using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoiCal.Models
{
    public interface IMotorSports
    {
        string FilePath { get; }
        string SportIdentifier { get; }
        string EventTablePath { get; }
        string ClassNamePath { get; }
        string SessionNamePath { get; }
        string RaceNamePath { get; }
        string CircuitNamePath { get; }
        string LocationNamePath { get; }
        string StartDatePath { get; }
        string StartDateAttribute { get; }
        string EndDatePath { get; }
        string EndDateAttribute { get; }
        string GMTOffset { get; }

        string[] EventURLs { get; }
        string[] ExcludedClasses { get; }
        string[] ExcludedEvents { get; }
        string[] ExcludedWords { get; }
    }
}
