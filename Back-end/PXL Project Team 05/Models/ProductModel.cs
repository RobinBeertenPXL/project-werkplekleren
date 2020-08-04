using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PXL_Project_Team_05.Models
{
    public class ProductModel
    {
        public ProductModel(string json)
        {
            JObject jObject = JObject.Parse(json);
            // JToken jUser = jObject["userData"];
            // productId = (int)jObject["productId"];
            productName = (string)jObject["productName"];
            price = (decimal)jObject["price"];
            description = (string)jObject["description"];
            category = (string)jObject["category"];
            productimage = (string)jObject["productimage"];
            // creationId = (int)jObject["creationId"];
            // creationDate = (DateTime)jObject["creationDate"];
        }
        // public int? productId { get; set; }
        public string productName { get; set; }
        public decimal price { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public string productimage { get; set; }
        // public int? creationId { get; set; }
        // public DateTime? creationDate { get; set; }
    }
}