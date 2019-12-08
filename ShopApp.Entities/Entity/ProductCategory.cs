using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Entities.Entity
{
    //Junction Table
    // 1    1
    // 1    2 bunun içim CategoryId ve ProductId PK yapmak zorundayız
    public class ProductCategory
    {
        //public int Id { get; set; }

        //Fluent Api ile PK yapcaz
        public int CategoryId { get; set; }
        //Navigation Property
        public Category Category { get; set; }

        //Fluent Api ile PK yapcaz
        public int ProductId { get; set; }
        //Navigation Property
        public Product Product { get; set; }
    }
}
