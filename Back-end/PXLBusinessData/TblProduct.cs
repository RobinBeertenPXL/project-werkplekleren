using PXLData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXLBusinessData
{
    public class ProdDto
    {
        public int Productid { get; set; }
        public string Productname { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int? Creationid { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? amount { get; set; }
        public string category { get; set; }
        public string productImage { get; set; }
    }
    class TblProdDto : TableHelper
    {
        private ProdDto prodDto;
        public TblProdDto(ProdDto prodDtoParam, string primaryKey)
        {
            prodDto = prodDtoParam;
            var properties = typeof(ProdDto).GetProperties();
            GetColumnData(properties, prodDtoParam);
        }
    }
    public class ProdData : DatabaseHelper
    {
        private TblProd tblProd;
        public ProdData()
        {
            TableName = "TblProduct";
            PrimaryKey = "ProductID";
        }
        public ProdData(ProdDto prodDto)
        {
            TableName = "TblProduct";
            PrimaryKey = "ProductID";
            tblProd = new TblProd(prodDto, PrimaryKey);
        }
        public int CreateRecord()
        {
            return CreateRecord(tblProd.GetInsertColumnsData, tblProd.GetInsertColumnValuesData);
        }
        public void UpdateRecord(int primaryKeyValue)
        {
            UpdateRecord(tblProd.GetUpdateColumnsData, primaryKeyValue);
        }
        public ProdDto EntityUser(int primaryKeyValue)
        {
            var dt = GetEntity(primaryKeyValue);
            ProdDto prodDto = DataTableHelper.ConvertDataTableToObject<ProdDto>(dt);
            return prodDto;
        }
        public List<ProdDto> EntityUsers()
        {
            var dt = GetEntities();
            List<ProdDto> prodDtoLst = DataTableHelper.ConvertDataTableToObjectList<ProdDto>(dt);
            return prodDtoLst;
        }

        protected class TblProd : TableHelper
        {
            public TblProd(ProdDto prodDto, string primaryKey)
            {
                var properties = typeof(ProdDto).GetProperties();
                PrimaryKey = primaryKey;
                GetColumnData(properties, prodDto);
            }
        }

    }
}
