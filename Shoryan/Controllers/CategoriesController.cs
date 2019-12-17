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

		[HttpPost("api/categories")]
		public JsonResult addCategory([FromBody] Dictionary<string, object> JSONinput)
		{
			var CategoriesJson = JsonConvert.SerializeObject(JSONinput["Categories"], Newtonsoft.Json.Formatting.Indented);
			var Category = JsonConvert.DeserializeObject<Categories>(CategoriesJson);

			string StoredProcedureName = CategoriesProcedures.addCategory;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@name", Category.name);

			return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
		}

		[HttpDelete("api/categories/{categoryId}")]
		public JsonResult deleteCategory(int categoryId)
		{
			string StoredProcedureName = CategoriesProcedures.deleteCategory;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@id", categoryId);

			return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
		}

	}
}