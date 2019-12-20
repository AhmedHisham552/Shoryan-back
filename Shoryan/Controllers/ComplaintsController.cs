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

            DataTable dt = dbMan.ExecuteReader(ComplaintsProcedure.getAllComplaints, Parameters);

            return Json(dt);
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


            int returnValue = dbMan.ExecuteNonQuery(ComplaintsProcedure.addComplaint, Parameters);

            if (returnValue == 0) return StatusCode(500);
            return Json("Data Added successfully");

        }

    }
}