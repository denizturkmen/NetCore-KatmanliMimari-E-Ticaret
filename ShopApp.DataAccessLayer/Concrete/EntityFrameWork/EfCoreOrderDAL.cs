using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccessLayer.Abstract;
using ShopApp.Entities.Entity;

namespace ShopApp.DataAccessLayer.Concrete.EntityFrameWork
{
    public class EfCoreOrderDal : EfCoreGenericRepository<Order, DataBaseContext>, IOrderDAL
    {
        public List<Order> GetOrders(string userId)
        {
            using (var context = new DataBaseContext())
            {
                var orders = context.Orders
                    .Include(i => i.OrderItems)
                    .ThenInclude(i => i.Product)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(userId))
                {
                    orders = orders.Where(i => i.UserId == userId);
                }

                return orders.ToList();
            }
        }
    }
}
