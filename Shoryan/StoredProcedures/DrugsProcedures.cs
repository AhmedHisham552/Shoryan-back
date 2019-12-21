using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBapplication
{
	public class DrugsProcedures
	{
		//getters
		public static string getDrug = "getDrug";
		public static string getAllDrugsIds = "getAllDrugsIds";
		public static string getDrugsByCategory = "getDrugsByCategory";
		public static string getDrugsByName = "getDrugByName";
		public static string getDrugImgs = "getDrugImgs";
		public static string getEffectiveSubstances = "getEffectiveSubstances";
		public static string getCategoryOfDrug = "getCategoryOfDrug";

		public static string getAllDrugs = "getAllDrugsInformation";
		public static string getAllDrugEffectiveSubstances = "getAllDrugEffectiveSubstances";
		public static string getAllDrugImages = "getAllDrugImages";
		public static string getAllDrugCategories = "getAllDrugCategories";


		//adders
		public static string addDrugImg = "addDrugImg";
		public static string addDrugToCategory = "addDrugToCategory";
		public static string addEffectiveSubstanceToDrug = "addEffectiveSubstanceToDrug";
		public static string addDrug = "addDrug";
		//deleters
		public static string deleteDrug = "deleteDrug";
		public static string deleteDrugFromCategory = "deleteDrugFromCategory";
		public static string deleteDrugImg = "deleteDrugImg";
		public static string deleteEffectiveSubstance = "deleteEffectiveSubstance";
		//editers
		public static string editDrug = "editDrug";
	}
}