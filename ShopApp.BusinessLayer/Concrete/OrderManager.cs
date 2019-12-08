using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.BusinessLayer.Abstract;
using ShopApp.DataAccessLayer.Abstract;
using ShopApp.Entities.Entity;

namespace ShopApp.BusinessLayer.Concrete
{
    public class OrderManager : IOrderService
    {
        private IOrderDAL _orderDal;
        public OrderManager(IOrderDAL orderDal)
        {
            _orderDal = orderDal;
        }
        public void Create(Order entity)
        {
            _orderDal.Create(entity);
        }
        public List<Order> GetOrders(string userId)
        {
            return _orderDal.GetOrders(userId);
        }
    }
}
