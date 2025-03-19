using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eversports.Models
{
    class Response
    {
        public string status { get; set; } = string.Empty;
        public UserInfo user { get; set; } = new UserInfo();
    }
}
