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

namespace Shoryan.Controllers
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
			if(dt == null)
			{
				return Json(null);
			}
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
		public IActionResult insertItemToCart(int userId, int listingId)
		{
			string StoredProcedureName = UserCartProcedures.addCartItem;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@user_id", userId);
			Parameters.Add("@listing_id", listingId);

			try
			{
				int returnCode = dbMan.ExecuteNonQuery(StoredProcedureName, Parameters);
				if (returnCode == -1)
				{
					return StatusCode(200, "Item added to cart");
				}
				else
					return StatusCode(500, "Failed to add item to cart");
			}
			catch (Exception)
			{
				return StatusCode(500, "Failed to add item to cart");
				throw;
			}
		}

		[HttpDelete("api/userCart/{userId}/{listingId}")]
		public IActionResult removeItemFromCart(int userId, int listingId)
		{
			string StoredProcedureName = UserCartProcedures.deleteCartItem;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@user_id", userId);
			Parameters.Add("@listing_id", listingId);

			try
			{
				int returnCode = dbMan.ExecuteNonQuery(StoredProcedureName, Parameters);
				if (returnCode == -1)
					return StatusCode(200, "Item removed successfully");
				else
					return StatusCode(500, "Failed to remove item");
			}
			catch (Exception)
			{
				return StatusCode(500, "Failed to remove item");
				throw;
			}
		}

		[HttpDelete("api/emptyCart/{userId}")]
		public IActionResult emptyUserCart(int userId)
		{
			string StoredProcedureName = UserCartProcedures.emptyUserCart;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@user_id", userId);

			try
			{
				int returnCode = dbMan.ExecuteNonQuery(StoredProcedureName, Parameters);
				if (returnCode == -1)
				{
					return StatusCode(200, "Cart emptied successfuly");
				}
				else
					return StatusCode(500, "Failed to empty cart");
			}
			catch (Exception)
			{
				return StatusCode(500, "Failed to empty cart");
				throw;
			}
		}

	}
}