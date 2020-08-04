using Newtonsoft.Json;
using PXL_Project_Team_05.Models;
using PXLBusiness;
using PXLBusinessData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace PXL_Project_Team_05.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "XCustom-Header")]

    public class OrderController : ApiController
    {
        [System.Web.Http.HttpGet]
        public JsonResult GetAccountData(int userId)
        {
            LoginData.ConnectionString =
            ConfigurationManager.ConnectionStrings["PXLDB"].ConnectionString;
            LoginData.LoginSuccessFul = true;
            OrderBusiness ob = new OrderBusiness();
            DataTable dt = ob.GetOrderData(userId);
            JsonResult json = new JsonResult();
            string jsonString = JsonConvert.SerializeObject(dt);
            json.Data = jsonString;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            if (string.Equals(json.Data, "[]"))
            {
                json.Data = false;
            }

            return json;
        }
        [System.Web.Http.HttpPost]
        public JsonResult CreateOrder(object json, int userID)
        {
            OrderModel orderModel = new OrderModel(json.ToString());
            LoginData.ConnectionString =
            ConfigurationManager.ConnectionStrings["PXLDB"].ConnectionString;
            LoginData.LoginSuccessFul = true;
            OrderBusiness ob = new OrderBusiness();
            OrderDto order = new OrderDto();
            order.Userid = userID;
            List<OrderLineDto> orderLines = new List<OrderLineDto>();
            foreach (int productID in orderModel.cart.Keys)
            {
                orderLines.Add(new OrderLineDto()
                {
                    Productid = orderModel.cart[productID].productId,
                    Amount = orderModel.cart[productID].amount
                });
            }

            // TODO: create validation
            JsonResult json2 = new JsonResult();
            bool validation;

            ob.CreateOrder(order, orderLines);
            validation = true;

            json2.Data = validation;

            return json2;
        }
    }
}
