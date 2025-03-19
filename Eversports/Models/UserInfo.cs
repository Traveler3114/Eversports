using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eversports.Models
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
        //kasnije mozemo dodati logiku u get set sto je enkapsulacija
        //private string _name = string.Empty;

        //public string name
        //{
        //    get
        //    {
        //        return _name;
        //    }
        //    set
        //    {
        //        // Validation logic (e.g., checking if the name is empty)
        //        if (string.IsNullOrEmpty(value))
        //        {
        //            throw new ArgumentException("Name cannot be empty or null.");
        //        }
        //        _name = value;
        //    }
        //}

        //ako ovako napisem onda mozemo postaviti varijablu samo u klasi
        //public string Name { get; private set; }

        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string password { get; set; }
        public string email { get; set; }

    }
}
