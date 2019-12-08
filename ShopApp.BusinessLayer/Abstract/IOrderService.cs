using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Entities.Entity;

namespace ShopApp.BusinessLayer.Abstract
{
    public interface IOrderService
    {
        void Create(Order entity);
        List<Order> GetOrders(string userId);
    }
}
