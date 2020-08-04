using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PXLData;

namespace PXLBusinessData
{
    public class QueryData : DatabaseHelper
    {
        public DataTable GetAccountData()
        {
            string sql = "select tbluser.userid, tbluser.email, tbluser.password, ";
            sql += "tblperson.firstname, tblperson.name, tblperson.birthdate, tblperson.languageid, tblperson.addressid";
            sql += " from tbluser inner join tblperson on tbluser.userid = tblperson.userid";
            return GetQueryData(sql);
        }
        public DataTable GetAccountData(string email, string pwd)
        {
            string sql = "select tbluser.userid, tbluser.email, tbluser.password, ";
            sql += "tblperson.firstname, tblperson.name, tblperson.birthdate, tblperson.languageid, tblperson.addressid, tblperson.paymentmethod";
            sql += " from tbluser inner join tblperson on tbluser.userid = tblperson.userid";
            sql += $" where tbluser.email='{email}' and tbluser.password = '{pwd}'";
            return GetQueryData(sql);
        }
        public DataTable GetOrderData(int userid)
        {
            //string sql = "select tblorder.orderid, tblorder.userid, tblorder.orderdate, tblorder.creationid, tblorder.creationdate, ";
            //sql += "tblorderline.orderlineid, tblorderline.orderid, tblorderline.productid, tblorderline.amount";
            //sql += " from tblorder inner join tblorderline on tblorder.orderid = tblorderline.orderid";
            //sql += $" where tblorder.userid='{userid}'";
            string sql = "select tblorder.*, tblorderline.*, tblproduct.* ";
            sql += "from tblorder inner join tblorderline on tblorder.orderid = tblorderline.orderid ";
            sql += "inner join tblproduct on tblproduct.productid = tblorderline.productid ";
            sql += $"where tblorder.userid='{userid}'";
            return GetQueryData(sql);
        }
        private DataTable GetQueryData(string query)
        {
            DataTable dt = GetExecuteReader(query);
            return dt;
        }
    }
}
