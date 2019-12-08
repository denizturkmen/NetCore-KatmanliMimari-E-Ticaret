using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccessLayer.Abstract;
using ShopApp.Entities.Entity;

namespace ShopApp.DataAccessLayer.Concrete.EntityFrameWork
{
    public class EfCoreCartDAL : EfCoreGenericRepository<Cart, DataBaseContext>, ICartDAL
    {
        public Cart GetByUserId(string userId)
        {
            using (var context = new DataBaseContext())
            {
                return context
                    .Carts
                    .Include(i => i.CartItems)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefault(i => i.UserId == userId);
            }
        }


        public override void Update(Cart entity)
        {
            //bunu diyerek cartın işikili olduğu cartItems güncellemiş olduk
            using (var context = new DataBaseContext())
            {
                context.Carts.Update(entity);
                context.SaveChanges();
            }
        }

        public void DeleteFromCart(int cartId, int productId)
        {
            using (var context = new DataBaseContext())
            {
                var cmd = @"delete from CartItem where CartId=@p0 And ProductId=@p1";
                context.Database.ExecuteSqlCommand(cmd, cartId, productId);
            }
        }

        public void ClearCart(string cartId)
        {
            using (var context = new DataBaseContext())
            {
                var cmd = @"delete from CartItem where CartId=@p0";
                context.Database.ExecuteSqlCommand(cmd, cartId);
            }
        }
    }
}
