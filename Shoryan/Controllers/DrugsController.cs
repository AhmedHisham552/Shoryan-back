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
	public class DrugsController : Controller
    {
		DBManager dbMan;
		public DrugsController()
		{
			dbMan = new DBManager();
		}

		[HttpGet("api/getDrugsByIdd")]
		public JsonResult getDrugById([FromBody] Dictionary<string, object> JSONinput)
		{
			var DrugJson = JsonConvert.SerializeObject(JSONinput["Drugs"], Newtonsoft.Json.Formatting.Indented);
			var Drug = JsonConvert.DeserializeObject<GiftCards>(DrugJson);

			string StoredProcedureName = DrugsProcedures.getDrugById;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@drugId", Drug.id);

			return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
		}

		[HttpPost("api/getDrugsByCategory")]
		public JsonResult getDrugsByCategory([FromBody] Dictionary<string, object> JSONinput)
		{
			var CategoriesJson = JsonConvert.SerializeObject(JSONinput["Categories"], Newtonsoft.Json.Formatting.Indented);
			var Categories = JsonConvert.DeserializeObject<GiftCards>(CategoriesJson);

			string StoredProcedureName = DrugsProcedures.getDrugById;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@categoryId", Categories.id);

			return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
		}


	}
}