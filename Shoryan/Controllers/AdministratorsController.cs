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

namespace Shoryan.Controllers
{
    [ApiController]
    public class AdministratorsController : Controller
    {
		DBManager dbMan;
		public AdministratorsController()
		{
			dbMan = new DBManager();
		}

	}
}