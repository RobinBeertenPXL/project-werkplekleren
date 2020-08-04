using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using PXLData;

namespace PXLBusinessData
{
    public class UserDto
    {
        public int Userid { get; set; }
        //public string Voornaam { get; set; }
        //public string Naam { get; set; }
        //public DateTime? Geboortedatum { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? creationid { get; set; }
        public DateTime? creationdate { get; set; }
    }
    class TblUserDto : TableHelper
    {
        private UserDto userDto;
        public TblUserDto(UserDto userDtoParam, string primaryKey)
        {
            userDto = userDtoParam;
            var properties = typeof(UserDto).GetProperties();
            GetColumnData(properties, userDtoParam);
        }
       
    }
    public class UserData : DatabaseHelper
    {
        private TblUser tblUser;
        public UserData()
        {
            TableName = "TblUser";
            PrimaryKey = "UserID";
        }
        public UserData(UserDto userDto)
        {
            TableName = "TblUser";
            PrimaryKey = "UserID";
            tblUser = new TblUser(userDto, PrimaryKey);
        }
        public int CreateRecord()
        {
           return CreateRecord(tblUser.GetInsertColumnsData, tblUser.GetInsertColumnValuesData);
        }
        public void UpdateRecord(int primaryKeyValue)
        {
            UpdateRecord(tblUser.GetUpdateColumnsData, primaryKeyValue);
        }
        public UserDto EntityUser(int primaryKeyValue)
        {
            var dt = GetEntity(primaryKeyValue);
            UserDto userDto = DataTableHelper.ConvertDataTableToObject<UserDto>(dt);
            return userDto;
        }
        public List<UserDto> EntityUsers()
        {
            var dt = GetEntities();
            List<UserDto> userDtoLst = DataTableHelper.ConvertDataTableToObjectList<UserDto>(dt);
            return userDtoLst;
        }

        protected class TblUser : TableHelper
        {
            public TblUser(UserDto userDto, string primaryKey)
            {
                var properties = typeof(UserDto).GetProperties();
                PrimaryKey = primaryKey;
                GetColumnData(properties, userDto);
            }
        }

    }
}
