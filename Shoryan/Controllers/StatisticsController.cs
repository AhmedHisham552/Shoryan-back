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
using Shoryan.Controllers;

namespace Shoryan.Controllers
{
    [ApiController]
    public class StatisticsController : Controller
    {

		DBManager dbMan;
		public StatisticsController()
		{
			dbMan = new DBManager();
		}

		[HttpGet("api/getTotalIncomeMonth")]
		public JsonResult getTotalIncomeThisMonth()
		{
			string StoredProcedureName = StatisticsProcedures.getTotalIncomeThisMonth;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			var dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);

			if(dt!=null)
			{
				return Json(dt.Rows[0]["sum"]);
			}else
			{
				return Json(0);
			}


		}

		[HttpGet("api/getCountUsersMonth")]
		public JsonResult getCountUsersMonth()
		{
			string StoredProcedureName = StatisticsProcedures.getCountUsersMonth;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			var dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);

			if (dt != null)
				return Json(dt.Rows[0]["count"]);
			else
				return Json(0);
		}

		[HttpGet("api/getCountOrdersMonth")]
		public JsonResult getCountOrdersMonth()
		{
			string StoredProcedureName = StatisticsProcedures.getCountOrdersMonth;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			var dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);

			if (dt != null)
				return Json(dt.Rows[0]["count"]);
			else
				return Json(0);
		}

		[HttpGet("api/getComplaintsMonth")]
		public JsonResult getComplaintsMonth()
		{
			string StoredProcedureName = StatisticsProcedures.getComplaintsMonth;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			var dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);

			if (dt != null)
				return Json(dt.Rows[0]["count"]);
			else
				return Json(0);
		}


        [HttpGet("api/UsersOfEachType")]
        public JsonResult getUsersOfEachType()
        {
            string StoredProcedureName = StatisticsProcedures.getTypesOfUsersCount;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();

            var dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);

            if (dt != null)
            {
                return Json(dt);
            }
            else
            {
                return Json(0);
            }


        }


        [HttpGet("api/DrugsOfEachCategory")]
        public JsonResult getDrugsCountInCategories()
        {
            string StoredProcedureName = StatisticsProcedures.getDrugCountinCategory;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();

            var dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);

            if (dt != null)
            {
                return Json(dt);
            }
            else
            {
                return Json(0);
            }


        }
    }
}