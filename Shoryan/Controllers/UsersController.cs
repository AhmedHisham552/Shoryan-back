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

        [HttpDelete("api/deletePhoneNumber/{userId}/{phoneNumber}")]
        public IActionResult deletePhoneNumber(int userId, string phoneNumber)
        {
            string StoredProcedureName = UsersProcedures.deletePhoneNumber;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@userId", userId);
            Parameters.Add("@PhoneNumber", phoneNumber);
            try
            {
                int returnCode = dbMan.ExecuteNonQuery(StoredProcedureName, Parameters);
                if (returnCode == -1)
                {
                    return StatusCode(200, "Phone Number deleted successfully");
                }
                else
                {
                    return StatusCode(500, "Phone Number doesn't exist");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, "Phone Number doesn't exist");
            }

        }

        [HttpPost("api/userPhoneNumber/{userId}/{phoneNumber}")]
		public IActionResult addPhoneNumber(int userId,string phoneNumber)
		{
			string StoredProcedureName = UsersProcedures.addPhoneNumber;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@userId", userId);
			Parameters.Add("@PhoneNumber", phoneNumber);
			try
			{
				int returnCode = dbMan.ExecuteNonQuery(StoredProcedureName, Parameters);
				if(returnCode == -1)
				{
					return StatusCode(200, "Phone Number added successfully");
				}else
				{
					return StatusCode(500, "Phone Number already exists");
				}
			}
			catch(Exception e)
			{
				return StatusCode(500, "Phone Number already exists");
			}

		}

		private List<string>  aux_getPhoneNumbers(int id)
		{
			string StoredProcedureName = UsersProcedures.getPhoneNumber;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@userId", id);
			DataTable dt;
			try
			{
				dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);
			}
			catch (Exception e)
			{
				throw e;
			}

			List<string> numbers = new List<string>();
			if (dt != null)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					numbers.Add(Convert.ToString(dt.Rows[i][0]));
				}
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
			gender:"M",
            area:""
		},
	}*/

		[HttpPost("api/user")]
		public IActionResult addUser([FromBody] Dictionary<string,object> JSONinput)
		{
			string User_DetailsJson;
			User_Details User_Details;
			DataTable dt;
			try {
				User_DetailsJson = JsonConvert.SerializeObject(JSONinput["User_Details"], Newtonsoft.Json.Formatting.Indented);
				User_Details = JsonConvert.DeserializeObject<User_Details>(User_DetailsJson);
			}
			catch (Exception e)
			{
				return StatusCode(500, "Error parsing JSON");
			}

			if (User_Details.type == "Normal")
			{
				var NormalUsersJson = JsonConvert.SerializeObject(JSONinput["NormalUsers"], Newtonsoft.Json.Formatting.Indented);
				var NormalUsers = JsonConvert.DeserializeObject<NormalUsers>(NormalUsersJson);
				string StoredProcedureName = UsersProcedures.addUser;
				Dictionary<string, object> Parameters = new Dictionary<string, object>();
				Parameters.Add("@name", User_Details.name);
				Parameters.Add("@email", User_Details.email);
				Parameters.Add("@address", User_Details.address);
				Parameters.Add("@password", User_Details.password);
				Parameters.Add("@imgUrl", User_Details.imgUrl);
				Parameters.Add("@type", User_Details.type);
				Parameters.Add("@gender", NormalUsers.gender);
                Parameters.Add("@area", NormalUsers.area);
				
				dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);
				
				if(dt == null)
				{
					return StatusCode(500, "Email already already exists");
				}

				User_Details.id = Convert.ToInt32(dt.Rows[0][0]);
				for (int i = 0; i < User_Details.phoneNumbers.Count; i++)
				{
					addPhoneNumber(User_Details.id, User_Details.phoneNumbers[i]);
				}
			}
			else if(User_Details.type == "Courier")
			{
				var CouriersJson= JsonConvert.SerializeObject(JSONinput["Couriers"], Newtonsoft.Json.Formatting.Indented);
				var Couriers = JsonConvert.DeserializeObject<Couriers>(CouriersJson);             
				string StoredProcedureName = UsersProcedures.addUser;
				Dictionary<string, object> Parameters = new Dictionary<string, object>();
				Parameters.Add("@name", User_Details.name);
				Parameters.Add("@email", User_Details.email);
				Parameters.Add("@address", User_Details.address);
				Parameters.Add("@password", User_Details.password);
				Parameters.Add("@imgUrl", User_Details.imgUrl);
				Parameters.Add("@type", User_Details.type);
				Parameters.Add("@area", Couriers.area);
				dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);

				if (dt == null)
				{
					return StatusCode(500, "Email already already exists");
				}

				User_Details.id = Convert.ToInt32(dt.Rows[0][0]);

				for (int i = 0; i < User_Details.phoneNumbers.Count; i++)
				{
					addPhoneNumber(User_Details.id, User_Details.phoneNumbers[i]);
				}
			}
			else
			{
				string StoredProcedureName = UsersProcedures.addUser;
				Dictionary<string, object> Parameters = new Dictionary<string, object>();
				Parameters.Add("@name", User_Details.name);
				Parameters.Add("@email", User_Details.email);
				Parameters.Add("@address", User_Details.address);
				Parameters.Add("@password", User_Details.password);
				Parameters.Add("@imgUrl", User_Details.imgUrl);
				Parameters.Add("@type", User_Details.type);
				dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);

				if (dt == null)
				{
					return StatusCode(500, "Email already already exists");
				}

				User_Details.id = Convert.ToInt32(dt.Rows[0][0]);
				for (int i = 0; i < User_Details.phoneNumbers.Count; i++)
				{
					addPhoneNumber(User_Details.id, User_Details.phoneNumbers[i]);
				}
			}
			return Json(dt);
		}
		[HttpGet("api/user")]
		public IActionResult getAllUsers()
		{
			string StoredProcedureName = UsersProcedures.getAllUsers;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			try
			{
				return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}

		}
		[HttpPost("api/user/{userId}")]
		public IActionResult deleteUser(int userId)
		{
			string StoredProcedureName = UsersProcedures.deleteUser;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@userId", userId);
			try
			{
				int returnCode = dbMan.ExecuteNonQuery(StoredProcedureName, Parameters);
				if(returnCode == -1)
				{
					return StatusCode(200, "User deleted successfully");
				}else
				{
					return StatusCode(500, "User not found");
				}
			}
			catch (Exception e)
			{
				return StatusCode(500, "Internal server error");
			}

		}
        /* IF COURIER JSON : 
         *                      {
                                    "User_Details":{"id":28,
	                                    "gender":"M",
	                                    "balance":111,
	                                    "name":"hamad hamadeno",
	                                    "registrationDate":"2019-12-20T00:00:00",
	                                    "email":"hamada@gmail.com",
	                                    "address":"6 6 ",
	                                    "rating":0,
	                                    "password":"123456",
	                                    "imgUrl":"",
	                                    "type":"Courier",
	                                    },
                                    "Couriers":{
                                        "area":"value"
	                                    }
                                  }
          ELSE JSON NORMAL :
                         {
                                    "User_Details":{"id":28,
	                                    "gender":"M",
	                                    "balance":111,
	                                    "name":"hamad hamadeno",
	                                    "registrationDate":"2019-12-20T00:00:00",
	                                    "email":"hamada@gmail.com",
	                                    "address":"6 6 ",
	                                    "rating":0,
	                                    "password":"123456",
	                                    "imgUrl":"",
	                                    "type":"Normal"
                                        "NormalUsers":{
                                            "area":" "
                                        }
	                      }
                
                
         *                   
         */
        [HttpPut("api/user")]
		public IActionResult editUserDetails([FromBody] Dictionary<string, object> JSONinput)
		{
			var User_DetailsJson = JsonConvert.SerializeObject(JSONinput["User_Details"], Newtonsoft.Json.Formatting.Indented);
			var User_Details = JsonConvert.DeserializeObject<User_Details>(User_DetailsJson);
			
			if (User_Details.type == "Courier")
			{
				Couriers Couriers = new Couriers();
				try
				{
					var CouriersJson = JsonConvert.SerializeObject(JSONinput["Couriers"], Newtonsoft.Json.Formatting.Indented);
					Couriers = JsonConvert.DeserializeObject<Couriers>(CouriersJson);
				}
				catch(Exception)
				{
					return StatusCode(500, "Error parsing JSON");
				}

				string StoredProcedureName = UsersProcedures.editUserDetails;
				Dictionary<string, object> Parameters = new Dictionary<string, object>();
				Parameters.Add("@userId", User_Details.id);
				Parameters.Add("@name", User_Details.name);
				Parameters.Add("@email", User_Details.email);
				Parameters.Add("@password", User_Details.password);
				Parameters.Add("@address", User_Details.address);
				Parameters.Add("@imgUrl", User_Details.imgUrl);
				Parameters.Add("@area", Couriers.area);
				try
				{
					int returnCode = dbMan.ExecuteNonQuery(StoredProcedureName, Parameters);
					if (returnCode == -1)
						return StatusCode(200, "User edited successfully");
					else
						return StatusCode(500, "Internal Server Error");
				}
				catch (Exception)
				{
					return StatusCode(500, "Internal Server Error");
				}
			}
           else if(User_Details.type == "Normal")
            {
                NormalUsers NormalUser = new NormalUsers();
                try
                {
                    var NormalJson = JsonConvert.SerializeObject(JSONinput["NormalUsers"], Newtonsoft.Json.Formatting.Indented);
                    NormalUser = JsonConvert.DeserializeObject<NormalUsers>(NormalJson);
                }
                catch (Exception)
                {
                    return StatusCode(500, "Error parsing JSON");
                }
                string StoredProcedureName = UsersProcedures.editUserDetails;
                Dictionary<string, object> Parameters = new Dictionary<string, object>();
                Parameters.Add("@userId", User_Details.id);
                Parameters.Add("@name", User_Details.name);
                Parameters.Add("@email", User_Details.email);
                Parameters.Add("@password", User_Details.password);
                Parameters.Add("@address", User_Details.address);
                Parameters.Add("@imgUrl", User_Details.imgUrl);
                Parameters.Add("@area", NormalUser.area);
                try
                {
                    int returnCode = dbMan.ExecuteNonQuery(StoredProcedureName, Parameters);
                    if (returnCode == -1)
                        return StatusCode(200, "User edited successfully");
                    else
                        return StatusCode(500, "Internal Server Error");
                }
                catch (Exception)
                {
                    return StatusCode(500, "Internal Server Error");
                }
            }
            else
            {
                string StoredProcedureName = UsersProcedures.editUserDetails;
                Dictionary<string, object> Parameters = new Dictionary<string, object>();
                Parameters.Add("@userId", User_Details.id);
                Parameters.Add("@name", User_Details.name);
                Parameters.Add("@email", User_Details.email);
                Parameters.Add("@password", User_Details.password);
                Parameters.Add("@address", User_Details.address);
                Parameters.Add("@imgUrl", User_Details.imgUrl);
                Parameters.Add("@area", DBNull.Value);

                try
                {
                    int returnCode = dbMan.ExecuteNonQuery(StoredProcedureName, Parameters);
                    if (returnCode == -1)
                        return StatusCode(200, "User edited successfully");
                    else
                        return StatusCode(500, "Internal Server Error");
                }
                catch (Exception)
                {
                    return StatusCode(500, "Internal Server Error");
                }
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
			Parameters.Add("@password", User_Details.password);

			DataTable dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);
			if (dt == null)
			{
				return StatusCode(404,"Email or Password is incorrect");
			}
			else
			{
				return Json(dt);
			}
			
		}
		[HttpGet("api/user/{userId}")]
		public IActionResult getUserDetails(int userId)
		{
			string StoredProcedureName = UsersProcedures.getUserDetails;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@userId", userId);
			DataTable dt;
			dt=dbMan.ExecuteReader(StoredProcedureName, Parameters);

			if(dt == null)
			{
				return StatusCode(500, "User not found");
			}

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
        [HttpGet("api/ActiveListings/{userId}")]
        public IActionResult getUserActiveListings(int userId)
        {
            string StoredProcedureName = UsersProcedures.getActiveListings;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@userId", userId);
			try
			{
				return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
			}
			catch (Exception)
			{
				return StatusCode(500, "User id not found");
			}
        }
        [HttpGet("api/searchUsers/{text}")]
        public IActionResult searchInUsers(string text)
        {
            string StoredProcedureName = UsersProcedures.searchInUsers;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@search", text);
            return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
        }
    }

}