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

		[HttpGet("api/GiftCards")]
		public JsonResult getGiftCards()
		{
			string StoredProcedureName = GiftCardsProcedures.getGiftCards;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));

		}

		[HttpGet("api/GiftCards/{userId}")]
		public JsonResult getGiftCardsByUserId(int userId)
		{
			string StoredProcedureName = GiftCardsProcedures.getGiftCards;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@userId", userId);

			return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));

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

		[HttpPut("api/GiftCards/{code}/{claimingUserId}")]
		public JsonResult redeemGiftCard(string code, int claimingUserId )
		{
			string StoredProcedureName = GiftCardsProcedures.redeemGiftCard;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@code", code);
			Parameters.Add("@claimingUserId", claimingUserId);

			return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
		}

		[HttpDelete("api/GiftCards/{id}")]
		public JsonResult deleteGiftCard(int id)
		{
			string StoredProcedureName = GiftCardsProcedures.deleteGiftCard;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@id", id);

			return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
		}

	}
}