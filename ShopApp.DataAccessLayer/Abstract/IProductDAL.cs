using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ShopApp.Entities.Entity;

namespace ShopApp.DataAccessLayer.Abstract
{
    public interface IProductDAL:IRepository<Product>
    {

        //category bilgisine göre product sorgulama
        List<Product> GetProductsByCategory(string category,int page ,int pageSize);

        Product GetProductDetails(int id);
        
        //Pagination için
        int GetCountByCategory(string category);

        Product GetByIdWithCategories(int id);

        void Update(Product entity, int[] categoryIds);


        /*
        //generic yapıdan implement ettiğimiz için kaldırdık

        Product Get(int id);
        Product GetOne(Expression<Func<Product, bool>> filter);
        //Product ile ilgili colonlarda sorgu yapmak için (LinQ )
        IQueryable<Product> GetAll(Expression<Func<Product, bool>> filter);

        void Create(Product entity);
        void Update(Product entity);
        void Delete(Product entity);
        */
    }
}
