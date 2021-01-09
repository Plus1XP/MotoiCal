using System.Collections.Generic;

namespace MotoiCal.Interfaces
{
    public interface IDocNodePath
    {
        List<string> EventUrlList { get; set; }
        string Url { get; }
        string UrlPartial { get; }
        string UrlPath { get; }
        string UrlAttribute { get; }
        string EventTablePath { get; }
        string SeriesNamePath { get; }
        string SessionNamePath { get; }
        string GrandPrixNamePath { get; }
        string SponserNamePath { get; }
        string LocationNamePath { get; }
        string StartDatePath { get; }
        string StartDateAttribute { get; }
        string EndDatePath { get; }
        string EndDateAttribute { get; }
        string GMTOffset { get; }
    }
}
