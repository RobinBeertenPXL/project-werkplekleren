using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXLBusinessData
{    
    public class ColumnDataHelper
    {
        public List<string> Fields;
        public List<string> FieldValues;
        public List<Type> FieldTypes;
        public ColumnDataHelper()
        {
            Fields = new List<string>();
            FieldValues = new List<string>();
            FieldTypes = new List<Type>();
        }
        //public string GetWhereClause()
        //{
        //    string whereSQL = "";
        //    if (FieldTypes.Count > 0)
        //    {
        //        if (FieldTypes[0] == typeof(int))
        //        {
        //            whereSQL = $" where {Fields[0]}='{FieldValues[0]}'";
        //        }
        //        else
        //        {
        //            whereSQL = $" where {Fields[0]}='{FieldValues[0]}'";
        //        }
        //        for (int i = 1; i < FieldTypes.Count; i++)
        //        {
        //            if (FieldTypes[0] == typeof(int))
        //            {
        //                whereSQL += $" and {Fields[i]}={FieldValues[i]}";
        //            }
        //            else
        //            {
        //                whereSQL += $" and {Fields[i]}='{FieldValues[i]}'";
        //            }
        //        }
        //    }
        //    return whereSQL;
        //}
        public string GetWhereClause(bool ignoreCurrentUser = false)
        {
            string whereSQL = "";
            string equalityCheck = "=";
            string whereAnd = " where";
            if (FieldTypes.Count > 0)
            {
                //if (FieldTypes[0] == typeof(int))
                //{
                //    whereSQL = $" where {Fields[0]}{equalityCheck}{FieldValues[0]}";
                //}
                //else
                //{
                //    whereSQL = $" where {Fields[0]}{equalityCheck}'{FieldValues[0]}'";
                //}
                for (int i = 0; i < FieldTypes.Count; i++)
                {
                    if (i > 0)
                    {
                        whereAnd = " and";
                    }

                    if (Fields[i] == "userid" && ignoreCurrentUser)
                    {
                        equalityCheck = "!=";
                    }
                    else
                    {
                        equalityCheck = "=";
                    }

                    if (FieldTypes[i] == typeof(int))
                    {
                        whereSQL += $"{whereAnd} {Fields[i]}{equalityCheck}{FieldValues[i]}";
                    }
                    else
                    {
                        whereSQL += $"{whereAnd} {Fields[i]}{equalityCheck}'{FieldValues[i]}'";
                    }
                }
            }

            return whereSQL;
        }  
    }
}
