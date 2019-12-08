using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Entities.Entity
{
    public class CartItem
    {
        public int Id { get; set; }
        //Navigation property
        public Product Product { get; set; }
        public int ProductId { get; set; }
        
        //Navigation property
        public Cart Cart { get; set; }
        public int CartId { get; set; }

        public int Quantity { get; set; }
    }
}


