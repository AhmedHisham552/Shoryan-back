using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoryan.Models
{
    public class GiftCards
    {
        public int id { get; set; }
        public string code { get; set; }
        public int value { get; set; }
        public DateTime expiryDate { get; set; }
        public Boolean used { get; set; }
        public int claimingUserId { get; set; }
    }
}
