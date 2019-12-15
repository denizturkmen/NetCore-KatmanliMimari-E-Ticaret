using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Entities.Entity
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        //cart tan cartItem geçip ordan product ya da category ile ilgii işlemler için
        public List<CartItem> CartItems { get; set; }
    }
}
