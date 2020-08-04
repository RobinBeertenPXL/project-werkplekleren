using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Globalization;

namespace PXLData
{    
    public class DatabaseHelper : DatabaseCommand
    {
        public string TableName { get; set; }
        public string PrimaryKey { get; set; }
        #region get        
        public DataTable GetRecords()
        {
            string query = $"select * from {TableName}";
            var dt = GetDatabaseRecords(query);           
            return dt;
        }
        protected DataTable GetEntities()
        {
            string query = $"select * from {TableName} ";
            var dt = GetDatabaseRecords(query);
            return dt;
        }
        protected DataTable GetEntity(int primaryKeyValue)
        {
            string query = $"select * from {TableName} where {PrimaryKey}={primaryKeyValue}";
            var dt = GetDatabaseRecords(query);
            return dt;
        }
        public DataTable GetRecords(string whereQuery)
        {           
            string query = $"select * from {TableName} {whereQuery}";            
            var dt = GetDatabaseRecords(query);
            return dt;
        }
        private DataTable GetDatabaseRecords(string query)
        {            
            var dt= GetExecuteReader(query);
            dt.TableName = TableName;
            return dt;
        }
        #endregion
        #region db commands
        protected int CreateRecord(string columns, string columnValues)
        {
            return CreateDBRecord(CreateCommand(columns, columnValues));            
        }        
        protected void UpdateRecord(string updateColumnData, int primaryKeyValue)
        {
            UpdateDBRecord(UpdateCommand(updateColumnData, primaryKeyValue));                  
        }
        protected void DeleteRecord(int primaryKeyValue)
        {
            DeleteDBRecord(DeleteCommand(primaryKeyValue));
        }
        #endregion
        #region build sql command
        private string CreateCommand(string columns, string columnValues)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("INSERT INTO {0} ", TableName);
            sql.AppendFormat("({0})", columns);
            sql.AppendFormat(" VALUES ");
            sql.AppendFormat("({0})", columnValues);
            return sql.ToString();
        }
        private string UpdateCommand(string updateColumns, int primaryKeyValue)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("UPDATE {0} ", TableName);
            sql.AppendFormat(" SET {0}", updateColumns);
            sql.AppendFormat(" WHERE {0}={1} ", PrimaryKey, primaryKeyValue);
            return sql.ToString();
        }
        private string DeleteCommand(int primaryKeyValue)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("DELETE FROM {0} ", TableName);           
            sql.AppendFormat(" WHERE {0}={1} ", PrimaryKey, primaryKeyValue);
            return sql.ToString();
        }
        #endregion
    }    
    public class TableColumnObject
    {
        public string ColumnName { get; set; }
        public Type ColumnType { get; set; }
        public object ColumnValue { get; set; }
    }
    public class TableHelper
    {
        protected string PrimaryKey { get; set; }
        public List<TableColumnObject> TableColumnObjectList = new List<TableColumnObject>();
        public string GetInsertColumnsData { get; set; }
        public string GetInsertColumnValuesData { get; set; }
        public string GetUpdateColumnsData { get; set; }
        protected void GetColumnData(PropertyInfo[] properties, object dtoParam)
        {
            var tco = new TableColumnObject();
            StringBuilder columnsSB = new StringBuilder();
            StringBuilder columnValuesSB = new StringBuilder();
            StringBuilder updateSB = new StringBuilder();
            foreach (var propertyInfo in properties)
            {
                tco.ColumnName = propertyInfo.Name;
                tco.ColumnType = propertyInfo.PropertyType;
                tco.ColumnValue = propertyInfo.GetValue(dtoParam, null);
                TableColumnObjectList.Add(tco);
                if(tco.ColumnValue !=null && tco.ColumnName.Trim().ToLower() != PrimaryKey.Trim().ToLower())
                {
                    if (columnsSB.Length > 0)
                    {
                        columnsSB.Append(",");
                        columnValuesSB.Append(",");
                        updateSB.Append(",");
                    }
                    columnsSB.AppendFormat("{0}", tco.ColumnName);
                    columnValuesSB.AppendFormat("{0}", ColumHelper.GetColumValue(tco.ColumnValue));
                    updateSB.AppendFormat("{0}={1}", tco.ColumnName, ColumHelper.GetColumValue(tco.ColumnValue));
                }
            }
            GetInsertColumnsData = columnsSB.ToString();
            GetInsertColumnValuesData = columnValuesSB.ToString();
            GetUpdateColumnsData = updateSB.ToString();
        }
       
    }
    public static class ColumHelper
    {
        public static string GetColumValue(object objValue)
        {
            string returnValue = "NULL";
            if (objValue != null)
            {
                if (objValue is DateTime)
                {
                    DateTime dtm = (DateTime)objValue;
                    returnValue = string.Format("'{0}'", dtm.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (objValue is int)
                {
                    returnValue = string.Format("{0}", objValue.ToString());
                }
                // Localization Laptop != localization database
                else if (objValue is float)
                {
                    returnValue = string.Format("{0}", ((float)objValue).ToString("0.##", new CultureInfo("en-US")));
                }
                else if (objValue is decimal)
                {
                    returnValue = string.Format("{0}", ((decimal)objValue).ToString("0.##", new CultureInfo("en-US")));
                }
                else if (objValue is double)
                {
                    returnValue = string.Format("{0}", ((double)objValue).ToString("0.##", new CultureInfo("en-US")));
                }
                else
                {
                    returnValue = string.Format("'{0}'", objValue.ToString());
                }
            }
            return returnValue;
        }
    }

    public static class DataTableHelper
    {
        public static List<T> ConvertDataTableToObjectList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex) 
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                }
                return objT;
            }).ToList();
        }
        public static T ConvertDataTableToObject<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            
            var objT = Activator.CreateInstance<T>();
            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }
            return objT;
           
        }
    }
}
