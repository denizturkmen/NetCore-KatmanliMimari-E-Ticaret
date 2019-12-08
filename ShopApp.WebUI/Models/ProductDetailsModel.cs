using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.Entities.Entity;

namespace ShopApp.WebUI.Models
{
    public class ProductDetailsModel
    {
        public Product Product { get; set; }
        // Bir Product N Category olduğu için
        public List<Category> Categories { get; set; }
    }
}
