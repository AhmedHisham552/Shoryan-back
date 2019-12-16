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
			string StoredProcedureName = DrugsProcedures.getAllDrugsIds;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			
			DataTable dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);
			List<int> DrugsIds = new List<int>();
			if(dt!=null)
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
			if(dt != null)
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
			Parameters.Add("@drug_id", drugId);
			DataTable dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);


			List<string> DrugEffectiveSubstances = new List<string>();
			if(dt!= null)
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
			if(dt!=null)
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
			List<int> ids = aux_getAllDrugsIds();

			List<JsonResult> totalJson = new List<JsonResult>();
			foreach(var id in ids)
			{
				totalJson.Add(getDrugById(id));
			}

			return Json(totalJson);
		}



		[HttpGet("api/drugs/{drugId}")]
		public JsonResult getDrugById(int drugId)
		{
			string StoredProcedureName = DrugsProcedures.getDrug;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@id", drugId);

			DataTable dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);
			var Drug = new Drugs();
			if (dt != null)
			{
				Drug.id = drugId;
				Drug.name = Convert.ToString(dt.Rows[0]["name"]);
				Drug.officialPrice = Convert.ToInt32(dt.Rows[0]["officialPrice"]);
			}


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

		[HttpPost("api/drugImg/")]
		public JsonResult addDrugImg([FromBody] Dictionary<string, object> JSONinput)
		{
			var DrugsImgsJson = JsonConvert.SerializeObject(JSONinput["DrugsImgsUrls"], Newtonsoft.Json.Formatting.Indented);
			var DrugImgs = JsonConvert.DeserializeObject<DrugsImgsUrls>(DrugsImgsJson);

			string StoredProcedureName = DrugsProcedures.addDrugImg;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@drug_id", DrugImgs.drugId);
			Parameters.Add("@img_no", DrugImgs.imgNo);
			Parameters.Add("@url", DrugImgs.url);

			return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
		}

		[HttpPost("api/drugCategory/")]
		public JsonResult addDrugToCategory([FromBody] Dictionary<string, object> JSONinput)
		{
			var DrugsCategoriesJson = JsonConvert.SerializeObject(JSONinput["DrugsInCategory"], Newtonsoft.Json.Formatting.Indented);
			var DrugsCategories = JsonConvert.DeserializeObject<DrugsInCategory>(DrugsCategoriesJson);

			string StoredProcedureName = DrugsProcedures.addDrugToCategory;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@drug_id", DrugsCategories.drugId);
			Parameters.Add("@category_id", DrugsCategories.categoryId);

			return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
		}

		[HttpPost("api/drugEffectiveSubstance/")]
		public JsonResult addEffectiveSubstanceToDrug([FromBody] Dictionary<string, object> JSONinput)
		{
			var DrugsEffectiveSubstanceJson = JsonConvert.SerializeObject(JSONinput["EffectiveSubstances"], Newtonsoft.Json.Formatting.Indented);
			var DrugsEffectiveSubstance = JsonConvert.DeserializeObject<EffectiveSubstances>(DrugsEffectiveSubstanceJson);

			string StoredProcedureName = DrugsProcedures.addEffectiveSubstanceToDrug;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@drugId", DrugsEffectiveSubstance.drugId);
			Parameters.Add("@name", DrugsEffectiveSubstance.name);

			return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
		}


		[HttpPost("api/drugs/")]
		public JsonResult addDrug([FromBody] Dictionary<string, object> JSONinput)
		{
			var DrugsJson = JsonConvert.SerializeObject(JSONinput["Drugs"], Newtonsoft.Json.Formatting.Indented);
			var Drug = JsonConvert.DeserializeObject<Drugs>(DrugsJson);

			string StoredProcedureName = DrugsProcedures.addDrug;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@name", Drug.name);
			Parameters.Add("@officialPrice", Drug.officialPrice);

			DataTable dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);
			Drug.id = Convert.ToInt32(dt.Rows[0][0]);
			int i = 1;
			foreach (var drugImgUrl in Drug.imgsUrls)
			{
				var x = new DrugsImgsUrls();
				x.drugId = Drug.id;
				x.imgNo = i++;
				x.url = drugImgUrl;

				Dictionary<string, object> DrugsImgsUrlsParameters = new Dictionary<string, object>();
				DrugsImgsUrlsParameters.Add("DrugsImgsUrls", x);
				addDrugImg(DrugsImgsUrlsParameters);
			}


			foreach (var effectiveSubstance in Drug.effectiveSubstances)
			{
				var x = new EffectiveSubstances();
				x.drugId = Drug.id;
				x.name = effectiveSubstance;

				Dictionary<string, object> EffectiveSubstancesParameters = new Dictionary<string, object>();
				EffectiveSubstancesParameters.Add("EffectiveSubstances", x);
				addEffectiveSubstanceToDrug(EffectiveSubstancesParameters);
			}


			foreach (var DrugsInCategory in Drug.categoriesIds)
			{
				var x = new DrugsInCategory();
				x.drugId = Drug.id;
				x.categoryId = DrugsInCategory;

				Dictionary<string, object> DrugsInCategoryParameters = new Dictionary<string, object>();
				DrugsInCategoryParameters.Add("DrugsInCategory", x);
				addDrugToCategory(DrugsInCategoryParameters);
			}

			return Json(null);
		}

		

	}
}