using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DBapplication;
using Shoryan.Models;
using Shoryan.Routes;
using Shoryan.Controllers;
using Newtonsoft.Json;

namespace Shoryan.Controllers
{
    [ApiController]
    public class AdministratorsController : Controller
    {
		DBManager dbMan;
		public AdministratorsController()
		{
			dbMan = new DBManager();
		}

		[HttpGet("api/administrators/")]
		public JsonResult getAdministrators()
		{
			string StoredProcedureName = AdministratorsProcedures.getAdministrator;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
			
		}

		[HttpGet("api/administrators/{adminId}")]
		public JsonResult getAdministrator(int adminId)
		{
			string StoredProcedureName = AdministratorsProcedures.getAdministrator;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@id", adminId);

			return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
		}

		[HttpPost("api/administrators/")]
		public JsonResult addAdministrator([FromBody] Dictionary<string, object> JSONinput)
		{
			var administratorsJson = JsonConvert.SerializeObject(JSONinput["Administrators"], Newtonsoft.Json.Formatting.Indented);
			var userDetails = JsonConvert.DeserializeObject<User_Details>(administratorsJson);

			string StoredProcedureName = AdministratorsProcedures.addAdministrator;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@name", userDetails.name);
			Parameters.Add("@email", userDetails.email);
			Parameters.Add("@address", userDetails.address);
			Parameters.Add("@password", userDetails.password);
			Parameters.Add("@imgUrl", userDetails.imgUrl);

			return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
		}

		[HttpPut("api/administrators/{adminId}")]
		public JsonResult updateAdministrator(int adminId, [FromBody] Dictionary<string, object> JSONinput)
		{
			var userCont = new UsersController();
			return (userCont.editUserDetails(adminId, JSONinput));
		}

		[HttpDelete("api/administrators/{adminId}")]
		public JsonResult removeAdministrator(int adminId)
		{
			string StoredProcedureName = AdministratorsProcedures.removeAdministrator;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@id", adminId);

			return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
		}



	}
}