using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using PXLData;

namespace PXLBusinessData
{
    public class PersDto
    {
        public int Personid { get; set; }
        public int Userid { get; set; }
        public int Addressid { get; set; }
        public string FirstName { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Creationid { get; set; }
        public DateTime? CreationDate { get; set; }
        public string paymentMethod { get; set; }
    }
    class TblPersDto : TableHelper
    {
        private PersDto persDto;
        public TblPersDto(PersDto persDtoParam, string primaryKey)
        {
            persDto = persDtoParam;
            var properties = typeof(PersDto).GetProperties();
            GetColumnData(properties, persDtoParam);
        }
    }
    public class PersData : DatabaseHelper
    {
        private TblPers tblPers;
        public PersData()
        {
            TableName = "TblPerson";
            PrimaryKey = "PersonID";
        }
        public PersData(PersDto persDto)
        {
            TableName = "TblPerson";
            PrimaryKey = "PersonID";
            tblPers = new TblPers(persDto, PrimaryKey);
        }
        public int CreateRecord()
        {
            return CreateRecord(tblPers.GetInsertColumnsData, tblPers.GetInsertColumnValuesData);
        }
        public void UpdateRecord(int primaryKeyValue)
        {
            UpdateRecord(tblPers.GetUpdateColumnsData, primaryKeyValue);
        }
        public PersDto EntityUser(int primaryKeyValue)
        {
            var dt = GetEntity(primaryKeyValue);
            PersDto persDto = DataTableHelper.ConvertDataTableToObject<PersDto>(dt);
            return persDto;
        }
        public List<PersDto> EntityUsers()
        {
            var dt = GetEntities();
            List<PersDto> persDtoLst = DataTableHelper.ConvertDataTableToObjectList<PersDto>(dt);
            return persDtoLst;
        }

        protected class TblPers : TableHelper
        {
            public TblPers(PersDto persDto, string primaryKey)
            {
                var properties = typeof(PersDto).GetProperties();
                PrimaryKey = primaryKey;
                GetColumnData(properties, persDto);
            }
        }

    }
}
