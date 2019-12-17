using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoryan.Models
{
    public class Orders
    {
        public int id { get; set; }
        public DateTime orderDate { get; set; }
        public DateTime expectedDeliveryDate { get; set; }
        public int itemsPrice { get; set; }
        public int deliverPrice { get; set; }
        public int discount { get; set; }
        public float userReviewRating { get; set; }
        public string userReviewText { get; set; }
        public int userId { get; set; }
        public string courierReviewText { get; set; }
        public float courierReviewRating { get; set; }
        public int courierId { get; set; }
        public List<Listings> Listings { get; set; }
    }
}
