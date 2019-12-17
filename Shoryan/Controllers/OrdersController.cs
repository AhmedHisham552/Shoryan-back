//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using DBapplication;
//using Shoryan.Models;
//using Shoryan.Routes;
//using Newtonsoft.Json;
//using System.Data;

//namespace Shoryan.Controllers
//{

//	[ApiController]
//	public class OrdersController : Controller
//	{
//		DBManager dbMan;

//		public OrdersController()
//		{
//			dbMan = new DBManager();
//		}

//		[HttpPost("api/Order")]
//		public IActionResult addOrder([FromBody] Dictionary<string, object> JSONinput)
//		{
//			var orderJson = JsonConvert.SerializeObject(JSONinput["Order"], Newtonsoft.Json.Formatting.Indented);
//			var order = JsonConvert.DeserializeObject<Orders>(orderJson);


//			Object[] array = new Object[order.Items.Count];

//			foreach (var item in order.Items)
//			{
//				for (int runs = 0; runs < order.Items.Count; runs++)
//				{
//					array[runs] = order.Items[runs];
//				}
//			}
//			string StoredProcedureName = ListingsProcedures.addListing;

//			Dictionary<string, object> Parameters = new Dictionary<string, object>();


//			int returnValue = dbMan.ExecuteNonQuery(StoredProcedureName, Parameters);

//			//if (returnValue == 0) return StatusCode(500);
//			//return Json(new { test = "test"});
//			return Json(array);

//		}

//	}
//}