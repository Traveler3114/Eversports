using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eversports.Model
{
    public class UserInfo
    {
        //Umjesto da pisem
        // private int id; i
        // public int Id
        //{
        //    get { return id; }
        //    set { id = value; }
        //}
        //napisao sam samo
        // public int Id { get; set; }
        //zato sto je to automatski generira vaarijablu private int id u C#

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
