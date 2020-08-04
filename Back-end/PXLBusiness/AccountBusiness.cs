using PXLBusinessData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXLBusiness
{
    public class AccountBusiness
    {
        public AccountBusiness()
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
        ~AccountBusiness()
        {
            LoginData.Stop();
        }
        public DataTable GetAccountData()
        {
            QueryData qd = new QueryData();
            DataTable dt = qd.GetAccountData();
            return dt;
        }
        public DataTable GetAccountData(string email, string pwd)
        {
            QueryData qd = new QueryData();
            DataTable dt = qd.GetAccountData(email, pwd);
            return dt;
        }
        public void CreateAccount(UserDto userDto, PersDto persDto)
        {
            UserBusiness userBusiness = new UserBusiness();
            
            userBusiness.CreateUser(userDto);
            //userid komt in userdto object
            PersonBusiness pb = new PersonBusiness();
            persDto.Userid = userDto.Userid;
            persDto.Addressid = 1;
            //persDto.lan
            pb.CreatePerson(persDto);
            LoginData.Stop();
        }
        public void UpdateAccount(int userID, int personID, UserDto userDto, PersDto persDto)
        {
            PersonBusiness pb = new PersonBusiness();
            pb.UpdateUser(persDto, personID);
            UserBusiness userBusiness = new UserBusiness();
            userBusiness.UpdateUser(userDto, userID);
            LoginData.Stop();
        }
        public int GetEmail(string email, string userId = "")
        {
            int userID = -1;
            bool ignoreCurrentUser = false;

            UserData userData = new UserData();
            ColumnDataHelper columnDH = new ColumnDataHelper();
            columnDH.Fields.Add("email");
            columnDH.FieldValues.Add(email);
            columnDH.FieldTypes.Add(typeof(string));

            if (!String.IsNullOrEmpty(userId))
            {
                ignoreCurrentUser = true;
                columnDH.Fields.Add("userid");
                columnDH.FieldValues.Add(userId);
                columnDH.FieldTypes.Add(typeof(int));
            }

            DataTable dt = userData.GetRecords(columnDH.GetWhereClause(ignoreCurrentUser));
            if (dt.Rows.Count > 0)
                userID = Convert.ToInt32(dt.Rows[0]["userID"].ToString());

            return userID;
        }
        //public int GetEmail(string email, string userId)
        //{
        //    int userID = -1;

        //    UserData userData = new UserData();
        //    ColumnDataHelper columnDH = new ColumnDataHelper();
        //    columnDH.Fields.Add("email");
        //    columnDH.FieldValues.Add(email);
        //    columnDH.FieldTypes.Add(typeof(string));
        //    columnDH.Fields.Add("userid");
        //    columnDH.FieldValues.Add(userId);
        //    columnDH.FieldTypes.Add(typeof(int));
        //    DataTable dt = userData.GetRecords(columnDH.GetWhereClause(true));
        //    if (dt.Rows.Count > 0)
        //        userID = Convert.ToInt32(dt.Rows[0]["userID"].ToString());

        //    return userID;
        //}
    }
}
