using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DBapplication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shoryan.Models;
using Shoryan.StoredProcedures;

namespace Shoryan.Controllers
{

	[ApiController]
	public class UsersController : Controller
	{
		DBManager dbMan;

		public UsersController()
		{
			dbMan = new DBManager();
		}

		private int AuxaddPhoneNumber(int id,string phoneNumber)
		{
			string StoredProcedureName = UsersProcedures.addPhoneNumber;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@userId", id);
			Parameters.Add("@PhoneNumber", phoneNumber);
			return dbMan.ExecuteNonQuery(StoredProcedureName, Parameters);
		}

		private List<string>  aux_getPhoneNumbers(int id)
		{
			string StoredProcedureName = UsersProcedures.getPhoneNumber;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@userId", id);
			DataTable dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);
			List<string> numbers = new List<string>();
			for(int i = 0; i < dt.Rows.Count; i++)
			{
				numbers.Add(Convert.ToString(dt.Rows[i][0]));
			}
			return numbers;
		}

 /* {
	User_Details:{
		name:"tarek",
		registrationDate:"2019-02-12",
		email:"ahmed2344_2006319@yahoo.com",
		address:"haram",
		phoneNumbers:["01111002030","123489374893"]
		rating:3.5,
		password:"shoma",
		type:"Normal"
		},
		NormalUsers:{
			gender:"M"
		},
	}*/

		[HttpPost("api/user")]
		public JsonResult addUser([FromBody] Dictionary<string,object> JSONinput)
		{
			DataTable dt;
			var User_DetailsJson = JsonConvert.SerializeObject(JSONinput["User_Details"], Newtonsoft.Json.Formatting.Indented);
			var User_Details = JsonConvert.DeserializeObject<User_Details>(User_DetailsJson);
			if (User_Details.type == "Normal")
			{
				var NormalUsersJson = JsonConvert.SerializeObject(JSONinput["NormalUsers"], Newtonsoft.Json.Formatting.Indented);
				var NormalUsers = JsonConvert.DeserializeObject<NormalUsers>(NormalUsersJson);
				string StoredProcedureName = UsersProcedures.addUser;
				Dictionary<string, object> Parameters = new Dictionary<string, object>();
				Parameters.Add("@name", User_Details.name);
				Parameters.Add("@registrationDate", User_Details.registrationDate);
				Parameters.Add("@email", User_Details.email);
				Parameters.Add("@address", User_Details.address);
				Parameters.Add("@rating", User_Details.rating);
				Parameters.Add("@password", User_Details.password);
				Parameters.Add("@imgUrl", User_Details.imgUrl);
				Parameters.Add("@type", User_Details.type);
				Parameters.Add("@gender", NormalUsers.gender);
				dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);
				User_Details.id = Convert.ToInt32(dt.Rows[0][0]);
				for (int i = 0; i < User_Details.phoneNumbers.Count; i++)
				{
					AuxaddPhoneNumber(User_Details.id, User_Details.phoneNumbers[i]);
				}
			}
			else if(User_Details.type == "Courier")
			{
				var CouriersJson= JsonConvert.SerializeObject(JSONinput["Couriers"], Newtonsoft.Json.Formatting.Indented);
				var Couriers = JsonConvert.DeserializeObject<Couriers>(CouriersJson);             
				string StoredProcedureName = UsersProcedures.addUser;
				Dictionary<string, object> Parameters = new Dictionary<string, object>();
				Parameters.Add("@name", User_Details.name);
				Parameters.Add("@registrationDate", User_Details.registrationDate);
				Parameters.Add("@email", User_Details.email);
				Parameters.Add("@address", User_Details.address);
				Parameters.Add("@rating", User_Details.rating);
				Parameters.Add("@password", User_Details.password);
				Parameters.Add("@imgUrl", User_Details.imgUrl);
				Parameters.Add("@type", User_Details.type);
				Parameters.Add("@area", Couriers.area);
				dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);
				User_Details.id = Convert.ToInt32(dt.Rows[0][0]);
				for (int i = 0; i < User_Details.phoneNumbers.Count; i++)
				{
					AuxaddPhoneNumber(User_Details.id, User_Details.phoneNumbers[i]);
				}
			}
			else
			{
				string StoredProcedureName = UsersProcedures.addUser;
				Dictionary<string, object> Parameters = new Dictionary<string, object>();
				Parameters.Add("@name", User_Details.name);
				Parameters.Add("@registrationDate", User_Details.registrationDate);
				Parameters.Add("@email", User_Details.email);
				Parameters.Add("@address", User_Details.address);
				Parameters.Add("@rating", User_Details.rating);
				Parameters.Add("@password", User_Details.password);
				Parameters.Add("@imgUrl", User_Details.imgUrl);
				Parameters.Add("@type", User_Details.type);
				dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);
				User_Details.id = Convert.ToInt32(dt.Rows[0][0]);
				for (int i = 0; i < User_Details.phoneNumbers.Count; i++)
				{
					AuxaddPhoneNumber(User_Details.id, User_Details.phoneNumbers[i]);
				}
			}
			return Json(dt);
		}
		[HttpGet("api/user")]
		public JsonResult getAllUsers()
		{
			string StoredProcedureName = UsersProcedures.getAllUsers;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
		}
		[HttpPost("api/user/{userId}")]
		public JsonResult deleteUser(int userId)
		{
			string StoredProcedureName = UsersProcedures.deleteUser;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@userId", userId);
			return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
		}
		[HttpPut("api/user")]
		public JsonResult editUserDetails([FromBody] Dictionary<string, object> JSONinput)
		{
			var User_DetailsJson = JsonConvert.SerializeObject(JSONinput["User_Details"], Newtonsoft.Json.Formatting.Indented);
			var User_Details = JsonConvert.DeserializeObject<User_Details>(User_DetailsJson);
			if (User_Details.type == "Normal")
			{
				var NormalUsersJson = JsonConvert.SerializeObject(JSONinput["NormalUsers"], Newtonsoft.Json.Formatting.Indented);
				var NormalUsers = JsonConvert.DeserializeObject<NormalUsers>(NormalUsersJson);
				string StoredProcedureName = UsersProcedures.editUserDetails;
				Dictionary<string, object> Parameters = new Dictionary<string, object>();
				Parameters.Add("@userId", User_Details.id);
				Parameters.Add("@name", User_Details.name);
				Parameters.Add("@email", User_Details.email);
				Parameters.Add("@password", User_Details.password);
				Parameters.Add("@address", User_Details.address);
				Parameters.Add("@imgUrl", User_Details.imgUrl);
				return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
			}
			else if (User_Details.type == "Courier")
			{
				var CouriersJson = JsonConvert.SerializeObject(JSONinput["Couriers"], Newtonsoft.Json.Formatting.Indented);
				var Couriers = JsonConvert.DeserializeObject<Couriers>(CouriersJson);
				string StoredProcedureName = UsersProcedures.editUserDetails;
				Dictionary<string, object> Parameters = new Dictionary<string, object>();
				Parameters.Add("@userId", User_Details.id);
				Parameters.Add("@name", User_Details.name);
				Parameters.Add("@email", User_Details.email);
				Parameters.Add("@password", User_Details.password);
				Parameters.Add("@address", User_Details.address);
				Parameters.Add("@imgUrl", User_Details.imgUrl);
				Parameters.Add("@area", Couriers.area);
				return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
			}
			else if (User_Details.type == "Admin")
			{
				//var AdminsJson = JsonConvert.SerializeObject(JSONinput["Adminstartors"], Newtonsoft.Json.Formatting.Indented);
				//var Admins = JsonConvert.DeserializeObject<Adminstrators>(AdminsJson);
				string StoredProcedureName = UsersProcedures.editUserDetails;
				Dictionary<string, object> Parameters = new Dictionary<string, object>();
				Parameters.Add("@userId", User_Details.id);
				Parameters.Add("@name", User_Details.name);
				Parameters.Add("@email", User_Details.email);
				Parameters.Add("@password", User_Details.password);
				Parameters.Add("@address", User_Details.address);
				Parameters.Add("@imgUrl", User_Details.imgUrl);
				return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
			}
			else
			{
			   // var PharmaciesJson = JsonConvert.SerializeObject(JSONinput["Pharmacies"], Newtonsoft.Json.Formatting.Indented);
			   // var Pharmacies = JsonConvert.DeserializeObject<Pharmacies>(PharmaciesJson);
				string StoredProcedureName = UsersProcedures.editUserDetails;
				Dictionary<string, object> Parameters = new Dictionary<string, object>();
				Parameters.Add("@userId", User_Details.id);
				Parameters.Add("@name", User_Details.name);
				Parameters.Add("@email", User_Details.email);
				Parameters.Add("@password", User_Details.password);
				Parameters.Add("@address", User_Details.address);
				Parameters.Add("@imgUrl", User_Details.imgUrl);
				return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
			}
		}
		[HttpPost("api/login")]
		public IActionResult authenticateUser([FromBody] Dictionary<string, object> JSONinput)
		{
			var User_DetailsJson = JsonConvert.SerializeObject(JSONinput["User_Details"], Newtonsoft.Json.Formatting.Indented);
			var User_Details = JsonConvert.DeserializeObject<User_Details>(User_DetailsJson);

			string StoredProcedureName = UsersProcedures.authenticateUser;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@email", User_Details.email);

			DataTable dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);
			if (dt == null)
			{
				return StatusCode(404,"Email doesn't exist");
			}
			else
			{
				if (User_Details.password == Convert.ToString(dt.Rows[0]["password"]))
				{
					return Json(dt);
				}
				else
				{
					return StatusCode(500, "Password is incorrect");
                }
			}
			
		}
		[HttpGet("api/user/{userId}")]
		public JsonResult getUserDetails(int userId)
		{
			string StoredProcedureName = UsersProcedures.getUserDetails;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@userId", userId);

			DataTable dt=dbMan.ExecuteReader(StoredProcedureName, Parameters);
			string type = Convert.ToString(dt.Rows[0]["type"]);
			if (type == "Normal")
			{
				NormalUsers user = new NormalUsers();
				user.id = Convert.ToInt16(dt.Rows[0]["id"]);
				user.name = Convert.ToString(dt.Rows[0]["name"]);
				user.registrationDate = Convert.ToDateTime(dt.Rows[0]["registeration_date"]);
				user.email = Convert.ToString(dt.Rows[0]["email"]);
				user.address = Convert.ToString(dt.Rows[0]["address"]);
				user.rating = Convert.ToInt16(dt.Rows[0]["rating"]);
				user.password = Convert.ToString(dt.Rows[0]["password"]);
				user.imgUrl = Convert.ToString(dt.Rows[0]["img_url"]);
				user.type = type;
				user.gender = Convert.ToChar(dt.Rows[0]["gender"]);
				user.balance = Convert.ToInt32(dt.Rows[0]["balance"]);
				user.phoneNumbers = aux_getPhoneNumbers(user.id);
				return Json(user);
			}
			else if (type == "Courier")
			{
				Couriers user = new Couriers();
				user.id = Convert.ToInt16(dt.Rows[0]["id"]);
				user.name = Convert.ToString(dt.Rows[0]["name"]);
				user.registrationDate = Convert.ToDateTime(dt.Rows[0]["registeration_date"]);
				user.email = Convert.ToString(dt.Rows[0]["email"]);
				user.address = Convert.ToString(dt.Rows[0]["address"]);
				user.rating = Convert.ToInt16(dt.Rows[0]["rating"]);
				user.password = Convert.ToString(dt.Rows[0]["password"]);
				user.imgUrl = Convert.ToString(dt.Rows[0]["img_url"]);
				user.type = type;
				user.area = Convert.ToString(dt.Rows[0]["area"]);
				user.phoneNumbers = aux_getPhoneNumbers(user.id);
				return Json(user);
			}
			else
			{
				User_Details user = new User_Details();
				user.id = Convert.ToInt16(dt.Rows[0]["id"]);
				user.name = Convert.ToString(dt.Rows[0]["name"]);
				user.registrationDate = Convert.ToDateTime(dt.Rows[0]["registeration_date"]);
				user.email = Convert.ToString(dt.Rows[0]["email"]);
				user.address = Convert.ToString(dt.Rows[0]["address"]);
				user.rating = Convert.ToInt16(dt.Rows[0]["rating"]);
				user.password = Convert.ToString(dt.Rows[0]["password"]);
				user.imgUrl = Convert.ToString(dt.Rows[0]["img_url"]);
				user.type = type;
				user.phoneNumbers = aux_getPhoneNumbers(user.id);
				return Json(user);
			}

		}
	}

}