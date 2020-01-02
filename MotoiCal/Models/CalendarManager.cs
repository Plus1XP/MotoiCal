using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoiCal.Models
{
    public class CalendarManager
    {
        public CalendarManager()
        {

        }

        public DateTime ParseDateTimeToUTC(string dateTime)
        {
            DateTime parsedDateTime;
            DateTime.TryParse(dateTime, out parsedDateTime);
            return parsedDateTime.ToUniversalTime();
        }
    }
}
