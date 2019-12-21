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
    public class OrdersController : Controller
    {
		DBManager dbMan;

		public OrdersController()
		{
			dbMan = new DBManager();
		}

        private List<int> aux_getListingsInOrder(int orderId)
        {
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@orderId", orderId);
            DataTable dt = dbMan.ExecuteReader("getItemsInOrder", Parameters);


            List<int> listingsIds = new List<int>();
            if (dt != null)
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    listingsIds.Add(Convert.ToInt32(dt.Rows[i]["listingId"]));

                }

            return listingsIds;
        }

        [HttpPost("api/Order")]
        public IActionResult addOrder([FromBody] Dictionary<string, object> JSONinput)
        {

            //takes an order json with its listings

            var orderJson = JsonConvert.SerializeObject(JSONinput["Order"], Newtonsoft.Json.Formatting.Indented);
            var order = JsonConvert.DeserializeObject<Orders>(orderJson);

            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@OrderDate", order.orderDate);
            Parameters.Add("@ExpectedDeliveryDate", order.expectedDeliveryDate);
            Parameters.Add("@ItemsPrice", order.itemsPrice);
            Parameters.Add("@userId", order.userId);
            Parameters.Add("@DeliverPrice", order.deliverPrice);
            Parameters.Add("@Discount", order.discount);

            DataTable dt = dbMan.ExecuteReader( OrdersProcedures.addOrder , Parameters);

            if (dt == null) return StatusCode(500);

            foreach (var listingId in order.listingsIds)
            {
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("@orderId", dt.Rows[0][0]);
                Params.Add("@listingId", listingId);
                int returnedValue = dbMan.ExecuteNonQuery(OrdersProcedures.addItemToOrder, Params);
                if (returnedValue == 0) return StatusCode(500);
            }

            return Json("Order added successfully");

        }

        [HttpGet("api/Order/{orderId}")]
        public IActionResult getOrderById(int orderId)
        {
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@orderId", orderId);

            DataTable dt = dbMan.ExecuteReader(OrdersProcedures.getOrderById, Parameters);

            if (dt == null) return StatusCode(404);

            Orders order = new Orders();
            order.id = orderId;
            order.orderDate = Convert.ToDateTime(dt.Rows[0]["OrderDate"]);
            order.expectedDeliveryDate = Convert.ToDateTime(dt.Rows[0]["ExpectedDeliveryDate"]);
            order.itemsPrice = Convert.ToInt32(dt.Rows[0]["ItemsPrice"]);
            order.deliverPrice = Convert.ToInt32(dt.Rows[0]["DeliverPrice"]);
            order.discount = Convert.ToInt32(dt.Rows[0]["Discount"]);

            //check if user review rating is defined and add it to order
            if(dt.Rows[0]["userReviewRating"].GetType() != typeof(DBNull))
                order.userReviewRating = Convert.ToInt32(dt.Rows[0]["userReviewRating"]);

            order.userReviewText = Convert.ToString(dt.Rows[0]["userReviewText"]);
            order.userId = Convert.ToInt32(dt.Rows[0]["userId"]);

            //check if courier review rating is defined and add it to order
            if (dt.Rows[0]["courierReviewRating"].GetType() != typeof(DBNull))
                order.courierReviewRating = Convert.ToInt32(dt.Rows[0]["courierReviewRating"]);
            
            order.courierReviewText = Convert.ToString(dt.Rows[0]["courierReviewText"]);

            //check if the order is assigned to courier
            if (dt.Rows[0]["courierId"].GetType() != typeof(DBNull))
                order.courierId = Convert.ToInt32(dt.Rows[0]["courierId"]);

            order.state = Convert.ToString(dt.Rows[0]["state"]);
            
            order.listingsIds = aux_getListingsInOrder(orderId);

            return Json(order);
        }


        [HttpGet("api/UpcommingOrders/{courierId}")]
        public IActionResult getCourierUpcommingOrders(int courierId)
        {
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@courierId", courierId);

            DataTable dt = dbMan.ExecuteReader(OrdersProcedures.getCourierUpcomingOrders, Parameters);

            if (dt == null) return StatusCode(404);
            return Json(dt);
        }

        [HttpGet("api/PastOrders/{courierId}")]
        public IActionResult getCourierPastOrders(int courierId)
        {
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@courierId", courierId);

            DataTable dt = dbMan.ExecuteReader(OrdersProcedures.getCourierPastOrders, Parameters);

            if (dt == null) return StatusCode(404);
            return Json(dt);
        }

        [HttpGet("api/OrdersOfUser/{userId}")]
        public IActionResult getOrdersOfUser(int userId)
        {
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@UserId", userId);

            DataTable dt = dbMan.ExecuteReader(OrdersProcedures.getOrderOfUser, Parameters);

            if (dt == null) return StatusCode(404);
            return Json(dt);
        }

    }
}