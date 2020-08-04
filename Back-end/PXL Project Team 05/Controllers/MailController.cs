using PXL_Project_Team_00.Models;
using PXLBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace PXL_Project_Team_00.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class MailController : ApiController
    {        
        [System.Web.Http.HttpPost]
        public JsonResult SendEmail(object json)
        {
            MailModel mail=new MailModel(json.ToString());            
            MailData md = new MailData();
            md.MailTo = mail.MailTo;
            md.MailSubject = mail.MailSubject;
            md.MailBody = mail.MailBody;
            MailBusiness mb = new MailBusiness(md);
            JsonResult jsonReturn = new JsonResult();
            try
            {
                mb.SendEmail();
                jsonReturn.Data = true;
            }
            catch
            {
                jsonReturn.Data = false;
            }
            //string jsonString = JsonConvert.SerializeObject(am.GetAccountModels(dt));
            //jsonReturn.Data = "mail sending ...";
            jsonReturn.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jsonReturn.ContentType = "application/json";
            return jsonReturn;
        }
    }
}
