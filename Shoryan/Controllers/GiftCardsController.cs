using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DBapplication;
using Shoryan.Models;
using Shoryan.Routes;
using Newtonsoft.Json;

namespace Shoryan.Controllers
{
    
    [ApiController]
    public class GiftCardsController : Controller
    {
		DBManager dbMan;

		public GiftCardsController()
		{
			dbMan = new DBManager();
		}

		[HttpPost("api/addGiftCard")]
		public JsonResult addGiftCard(GiftCards giftCard)
		{
			

			
			string StoredProcedureName = GiftCardsProcedures.addGiftCard;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@code", giftCard.code);
			Parameters.Add("@value", giftCard.value);
			Parameters.Add("@expiryDate", giftCard.expiryDate);
			return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));

		}

		[HttpPost("api/redeemGiftCard")]
		public JsonResult redeemGiftCard( [FromBody] Dictionary<string, object> JSONinput)
		{
			var giftCardsJson = JsonConvert.SerializeObject(JSONinput["giftCards"], Newtonsoft.Json.Formatting.Indented);
			var giftCard = JsonConvert.DeserializeObject<GiftCards>(giftCardsJson);

			var normalUserJson = JsonConvert.SerializeObject(JSONinput["normalUser"], Newtonsoft.Json.Formatting.Indented);
			var normalUser = JsonConvert.DeserializeObject<GiftCards>(normalUserJson);

			string StoredProcedureName = GiftCardsProcedures.redeemGiftCard;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@code", giftCard.code);
			Parameters.Add("@claimingUserId", normalUser.id);

			return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
		}

		//[HttpGet("api/addGiftsCards")]
		//public IActionResult getGiftCards()
		//{
		//	string StoredProcedureName = GiftCardsProcedures.getGiftCards;
		//	Dictionary<string, object> Parameters = new Dictionary<string, object>();
		//	return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
		//}

	}
}