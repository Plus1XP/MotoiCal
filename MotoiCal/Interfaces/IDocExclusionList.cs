using System.Collections.Generic;

namespace MotoiCal.Interfaces
{
    public interface IDocExclusionList
    {
        string[] ExcludedUrls { get; }
        string[] ExcludedClasses { get; }
        List<string> ExcludedEvents { get; set; }
        string[] ExcludedWords { get; }
    }
}
