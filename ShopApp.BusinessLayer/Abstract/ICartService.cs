using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Entities.Entity;

namespace ShopApp.BusinessLayer.Abstract
{
    public interface ICartService
    {
        void InitializeCart(string userId);

        Cart GetCartByUserId(string userId);

        void AddToCart(string userId, int productId, int quantity);

        void DeleteFromCart(string userId, int productId);

        void ClearCart(string cartId);
    }
}
