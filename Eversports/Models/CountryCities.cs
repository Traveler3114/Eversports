using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eversports.Models
{
    public class CountryCities
    {
        public string Country { get; set; }
        public string ISOCode { get; set; }
        public List<string> Cities { get; set; }
    }
}
