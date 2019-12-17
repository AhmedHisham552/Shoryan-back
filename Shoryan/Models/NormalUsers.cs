using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoryan.Models
{
    public class NormalUsers:User_Details
    {
        public int id { get; set; }
        public char gender { get; set; }
        public int balance { get; set; }
    }
}
