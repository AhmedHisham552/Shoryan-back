using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoryan.Models
{
    public class OrderAssignment
    {
        public int userId { get; set; }
        public int courierId { get; set; }
        public int orderId { get; set; }
    }
}
