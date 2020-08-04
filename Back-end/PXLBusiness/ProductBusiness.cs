using PXLBusinessData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXLBusiness
{
    public class ProductBusiness
    {
        public ProductBusiness()
        {
            if (LoginData.LoginSuccessFul)
            {
                LoginData.Start();
            }
            else
            {
                throw new Exception("Gelieve correct in te loggen!");
            }
        }
        ~ProductBusiness()
        {
            LoginData.Stop();
        }
        public void CreateProduct(ProdDto prodDto)
        {
            prodDto.Creationid = LoginData.UserID;
            prodDto.CreationDate = DateTime.Now;
            ProdData prodData = new ProdData(prodDto);
            prodDto.Productid = prodData.CreateRecord();
            LoginData.Stop();
        }
        public void UpdateProduct(ProdDto prodDto, int primaryKeyValue)
        {
            prodDto.Creationid = LoginData.UserID;
            prodDto.CreationDate = DateTime.Now;
            ProdData prodData = new ProdData(prodDto);
            prodData.UpdateRecord(primaryKeyValue);
            LoginData.Stop();
        }
        public ProdDto GetProdDto(int primaryKeyValue)
        {
            ProdData prodData = new ProdData();
            return prodData.EntityUser(primaryKeyValue);
        }
        public List<ProdDto> GetProdDto()
        {
            ProdData prodData = new ProdData();
            return prodData.EntityUsers();
        }
        public DataTable GetProdData()
        {
            ProdData prodData = new ProdData();
            DataTable dt = prodData.GetRecords();
            //DatabaseLauncher.StopWithoutTransaction();
            return dt;
        }
    }
}
