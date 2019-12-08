using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.Entities.Entity;

namespace ShopApp.WebUI.Models
{

    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; } // bir sayfdada kaç tane öğe olacağı
        public int CurrentPage { get; set; }
        public string CurrentCategory { get; set; }

        public int TotalPages()
        {
            return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
        }
    }

    public class ProductListModel
    {
        public PagingInfo PagingInfo { get; set; }
        public List<Product> Products { get; set; }
    }
}
