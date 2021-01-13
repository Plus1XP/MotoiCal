using MotoiCal.Enums;

using System;

namespace MotoiCal.Interfaces
{
    public interface IRaceTimeTable
    {
        MotorSportID SportIdentifier { get; }
        string DisplayHeader { get; }
        string DisplayBody { get; }
        string Series { get; set; }
        string GrandPrix { get; set; }
        string Session { get; set; }
        string Sponser { get; set; }
        string Location { get; set; }
        DateTime Season { get; set; }
        DateTime Start { get; set; }
        DateTime End { get; set; }
        DateTime StartUTC { get; set; }
        DateTime EndUTC { get; set; }
    }
}
