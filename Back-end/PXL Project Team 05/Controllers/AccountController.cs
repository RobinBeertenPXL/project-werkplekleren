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

    public class AccountController : ApiController
    {
        [System.Web.Http.HttpGet]
        //public JsonResult GetAccountData()
        //{
        //    LoginData.ConnectionString =
        //    ConfigurationManager.ConnectionStrings["PXLDB"].ConnectionString;
        //    LoginData.LoginSuccessFul = true;
        //    AccountBusiness ab = new AccountBusiness();
        //    DataTable dt = ab.GetAccountData();
        //    JsonResult json = new JsonResult();
        //    string jsonString = JsonConvert.SerializeObject(dt);
        //    json.Data = jsonString;
        //    json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    return json;
        //}
        public JsonResult GetAccountData(string email, string pwd)
        {
            LoginData.ConnectionString =
            ConfigurationManager.ConnectionStrings["PXLDB"].ConnectionString;
            LoginData.LoginSuccessFul = true;
            AccountBusiness ab = new AccountBusiness();
            DataTable dt = ab.GetAccountData(email, pwd);
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
        public JsonResult CreateAccount(object json)
        {
            AccountModel accountModel = new AccountModel(json.ToString());
            LoginData.ConnectionString =
            ConfigurationManager.ConnectionStrings["PXLDB"].ConnectionString;
            LoginData.LoginSuccessFul = true;
            AccountBusiness ab = new AccountBusiness();
            UserDto user = new UserDto();
            user.Email = accountModel.email;
            user.Password = accountModel.password;
            PersDto pers = new PersDto();
            pers.FirstName = accountModel.firstName;
            pers.Name = accountModel.lastName;
            pers.paymentMethod = "Geen betaalmethode gekozen";
            DateTime birthdate = new DateTime(2000, 1, 1);

            if (DateTime.TryParse(accountModel.birthDate, out birthdate))
            {
                pers.BirthDate = birthdate;
            }
            else
            {
                pers.BirthDate = null;
            }

            AccountBusiness account = new AccountBusiness();

            JsonResult json2 = new JsonResult();
            bool validation = false;

            if (account.GetEmail(user.Email) == -1 && pers.BirthDate != null && user.Email != null && pers.FirstName != null && pers.Name != null && user.Password != null)
            {
                ab.CreateAccount(user, pers);
                validation = true;
            }
            else
            {
                validation = false;
            }

            json2.Data = validation;

            return json2;
        }
        public JsonResult UpdateAccount(int userID, int personID, object json)
        {
            AccountModel accountModel = new AccountModel(json.ToString());
            LoginData.ConnectionString =
            ConfigurationManager.ConnectionStrings["PXLDB"].ConnectionString;
            LoginData.LoginSuccessFul = true;
            AccountBusiness ab = new AccountBusiness();
            UserDto user = new UserDto();
            user.Userid = userID;
            user.Email = accountModel.email;
            user.Password = accountModel.password;
            PersDto pers = new PersDto();
            pers.Userid = userID;
            pers.Personid = personID;
            pers.FirstName = accountModel.firstName;
            pers.Name = accountModel.lastName;
            pers.paymentMethod = accountModel.paymentMethod;
            DateTime birthdate = new DateTime(2000, 1, 1);

            if (DateTime.TryParse(accountModel.birthDate, out birthdate))
            {
                pers.BirthDate = birthdate;
            }
            else
            {
                pers.BirthDate = null;
            }

            AccountBusiness account = new AccountBusiness();

            JsonResult json2 = new JsonResult();
            bool validation = false;

            if (account.GetEmail(user.Email, (user.Userid).ToString()) == -1 && pers.BirthDate != null && user.Email != null && pers.FirstName != null && pers.Name != null && user.Password != null)
            {
                ab.UpdateAccount(userID, personID, user, pers);
                validation = true;
            }
            else
            {
                validation = false;
            }

            json2.Data = validation;

            return json2;
        }
    }
}
