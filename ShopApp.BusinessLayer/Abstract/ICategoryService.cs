using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Entities.Entity;

namespace ShopApp.BusinessLayer.Abstract
{
    //BusinessLayer = DAL ile WebUI arasındaki iletişim sağlayan katman
    public interface ICategoryService
    {
        Category GetById(int id);
        List<Category> GetAll();
        Category GetByIdWithProducts(int id);
       
        void Create(Category entity);
        void Update(Category entity);
        void Delete(Category entity);
        void DeleteFromCategory(int categoryId, int productId);
    }
}
