using PXLBusiness;
using PXLBusinessData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace PXL_Project_Team_05.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "XCustom-Header")]

    public class PersonController : ApiController
    {
        public void CreatePerson(string firstname, string lastname, DateTime birthDate)
        {
            LoginData.ConnectionString = ConfigurationManager.ConnectionStrings["PXLDB"].ConnectionString;
            LoginData.LoginSuccessFul = true;
            PersonBusiness persBusiness = new PersonBusiness();// connection);
            PersDto persDto = new PersDto();
            persDto.FirstName = firstname;
            persDto.Name = lastname;
            persDto.BirthDate = birthDate;
            persBusiness.CreatePerson(persDto);
        }
        public JsonResult GetPersonData()
        {
            LoginData.ConnectionString = ConfigurationManager.ConnectionStrings["PXLDB"].ConnectionString;
            LoginData.LoginSuccessFul = true;
            PersonBusiness persBusiness = new PersonBusiness();
            List<PersDto> persDtos = persBusiness.GetPersDto();
            JsonResult json = new JsonResult();
            json.Data = persDtos;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }
        [System.Web.Http.HttpGet]
        public JsonResult GetPersonData(int userID)
        {
            LoginData.ConnectionString = ConfigurationManager.ConnectionStrings["PXLDB"].ConnectionString;
            LoginData.LoginSuccessFul = true;
            PersonBusiness persBusiness = new PersonBusiness();
            PersDto persDtos = persBusiness.GetUserDto(userID);
            JsonResult json = new JsonResult();
            json.Data = persDtos;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }
    }
}
