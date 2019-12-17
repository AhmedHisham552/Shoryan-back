using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoryan.Models
{
    public class Couriers:User_Details
    {
        public int userId { get; set; }
        public string area { get; set; }
    }
}
