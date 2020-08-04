using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PXL_Project_Team_05.Models
{
    public class OrderModel
    {
        public OrderModel(string json)
        {
            //cart = new Dictionary<int, OrderLineModel>();
            JObject jObject = JObject.Parse(json);
            //JToken jCart = jObject["cart"];
            // JToken jUser = jObject["userData"];
            //var des = (OrderModel)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(OrderModel));
            cart = JsonConvert.DeserializeObject<Dictionary<int, OrderLineModel>>(json);
            //cart = (Dictionary<int, OrderLineModel>)jObject["cart"];
        }
        public Dictionary<int, OrderLineModel> cart { get; set; }
    }
}