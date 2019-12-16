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

		[HttpGet("api/drugs/{drugId}")]
		public JsonResult getDrugById(int drugId)
		{
			string StoredProcedureName = DrugsProcedures.getDrugById;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@drugId", drugId);

			return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
		}

		[HttpGet("api/drugsByCategory/{categoryId}")]
		public JsonResult getDrugsByCategory(int categoryId)
		{

			string StoredProcedureName = DrugsProcedures.getDrugById;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@categoryId", categoryId);

			return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
		}


	}
}