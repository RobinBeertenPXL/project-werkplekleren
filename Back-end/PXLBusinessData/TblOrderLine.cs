using PXLData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXLBusinessData
{
    public class OrderLineDto
    {
        public int OrderLineid { get; set; }
        public int Orderid { get; set; }
        public int Productid { get; set; }
        public int Amount { get; set; }
    }
    class TblOrderLineDto : TableHelper
    {
        private OrderLineDto orderLineDto;
        public TblOrderLineDto(OrderLineDto orderLineDtoParam, string primaryKey)
        {
            orderLineDto = orderLineDtoParam;
            var properties = typeof(OrderLineDto).GetProperties();
            GetColumnData(properties, orderLineDtoParam);
        }
    }
    public class OrderLineData : DatabaseHelper
    {
        private TblOrderLine tblOrderLine;
        public OrderLineData()
        {
            TableName = "TblOrderLine";
            PrimaryKey = "OrderLineID";
        }
        public OrderLineData(OrderLineDto orderLineDto)
        {
            TableName = "TblOrderLine";
            PrimaryKey = "OrderLineID";
            tblOrderLine = new TblOrderLine(orderLineDto, PrimaryKey);
        }
        public int CreateRecord()
        {
            return CreateRecord(tblOrderLine.GetInsertColumnsData, tblOrderLine.GetInsertColumnValuesData);
        }
        public void UpdateRecord(int primaryKeyValue)
        {
            UpdateRecord(tblOrderLine.GetUpdateColumnsData, primaryKeyValue);
        }
        public OrderLineDto EntityUser(int primaryKeyValue)
        {
            var dt = GetEntity(primaryKeyValue);
            OrderLineDto orderLineDto = DataTableHelper.ConvertDataTableToObject<OrderLineDto>(dt);
            return orderLineDto;
        }
        public List<OrderLineDto> EntityUsers()
        {
            var dt = GetEntities();
            List<OrderLineDto> orderLineDtoLst = DataTableHelper.ConvertDataTableToObjectList<OrderLineDto>(dt);
            return orderLineDtoLst;
        }

        protected class TblOrderLine : TableHelper
        {
            public TblOrderLine(OrderLineDto orderLineDto, string primaryKey)
            {
                var properties = typeof(OrderLineDto).GetProperties();
                PrimaryKey = primaryKey;
                GetColumnData(properties, orderLineDto);
            }
        }

    }
}
