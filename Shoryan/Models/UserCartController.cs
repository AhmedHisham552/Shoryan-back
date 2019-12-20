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

		private List<int> aux_getListingIdsInCart(int userId)
		{
			string StoredProcedureName = UserCartProcedures.getCartItems;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@user_id", userId);

			List<int> ListingsIds = new List<int>();

			DataTable dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				ListingsIds.Add(Convert.ToInt32(dt.Rows[i][0]));
			}

			return ListingsIds;
		}

		[HttpGet("api/userCart/{userId}")]
		public JsonResult getListingsInCart(int userId)
		{
			List<int> ListingsIds = aux_getListingIdsInCart(userId);
			var listingsController = new ListingsController();
			List<Listings> Listings = new List<Listings>();
			
			for(int i=0;i < ListingsIds.Count;i++)
			{
				var listingControllerJson = (JsonResult)listingsController.getListingById(ListingsIds[i]);
				
				var listingJson = JsonConvert.SerializeObject(listingControllerJson.Value, Newtonsoft.Json.Formatting.Indented);
				var listing = JsonConvert.DeserializeObject<Listings>(listingJson);

				Listings.Add(listing);
			}

			return Json(Listings);
		}

		[HttpPost("api/userCart/{userId}/{listingId}")]
		public JsonResult insertItemToCart(int userId, int listingId)
		{
			string StoredProcedureName = UserCartProcedures.addCartItem;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@user_id", userId);


			return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
		}

		[HttpDelete("api/userCart/{userId}")]
		public JsonResult removeItemFromCart(int userId, int listingId)
		{
			string StoredProcedureName = UserCartProcedures.getCartItems;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@user_id", userId);
			Parameters.Add("@listing_id", listingId);

			return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
		}

	}
}