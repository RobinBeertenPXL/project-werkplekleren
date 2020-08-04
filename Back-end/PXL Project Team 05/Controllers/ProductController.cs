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

    public class ProductController : ApiController
    {
        [System.Web.Http.HttpGet]
        public JsonResult GetProductData()
        {
            LoginData.ConnectionString =
            ConfigurationManager.ConnectionStrings["PXLDB"].ConnectionString;
            LoginData.LoginSuccessFul = true;
            ProductBusiness pb = new ProductBusiness();
            DataTable dt = pb.GetProdData();
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
        [System.Web.Http.HttpGet]
        public JsonResult GetProductById(int productId)
        {
            LoginData.ConnectionString =
            ConfigurationManager.ConnectionStrings["PXLDB"].ConnectionString;
            LoginData.LoginSuccessFul = true;
            ProductBusiness pb = new ProductBusiness();
            ProdDto dt = pb.GetProdDto(productId);
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
        public JsonResult CreateProduct(object json)
        {
            ProductModel prodModel = new ProductModel(json.ToString());
            LoginData.ConnectionString =
            ConfigurationManager.ConnectionStrings["PXLDB"].ConnectionString;
            LoginData.LoginSuccessFul = true;
            ProductBusiness pb = new ProductBusiness();
            ProdDto product = new ProdDto();
            product.Price = prodModel.price;
            product.Productname = prodModel.productName;
            product.Description = prodModel.description;
            product.category = prodModel.category;
            product.productImage = prodModel.productimage;

            JsonResult json2 = new JsonResult();
            bool validation;

            pb.CreateProduct(product);
            validation = true;

            json2.Data = validation;

            return json2;
        }
        [System.Web.Http.HttpPost]
        public JsonResult UpdateProduct(int productID, object json)
        {
            ProductModel prodModel = new ProductModel(json.ToString());
            LoginData.ConnectionString =
            ConfigurationManager.ConnectionStrings["PXLDB"].ConnectionString;
            LoginData.LoginSuccessFul = true;
            ProductBusiness pb = new ProductBusiness();
            ProdDto product = new ProdDto();
            product.Price = prodModel.price;
            product.Productname = prodModel.productName;
            product.Description = prodModel.description;
            product.category = prodModel.category;
            product.productImage = prodModel.productimage;

            JsonResult json2 = new JsonResult();
            bool validation = false;

            pb.UpdateProduct(product, productID);
            validation = true;

            json2.Data = validation;

            return json2;
        }
    }
}
