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
using System.Data;

namespace Shoryan.Controllers
{
    
    [ApiController]
    public class ComplaintsController : Controller
    {
		DBManager dbMan;

		public ComplaintsController()
		{
			dbMan = new DBManager();
		}

        [HttpGet("api/complaints")]
        public IActionResult getAllListing()
        {
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
			try {
				DataTable dt = dbMan.ExecuteReader(ComplaintsProcedure.getAllComplaints, Parameters);
				return Json(dt);
			} catch (Exception e)
			{
				return StatusCode(500, "Internal Server Error");
			}
        }

        [HttpPost("api/Complaints")]
        public IActionResult addComplaint([FromBody] Dictionary<string, object> JSONinput)
        {
            var complaintJson = JsonConvert.SerializeObject(JSONinput["Complaint"], Newtonsoft.Json.Formatting.Indented);
            var complaint = JsonConvert.DeserializeObject<Complaints>(complaintJson);



            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@subject", complaint.subject);
            Parameters.Add("@message", complaint.message);
            Parameters.Add("@normalUserId", complaint.normalUserID);
            Parameters.Add("@courierId", complaint.courierID);
            Parameters.Add("@fromCourierToUser", complaint.fromCourierToUser);

			try
			{
				int returnValue = dbMan.ExecuteNonQuery(ComplaintsProcedure.addComplaint, Parameters);
				if (returnValue == -1)
					return StatusCode(200, "Complaint added successfully");
				else
					return StatusCode(500, "Internal Server Error");
			}
			catch(Exception e)
			{
				return StatusCode(500, "Internal Server Error");
			}
        }

        [HttpGet("api/searchComplaints/{text}")]
        public IActionResult searchInUsers(string text)
        {
            string StoredProcedureName = ComplaintsProcedure.searchComplaints;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@search", text);
            return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
        }

    }
}