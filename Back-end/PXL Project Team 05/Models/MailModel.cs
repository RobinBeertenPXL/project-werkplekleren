using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PXL_Project_Team_00.Models
{
    public class MailModel
    {
        public MailModel(string json)
        {
            JObject jObject = JObject.Parse(json);
            MailTo = (string)jObject["mailto"];
            MailSubject = (string)jObject["mailsubject"];
            MailBody = (string)jObject["mailbody"];            
        }
        public string MailTo { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
    }
}