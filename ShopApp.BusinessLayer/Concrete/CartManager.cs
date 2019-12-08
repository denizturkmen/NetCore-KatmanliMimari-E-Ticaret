using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.BusinessLayer.Abstract;
using ShopApp.DataAccessLayer.Abstract;
using ShopApp.Entities.Entity;

namespace ShopApp.BusinessLayer.Concrete
{
    public class CartManager:ICartService
    {
        private ICartDAL _cartDal;

        public CartManager(ICartDAL cartDal)
        {
            _cartDal = cartDal;
        }

        public void InitializeCart(string userId)
        {
          _cartDal.Create(new Cart()
          {
              UserId = userId
          });
        }

        public Cart GetCartByUserId(string userId)
        {
            return _cartDal.GetByUserId(userId);
        }

        public void AddToCart(string userId, int productId, int quantity)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                var index = cart.CartItems.FindIndex(i => i.ProductId == productId);

                if (index < 0)
                {
                    cart.CartItems.Add(new CartItem()
                    {
                        ProductId = productId,
                        Quantity = quantity,
                        CartId = cart.Id
                    });
                }
                else
                {
                    cart.CartItems[index].Quantity += quantity;
                }

                _cartDal.Update(cart);
            }
        }

        public void DeleteFromCart(string userId, int productId)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                _cartDal.DeleteFromCart(cart.Id, productId);
            }
        }

        public void ClearCart(string cartId)
        {
            _cartDal.ClearCart(cartId);
        }
    }
}
