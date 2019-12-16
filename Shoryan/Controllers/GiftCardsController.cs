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

		[HttpPost("api/GiftCards")]
		public JsonResult addGiftCard([FromBody] Dictionary<string, object> JSONinput)
		{
			var giftCardsJson = JsonConvert.SerializeObject(JSONinput["giftCards"], Newtonsoft.Json.Formatting.Indented);
			var giftCard = JsonConvert.DeserializeObject<GiftCards>(giftCardsJson);

			string StoredProcedureName = GiftCardsProcedures.addGiftCard;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@code", giftCard.code);
			Parameters.Add("@value", giftCard.value);
			Parameters.Add("@expiryDate", giftCard.expiryDate);
			return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));

		}

		[HttpPost("api/GiftCards")]
		public JsonResult redeemGiftCard( [FromBody] Dictionary<string, object> JSONinput)
		{
			var giftCardsJson = JsonConvert.SerializeObject(JSONinput["GiftCards"], Newtonsoft.Json.Formatting.Indented);
			var giftCard = JsonConvert.DeserializeObject<GiftCards>(giftCardsJson);

			var normalUserJson = JsonConvert.SerializeObject(JSONinput["NormalUsers"], Newtonsoft.Json.Formatting.Indented);
			var normalUser = JsonConvert.DeserializeObject<GiftCards>(normalUserJson);

			string StoredProcedureName = GiftCardsProcedures.redeemGiftCard;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@code", giftCard.code);
			Parameters.Add("@claimingUserId", normalUser.id);

			return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
		}

		[HttpPost("api/addGiftsCards")]
		public JsonResult getGiftCards([FromBody] Dictionary<string, object> JSONinput)
		{
			var giftCardsJson = JsonConvert.SerializeObject(JSONinput["giftCards"], Newtonsoft.Json.Formatting.Indented);
			var giftCard = JsonConvert.DeserializeObject<GiftCards>(giftCardsJson);

			string StoredProcedureName = GiftCardsProcedures.getGiftCards;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
		}

	}
}