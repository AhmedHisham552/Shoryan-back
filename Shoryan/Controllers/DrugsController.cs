using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DBapplication;
using Shoryan.Models;
using System.Data;
using Shoryan.Routes;
using Newtonsoft.Json;

namespace Shoryan.Controllers
{
	[ApiController]
	public class DrugsController : Controller
    {
		DBManager dbMan;
		public DrugsController()
		{
			dbMan = new DBManager();
		}

		private List<int> aux_getAllDrugsIds()
		{
			string StoredProcedureName = DrugsProcedures.getDrug;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			
			DataTable dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);
			List<int> DrugsIds = new List<int>();
			for (int i = 0; i < dt.Rows.Count; i++)
				DrugsIds.Add(Convert.ToInt32(dt.Rows[i]["drugsIds"]));

			return DrugsIds;
		}

		private List<string> aux_getImgsFromDrug(int drugId)
		{
			string StoredProcedureName = DrugsProcedures.getDrugImgs;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@drug_id", drugId);

			DataTable dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);

			List<string> DrugImgsUrls = new List<string>();
			for(int i=0;i<dt.Rows.Count;i++)
			{
				DrugImgsUrls.Add(Convert.ToString(dt.Rows[i]["imgsUrls"]));
			}

			return DrugImgsUrls;
		}

		private List<string> aux_getEffectiveSubstancesFromDrug(int drugId)
		{
			string StoredProcedureName = DrugsProcedures.getEffectiveSubstances;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			DataTable dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);
			Parameters.Add("@drug_id", drugId);

			List<string> DrugEffectiveSubstances = new List<string>();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				DrugEffectiveSubstances.Add(Convert.ToString(dt.Rows[i]["effectiveSubstanceName"]));
			}

			return DrugEffectiveSubstances;
		}

		private List<int> aux_getCategoriesFromDrug(int drugId)
		{
			string StoredProcedureName = DrugsProcedures.getCategoryOfDrug;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@drug_id", drugId);

			DataTable dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);


			List<int> DrugCategories = new List<int>();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				DrugCategories.Add(Convert.ToInt32(dt.Rows[i]["categoriesIds"]));
			}

			return DrugCategories;
		}


		//[HttpGet("api/drugs/")]
		//public JsonResult getAllDrugs()
		//{
		//	string StoredProcedureName = DrugsProcedures.getDrug;
		//	Dictionary<string, object> Parameters = new Dictionary<string, object>();

		//	return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
		//}

		[HttpGet("api/drugs/")]
		public JsonResult getAllDrugs()
		{
			string StoredProcedureName = DrugsProcedures.getDrug;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
		}



		[HttpGet("api/drugs/{drugId}")]
		public JsonResult getDrugById(int drugId)
		{
			string StoredProcedureName = DrugsProcedures.getDrug;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@id", drugId);

			var DrugJson = JsonConvert.SerializeObject(Json(dbMan.ExecuteReader(StoredProcedureName, Parameters)), Newtonsoft.Json.Formatting.Indented);
			var Drug = JsonConvert.DeserializeObject<Drugs>(DrugJson);

			List<string> imgsUrls = aux_getImgsFromDrug(drugId);
			List<string> effectiveSubstances = aux_getEffectiveSubstancesFromDrug(drugId);
			List<int> categories = aux_getCategoriesFromDrug(drugId);

			Drug.imgsUrls = imgsUrls;
			Drug.effectiveSubstances = effectiveSubstances;
			Drug.categoriesIds = categories;

			return Json(Drug);
		}

		[HttpGet("api/drugsByCategory/{categoryId}")]
		public JsonResult getDrugsByCategory(int categoryId)
		{

			string StoredProcedureName = DrugsProcedures.getDrugsByCategory;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@categoryId", categoryId);

			return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
		}

		[HttpGet("api/drugImg/{drugId}")]
		public JsonResult getAllDrugImgs(int drugId)
		{
			string StoredProcedureName = DrugsProcedures.getDrugImgs;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@drug_id", drugId);

			return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
		}

		[HttpGet("api/drugImg/{drugId}/{imgNo}")]
		public JsonResult getDrugImg(int drugId, int imgNo)
		{
			string StoredProcedureName = DrugsProcedures.getDrugImgs;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@drug_id", drugId);
			Parameters.Add("@img_no", imgNo);

			return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
		}

		[HttpGet("api/effectiveSubstances/{drugId}")]
		public JsonResult getEffectiveSubstances(int drugId)
		{
			string StoredProcedureName = DrugsProcedures.getEffectiveSubstances;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@drug_id", drugId);

			return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
		}

		/*
		 JSON:
		 {
			drugs:
			{
				id:
				name:
				officialPrice:
				urls: ["url1", "url2", "url3", ..]
				categories: [cat1_id, cat2_id, cat3_id, ..]
				effectivesSubstances: ["name1", "name2", "name3"]
			}
		 }
		*/

		[HttpPost("api/drug/")]
		public JsonResult addDrug([FromBody] Dictionary<string, object> JSONinput)
		{
			var DrugsJson = JsonConvert.SerializeObject(JSONinput["Drugs"], Newtonsoft.Json.Formatting.Indented);
			var Drug = JsonConvert.DeserializeObject<Drugs>(DrugsJson);

			string StoredProcedureName = AdministratorsProcedures.addAdministrator;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@drugs", Drug.name);
			Parameters.Add("@email", userDetails.email);
			Parameters.Add("@address", userDetails.address);
			Parameters.Add("@password", userDetails.password);
			Parameters.Add("@imgUrl", userDetails.imgUrl);

			return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
		}

	}
}