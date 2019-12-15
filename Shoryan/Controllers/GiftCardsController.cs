using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DBapplication;

namespace Shoryan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftCardsController : ControllerBase
    {
		DBManager dbMan;

		public GiftCardsController()
		{
			dbMan = new DBManager();
		}

		public void addGiftCard(string code, int value, DateTime expiryDate)
        {
            string StoredProcedureName = GiftCardsProcedures.addGiftCard;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@code", code);
            Parameters.Add("@value", value);
            Parameters.Add("@expiryDate", expiryDate);
            dbMan.ExecuteNonQuery(StoredProcedureName, Parameters);
        }
    }
}