using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using PXLBusinessData;
using PXLBusiness;
using System.Web.Mvc;
using System.Web.Http.Cors;

namespace PXL_Project_Team_05.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "XCustom-Header")]

    public class UserController : ApiController
    {
        public void CreateUser(string email, string paswoord)
        {
            LoginData.ConnectionString = ConfigurationManager.ConnectionStrings["PXLDB"].ConnectionString;
            LoginData.LoginSuccessFul = true;
            UserBusiness userBusiness = new UserBusiness();// connection);
            UserDto userDto = new UserDto();
            userDto.Email = email;
            userDto.Password = paswoord;
            userBusiness.CreateUser(userDto);
        }

        public JsonResult GetUserData()
        {
            LoginData.ConnectionString = ConfigurationManager.ConnectionStrings["PXLDB"].ConnectionString;
            LoginData.LoginSuccessFul = true;
            UserBusiness userBusiness = new UserBusiness();
            List<UserDto> userDtos = userBusiness.GetUserDto();
            JsonResult json = new JsonResult();

            json.Data = userDtos;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }
    }
}
