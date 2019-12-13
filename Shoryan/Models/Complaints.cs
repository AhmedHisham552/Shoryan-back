using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoryan.Models
{
    public class Complaints
    {
        public int id { get; set; }
        public string subject { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public DateTime timeStamp { get; set; }
        public int normalUserID { get; set; }
        public int courierID { get; set; }
        public Boolean fromCourierToUser { get; set; }
    }
}
