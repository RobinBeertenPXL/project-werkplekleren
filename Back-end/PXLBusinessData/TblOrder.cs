using PXLData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXLBusinessData
{
    public class OrderDto
    {
        public int Orderid { get; set; }
        public int Userid { get; set; }
        public DateTime OrderDate { get; set; }
        public int? Creationid { get; set; }
        public DateTime? CreationDate { get; set; }
    }
    class TblOrderDto : TableHelper
    {
        private OrderDto orderDto;
        public TblOrderDto(OrderDto orderDtoParam, string primaryKey)
        {
            orderDto = orderDtoParam;
            var properties = typeof(OrderDto).GetProperties();
            GetColumnData(properties, orderDtoParam);
        }
    }
    public class OrderData : DatabaseHelper
    {
        private TblOrder tblOrder;
        public OrderData()
        {
            TableName = "TblOrder";
            PrimaryKey = "OrderID";
        }
        public OrderData(OrderDto orderDto)
        {
            TableName = "TblOrder";
            PrimaryKey = "OrderID";
            tblOrder = new TblOrder(orderDto, PrimaryKey);
        }
        public int CreateRecord()
        {
            return CreateRecord(tblOrder.GetInsertColumnsData, tblOrder.GetInsertColumnValuesData);
        }
        public void UpdateRecord(int primaryKeyValue)
        {
            UpdateRecord(tblOrder.GetUpdateColumnsData, primaryKeyValue);
        }
        public OrderDto EntityUser(int primaryKeyValue)
        {
            var dt = GetEntity(primaryKeyValue);
            OrderDto orderDto = DataTableHelper.ConvertDataTableToObject<OrderDto>(dt);
            return orderDto;
        }
        public List<OrderDto> EntityUsers()
        {
            var dt = GetEntities();
            List<OrderDto> orderDtoLst = DataTableHelper.ConvertDataTableToObjectList<OrderDto>(dt);
            return orderDtoLst;
        }

        protected class TblOrder : TableHelper
        {
            public TblOrder(OrderDto orderDto, string primaryKey)
            {
                var properties = typeof(OrderDto).GetProperties();
                PrimaryKey = primaryKey;
                GetColumnData(properties, orderDto);
            }
        }

    }
}
