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
			DataTable dt;

			dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);

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
		public IActionResult getAllDrugs()
		{
			string StoredProcedureName = DrugsProcedures.getAllDrugEffectiveSubstances;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			DataTable EffectiveSubstances = dbMan.ExecuteReader(StoredProcedureName, Parameters);

			string StoredProcedureName2 = DrugsProcedures.getAllDrugImages;
			Dictionary<string, object> Parameters2 = new Dictionary<string, object>();

			DataTable Drugs_imgs_urls = dbMan.ExecuteReader(StoredProcedureName2, Parameters2);

			string StoredProcedureName3 = DrugsProcedures.getAllDrugCategories;
			Dictionary<string, object> Parameters3 = new Dictionary<string, object>();
			DataTable Drugs_in_categories = dbMan.ExecuteReader(StoredProcedureName3, Parameters3);

			string StoredProcedureName4 = DrugsProcedures.getAllDrugs;
			Dictionary<string, object> Parameters4 = new Dictionary<string, object>();
			DataTable DrugsInformation = dbMan.ExecuteReader(StoredProcedureName4, Parameters4);


			Dictionary<int, Drugs> DrugsDict = new Dictionary<int, Drugs>();
			if (DrugsInformation != null)
			{
				for (int i = 0; i < DrugsInformation.Rows.Count; i++)
				{
					Drugs Drug = new Drugs();
					Drug.effectiveSubstances = new List<string>();
					Drug.categoriesIds = new List<int>();
					Drug.imgsUrls = new List<string>();
					Drug.id = Convert.ToInt32(DrugsInformation.Rows[i]["id"]);
					Drug.name = Convert.ToString(DrugsInformation.Rows[i]["name"]);
					Drug.officialPrice = Convert.ToInt32(DrugsInformation.Rows[i]["officialPrice"]);

					DrugsDict.Add(Drug.id, Drug);
				}
			}

			if (EffectiveSubstances != null)
			{
				for (int i = 0; i < EffectiveSubstances.Rows.Count; i++)
				{
					int drugId = Convert.ToInt32(EffectiveSubstances.Rows[i]["drugId"]);
					string substanceName = Convert.ToString(EffectiveSubstances.Rows[i]["name"]);

					if (DrugsDict.ContainsKey(drugId))
					{
						DrugsDict[drugId].effectiveSubstances.Add(substanceName);
					}
				}
			}


			if (Drugs_in_categories != null)
			{
				for (int i = 0; i < Drugs_in_categories.Rows.Count; i++)
				{
					int drugId = Convert.ToInt32(Drugs_in_categories.Rows[i]["drug_id"]);
					int categoryId = Convert.ToInt32(Drugs_in_categories.Rows[i]["category_id"]);

					if (DrugsDict.ContainsKey(drugId))
					{
						DrugsDict[drugId].categoriesIds.Add(categoryId);
					}
				}
			}

			if (Drugs_imgs_urls != null)
			{
				for (int i = 0; i < Drugs_imgs_urls.Rows.Count; i++)
				{
					int drugId = Convert.ToInt32(Drugs_imgs_urls.Rows[i]["drug_id"]);
					string drugImgUrl = Convert.ToString(Drugs_imgs_urls.Rows[i]["url"]);

					if (DrugsDict.ContainsKey(drugId))
					{
						DrugsDict[drugId].imgsUrls.Add(drugImgUrl);
					}
				}
			}

			return Json(DrugsDict.Values);

		}



		[HttpGet("api/drugs/{drugId}")]
		public IActionResult getDrugById(int drugId)
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
			}else
			{
				return StatusCode(500, "Drug not found");
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
		public IActionResult getDrugsByCategory(int categoryId)
		{

			string StoredProcedureName = DrugsProcedures.getDrugsByCategory;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@categoryId", categoryId);

			try
			{
				return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
			}
			catch (Exception)
			{

				return StatusCode(500, "Category not found");
			}
			
		}

		[HttpGet("api/drugsByName/{drugName}")]
		public IActionResult getDrugsByName(string drugName)
		{
			string StoredProcedureName = DrugsProcedures.getDrugsByName;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@name", drugName);

			DataTable dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);
			if (dt == null)
				return Json(null);

			List<Drugs> drugList = new List<Drugs>();

			for (int i = 0;i<dt.Rows.Count;i++)
			{
				Drugs Drug = new Drugs();
				try
				{
					var DrugsJson = JsonConvert.SerializeObject(Json(getDrugById(Convert.ToInt32(dt.Rows[0][0]))).Value, Newtonsoft.Json.Formatting.Indented);
					Drug = JsonConvert.DeserializeObject<Drugs>(DrugsJson);
				}
				catch (Exception)
				{

					return StatusCode(500, "Error parsing JSON");
				}

				drugList.Add(Drug);
			}
			return Json(drugList);
		}

		[HttpGet("api/drugImg/{drugId}")]
		public IActionResult getAllDrugImgs(int drugId)
		{
			string StoredProcedureName = DrugsProcedures.getDrugImgs;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@drug_id", drugId);

			try
			{
				return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
			}
			catch (Exception)
			{

				return StatusCode(500, "Drug not found");
			}


		}

		[HttpGet("api/drugImg/{drugId}/{imgNo}")]
		public IActionResult getDrugImg(int drugId, int imgNo)
		{
			string StoredProcedureName = DrugsProcedures.getDrugImgs;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@drug_id", drugId);
			Parameters.Add("@img_no", imgNo);

			try
			{
				return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
			}
			catch (Exception)
			{

				return StatusCode(500, "Drug or image not found");
			}
		}

		[HttpGet("api/effectiveSubstances/{drugId}")]
		public IActionResult getEffectiveSubstances(int drugId)
		{
			string StoredProcedureName = DrugsProcedures.getEffectiveSubstances;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@drug_id", drugId);

			try
			{
				return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
			}
			catch (Exception)
			{

				return StatusCode(500, "Drug not found");
			}
		}

		[HttpPost("api/drugImg/")]
		public IActionResult addDrugImg([FromBody] Dictionary<string, object> JSONinput)
		{
			var DrugsImgsJson = JsonConvert.SerializeObject(JSONinput["DrugsImgsUrls"], Newtonsoft.Json.Formatting.Indented);
			var DrugImgs = JsonConvert.DeserializeObject<DrugsImgsUrls>(DrugsImgsJson);

			string StoredProcedureName = DrugsProcedures.addDrugImg;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@drug_id", DrugImgs.drugId);
			Parameters.Add("@img_no", DrugImgs.imgNo);
			Parameters.Add("@url", DrugImgs.url);

			try
			{
				return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
			}
			catch (Exception)
			{

				return StatusCode(500, "Drug not found");
			}
		}

		[HttpPost("api/drugCategory/")]
		public IActionResult addDrugToCategory([FromBody] Dictionary<string, object> JSONinput)
		{
			var DrugsCategoriesJson = JsonConvert.SerializeObject(JSONinput["DrugsInCategory"], Newtonsoft.Json.Formatting.Indented);
			var DrugsCategories = JsonConvert.DeserializeObject<DrugsInCategory>(DrugsCategoriesJson);

			string StoredProcedureName = DrugsProcedures.addDrugToCategory;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@drug_id", DrugsCategories.drugId);
			Parameters.Add("@category_id", DrugsCategories.categoryId);

			try
			{
				return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
			}
			catch (Exception)
			{

				return StatusCode(500, "Drug or category not found");
			}
		}

		[HttpPost("api/drugEffectiveSubstance/")]
		public IActionResult addEffectiveSubstanceToDrug([FromBody] Dictionary<string, object> JSONinput)
		{
			var DrugsEffectiveSubstanceJson = JsonConvert.SerializeObject(JSONinput["EffectiveSubstances"], Newtonsoft.Json.Formatting.Indented);
			var DrugsEffectiveSubstance = JsonConvert.DeserializeObject<EffectiveSubstances>(DrugsEffectiveSubstanceJson);

			string StoredProcedureName = DrugsProcedures.addEffectiveSubstanceToDrug;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();

			Parameters.Add("@drugId", DrugsEffectiveSubstance.drugId);
			Parameters.Add("@name", DrugsEffectiveSubstance.name);

			try
			{
				return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
			}
			catch (Exception)
			{

				return StatusCode(500, "Drug not found or substance already exists");
			}
		}


		[HttpPost("api/drugs/")]
		public IActionResult addDrug([FromBody] Dictionary<string, object> JSONinput)
		{
			Drugs Drug = new Drugs();
			try
			{
				var DrugsJson = JsonConvert.SerializeObject(JSONinput["Drugs"], Newtonsoft.Json.Formatting.Indented);
				Drug = JsonConvert.DeserializeObject<Drugs>(DrugsJson);
			}
			catch (Exception)
			{

				return StatusCode(500, "Error parsing JSON");
			}

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

		[HttpDelete("api/drugFromCategory/{drugId}/{categoryId}")]
		public IActionResult deleteDrugFromCategory(int drugId, int categoryId)
		{
			string StoredProcedureName = DrugsProcedures.deleteDrugFromCategory;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@drug_id", drugId);
			Parameters.Add("@category_id", categoryId);
			try
			{
				return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
			}
			catch (Exception)
			{

				return StatusCode(500,"Drug not in category");
			}
		}

		[HttpDelete("api/effectiveSubstance/{drugId}/{effectiveSubstanceName}")]
		public IActionResult deleteEffectiveSubstance(int drugId,string effectiveSubstanceName)
		{
			string StoredProcedureName = DrugsProcedures.deleteEffectiveSubstance;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@drug_id", drugId);
			Parameters.Add("@name", effectiveSubstanceName);
			try
			{
				return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
			}
			catch (Exception)
			{

				return StatusCode(500, "Drug doesn't have this effective substance");
			}
		}

		[HttpDelete("api/drugImgUrl/{drugId}/{imgNo}")]
		public IActionResult deleteDrugImgUrl(int drugId, int imgNo)
		{
			string StoredProcedureName = DrugsProcedures.deleteDrugImg;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@drug_id", drugId);
			Parameters.Add("@img_no", imgNo);
			try
			{
				return Json(dbMan.ExecuteNonQuery(StoredProcedureName, Parameters));
			}
			catch (Exception)
			{

				return StatusCode(500, "Drug doesn't have this image");
			}
		}

		[HttpDelete("api/drugs/{drugId}")]
		public IActionResult deleteDrug(int drugId)
		{
			Drugs Drug = new Drugs();
			try
			{
				var DrugJson = JsonConvert.SerializeObject(Json(getDrugById(drugId)).Value, Newtonsoft.Json.Formatting.Indented);
				Drug = JsonConvert.DeserializeObject<Drugs>(DrugJson);
			}
			catch (Exception)
			{

				return StatusCode(500, "Error parsing JSON");
			}


			foreach (var x in Drug.effectiveSubstances)
			{
				deleteEffectiveSubstance(Drug.id, x);
			}

			foreach(var x in Drug.categoriesIds)
			{
				deleteDrugFromCategory(drugId, x);
			}

			int i = 1;
			foreach (var x in Drug.imgsUrls)
			{
				deleteDrugImgUrl(drugId, i++);
			}

			string StoredProcedureName = DrugsProcedures.deleteDrug;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@id", drugId);
			try
			{
				int returnCode = dbMan.ExecuteNonQuery(StoredProcedureName, Parameters);
				if(returnCode == -1)
				{
					return StatusCode(200, "Drug deleted successfully");
				}else
				{
					return StatusCode(500, "Internal Server Error");
				}
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}

	}
}