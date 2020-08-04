using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PXL_Project_Team_05.Models
{
    public class OrderLineModel
    {
        //public OrderLineModel(string json)
        //{
        //    JObject jObject = JObject.Parse(json);
        //    // JToken jUser = jObject["userData"];
        //    productId = (int)jObject["productId"];
        //    //price = (double)jObject["price"];
        //    quantity = (int)jObject["amount"];
        //}
        public int productId { get; set; }
        //public double price { get; set; }
        public int amount { get; set; }
    }
}