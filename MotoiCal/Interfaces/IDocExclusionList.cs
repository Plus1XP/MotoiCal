using System.Collections.Generic;

namespace MotoiCal.Interfaces
{
    public interface IDocExclusionList
    {
        string[] ExcludedUrls { get; }
        List<string> ExcludedClasses { get; set; }
        List<string> ExcludedEvents { get; set; }
        string[] ExcludedWords { get; }
    }
}
