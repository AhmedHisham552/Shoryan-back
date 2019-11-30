using DBapplication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shoryan.Models;
using Shoryan.Routes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Shoryan.Controllers
{
    
    public class PostsController : Controller
    {
        private List<Post> posts;
        
        public PostsController()
        {
            posts = new List<Post>();
            for(int i=0; i<5; i++)
            {
                posts.Add(new Post { id = i });
            }
        }

        [HttpGet(ApiRoutes.posts.getAll)]
        public IActionResult getAll()
        {
            try
            {
                DBManager dbMan = new DBManager();
                string query = "SELECT * FROM Users;";
                DataTable dt = dbMan.ExecuteReader(query);
                string result = JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented);
                //if (result == "null") throw new Exception("error kber");
                return Json(dt);
            }
            catch(Exception e)
            {
                return Json(e);
            }
            
        }

        [HttpPost("api/add")]
        public JsonResult AddEmployee([FromBody] Employee employee) {
            
            Employee test = employee;

            return Json(employee);
        }


    }
}
