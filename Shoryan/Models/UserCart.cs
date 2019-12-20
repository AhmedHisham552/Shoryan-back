using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoryan.Models
{
	public class UserCart
	{
		public int sellerId { get; set; }
		public string sellerName { get; set; }
		public int drugId { get; set; }
		public string drugName { get; set; }
		public int listingId { get; set; }
		public int shreets { get; set; }
		public int elbas { get; set; }
		public int price { get; set; }


	}
}
