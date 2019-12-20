using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DBapplication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shoryan.Models;
using Shoryan.StoredProcedures;
using Shoryan.Controllers;

namespace Shoryan.Models
{
    [ApiController]
	public class UserCartController : Controller
    {
		DBManager dbMan;
		public UserCartController()
		{
			dbMan = new DBManager();
		}


		[HttpGet("api/userCart/{userId}")]
		public JsonResult getCartItems(int userId)
		{
			string StoredProcedureName = UserCartProcedures.getCartItems;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@user_id", userId);

			var dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);

			List<UserCart> UserCarts = new List<UserCart>();

			for (int i = 0; i < dt.Rows.Count; i++)
			{
				var Cart = new UserCart();
				Cart.drugId = Convert.ToInt32(dt.Rows[i]["drugId"]);
				Cart.drugName = Convert.ToString(dt.Rows[i]["drugName"]);
				Cart.sellerId = Convert.ToInt32(dt.Rows[i]["sellerId"]);
				Cart.sellerName = Convert.ToString(dt.Rows[i]["sellerName"]);
				Cart.listingId = Convert.ToInt32(dt.Rows[i]["listingId"]);
				Cart.shreets = Convert.ToInt32(dt.Rows[i]["shreets"]);
				Cart.elbas = Convert.ToInt32(dt.Rows[i]["elbas"]);
				Cart.price = Convert.ToInt32(dt.Rows[i]["price"]);

				UserCarts.Add(Cart);
			}
			

			return Json(UserCarts);
		}

		[HttpPost("api/userCart/{userId}/{listingId}")]
		public JsonResult insertItemToCart(int userId, int listingId)
		{
			string StoredProcedureName = UserCartProcedures.addCartItem;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@user_id", userId);
			Parameters.Add("@listing_id", listingId);

			return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
		}

		[HttpDelete("api/userCart/{userId}/{listingId}")]
		public JsonResult removeItemFromCart(int userId, int listingId)
		{
			string StoredProcedureName = UserCartProcedures.deleteCartItem;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@user_id", userId);
			Parameters.Add("@listing_id", listingId);

			return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
		}

	}
}