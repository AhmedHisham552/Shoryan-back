using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoryan.Models
{
    public class Listings
    {
        public int id { get; set; }
        public DateTime expirationDate { get; set; }
        public int price { get; set; }
        public int shreets { get; set; }
        public int elbas { get; set; }
        public int drugId { get; set; }
        public int userId { get; set; }
    }
}
