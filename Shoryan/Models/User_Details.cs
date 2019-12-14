using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoryan.Models
{
    public class User_Details
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime registrationDate { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public float rating { get; set; }
        public string password { get; set; }
        public string imgUrl { get; set; }
    }
}
