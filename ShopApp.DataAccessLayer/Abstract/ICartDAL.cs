using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Entities.Entity;

namespace ShopApp.DataAccessLayer.Abstract
{
    public interface ICartDAL : IRepository<Cart>
    {
        Cart GetByUserId(string userId);

        void DeleteFromCart(int cartId, int productId);

        void ClearCart(string cartId);
    }
}
