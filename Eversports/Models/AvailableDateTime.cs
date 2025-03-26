using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eversports.Models
{
    class AvailableDateTime
    {
        public string? Date { get; set; }
        public string? FromTime { get; set; }
        public string? ToTime { get; set; }

        public override bool Equals(object? obj)
        {
            var other = obj as AvailableDateTime;
            return Date == other?.Date && FromTime == other?.FromTime && ToTime == other?.ToTime;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Date, FromTime, ToTime);
        }
    }
}
