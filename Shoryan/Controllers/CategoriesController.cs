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
		public IActionResult getAllCategories()
		{
			string StoredProcedureName = CategoriesProcedures.getAllCategories;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			try
			{
				return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
			}
			catch (Exception e)
			{
				return StatusCode(500, "Failed to retrieve categories");
			}
		}

		[HttpPost("api/categories")]
		public IActionResult addCategory([FromBody] Dictionary<string, object> JSONinput)
		{
			var CategoriesJson = JsonConvert.SerializeObject(JSONinput["Categories"], Newtonsoft.Json.Formatting.Indented);
			var Category = JsonConvert.DeserializeObject<Categories>(CategoriesJson);

			string StoredProcedureName = CategoriesProcedures.addCategory;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@name", Category.name);

			try
			{
				return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
			}
			catch (Exception)
			{

				return StatusCode(500, "Category with same name already exists");
			}

		}

		[HttpDelete("api/categories/{categoryId}")]
		public IActionResult deleteCategory(int categoryId)
		{
			string StoredProcedureName = CategoriesProcedures.deleteCategory;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@id", categoryId);
			try
			{
				int returnCode = dbMan.ExecuteNonQuery(StoredProcedureName, Parameters);
				if (returnCode == -1)
				{
					return StatusCode(200, "Category deleted successfully");
				}
				else
				{
					return StatusCode(500, "Email doesn't exist");
				}
			}
			catch(Exception e)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}

	}
}