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
            DateTime.TryParse(dateTime, out DateTime parsedDateTime);
            return parsedDateTime.ToUniversalTime();
        }

        public DateTime ParseDateTimeToLocal(string dateTime)
        {
            DateTime.TryParse(dateTime, out DateTime parsedDateTime);
            return parsedDateTime.ToLocalTime();
        }
    }
}
