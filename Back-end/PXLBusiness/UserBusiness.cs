using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PXLBusinessData;
using System.Data;

namespace PXLBusiness
{
    public class UserBusiness
    {
        public UserBusiness()
        {
            if(LoginData.LoginSuccessFul)
            {
                LoginData.Start();
            }
            else
            {
                throw new Exception("Gelieve correct in te loggen!");
            }            
        }
        ~ UserBusiness()
        {
            LoginData.Stop();
        }
        public void CreateUser(UserDto userDto)
        {           
            userDto.creationid = LoginData.UserID;
            userDto.creationdate = DateTime.Now;
            UserData userData = new UserData(userDto);            
            userDto.Userid= userData.CreateRecord();            
        }
        public void UpdateUser(UserDto userDto,int primaryKeyValue)
        {                    
            UserData userData = new UserData(userDto);
            userData.UpdateRecord(primaryKeyValue);            
        }
        public UserDto GetUserDto(int primaryKeyValue)
        {
            UserData userData = new UserData();
            return userData.EntityUser(primaryKeyValue);
           
        }
        public List<UserDto> GetUserDto()
        {           
            UserData userData = new UserData();
            return userData.EntityUsers();
        }
        public DataTable GetUserData()
        {            
            UserData userData = new UserData();
            DataTable dt = userData.GetRecords();
            //DatabaseLauncher.StopWithoutTransaction();
            return dt;
        }       
        public int GetLoginData(string email, string paswoord)
        {
            int userID = -1;
            
            UserData userData = new UserData();
            ColumnDataHelper columnDH = new ColumnDataHelper();
            columnDH.Fields.Add("email");
            columnDH.FieldValues.Add(email);
            columnDH.FieldTypes.Add(typeof(string));
            columnDH.Fields.Add("paswoord");
            columnDH.FieldValues.Add(paswoord);
            columnDH.FieldTypes.Add(typeof(string));
            DataTable dt = userData.GetRecords(columnDH.GetWhereClause());
            if (dt.Rows.Count > 0)
                userID = Convert.ToInt32(dt.Rows[0]["userID"].ToString());
           
            return userID;
        }
    }
}
