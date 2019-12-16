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
    public class CategoriesController : Controller
    {
		DBManager dbMan;
		public CategoriesController()
		{
			dbMan = new DBManager();
		}

		[HttpGet("api/categories")]
		public JsonResult getAllCategories()
		{
			string StoredProcedureName = CategoriesProcedures.getAllCategories;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
		}

	}
}