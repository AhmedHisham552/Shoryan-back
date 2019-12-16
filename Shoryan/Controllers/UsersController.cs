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

 /* {
	User_Details:{
		name:"tarek",
		registrationDate:"2019-02-12",
		email:"ahmed2344_2006319@yahoo.com",
		address:"haram",
		rating:3.5,
		password:"shoma",
		type:"Normal"
	    },
	    NormalUsers:{
		    gender:"M"
	    },
	    PhoneNumbers:{
		    numbers:["011108292952","011120892955","011102723829"]
	    }
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
                var PhoneNumbersJson = JsonConvert.SerializeObject(JSONinput["PhoneNumbers"], Newtonsoft.Json.Formatting.Indented);
                var NormalUsers = JsonConvert.DeserializeObject<NormalUsers>(NormalUsersJson);
                var PhoneNumbers = JsonConvert.DeserializeObject<PhoneNumbers>(PhoneNumbersJson);
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
                PhoneNumbers.userId = Convert.ToInt32(dt.Rows[0][0]);
                for (int i = 0; i < PhoneNumbers.numbers.Count; i++)
                {
                    AuxaddPhoneNumber(PhoneNumbers.userId, PhoneNumbers.numbers[i]);
                }
            }
            else if(User_Details.type == "Courier")
            {
                var CouriersJson= JsonConvert.SerializeObject(JSONinput["Couriers"], Newtonsoft.Json.Formatting.Indented);
                var Couriers = JsonConvert.DeserializeObject<Couriers>(CouriersJson);
                var PhoneNumbersJson = JsonConvert.SerializeObject(JSONinput["PhoneNumbers"], Newtonsoft.Json.Formatting.Indented);
                var PhoneNumbers = JsonConvert.DeserializeObject<PhoneNumbers>(PhoneNumbersJson);
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
                PhoneNumbers.userId = Convert.ToInt32(dt.Rows[0][0]);
                for (int i = 0; i < PhoneNumbers.numbers.Count; i++)
                {
                    AuxaddPhoneNumber(PhoneNumbers.userId, PhoneNumbers.numbers[i]);
                }
            }
            else
            {
                var PhoneNumbersJson = JsonConvert.SerializeObject(JSONinput["PhoneNumbers"], Newtonsoft.Json.Formatting.Indented);
                var PhoneNumbers = JsonConvert.DeserializeObject<PhoneNumbers>(PhoneNumbersJson);
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
                PhoneNumbers.userId = Convert.ToInt32(dt.Rows[0][0]);
                for (int i = 0; i < PhoneNumbers.numbers.Count; i++)
                {
                    AuxaddPhoneNumber(PhoneNumbers.userId, PhoneNumbers.numbers[i]);
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
                var AdminsJson = JsonConvert.SerializeObject(JSONinput["Adminstartors"], Newtonsoft.Json.Formatting.Indented);
                var Admins = JsonConvert.DeserializeObject<Adminstrators>(AdminsJson);
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
                var PharmaciesJson = JsonConvert.SerializeObject(JSONinput["Pharmacies"], Newtonsoft.Json.Formatting.Indented);
                var Pharmacies = JsonConvert.DeserializeObject<Pharmacies>(PharmaciesJson);
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
        [HttpGet("api/login")]
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
        [HttpGet("api/user/{userId}")]
        public JsonResult getUserDetails(int userId)
        {
            string StoredProcedureName = UsersProcedures.getUserDetails;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@userId", userId);

            return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
        }
    }

}