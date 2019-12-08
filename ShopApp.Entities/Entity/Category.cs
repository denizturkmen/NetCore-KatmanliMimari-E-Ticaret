using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Entities.Entity
{
    public class Category
    {
        //(product-category) relationship N*N
        public int  Id { get; set; }
        public string Name { get; set; }

        //Join için
        public List<ProductCategory> ProductCategories { get; set; }
    }
}
