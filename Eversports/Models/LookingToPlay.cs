using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eversports.Models
{
    internal class LookingToPlay
    {
        public int id { get; set; }

        public List<AvailableDateTime> availableDateTimes { get; set; }
        public string country {  get; set; }
        public string city { get; set; }
        public string detailedLocation { get; set; }
        public List<string> choosenSports { get; set; }
        public string description { get; set; }
        public string jwt { get; set; }
    }
}
