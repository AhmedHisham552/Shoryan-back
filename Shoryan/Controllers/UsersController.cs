using System;
using System.Collections.Generic;
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
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        DBManager dbMan;

        public UsersController()
        {
            dbMan = new DBManager();
        }
       
        public JsonResult addUser([FromBody] Dictionary<string,object> JSONinput)
        {
            var User_DetailsJson = JsonConvert.SerializeObject(JSONinput["User_Details"], Newtonsoft.Json.Formatting.Indented);
            var NormalUsersJson= JsonConvert.SerializeObject(JSONinput["NormalUsers"], Newtonsoft.Json.Formatting.Indented);
            var CouriersJson= JsonConvert.SerializeObject(JSONinput["Couriers"], Newtonsoft.Json.Formatting.Indented);

            var User_Details =JsonConvert.DeserializeObject<User_Details>(User_DetailsJson);
            var NormalUsers = JsonConvert.DeserializeObject<NormalUsers>(NormalUsersJson);
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
            Parameters.Add("@gender", NormalUsers.gender);
            Parameters.Add("@area", Couriers.area);

            return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
        }

		[HttpGet("api/users/")]
        public JsonResult getAllUsers()
        {
            string StoredProcedureName = UsersProcedures.getAllUsers;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();

            return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
        }

		[HttpDelete("api/users/{userId}")]
		public JsonResult deleteUser(int userId)
        {
            string StoredProcedureName = UsersProcedures.deleteUser;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();

            Parameters.Add("@userId", userId);
            return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
        }

		[HttpPut("api/users/{userId}")]
		public JsonResult editUserDetails(int userId, [FromBody] Dictionary<string, object> JSONinput)
        {
            var User_DetailsJson = JsonConvert.SerializeObject(JSONinput["User_Details"], Newtonsoft.Json.Formatting.Indented);
            var NormalUsersJson = JsonConvert.SerializeObject(JSONinput["NormalUsers"], Newtonsoft.Json.Formatting.Indented);
            var PharmaciesJson = JsonConvert.SerializeObject(JSONinput["Pharmacies"], Newtonsoft.Json.Formatting.Indented);
            var CouriersJson = JsonConvert.SerializeObject(JSONinput["Couriers"], Newtonsoft.Json.Formatting.Indented);

            var User_Details = JsonConvert.DeserializeObject<User_Details>(User_DetailsJson);
            var NormalUsers = JsonConvert.DeserializeObject<NormalUsers>(NormalUsersJson);
            var Pharmacies = JsonConvert.DeserializeObject<Pharmacies>(PharmaciesJson);
            var Couriers = JsonConvert.DeserializeObject<Couriers>(CouriersJson);

            string StoredProcedureName = UsersProcedures.editUserDetails;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@userId", userId); 
            Parameters.Add("@name", User_Details.name);
            Parameters.Add("@email", User_Details.email);
            Parameters.Add("@password", User_Details.password);
            Parameters.Add("@address", User_Details.address);
            Parameters.Add("@imgUrl", User_Details.imgUrl);
            Parameters.Add("@area", Couriers.area);

            return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
        }


        public JsonResult getUserPastOrders([FromBody] Dictionary<string, object> JSONinput)
        {
            var User_DetailsJson = JsonConvert.SerializeObject(JSONinput["User_Details"], Newtonsoft.Json.Formatting.Indented);
            var User_Details = JsonConvert.DeserializeObject<User_Details>(User_DetailsJson);
            string StoredProcedureName = UsersProcedures.getPastOrders;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@userId", User_Details.id);

            return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
        }

		[HttpGet("api/authenticateUser/")]
        public JsonResult authenticateUser([FromBody] Dictionary<string, object> JSONinput)
        {
            var User_DetailsJson = JsonConvert.SerializeObject(JSONinput["User_Details"], Newtonsoft.Json.Formatting.Indented);
            var User_Details = JsonConvert.DeserializeObject<User_Details>(User_DetailsJson);

            string StoredProcedureName = UsersProcedures.authenticateUser;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@email", User_Details.email);
            Parameters.Add("@password", User_Details.password);

            return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
        }

		[HttpGet("api/users/{userId}")]
		public JsonResult getUserDetails(int userId)
        {
            string StoredProcedureName = UsersProcedures.getUserDetails;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();

            Parameters.Add("@userId", userId);

            return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
        }
    }

}