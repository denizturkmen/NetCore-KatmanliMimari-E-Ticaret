using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ShopApp.Entities.Entity;


namespace ShopApp.DataAccessLayer.Abstract
{
    public interface ICategoryDAL:IRepository<Category>
    {
        //IcategoryDal oluşturmamısın nedeni Icategoryözgü metotlar yazmak için

        //category içindeki productları almak için
        Category GetByIdWithProducts(int id);

        //category içinden ürün silincek ama veritabanından değil
        void DeleteFromCategory(int categoryId, int productId);

    }
}
