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
		public IActionResult getGiftCards()
		{
			string StoredProcedureName = GiftCardsProcedures.getGiftCards;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			try
			{
				return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
				throw;
			}


		}

		[HttpGet("api/GiftCards/{userId}")]
		public IActionResult getGiftCardsByUserId(int userId)
		{
			string StoredProcedureName = GiftCardsProcedures.getGiftCards;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@userId", userId);

			try
			{
				return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
				throw;
			}

		}

		[HttpPost("api/GiftCards")]
		public IActionResult addGiftCard([FromBody] Dictionary<string, object> JSONinput)
		{
			GiftCards giftCard = new GiftCards();

			try
			{
				var giftCardsJson = JsonConvert.SerializeObject(JSONinput["giftCards"], Newtonsoft.Json.Formatting.Indented);
				giftCard = JsonConvert.DeserializeObject<GiftCards>(giftCardsJson);
			}
			catch (Exception)
			{
				return StatusCode(500, "Error parsing JSON");
				throw;
			}

			string StoredProcedureName = GiftCardsProcedures.addGiftCard;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@code", giftCard.code);
			Parameters.Add("@value", giftCard.value);
			Parameters.Add("@expiryDate", giftCard.expiryDate);

			try
			{
				int returnCode = dbMan.ExecuteNonQuery(StoredProcedureName, Parameters);
				if(returnCode == -1)
				{
					return StatusCode(200, "Giftcard added successfully");
				}else
				{
					return StatusCode(500, "Failed to add giftcard");
				}
			}
			catch (Exception)
			{
				return StatusCode(500, "Failed to add giftcard");
				throw;
			}


		}

		[HttpPut("api/GiftCards/{code}/{claimingUserId}")]
		public IActionResult redeemGiftCard(string code, int claimingUserId )
		{
			string StoredProcedureName = GiftCardsProcedures.redeemGiftCard;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@code", code);
			Parameters.Add("@claimingUserId", claimingUserId);

			try
			{
				int returnCode = dbMan.ExecuteNonQuery(StoredProcedureName, Parameters);
				if(returnCode != -1)
				{
					return StatusCode(500, "Giftcard code is invalid");
				}else
				{
					return StatusCode(200, "Giftcard redeemed successfully");
				}
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
				throw;
			}
		}

		[HttpDelete("api/GiftCards/{id}")]
		public IActionResult deleteGiftCard(int id)
		{
			string StoredProcedureName = GiftCardsProcedures.deleteGiftCard;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@id", id);

			try
			{
				int returnCode = dbMan.ExecuteNonQuery(StoredProcedureName, Parameters);
				if(returnCode == -1)
				{
					return StatusCode(200, "Giftcard deleted successfully");
				}else
				{
					return StatusCode(500, "Internal server error");
				}
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal server error");
				throw;
			}
		}

	}
}