using PXLBusinessData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXLBusiness
{
    public class OrderBusiness
    {
        public OrderBusiness()
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
        ~OrderBusiness()
        {
            LoginData.Stop();
        }
        //public DataTable GetOrderData()
        //{
        //    QueryData qd = new QueryData();
        //    DataTable dt = qd.GetOrderData();
        //    return dt;
        //}
        public DataTable GetOrderData(int userid)
        {
            QueryData qd = new QueryData();
            DataTable dt = qd.GetOrderData(userid);
            return dt;
        }
        private void CreateOrder(OrderDto orderDto)
        {
            //orderDto.Userid = LoginData.UserID;
            orderDto.OrderDate = DateTime.Now;
            orderDto.Creationid = LoginData.UserID;
            orderDto.CreationDate = DateTime.Now;
            OrderData orderData = new OrderData(orderDto);
            orderDto.Orderid = orderData.CreateRecord();
        }

        public void CreateOrder(OrderDto orderDto, List<OrderLineDto> orderLineDtoList)
        {
            CreateOrder(orderDto);

            foreach (OrderLineDto orderLineDto in orderLineDtoList)
            {
                orderLineDto.Orderid = orderDto.Orderid;
                OrderLineData orderLineData = new OrderLineData(orderLineDto);
                orderLineDto.OrderLineid = orderLineData.CreateRecord();
            }

            LoginData.Stop();
        }
        public void UpdateAccount(int userID, int personID, UserDto orderDto, PersDto orderLineDto)
        {
            PersonBusiness pb = new PersonBusiness();
            pb.UpdateUser(orderLineDto, personID);
            UserBusiness userBusiness = new UserBusiness();
            userBusiness.UpdateUser(orderDto, userID);
            LoginData.Stop();
        }
    }
}
