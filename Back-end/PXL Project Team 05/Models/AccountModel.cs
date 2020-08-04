using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PXL_Project_Team_05.Models
{
    public class AccountModel
    {
        public AccountModel(string json)
        {
            JObject jObject = JObject.Parse(json);
            // JToken jUser = jObject["userData"];
            firstName = (string)jObject["firstName"];
            lastName = (string)jObject["lastName"];
            birthDate = (string)jObject["birthdate"];
            email = (string)jObject["email"];
            password = (string)jObject["password"];
            paymentMethod = (string)jObject["paymentMethod"];
        }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string birthDate { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string paymentMethod { get; set; }
    }
}