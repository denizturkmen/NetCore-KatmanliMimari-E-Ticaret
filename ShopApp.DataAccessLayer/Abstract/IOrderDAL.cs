using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Entities.Entity;

namespace ShopApp.DataAccessLayer.Abstract
{
    public interface IOrderDAL : IRepository<Order>
    {
        List<Order> GetOrders(string userId);
    }
}
