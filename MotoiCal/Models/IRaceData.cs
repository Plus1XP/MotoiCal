using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoiCal.Models
{
    public interface IRaceData
    {
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
        string DisplayHeader { get; }
        string DisplayBody { get; }
        string IcalendarSubject {get;}
        string IcalendarLocation { get; }
        string IcalendarDescription { get; }
    }
}
